using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeUIMabager : MonoBehaviour
{
    [SerializeField] Text oreValue;
    [SerializeField] Text rubyValue;
    [SerializeField] Text sapphireValue;
    [SerializeField] Text goldValue;
    [SerializeField] Text moneyValue;
    [SerializeField] Text floorValue;

    // Start is called before the first frame update
    void Start()
    {
        floorValue.text = "Home";
    }

    // Update is called once per frame
    void Update()
    {
        oreValue.text = DataManager.Instance.saveData.oreData.ore.ToString();
        rubyValue.text = DataManager.Instance.saveData.oreData.ruby.ToString();
        sapphireValue.text = DataManager.Instance.saveData.oreData.sapphire.ToString();
        goldValue.text = DataManager.Instance.saveData.oreData.gold.ToString();
        moneyValue.text = DataManager.Instance.saveData.money.ToString();
    }
}
