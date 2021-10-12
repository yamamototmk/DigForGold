using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : SingletonMonoBehaviour<BattleSceneManager>
{
    [SerializeField, Header("�G�����ʒu���s�g�p")] public List<Transform> spawnPoints;

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
        Debug.Log("�X�e�[�W�N���A");
    }
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
}
