using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 649
/// <summary>
/// BGM、システム音のマネージャー　SEは各種オブジェクトから鳴らす
/// </summary>
[RequireComponent(typeof(AudioMixerManager))]
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioMixerManager audioMixerManager;
    [SerializeField] private Transform audioListenerTransform;
    [SerializeField] private AudioSource audioSourceMenuSe;
    [SerializeField] private AudioSource audioSourceJingle;
    [SerializeField] private List<AudioSource> audioSourceBGMList = new List<AudioSource>(2);
    [SerializeField] private List<AudioClip> menuSeAudioClipList, bgmAudioClipList, jingleClipList, seList;

    private List<IEnumerator> fadeCoroutineList = new List<IEnumerator>();
    private IEnumerator jinglePlayCompCallbackCoroutine;

    public bool IsPaused { get; private set; }
    private List<IAudioPausable> pausableList = new List<IAudioPausable>();

    public void AddPausebleList(IAudioPausable audioPausable)
    {
        pausableList.Add(audioPausable);
    }
    public void RemovePausebleList(IAudioPausable audioPausable)
    {
        pausableList.Remove(audioPausable);
    }
    private void Awake()
    {
        base.Awake();

        audioSourceBGMList.ForEach(asb => asb.loop = true);//BGMのループを全てtrue
    }

    public AudioMixerManager GetAudioMixerManager()
    {
        return audioMixerManager;
    }
    public void PlaySe(string clipName, Vector3 position)
    {
        GameObject speaker = ObjectPoolManager.Instance.GetObject("Speaker");
        speaker.transform.position = position;
        AudioSource source = speaker.GetComponent<AudioSource>();
        AudioClip audioClip = seList.FirstOrDefault(clip => clip.name == clipName);

        source.clip = audioClip;
        StartCoroutine(PlaySeCoroutine(source));
    }

    public void PlayJungle(string clipName, UnityAction compCallback = null)
    {
        if (IsPaused) return;

        //コールバックをnullにするイベント追加？
        compCallback += () => { jinglePlayCompCallbackCoroutine = null; };

        AudioClip audioClip = jingleClipList.FirstOrDefault(clip => clip.name == clipName);

        //clipがなければ処理を中止
        if (audioClip == null)
        {
            DebugLogManager.Instance.AddLog("<color=red>存在しないClip名:" + clipName + "</color>", My_LogType.KEY_AUDIO_LOG);
            return;
        }

        jinglePlayCompCallbackCoroutine = audioSourceJingle.PlayWithCompCallback(audioClip: audioClip, compCallback: compCallback);
        StartCoroutine(jinglePlayCompCallbackCoroutine);
    }
    public void PlayMenuSe(string clipName)
    {
        if (IsPaused) return;

        AudioClip audioClip = menuSeAudioClipList.FirstOrDefault(clip => clip.name == clipName);

        if (audioClip == null)
        {
            DebugLogManager.Instance.AddLog("<color=red>存在しないClip名:" + clipName + "</color>", My_LogType.KEY_AUDIO_LOG);
            return;
        }

        audioSourceMenuSe.Play(audioClip);
    }
    public void PlayBGM(string clipName)
    {
        PlayBGMWithFade(clipName, 0.1f);
    }

    //uGUIから呼ぶ用//
    public void PlayBGMWithFade(string clipName)
    {
        PlayBGMWithFade(clipName, 2f);
    }
    public void PlayBGMWithFade(string clipName, float fadeTime)
    {
        if (IsPaused) return;

        //リストからAudioClipを取得//
        AudioClip audioClip = bgmAudioClipList.FirstOrDefault(clip => clip.name == clipName);

        //clipがなかったら処理を中止//
        if (audioClip == null)
        {
            DebugLogManager.Instance.AddLog("<color=red>存在しないClip名:" + clipName + "</color>", My_LogType.KEY_AUDIO_LOG);
            return;
        }

        AudioSource audioSourceEmpty = audioSourceBGMList.FirstOrDefault(asb => asb.isPlaying == false);

        if (audioSourceEmpty == null)
        {
            DebugLogManager.Instance.AddLog("<color=red>フェード処理中は新たなBGMを再生開始できません</color>", My_LogType.KEY_AUDIO_LOG);
            return;
        }
        else
        {
            StopFadeCoroutine();

            //どちらか片方が再生中ならフェードアウト処理//
            AudioSource audioSourcePlaying = audioSourceBGMList.FirstOrDefault(asb => asb.isPlaying == true);
            if (audioSourcePlaying != null)
            {
                AddFadeCoroutineListAndStart(audioSourcePlaying.StopWithFadeOut(fadeTime));
            }

            AddFadeCoroutineListAndStart(audioSourceEmpty.PlayWithFadeIn(audioClip, fadeTime: fadeTime));
        }
    }
    private void AddFadeCoroutineListAndStart(IEnumerator routine)
    {
        fadeCoroutineList.Add(routine);
        StartCoroutine(routine);
    }

    private void StopFadeCoroutine()
    {
        fadeCoroutineList.ForEach(routine => StopCoroutine(routine));
        fadeCoroutineList.Clear();
    }
    public void StopBGM()
    {
        StopBGMWithFade(0.1f);
    }
    public void StopBGMWithFade(float fadeTime)
    {
        if (IsPaused) return;

        StopFadeCoroutine();

        //再生しているbgm audio sourceがあったら止める//
        foreach (AudioSource asb in audioSourceBGMList.Where(asb => asb.isPlaying == true))
        {
            AddFadeCoroutineListAndStart(asb.StopWithFadeOut(fadeTime));
        }
    }

    public void SetAudioListener(Transform followTransform)
    {
        audioListenerTransform.SetParent(followTransform);
        audioListenerTransform.SetPositionAndRotation(followTransform.position, followTransform.rotation);
    }

    public void ClearAudioListenerPos()
    {
        audioListenerTransform.SetParent(this.transform);
        audioListenerTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void Pause()
    {
        IsPaused = true;

        fadeCoroutineList.ForEach(routine => StopCoroutine(routine));
        audioSourceBGMList.ForEach(asb => asb.Pause());

        PauseExeptBGM();
    }

    public void PauseExeptBGM()
    {
        IsPaused = true;

        audioSourceMenuSe.Pause();
        audioSourceJingle.Pause();

        pausableList.ForEach(p => p.Pause());

        if (jinglePlayCompCallbackCoroutine != null)
        {
            StopCoroutine(jinglePlayCompCallbackCoroutine);
        }
    }

    public void Resume()
    {
        IsPaused = false;

        fadeCoroutineList.ForEach(routine => StartCoroutine(routine));
        audioSourceBGMList.ForEach(asb => asb.UnPause());

        ResumeExeptBGM();
    }

    public void ResumeExeptBGM()
    {
        IsPaused = false;

        audioSourceMenuSe.UnPause();
        audioSourceJingle.UnPause();

        pausableList.ForEach(p => p.Resume());

        if (jinglePlayCompCallbackCoroutine != null)
        {
            StartCoroutine(jinglePlayCompCallbackCoroutine);
        }
    }

    IEnumerator PlaySeCoroutine(AudioSource audioSource)
    {
        audioSource.Play(audioSource.clip);
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.gameObject.SetActive(false);
    }
}
