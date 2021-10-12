using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AddOre : MonoBehaviour
{
    [SerializeField] bool isActive;
    [SerializeField] float activeTime;
    [SerializeField] float rarityActiveTime;

    [SerializeField] float disableInterval;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 currentVelocity;
    [SerializeField] float speed;
    [SerializeField] Vector3 defaultScale;
    [SerializeField] Vector3 targetScale;

    [SerializeField] Text text;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 startPosOffset;
    [SerializeField] int counter = 0;

    [SerializeField] Color[] textColors;
    CanvasGroup mainGroup;
    [SerializeField] CanvasGroup rarityGroup;

    [SerializeField] Text rarityText;
    string beforeRarity = "";
    Color beforColor;
    private IEnumerator rarityCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        beforColor = textColors[0];
        defaultScale = transform.localScale;
        DataManager.Instance.addOreUI = this;
        isActive = false;
        counter = 0;

        TryGetComponent(out mainGroup);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Init()
    {

    }
    public void AddOre(int num, Item.ItemType type = Item.ItemType.common)
    {
        activeTime = disableInterval;


        Color color = textColors[0];
        string rearity = "";
        string size = "60";
        switch (type)
        {
            case Item.ItemType.common:
                rearity = beforeRarity == "" ? "" : beforeRarity;
                color = beforColor == textColors[0] ? textColors[0] : beforColor;
                size = "60";
                break;
            case Item.ItemType.ruby:
                color = textColors[1];
                rearity = "RARE";
                size = "60";
                break;
            case Item.ItemType.Sapphire:
                color = textColors[2];
                rearity = "EPIC";
                size = "60";
                break;
            case Item.ItemType.Gold:
                color = textColors[3];
                rearity = "LEGENDARY";
                size = "60";
                break;
        }
        //‘O‰ñ‚©‚çˆá‚¤Rarity‚É‚È‚Á‚½
        if (rearity != "")
        {
            rarityCoroutine = DoShowRarity();
            StopCoroutine(rarityCoroutine);
            StartCoroutine(rarityCoroutine);
        }
        counter += num;
        rarityText.text = rearity;
        rarityText.color = color;
        text.text = "+" + counter;

        beforeRarity = rearity;
        beforColor = color;


        if (!isActive)
        {
            StartCoroutine(DoAdd());
        }

    }
    IEnumerator DoAdd()
    {
        isActive = true;
        currentVelocity = startPosOffset;
        counter = 0;
        transform.position = startPosOffset;
        mainGroup.alpha = 1;
        while (activeTime > 0)
        {
            Vector3 playerPos = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position);
            activeTime -= Time.deltaTime;
            currentVelocity = Vector3.Lerp(currentVelocity, startPosOffset + velocity, speed * Time.deltaTime);
            transform.position = playerPos + currentVelocity;

            transform.localScale = Vector3.Lerp(targetScale, defaultScale, activeTime / (disableInterval * 0.6f));
            yield return null;
        }
        counter = 0;
        mainGroup.alpha = 0;
        isActive = false;
    }
    IEnumerator DoShowRarity()
    {
        rarityActiveTime = disableInterval * 1.5f;
        rarityGroup.alpha = 1;
        Vector3 rarityCurrentVelocity = startPosOffset + Vector3.up * 120;
        rarityGroup.transform.position = rarityCurrentVelocity;
        while (rarityActiveTime > 0)
        {
            Vector3 playerPos = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position);
            rarityActiveTime -= Time.deltaTime;
            rarityCurrentVelocity = Vector3.Lerp(rarityCurrentVelocity, startPosOffset + new Vector3(0, 120) + velocity, speed * Time.deltaTime);
            rarityGroup.transform.position = playerPos + rarityCurrentVelocity;

            yield return null;
        }
        beforeRarity = "";
        beforColor = textColors[0];
        rarityGroup.alpha = 0;
    }
}
