using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SavePoint : Window_UI
{
    /// <summary>
    /// �_���W������E�o����Home�ɖ߂�
    /// </summary>
    public void AwayDangeon()
    {
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("Home");
    }
}
