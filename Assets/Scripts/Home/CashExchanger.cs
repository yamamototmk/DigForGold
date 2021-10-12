using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashExchanger : MonoBehaviour
{

    //���p������UI���Ɏ������Ă���


    [SerializeField, Header("�z�Δ͈�")] Vector2Int oreRange;
    [SerializeField, Header("����͈�")] Vector2Int goldRange;
    [SerializeField, Header("�T�t�@�C�A�͈�")] Vector2Int sapphireRange;
    [SerializeField, Header("���r�[�͈�")] Vector2Int rubyRange;
    [SerializeField] public string[] rateTag;
    [SerializeField] GameObject UI;

    public Data_Exchanger data;
    void Awake()
    {
        rateTag = new string[4];
        data = DataManager.Instance.saveData.exchangerData;
        RateChange();
    }
    /// <summary>
    /// �בփ��[�g�ύX
    /// </summary>
    public void RateChange()
    {
        data.oreRate = Random.Range(oreRange.x, oreRange.y);
        data.goldRate = Random.Range(goldRange.x, goldRange.y);
        data.sapphireRate = Random.Range(sapphireRange.x, sapphireRange.y);
        data.rubyRate = Random.Range(rubyRange.x, rubyRange.y);
        UpdateTag();

    }
    /// <summary>
    /// �בփ��[�g�ύX�i�����[�g�j
    /// </summary>
    public void RateChangeHighRate()
    {
        data.oreRate = (int)Random.Range(oreRange.y * 0.9f, oreRange.y);
        data.goldRate = goldRange.y;
        data.sapphireRate = (int)Random.Range(sapphireRange.y * 0.9f, sapphireRange.y);
        data.rubyRate = (int)Random.Range(rubyRange.y * 0.9f, rubyRange.y);
        UpdateTag();
    }
    public void UpdateTag()
    {
        rateTag[0] = RateTag(data.oreRate, oreRange.y);
        rateTag[1] = RateTag(data.rubyRate, rubyRange.y);
        rateTag[2] = RateTag(data.sapphireRate, sapphireRange.y);
        rateTag[3] = RateTag(data.goldRate, goldRange.y);
    }
    public string RateTag(int currentRate, int max)
    {
        if (currentRate > max * 0.8) return "(Perfect)";
        if (currentRate > max * 0.6) return "(Good)";
        return "(Bad)";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UI.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UI.SetActive(false);
    }

}
