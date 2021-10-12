using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BlackSmith : Window_UI
{
    [SerializeField] BlackSmith blackSmith;
    [SerializeField] Text pick_levelUpPrise;
    [SerializeField] Text pick_RepairPrise;
    [SerializeField] Text armor_levelUpPrise;
    [SerializeField] Text armor_RepairPrise;
    [SerializeField] Toggle pickToggle;
    [SerializeField] Toggle armorToggle;

    [SerializeField] Text pickStatus;
    [SerializeField] Text armorStatus;

    private void OnEnable()
    {
        pickToggle.isOn = DataManager.Instance.saveData.blackSmithData.pickAutoRepair;

        armorToggle.isOn = DataManager.Instance.saveData.blackSmithData.armorAutoRepair;

    }
    private void Update()
    {
        pick_levelUpPrise.text = blackSmith.GetPickLevelUpPrise().ToString();
        pick_RepairPrise.text = blackSmith.GetPickRepairPrise().ToString();

        armor_levelUpPrise.text = blackSmith.GetArmorLevelUpPrise().ToString();
        armor_RepairPrise.text = blackSmith.GetArmorRepairPrise().ToString();

        PickAxeData axeData = DataManager.Instance.saveData.playerData.pickAxeData;
        ArmorData armorData = DataManager.Instance.saveData.playerData.armorData;
        pickStatus.text = axeData.pickaxeLevel + 1 + "\n" +
                            (1 + axeData.pickaxeLevel * PickAxe.PICK_DAMAGERATE) + "\n" +
                            axeData.PickaxeDurabilityMax();

        armorStatus.text = armorData.level + "\n" +
                            armorData.GetArmorMax();

    }

    public void PickToggleEvent()
    {
        DataManager.Instance.saveData.blackSmithData.pickAutoRepair = pickToggle.isOn;
        Debug.Log(pickToggle.isOn);
    }
    public void ArmorToggleEvent()
    {
        DataManager.Instance.saveData.blackSmithData.armorAutoRepair = armorToggle.isOn;
    }
}
