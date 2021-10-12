using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewordGem : Character
{
    Player_Auto player;

    [SerializeField] HpBar hpBar;
    [SerializeField] float dropCount;
    [SerializeField] string dropPrefabName = "Ore_Gold";
    [SerializeField] int id;
    public void Awake()
    {
        id = (BattleLevelManager.Instance.currentLevel) / 5;
        dropCount = (id+1) * 1.3f;
        Init();
        maxHp = 5;
        currentHp = DataManager.Instance.saveData.rewordItemDatas[id].hp;
        if (currentHp <= 0) gameObject.SetActive(false);

        player = GameObject.FindWithTag("Player").GetComponent<Player_Auto>();
        OnDamage += MyDamageCalc;
        OnDamage += AddOre;
        Dead += () => { currentHp = maxHp; player.target = null; player.Attack(false); gameObject.SetActive(false); };
    }
    private void Update()
    {
        hpBar.HpBarUpdate(maxHp, currentHp, transform.position);
    }
    public void AddOre(float f, GameObject other)
    {
        other.GetComponent<PickAxe>().Dig();
        for (int i = 0; i < (int)dropCount; i++)
        {
            BattleLevelManager.Instance.AddObj(transform.position);
            GameObject ore = ObjectPoolManager.Instance.GetObject(dropPrefabName);
            ore.transform.position = transform.position + Vector3.up;
        }
    }
    private void MyDamageCalc(float damage, GameObject other)
    {
        currentHp--;
        DataManager.Instance.saveData.rewordItemDatas[id].hp = (int)currentHp;
        if (currentHp <= 0) ApplyDead();
    }
}
