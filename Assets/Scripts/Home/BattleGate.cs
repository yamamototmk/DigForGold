using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�g���V�[���ւ̑J��
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
        if (HomeManager.Instance.progress == (int)TutorialID.GotoDangeon) HomeManager.Instance.SetProgress(5);//�Ȃ���6�ɂȂ�
        DataManager.Instance.SaveTemp();
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("BattleScene0");
    }
}
