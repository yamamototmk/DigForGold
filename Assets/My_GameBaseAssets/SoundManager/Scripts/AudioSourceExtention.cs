using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AudioSourceExtention
{
    public static void Play(this AudioSource audioSource, AudioClip audioClip, float volume = 1f, bool isRandomStartTime = false)
    {
        if (audioClip == null)
        {
            DebugLogManager.Instance.AddLog("<color=red>エラー:</color>　Clipが未設定", My_LogType.KEY_AUDIO_LOG);
            return;
        }
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        if (isRandomStartTime)
        {
            //結果がLengthと同じ値になるとシークエラーを起こすため-0.01秒する
            audioSource.time = UnityEngine.Random.Range(0f, audioClip.length - 0.01f);
        }

        audioSource.Play();
        DebugLogManager.Instance.AddLog("Play:" + audioClip.name, My_LogType.KEY_AUDIO_LOG);
    }
    /// <summary>
    /// 　Fade再生 ※注意　コルーチンです。
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="audioClip"></param>
    /// <param name="targetVolume"></param>
    /// <param name="fadeTime"></param>
    /// <param name="isRandomStartTime"></param>
    /// <returns></returns>
    public static IEnumerator PlayWithFadeIn(this AudioSource audioSource, AudioClip audioClip, float targetVolume = 1f, float fadeTime = 0.1f, bool isRandomStartTime = false)
    {
        //目標ボリュームが0以下の場合は再生キャンセル
        if (targetVolume <= 0f)
        {
            DebugLogManager.Instance.AddLog("<color=red>フェードエラー:</color>　目標ボリューム0:" + audioClip.name, My_LogType.KEY_AUDIO_LOG);
            yield break;
        }

        //再生開始
        audioSource.Play(audioClip, 0f, isRandomStartTime);

        DebugLogManager.Instance.AddLog("フェードイン:" + audioSource.clip.name, My_LogType.KEY_AUDIO_LOG);


        //フェードタイムが0かそれ以下ならフェード処理をキャンセル
        if (fadeTime <= 0)
        {
            audioSource.volume = targetVolume;
            yield break;
        }
        //目標ボリュームに到達するまで毎フレームボリュームを上げる
        while (audioSource.volume < targetVolume)
        {
            float tempVolume = audioSource.volume = audioSource.volume + (Time.deltaTime / fadeTime * targetVolume);

            yield return null;
        }
    }

    public static IEnumerator PlayWithCompCallback(this AudioSource audioSource, AudioClip audioClip, float volume = 1f, UnityAction compCallback = null)
    {
        audioSource.Play(audioClip, volume);
        float timer = 0f;
       
        while (timer < audioClip.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //再生完了コールバックを実行
        if (compCallback != null)
        {
            compCallback();
        }
    }
    public static IEnumerator StopWithFadeOut(this AudioSource audioSource, float fadeTime)
    {
        if (audioSource.isPlaying == false) yield break;

        //フェードタイムが0かそれより小さればフェード処理を行わない//
        if (fadeTime <= 0f)
        {
            audioSource.volume = 0f;
            audioSource.Stop();
            yield break;
        }

        DebugLogManager.Instance.AddLog("フェードアウト:" + audioSource.clip.name, My_LogType.KEY_AUDIO_LOG);

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
    }
}
