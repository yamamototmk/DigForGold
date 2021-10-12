using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AutoMine : MonoBehaviour
{
    // Start is called before the first frame update
    public const int ADD_MAXORE = 100;
    public const float ADDRATE = 0.16f;
    public const int LEVELUP_PRISE_UNIT = 300;
    public const float LEVELUP_PRISE_RATE = 1.1f;
    /// <summary>
    /// 経過時間
    /// </summary>
    public double elapsed;
    public int currentOre;
    public int maxOre;
    public Data_Mine saveData;
    public GameObject mineUI;
    public Text oreValueText;


    void Start()
    {

        if (DataManager.Instance.saveData.mineData != null)
        {
            saveData = DataManager.Instance.saveData.mineData;
        }

        if (saveData == null)
        {
            Debug.Log("Mineのデータが無いので新しく生成");
            saveData = new Data_Mine() { level = 1, lastGetTime = DateTime.Now.ToBinary() };
            DataManager.Instance.saveData.mineData = saveData;
            DataManager.Instance.Save();
        }
    }
    public int GetLevelUpPrise()
    {
        float prise = LEVELUP_PRISE_UNIT;
        for (int i = 0; i < saveData.level; i++)
        {
            prise *= LEVELUP_PRISE_RATE;
        }
        return (int)prise;
    }

    public void LevelUp()
    {
        int prise = GetLevelUpPrise();
        if (GetLevelUpPrise() > DataManager.Instance.saveData.money)
        {
            Debug.Log("お金足りない。何か表示だしたい");
            return;
        }
        DataManager.Instance.saveData.money -= prise;
        saveData.level++;
        DataManager.Instance.saveData.mineData = saveData;
    }
    // Update is called once per frame
    void Update()
    {
        DateTime last = DateTime.FromBinary(saveData.lastGetTime);
        DateTime now = DateTime.Now;
        TimeSpan elapsedSpan = now - last;
        elapsed = elapsedSpan.TotalSeconds;
        currentOre = (int)(elapsed * (saveData.level * ADDRATE));
        if(currentOre < 0)
        {
            saveData.lastGetTime = DateTime.Now.ToBinary();
            return;
        }
        maxOre = saveData.level * ADD_MAXORE;
        if (currentOre >= maxOre) currentOre = maxOre;
        oreValueText.text = currentOre.ToString("#,0") + "/" + maxOre.ToString("#,0");
    }
    void GetOre()
    {
        DataManager.Instance.saveData.mineData = saveData;
        if (currentOre < 1) return;
        DataManager.Instance.AddOre(currentOre);
        saveData.lastGetTime = DateTime.Now.ToBinary();
        currentOre = 0;

    }
    private void OnApplicationQuit()
    {
        DataManager.Instance.saveData.mineData = saveData;
        DataManager.Instance.Save();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GetOre();
        mineUI.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        mineUI.SetActive(false);
    }
    IEnumerable AddOre_Update()
    {
        while (true)
        {
            yield return null;
        }
    }
}
