using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_CashExchanger : Window_UI
{
    [SerializeField] CashExchanger cashExchenger;
    [SerializeField] Text totalSales;
    [SerializeField] Text rateText_Head;
    [SerializeField] Text[] rateText_In;
    [SerializeField] Text[] cum;
    [SerializeField] GameObject tutorial1;
    [SerializeField] GameObject tutorial2;
    [SerializeField] public GameObject tutorial3;


    // Start is called before the first frame update
    void Start()
    {
        Init();
        RateUpdate();

    }
    private void OnEnable()
    {
        if (HomeManager.Instance.progress == (int)TutorialID.ChashExchenger_PushSeleBtn)
        {
            cashExchenger.RateChangeHighRate();
            tutorial1.SetActive(true);
        }
        else if (HomeManager.Instance.progress == (int)TutorialID.CashExchenger_PushDisavleBtn)
        {
            HomeManager.Instance.progress = (int)TutorialID.GotoDangeon;
        }
        Init();
    }
    public void Init()
    {
        totalSales.text = "0";
        BatchSelection();
    }
    public void RateUpdate()
    {
        rateText_Head.text = cashExchenger.data.oreRate + cashExchenger.rateTag[0] + "\n" +
            cashExchenger.data.rubyRate + cashExchenger.rateTag[1] + "\n" +
            cashExchenger.data.sapphireRate + cashExchenger.rateTag[2] + "\n" +
            cashExchenger.data.goldRate + cashExchenger.rateTag[3];

        rateText_In[0].text = cashExchenger.data.oreRate + "\n" + cashExchenger.rateTag[0];
        rateText_In[1].text = cashExchenger.data.rubyRate + "\n" + cashExchenger.rateTag[1];
        rateText_In[2].text = cashExchenger.data.sapphireRate + "\n" + cashExchenger.rateTag[2];
        rateText_In[3].text = cashExchenger.data.goldRate + "\n" + cashExchenger.rateTag[3];

        totalSales.text = GetTotalSales().ToString();

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Tutorial1_Btn()
    {
        if (HomeManager.Instance.progress == (int)TutorialID.ChashExchenger_PushSeleBtn)
        {
            HomeManager.Instance.progress = (int)TutorialID.CashExchenger_PushDisavleBtn;
            tutorial1.SetActive(false);
            tutorial3.SetActive(true);
        }
    }
    public void Tutorial3_Btn()
    {
        if (HomeManager.Instance.progress == (int)TutorialID.CashExchenger_PushDisavleBtn)
        {
            HomeManager.Instance.SetProgress((int)TutorialID.GotoDangeon);
            tutorial1.SetActive(false);
            tutorial3.SetActive(false);
        }
    }

    /// <summary>
    /// �����z�ΑS�I��
    /// </summary>
    public void BatchSelection()
    {
        OreData oreData = DataManager.Instance.saveData.oreData;
        int[] units =
        {
            oreData.ore,
            oreData.ruby,
            oreData.sapphire,
            oreData.gold,
        };
        for (int i = 0; i < cum.Length; i++)
        {
            cum[i].text = "x" + units[i].ToString();
        }
        totalSales.text = GetTotalSales().ToString();

    }
    /// <summary>
    /// ���p
    /// </summary>
    public void Sale()
    {
        OreData oreData = DataManager.Instance.saveData.oreData;

        
        //���z�ǉ�
        DataManager.Instance.AddMoney(GetTotalSales());

        //�z�Δ��p
        oreData.ore = 0;
        //���r�[
        oreData.ruby = 0;
        //�T�t�@�C�A
        oreData.sapphire = 0;
        //�S�[���h
        oreData.gold = 0;

        //���炵���z���f�[�^�𔽉f
        DataManager.Instance.saveData.oreData = oreData;
        DataManager.Instance.Save();
        Init();
    }
    /// <summary>
    /// �����p���i��Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetTotalSales()
    {
        OreData oreData = DataManager.Instance.saveData.oreData;
        int[] units =
        {
            oreData.ore,
            oreData.ruby,
            oreData.sapphire,
            oreData.gold,
        };
        int oreNum = units[0];
        int rubyNum = units[1];
        int sapphireNum = units[2];
        int goldNum = units[3];

        int sales = 0;
        sales += cashExchenger.data.oreRate * oreNum;
        sales += cashExchenger.data.rubyRate * rubyNum;
        sales += cashExchenger.data.sapphireRate * sapphireNum;
        sales += cashExchenger.data.goldRate * goldNum;
        return sales;
    }
}
