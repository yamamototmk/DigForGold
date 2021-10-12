using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NextLevelGate : MonoBehaviour
{
    string sceneName = "BattleScene0";
    [SerializeField, Header("trueなら+falseなら-")] bool next;
    [SerializeField] Text text;
    [SerializeField] GameObject ui;
    bool canNext;
    private void Start()
    {
        canNext = true;
        if (next) text.text = "Go up one floor";
        else text.text = "Get off one floor";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ////プレイヤーの状態を更新
        //DataManager.Instance.saveData.playerData = other.GetComponent<Player_Auto>().playerData;
        // UI表示版
        // ui.SetActive(true);
        // Debug.Log("OnTriggerEnter");
        Next();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // UI表示版
        //ui.SetActive(false);
    }
    public void Next()
    {
        if (!canNext) return;
        canNext = false;
        BattleLevelManager bsm = BattleLevelManager.Instance;
        Disable();
        int currentLevel = bsm.currentLevel;
        bsm.SetLevel(bsm.currentLevel + (next ? +1 : -1));
        if (bsm.currentLevel < 0)
        {
            bsm.SetLevel(0);
            DataManager.Instance.Save();
            My_SceneManager.Instance.LoadScene("Home");
        }
        BattleLevelManager.Instance.startType = next ? BattleLevelManager.StartType.GoUp : BattleLevelManager.StartType.GetDown;

        DataManager.Instance.Save();
        if (!My_SceneManager.Instance.LoadScene(sceneName))
        {
            bsm.SetLevel(currentLevel);
            canNext = true;
        }
    }
    public void Disable()
    {
        ui.SetActive(false);
    }
}
