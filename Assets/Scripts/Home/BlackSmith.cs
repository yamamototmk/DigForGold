using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 鍛冶屋。武器の強化、修理を行う
/// </summary>
public class BlackSmith : MonoBehaviour
{
    public const int REPAIR_PRISE = 1;
    public const int LEVELUP_PRISE_PICK = 800;
    public const float LEVELUP_RATE_PICK = 1.08f;
    public const int LEVELUP_PRISE_ARMOR = 300;
    public const float LEVELUP_RATE_ARMOR = 1.12f;
    public const int ARMOR_REPAIR_PRISE = 20;


    PickAxeData pickAxeData;
    ArmorData armorData;
    [SerializeField] GameObject ui;
    ActiveSwicher playerEffectSwicher;
    private void Start()
    {
        pickAxeData = new PickAxeData();
        pickAxeData.Copy(DataManager.Instance.saveData.playerData.pickAxeData);
        armorData = new ArmorData();
        armorData.Copy(DataManager.Instance.saveData.playerData.armorData);
        GameObject.FindWithTag("Player").TryGetComponent(out playerEffectSwicher);

        if (DataManager.Instance.saveData.blackSmithData.pickAutoRepair) RepairPick(DataManager.Instance.saveData.money);
        if (DataManager.Instance.saveData.blackSmithData.armorAutoRepair) RepairArmor(DataManager.Instance.saveData.money);

    }
    private void Update()
    {
    }
    // Start is called before the first frame update
    /// <summary>
    /// ピッケルを修繕
    /// </summary>
    /// <param name="moneyInHand"></param>
    /// <returns>修繕費</returns>
    private int RepairPick(int moneyInHand)
    {
        //修繕値
        int repairValue = GetPickRepairValue();
        //修繕費用
        int prise = GetPickRepairPrise();

        if (moneyInHand < prise)
        {
            int emp_repairValue = moneyInHand / prise;
            if (emp_repairValue > 0) playerEffectSwicher.GameObjectSetActive_True(2);
            pickAxeData.pickaxeDurability += emp_repairValue;
            DataManager.Instance.saveData.playerData.pickAxeData.Copy(pickAxeData);

            //ここで金が足りないアラート出すか？
            return moneyInHand;
        }
        playerEffectSwicher.GameObjectSetActive_True(2);
        pickAxeData.pickaxeDurability = pickAxeData.PickaxeDurabilityMax();
        DataManager.Instance.saveData.playerData.pickAxeData.Copy(pickAxeData);

        //修理エフェクトだすか
        return prise;
    }
    public int RepairArmor(int moneyInHand)
    {
        int prise = GetArmorRepairPrise();
        if (moneyInHand < prise)
        {
            int emp_repairValue = moneyInHand / prise;
            if (emp_repairValue > 0) playerEffectSwicher.GameObjectSetActive_True(2);

            armorData.currentArmor += moneyInHand / prise;
            DataManager.Instance.saveData.playerData.armorData.Copy(armorData);
            //ここで金が足りないアラート出すか？
            return moneyInHand;
        }
        playerEffectSwicher.GameObjectSetActive_True(2);

        armorData.currentArmor = armorData.GetArmorMax();
        DataManager.Instance.saveData.playerData.armorData.Copy(armorData);

        //修理エフェクトだすか
        return prise;
    }
    public int GetArmorRepairPrise()
    {
        return GetArmorRepairValue() * ARMOR_REPAIR_PRISE;
    }
    /// <summary>
    /// 修繕値を取得
    /// </summary>
    /// <returns></returns>
    public int GetPickRepairValue()
    {
        return pickAxeData.PickaxeDurabilityMax() - pickAxeData.pickaxeDurability;
    }
    /// <summary>
    /// 修繕費用を取得
    /// </summary>
    /// <param name="repairValue"></param>
    /// <returns></returns>
    public int GetPickRepairPrise()
    {
        return GetPickRepairValue() * REPAIR_PRISE; ;
    }
    public int GetPickLevelUpPrise()
    {
        float prise = LEVELUP_PRISE_PICK;
        for (int i = 0; i < pickAxeData.pickaxeLevel; i++)
        {
            prise *= LEVELUP_RATE_PICK;
        }
        return (int)prise;
    }
    public int GetArmorLevelUpPrise()
    {
        float prise = LEVELUP_PRISE_ARMOR;
        for (int i = 0; i < DataManager.Instance.saveData.playerData.armorData.level; i++)
        {
            prise *= LEVELUP_RATE_ARMOR;
        }
        return (int)prise;
    }
    public int GetArmorRepairValue()
    {
        return armorData.GetArmorMax() - armorData.currentArmor;
    }
    public bool LevelUpThePick(int moneyInHand)
    {
        int prise = GetPickLevelUpPrise();
        if (moneyInHand < prise)
        {
            return false;
        }
        playerEffectSwicher.GameObjectSetActive_True(1);
        pickAxeData.LevelUp();
        DataManager.Instance.saveData.money -= prise;
        DataManager.Instance.saveData.playerData.pickAxeData.Copy(pickAxeData);

        return true;
    }
    public bool LevelUpTheArmor(int moneyInHand)
    {
        int prise = GetArmorLevelUpPrise();
        if (moneyInHand < prise)
        {
            return false;
        }
        playerEffectSwicher.GameObjectSetActive_True(1);
        armorData.LevelUp();
        DataManager.Instance.saveData.money -= prise;
        DataManager.Instance.saveData.playerData.armorData.Copy(armorData);

        return true;
    }
    public void Pick_RepairButtonEvent()
    {
        //直して費用を所持金から引く
        DataManager.Instance.saveData.money -= RepairPick(DataManager.Instance.saveData.money);
    }
    public void Pick_LevelUpButtonEvent()
    {
        if (LevelUpThePick(DataManager.Instance.saveData.money))
        {
            //強化成功エフェクト
        }
        else
        {
            //強化失敗アラート
        }
    }
    public void Armor_RepairButtonEvent()
    {
        DataManager.Instance.saveData.money -= RepairArmor(DataManager.Instance.saveData.money);

    }
    public void Armor_LevelUpButtonEvent()
    {
        if (LevelUpTheArmor(DataManager.Instance.saveData.money))
        {
            //強化成功エフェクト
        }
        else
        {
            //
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (DataManager.Instance.saveData.blackSmithData.pickAutoRepair) RepairPick(DataManager.Instance.saveData.money);
        if (DataManager.Instance.saveData.blackSmithData.armorAutoRepair) RepairArmor(DataManager.Instance.saveData.money);
        if (HomeManager.Instance.progress == (int)TutorialID.GotoBlackSmith) HomeManager.Instance.ProgressNext((int)TutorialID.End);
        ui.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ui.SetActive(false);
    }

}
[System.Serializable]
public class ArmorData
{
    public const int DEFAULT_ARMOR = 3;
    public int level = 1;
    public int currentArmor = DEFAULT_ARMOR;
    public ArmorData() { currentArmor = GetArmorMax(); }
    public int GetArmorMax()
    {
        return DEFAULT_ARMOR + level;
    }

    public void Copy(ArmorData armorData)
    {
        this.level = armorData.level;
        this.currentArmor = armorData.currentArmor;
    }
    public void LevelUp()
    {
        level++;
        currentArmor = GetArmorMax();
    }
}
