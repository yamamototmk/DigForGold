using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickAxe : MonoBehaviour
{
    //1���x���̑ϋv�l
    public const int DURABILITY_UNIT = 30;
    //�����x�オ��ϋv�l�̊���
    public const float DURABILITY_RATE = 0.2f;
    //1+���x��*���̒l�̃_���[�W�ɂȂ�
    public const float PICK_DAMAGERATE = 0.1f;
    [SerializeField] DamageContainer dc;
    PickAxeData pickAxeData;
    private void Start()
    {
        DataLoad();
    }
    private void Update()
    {
        //�^�_���ݒ�
        if (pickAxeData.pickaxeDurability >= 1)
        {
            dc.Damage = 1 + pickAxeData.pickaxeLevel * PICK_DAMAGERATE;
        }
        else
        {
            dc.Damage = 1;
        }
    }
    /// <summary>
    /// �@��
    /// </summary>
    /// <returns>���Ă�����false</returns>
    public bool Dig()
    {
        DataLoad();
        pickAxeData.pickaxeDurability -= 1;
        if (pickAxeData.pickaxeDurability <= 0)
        {
            pickAxeData.pickaxeDurability = 0;
            DataUpdate();
            return false;
        }
        DataUpdate();
        return true;
    }
    public void Repair()
    {
        DataLoad();
        //�ϋv10%��
        pickAxeData.pickaxeDurability += (int)(pickAxeData.PickaxeDurabilityMax() * 0.1f);
        if (pickAxeData.pickaxeDurability > pickAxeData.PickaxeDurabilityMax())
        {
            pickAxeData.pickaxeDurability = pickAxeData.PickaxeDurabilityMax();
        }
        DataUpdate();
        return;
    }
    public void Attack(int num)
    {
        DataLoad();
        pickAxeData.pickaxeDurability -= num;
        if (pickAxeData.pickaxeDurability <= 0)
        {
            pickAxeData.pickaxeDurability = 0;
        }

        DataUpdate();
    }
    /// <summary>
    /// �v���C���[�̃f�[�^����s�b�P���̏�Ԃ����[�h(�p�����[�^���R�s�[)
    /// </summary>
    private void DataLoad()
    {
        pickAxeData = new PickAxeData();
        pickAxeData.Copy(DataManager.Instance.saveData.playerData.pickAxeData);
        DataUpdate();
    }
    /// <summary>
    /// �v���C���[�̃f�[�^���X�V
    /// </summary>
    private void DataUpdate()
    {
        DataManager.Instance.saveData.playerData.pickAxeData.Copy(pickAxeData);
    }

}
[System.Serializable]
public class PickAxeData
{
    public int pickaxeDurability = PickAxe.DURABILITY_UNIT;
    public int pickaxeLevel = 0;
    public int PickaxeDurabilityMax()
    {
        return (int)(PickAxe.DURABILITY_UNIT + (pickaxeLevel * PickAxe.DURABILITY_RATE * PickAxe.DURABILITY_UNIT));
    }
    public void Copy(PickAxeData pickAxeData)
    {
        //�f�[�^���������return
        if (pickAxeData == null) return;
        //�f�[�^�̃R�s�[
        this.pickaxeLevel = pickAxeData.pickaxeLevel;
        this.pickaxeDurability = pickAxeData.pickaxeDurability;
    }
    public void LevelUp()
    {
        pickaxeLevel++;
        pickaxeDurability = PickaxeDurabilityMax();
    }
}
