using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogManager : SingletonMonoBehaviour<DebugLogManager>
{

    /// <summary>
    /// Log出力
    /// </summary>
    Dictionary<string, LogDisplay> logDisplays = new Dictionary<string, LogDisplay>();
    [SerializeField] private bool isActive;
    [SerializeField] Vector2 offset;
    void Awake()
    {
        base.Awake();
        //ログの表示位置準備　作ってるゲームによって自分でうまく設定してください
        logDisplays = new Dictionary<string, LogDisplay>();
        logDisplays.Add(My_LogType.KEY_PROFILER, new LogDisplay(My_LogType.KEY_PROFILER, 1, new Rect(0 + offset.x, -100 + offset.y, 400 + offset.x, 100 + offset.y)));
        logDisplays.Add(My_LogType.KEY_DEBUG_LOG, new LogDisplay(My_LogType.KEY_DEBUG_LOG, 3, new Rect(0 + offset.x, 0 + offset.y, 400 + offset.x, 100 + offset.y), true));
        logDisplays.Add(My_LogType.KEY_AUDIO_LOG, new LogDisplay(My_LogType.KEY_AUDIO_LOG, 3, new Rect(0 + offset.x, 100 + offset.y, 400 + offset.x, 200 + offset.y)));
        logDisplays.Add(My_LogType.KEY_SCENE_LOG, new LogDisplay(My_LogType.KEY_SCENE_LOG, 3, new Rect(0 + offset.x, 200 + offset.y, 400 + offset.x, 300 + offset.y)));

        //表示サンプル※無改変時のコードじゃないと表示したい対象のグループが消えて動かなくなるかも
        //StartCoroutine(test());
    }
    public void AddLogDisplays(string key, int maxlogCount, Rect rect)
    {
        logDisplays.Add(key, new LogDisplay(key, maxlogCount, rect));
    }
    /// <summary>
    /// ログ表示、非表示の切り替えなど
    /// </summary>
    private void Update()
    {
        //ゲームによって書き換えてください
        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.L))
        {
            isActive = !isActive;
        }
        logDisplays[My_LogType.KEY_PROFILER].AddLog((1f / Time.deltaTime).ToString());
    }
    /// <summary>
    /// ログ表示
    /// </summary>
    /// <param name="log">ログテキスト</param>
    /// <param name="key">表示したいテキストのグループキー</param>
    /// <param name="debugLog">デバッグログに表示するか</param>
    public void AddLog(string log, string key = "DebugLog", bool debugLog = false)
    {
        if (!logDisplays.ContainsKey(key))
        {
            Debug.LogError("キーが存在しません キー:" + key);
            Debug.Log(log);
            return;
        }
        //デバッグログに表示
        if (debugLog)
            Debug.Log(log);

        logDisplays[key].AddLog(log);
    }
    public void LogDisplaySetActive(string key, bool isActive)
    {
        if (!logDisplays.ContainsKey(key))
        {
            Debug.LogError("キーが存在しません キー:" + key);
            return;
        }
        logDisplays[key].isActive = isActive;
    }
    /// <summary>
    /// 表示サンプル。毎秒ログが出る　audioログだけDebugLogに表示される
    /// </summary>
    /// <returns></returns>
    IEnumerator test()
    {
        for (int i = 0; i < 10; i++)
        {
            AddLog("audioログ", My_LogType.KEY_AUDIO_LOG, true);
            AddLog("sceneログ", My_LogType.KEY_SCENE_LOG);
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnGUI()
    {
        if (!isActive) return;

        //全ログの表示
        foreach (KeyValuePair<string, LogDisplay> logDisplay in logDisplays)
        {
            //if (logDisplay.Value.isActive) //ログ表示のOnOffをログ毎に切り替えたい場合はこれ　今は全部まとめてやってるのでコメントアウト
            logDisplay.Value.OnGUI();
        }
    }

}
/// <summary>
/// ログ識別用
/// </summary>
public static class My_LogType
{
    public const string KEY_DEBUG_LOG = "DebugLog";
    public const string KEY_SCENE_LOG = "SceneLog";
    public const string KEY_AUDIO_LOG = "AudioLog";
    public const string KEY_PROFILER = "Profiler";
}
