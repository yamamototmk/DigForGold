using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tutorial_GetOreFromGem : MonoBehaviour
{
    [SerializeField] Gem_Home gem;
    // Start is called before the first frame update
    void Start()
    {
        gem.Dead += () => { HomeManager.Instance.ProgressNext((int)TutorialID.GotoAutoMine); };
        gem.OnDamage += SetLastTime;
    }
    public void SetLastTime(float f,GameObject obj)
    {
        DataManager.Instance.saveData.townGemLastGetTimes[0] = DateTime.MinValue.ToBinary();
    }
}
