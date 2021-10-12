using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SavePoint : Window_UI
{
    /// <summary>
    /// ƒ_ƒ“ƒWƒ‡ƒ“‚ğ’Eo‚µ‚ÄHome‚É–ß‚é
    /// </summary>
    public void AwayDangeon()
    {
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("Home");
    }
}
