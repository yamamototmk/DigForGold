using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Character
{
    Player_Auto player;

    public enum Rarity { Normal,Rare,Legendary}
    [SerializeField, Header("ÉåÉAìxäÑçá")] float[] rarityRate;
    [SerializeField] Rarity rarity;
    [SerializeField] Material[] materials;
    MeshRenderer meshRenderer;
    [SerializeField] HpBar hpBar;
    public void Awake()
    {
        TryGetComponent(out meshRenderer);
        switch (rarity)
        {
            case Rarity.Normal: meshRenderer.material = materials[0]; break;
            case Rarity.Rare: meshRenderer.material = materials[1]; break;
            case Rarity.Legendary: meshRenderer.material = materials[2]; break;
        }

        Init();
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
        BattleLevelManager.Instance.AddObj(transform.position);
        string objectName = "";


        if (other.GetComponent<PickAxe>().Dig())
        {
            for (int i = 0; i < (int)f; i++)
            {
                objectName = GetObjectName_Random();
                GameObject ore = ObjectPoolManager.Instance.GetObject(objectName);
                ore.transform.position = transform.position + Vector3.up;
                //ore.GetComponent<Ore_Item>().Pop();
            }
        }
        else
        {
            //âÛÇÍÇƒÇÈéû
            GameObject ore = ObjectPoolManager.Instance.GetObject("Ore_Common");
            ore.transform.position = transform.position + Vector3.up;
        }


    }

    string GetObjectName_Random()
    {
        string name = "Ore_Common";
        float rand = Random.Range(0f, 100f);
        if (rand < rarityRate[0]) name = "Ore_Common";
        else if (rand < rarityRate[1]) name = "Ore_Ruby";
        else if (rand < rarityRate[2]) name = "Ore_Sapphia";
        else name = "Ore_Gold";
        return name;
    }
    private void MyDamageCalc(float damage, GameObject other)
    {
        currentHp--;
        if (currentHp <= 0) ApplyDead();
    }
}
