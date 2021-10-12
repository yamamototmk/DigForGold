using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem_Home : Character
{
    Player_Auto player;
    public int myId;
    [SerializeField] HpBar hpBar;
    [SerializeField, Header("レア度割合")] float[] reaRate;
    public void Awake()
    {
        Init();
        player = GameObject.FindWithTag("Player").GetComponent<Player_Auto>();
        OnDamage += MyDamageCalc;
        OnDamage += AddOre;
        Dead += () => { currentHp = maxHp; player.target = null; player.Attack(false); gameObject.SetActive(false); };

        DateTime last = DateTime.FromBinary(DataManager.Instance.saveData.townGemLastGetTimes[myId]);
        DateTime now = DateTime.Now;
        TimeSpan elapsedSpan = now - last;
       
        if (elapsedSpan.TotalSeconds < 60 * 5)
        {
            gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        hpBar.HpBarUpdate(maxHp, currentHp, transform.position);
    }
    public void AddOre(float f, GameObject other)
    {
        DataManager.Instance.saveData.townGemLastGetTimes[myId] = DateTime.Now.ToBinary();
        if (other.GetComponent<PickAxe>().Dig())
        {
            for (int i = 0; i < (int)f; i++)
            {
                GameObject ore = ObjectPoolManager.Instance.GetObject(GetObjectName_Random());
                ore.transform.position = transform.position + Vector3.up;
            }
        }
        else
        {
            //ピッケルが壊れてる時は鉱石１つしか出ない
            GameObject ore = ObjectPoolManager.Instance.GetObject("Ore_Common");
            ore.transform.position = transform.position + Vector3.up;
        }



    }
    string GetObjectName_Random()
    {
        string name = "Ore_Common";
        float rand = UnityEngine.Random.Range(0f, 100f);
        if (rand < reaRate[0]) name = "Ore_Common";
        else if (rand < reaRate[1]) name = "Ore_Ruby";
        else if (rand < reaRate[2]) name = "Ore_Sapphia";
        else name = "Ore_Gold";
        return name;
    }
    private void MyDamageCalc(float damage, GameObject other)
    {
        currentHp--;
        if (currentHp <= 0) ApplyDead();
    }

}
