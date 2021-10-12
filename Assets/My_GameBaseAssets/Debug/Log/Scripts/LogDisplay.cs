using System.Collections;
using System.Collections.Generic; // Queueのために必要
using UnityEngine;
using System.Text; // StringBuilderのために必要

public class LogDisplay
{
    //何のログか、名前
    string m_Name;
    // ログを何個まで保持するか
    int m_MaxLogCount = 20;
    public bool logReceived { private set; get; }
    // 表示領域
    Rect m_Area = new Rect(0, 0, 400, 400);
    // ログの文字列を入れておくためのQueue
    Queue<string> m_LogMessages = new Queue<string>();

    // ログの文字列を結合するのに使う
    StringBuilder m_StringBuilder = new StringBuilder();

    public bool isActive;

    public LogDisplay(string name, int maxLogCount, Rect area, bool logReceived = false)
    {
        m_Name = name;
        m_MaxLogCount = maxLogCount;
        m_Area = area;
        this.logReceived = logReceived;
        isActive = true;
        //logReceivedがtrueなら DebugLogが呼ばれた時のイベント追加　DebugLogを表示する
        if(logReceived) Application.logMessageReceived += LogReceived;
    }
    /// <summary>
    /// ログ追加
    /// </summary>
    /// <param name="text"></param>
    public void AddLog(string text)
    {
        // ログをQueueに追加
        m_LogMessages.Enqueue(text);

        // ログの個数が上限を超えていたら、最古のものを削除する
        while (m_LogMessages.Count > m_MaxLogCount)
        {
            m_LogMessages.Dequeue();
        }
    }

    /// <summary>
    /// ログが出力される際に呼んでもらう関数
    /// </summary>
    /// <param name="text"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    void LogReceived(string text, string stackTrace, LogType type)
    {
        // ログをQueueに追加
        m_LogMessages.Enqueue(text);

        // ログの個数が上限を超えていたら、最古のものを削除する
        while (m_LogMessages.Count > m_MaxLogCount)
        {
            m_LogMessages.Dequeue();
        }
    }
    public void OnGUI()
    {
        // StringBuilderの内容をリセット
        m_StringBuilder.Length = 0;
        
        //ラベル表示
        m_StringBuilder.Append("<color=blue>-" + m_Name + "-</color>").Append(System.Environment.NewLine);
        
        // ログの文字列を結合する（1個ごとに末尾に改行を追加）
        foreach (string s in m_LogMessages)
        {
            m_StringBuilder.Append(s).Append(System.Environment.NewLine);
        }
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        // 画面に表示
        GUI.Label(m_Area, m_StringBuilder.ToString(),style); ;
    }
}