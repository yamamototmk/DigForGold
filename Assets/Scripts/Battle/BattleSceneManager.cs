using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : SingletonMonoBehaviour<BattleSceneManager>
{
    [SerializeField, Header("敵生成位置※不使用")] public List<Transform> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        UpdateArrivalFloor();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void StageClear()
    {
        Debug.Log("ステージクリア");
    }
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
}
