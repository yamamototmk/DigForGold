using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
#pragma warning disable 649
public class My_SceneManager : SingletonMonoBehaviour<My_SceneManager>
{
    [SerializeField, Header("Fade値に毎フレーム加減算される値")] float fadePoint;
    [SerializeField, Header("Play時にロードされるシーン名")] string InitSceneName;
    [SerializeField] bool canSceneChange;
    [SerializeField] public float fadeValue;
    [SerializeField] UnityAction fadeAction;
    private void Start()
    {
        base.Awake();
        //fadeAction = DammyAction;
        canSceneChange = true;
        fadeValue = 0f;
        fadeAction();
        if (InitSceneName != "")
            LoadScene(InitSceneName);
        else
        {
            DebugLogManager.Instance.AddLog("<color=yellow>InitSceneNameが空なため、シーンはロードされません。</color>", debugLog: true);
        }
    }
    public bool LoadScene(string sceneName, float delay = 0)
    {
        if (canSceneChange)
        {
            Time.timeScale = 1;

            StartCoroutine(LoadSceneAsync(sceneName, delay));
            return true;
        }
        else
        {
            DebugLogManager.Instance.AddLog("<color=red>シーン遷移ビジー状態</color>", My_LogType.KEY_SCENE_LOG);
            return false;
        }
    }

    /// <summary>
    /// 指定したシーン名のシーンが存在するか
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public bool Scene_Exists(string sceneName)
    {
        if (sceneName == "Managers")
        {
            DebugLogManager.Instance.AddLog("<color=red>Managersの呼び出しは許可されていません。</color>", My_LogType.KEY_SCENE_LOG);
            return false;
        }

#if UNITY_EDITOR
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.path == "Assets/Scenes/" + sceneName + ".unity")
                return true;
        }

        DebugLogManager.Instance.AddLog("<color=red>シーン名:" + sceneName + " がBuildSettingに登録されていない又はAssets/Scenes フォルダに存在しません。</color>", My_LogType.KEY_SCENE_LOG);

        return false;
# else 
        return true;
#endif

    }

    IEnumerator LoadSceneAsync(string sceneName, float delay = 0)
    {
        if (canSceneChange == false) yield break;
        canSceneChange = false;

        if (!Scene_Exists(sceneName))
        {
            DebugLogManager.Instance.AddLog("<color=red>シーンの遷移に失敗しました。</color>", My_LogType.KEY_SCENE_LOG);
            canSceneChange = true;
            yield return null;
        }

        DebugLogManager.Instance.AddLog("シーン遷移開始 -> " + sceneName, My_LogType.KEY_SCENE_LOG);
        yield return new WaitForSeconds(delay);
        //フェードアウト
        while (fadeValue < 1)
        {
            fadeValue += fadePoint * Time.deltaTime;
            fadeAction();
            yield return new WaitForEndOfFrame();
        }
        fadeValue = 1;
        //シーンの破棄 Managersシーン単体でなければ、読み込まれているアクティブなシーンを破棄
        if (SceneManager.GetActiveScene().name != "Managers")
        {
            AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (unloadAsync.progress < 1)
                yield return new WaitForEndOfFrame();
        }
        //シーンの読み込み
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadAsync.allowSceneActivation = false;    // シーン遷移をしない

        while (loadAsync.progress < 0.9f)
        {
            DebugLogManager.Instance.AddLog((loadAsync.progress * 100).ToString("F0") + "%", My_LogType.KEY_SCENE_LOG);
            //loadingBar.fillAmount = async.progress;
            yield return new WaitForEndOfFrame();
        }

        DebugLogManager.Instance.AddLog("シーン読み込み完了 -> " + sceneName, My_LogType.KEY_SCENE_LOG);
        //一応タイムスケールをリセット
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.1f);
        loadAsync.allowSceneActivation = true;    // シーン遷移許可
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForEndOfFrame();

        int waitframe = 900;
        //フェードイン
        while (fadeValue > 0)
        {
            waitframe--;
            fadeValue -= fadePoint * Time.deltaTime;
            fadeAction();
            yield return new WaitForEndOfFrame();
            if (waitframe <= 0)
            {
                Debug.Log("待機フレームが900を超えたため強制的にフェードを終了しました");
                fadeValue = 0;
                fadeAction();
                break;
            }
        }

        fadeValue = 0;
        Scene scene = new Scene();
        for (int i = 60 * 3; i >= 0; i--)
        {
            scene = SceneManager.GetSceneByName(sceneName);
            if (scene.name == sceneName) break;
            if (i == 0)
            {
                Debug.LogError("sceneの取得に失敗");
                canSceneChange = true;
                yield break;
            }
            yield return null;
        }

        while(!scene.IsValid())
        {
            yield return null;
            Debug.Log("ロード中...");
        }

        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();
        SceneManager.SetActiveScene(scene);

        canSceneChange = true;
    }
    public void SetFadeAction(UnityAction action)
    {
        fadeAction = action;
    }
    private void DammyAction() { }
    /// <summary>
    /// シーンのパスをstring型でまとめたものを返す
    /// </summary>
    /// <returns></returns>
    public string GetScenePath()
    {
        string str = "";
#if UNITY_EDITOR
        foreach (var scene in EditorBuildSettings.scenes)
        {
            str += scene.path + "\n";
        }
        return str;
#else
        return str;
#endif
    }
}
