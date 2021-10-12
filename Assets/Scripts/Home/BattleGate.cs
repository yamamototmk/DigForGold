using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルシーンへの遷移
/// </summary>
public class BattleGate : MonoBehaviour
{
    [SerializeField] GameObject ui;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ui.SetActive(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ui.SetActive(false);
    }
    public void Next(int level)
    {
        BattleLevelManager.Instance.SetLevel(level);
        BattleLevelManager.Instance.Init();
        if (HomeManager.Instance.progress == (int)TutorialID.GotoDangeon) HomeManager.Instance.SetProgress(5);//なぜか6になる
        DataManager.Instance.SaveTemp();
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("BattleScene0");
    }
}
