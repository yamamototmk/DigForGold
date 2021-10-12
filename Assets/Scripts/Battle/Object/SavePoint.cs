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
    //�s�g�p�@BattleSceneManager�ɋ@�\�ڐA�ς�
    private void UpdateArrivalFloor()
    {
        //�ŏI���B�_�����݂̃��x���ɐݒ�
        int currentFloor = BattleLevelManager.Instance.currentLevel; //���݂̃t���A

        int arrivalFloor = DataManager.Instance.saveData.arrivalFloor; //�ō����B�t���A
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

