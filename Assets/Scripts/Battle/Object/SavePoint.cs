using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject ui;
    void Start()
    {
    }
    //不使用　BattleSceneManagerに機能移植済み
    private void UpdateArrivalFloor()
    {
        //最終到達点を現在のレベルに設定
        int currentFloor = BattleLevelManager.Instance.currentLevel; //現在のフロア

        int arrivalFloor = DataManager.Instance.saveData.arrivalFloor; //最高到達フロア
        if (arrivalFloor < currentFloor)
        {
            DataManager.Instance.saveData.arrivalFloor = currentFloor;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //UpdateArrivalFloor();
        ui.SetActive(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ui.SetActive(false);
    }
   
}

