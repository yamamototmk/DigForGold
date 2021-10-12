using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickGauge : MonoBehaviour
{
    // Start is called before the first frame update
    Player_Auto player_Auto;
    PickAxeData pickAxeData;
    [SerializeField] Image inGauge;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pickAxeData = DataManager.Instance.saveData.playerData.pickAxeData;
        inGauge.fillAmount = (float)pickAxeData.pickaxeDurability / pickAxeData.PickaxeDurabilityMax();
    }
}
