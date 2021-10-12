using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SavePoint : Window_UI
{
    /// <summary>
    /// ダンジョンを脱出してHomeに戻る
    /// </summary>
    public void AwayDangeon()
    {
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("Home");
    }
}
