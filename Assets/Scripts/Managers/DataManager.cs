using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//TODO:�Z�[�u����^�C�~���O�l����
/// <summary>
/// �Z�[�u�f�[�^�Ǘ��N���X
/// </summary>
public class DataManager : SingletonMonoBehaviour<DataManager>
{
    public SaveData saveData;
    public OreData tempOreData;
    public UI_AddOre addOreUI;
    private void Awake()
    {
        saveData = PlayerPrefsUtils.GetObject<SaveData>("savedata");
        if (saveData == default)
        {
            Debug.Log("�f�[�^�����݂��܂���B�V�����������܂��B");
            PlayerPrefsUtils.SetObject("savedata", new SaveData());

            saveData = PlayerPrefsUtils.GetObject<SaveData>("savedata");
        }
        Debug.Log("LastPlayDateTime:" + DateTime.FromBinary(saveData.lastPlayDateBinary));
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(autoSave());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("�f�[�^���폜���܂���");
                PlayerPrefs.DeleteKey("savedata");
                saveData = new SaveData();
                Save();
            }
    }
    private void OnApplicationQuit()
    {
        saveData.lastPlayDateBinary = DateTime.Now.ToBinary();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BattleScene0") saveData.oreData = tempOreData;
        Save();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            saveData.lastPlayDateBinary = DateTime.Now.ToBinary();
            Save();
        }
        else
        {
            DateTime last = DateTime.FromBinary(saveData.lastPlayDateBinary);
            DateTime now = DateTime.Now;

            TimeSpan elapsedSpan = now - last;

            DebugLogManager.Instance.AddLog("�ĊJ �o�ߎ���:" + elapsedSpan.TotalSeconds, My_LogType.KEY_DEBUG_LOG, true);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus) return;
        saveData.lastPlayDateBinary = DateTime.Now.ToBinary();
        Save();

    }
    public void Save()
    {
        PlayerPrefsUtils.SetObject("savedata", saveData);
        saveData = PlayerPrefsUtils.GetObject<SaveData>("savedata");
        DebugLogManager.Instance.AddLog("�Z�[�u����", My_LogType.KEY_DEBUG_LOG, true);
    }

    public void AddMoney(int money)
    {
        saveData.money += money;
    }
    public void AddOre(int num = 1)
    {
        saveData.oreData.ore += num;
        addOreUI.AddOre(num, Item.ItemType.common);
    }
    public void AddGold(int num = 1)
    {
        saveData.oreData.gold += 1;
        addOreUI.AddOre(num, Item.ItemType.Gold);

    }
    public void AddSapphire(int num = 1)
    {
        saveData.oreData.sapphire++;
        addOreUI.AddOre(num, Item.ItemType.Sapphire);

    }
    public void AddRuby(int num = 1)
    {
        saveData.oreData.ruby++;
        addOreUI.AddOre(num, Item.ItemType.ruby);

    }
    public void SaveTemp()
    {
        tempOreData = saveData.oreData;
    }
    /// <summary>
    /// ���S���Ɏ擾�����z�΂����Z�b�g
    /// </summary>
    public void DataDeathReset()
    {
        saveData.oreData = tempOreData;
        saveData.playerData.armorData.currentArmor = saveData.playerData.armorData.GetArmorMax();
    }
    public int GetTempOre() { return saveData.oreData.ore - tempOreData.ore; }
    public int GetTempRuby() { return saveData.oreData.ruby - tempOreData.ruby; }
    public int GetTempSapphire() { return saveData.oreData.sapphire - tempOreData.sapphire; }
    public int GetTempGold() { return saveData.oreData.gold - tempOreData.gold; }

    IEnumerator autoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            Save();
            yield return null;
        }
    }

}

[System.Serializable]
public class SaveData
{
    public float playTime;
    /// <summary>
    /// �Q�[���̐i�s�x
    /// </summary>
    public int game_progress = 0;
    public int money;

    /// <summary>
    /// �z��
    /// </summary>
    public OreData oreData;
    public PlayerData playerData;
    /// <summary>
    /// �ō����Blevel
    /// </summary>
    public int arrivalFloor = 0;
    public int level = 0;
    public long lastPlayDateBinary;

    //�{�݃f�[�^
    public Data_Mine mineData;
    public Data_Exchanger exchangerData;
    public Data_BlackSmith blackSmithData;

    //Town�f�[�^
    public long[] townGemLastGetTimes;
    //�_���W�����f�[�^
    public RewordItemData[] rewordItemDatas;//�擾�ς݂̕�V�f�[�^
    public SaveData()
    {
        lastPlayDateBinary = new DateTime().ToBinary();
        playerData = new PlayerData();
        exchangerData = new Data_Exchanger();
        blackSmithData = new Data_BlackSmith();
        oreData = new OreData();
        townGemLastGetTimes = new long[10];
        rewordItemDatas = new RewordItemData[20];
    }

}
[System.Serializable]
public class PlayerData
{
    public PickAxeData pickAxeData;
    public ArmorData armorData;
    public PlayerData() { pickAxeData = new PickAxeData(); armorData = new ArmorData(); }
}
[Serializable]
public class OreData
{
    public int ore;
    public int ruby;
    public int sapphire;
    public int gold;
}

public static class PlayerPrefsUtils
{
    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̏���ۑ����܂�
    /// </summary>
    public static void SetObject<T>(string key, T obj)
    {
        var json = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(key, json);
    }

    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̏���ǂݍ��݂܂�
    /// </summary>
    public static T GetObject<T>(string key)
    {
        var json = PlayerPrefs.GetString(key, "none");
        //�f�[�^���������
        if (json == "none")
        {
            Debug.Log("�f�[�^�����݂��܂���");
            return default;
        }
        var obj = JsonUtility.FromJson<T>(json);
        return obj;
    }
}
[Serializable]
public class RewordItemData
{
    public int hp = 5;
}

