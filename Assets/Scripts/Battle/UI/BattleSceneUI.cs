using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float alpha;
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeDelay;
    [SerializeField] Text floorText;

    [SerializeField] Text oreValue;
    [SerializeField] Text rubyValue;
    [SerializeField] Text sapphireValue;
    [SerializeField] Text goldValue;
    [SerializeField] Text moneyValue;
    [SerializeField] Text floorValue;

    void Start()
    {
        StartCoroutine(DoFadeOut(fadeDelay));
        string floorName = BattleLevelManager.Instance.GetCurrentFloorName();
        floorText.text = floorName;
        floorValue.text = floorName;
    }

    // Update is called once per frame
    void Update()
    {
        oreValue.text = DataManager.Instance.GetTempOre().ToString();
        rubyValue.text = DataManager.Instance.GetTempRuby().ToString();
        sapphireValue.text = DataManager.Instance.GetTempSapphire().ToString();
        goldValue.text = DataManager.Instance.GetTempGold().ToString();
        moneyValue.text = DataManager.Instance.saveData.money.ToString();
    }

    IEnumerator DoFadeOut(float delay = 0)
    {
        Color dColor = floorText.color;
        yield return new WaitForSeconds(delay);
        while (floorText.color.a >= 0.01f)
        {
            alpha = floorText.color.a;
            alpha -= fadeSpeed * Time.deltaTime;
            floorText.color = new Color(dColor.r, dColor.g, dColor.b, alpha);
            yield return null;
        }
        alpha = 0;
    }
}
