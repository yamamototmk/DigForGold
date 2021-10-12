using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AutoMine : Window_UI
{
    [SerializeField] AutoMine autoMine;
    [SerializeField] Text valueText;
    [SerializeField] Text levelUpPriseText;
    public GameObject tutorialObj;
    private void Start()
    {

    }
    public void OnEnable()
    {
        if (HomeManager.Instance.progress == (int)TutorialID.GotoAutoMine)
        {
            tutorialObj.SetActive(true);
        }
        else
        {
            tutorialObj.SetActive(false);
        }
    }
    public void Update()
    {
        valueText.text =
            autoMine.saveData.level + "(+1)\n" +
            autoMine.maxOre + "(+" + AutoMine.ADD_MAXORE + ")\n" +
             autoMine.saveData.level * AutoMine.ADDRATE + "(+" + autoMine.saveData.level * AutoMine.ADDRATE + ")";

        levelUpPriseText.text = autoMine.GetLevelUpPrise().ToString();
    }
    public void DisableButtonEvent()
    {
        if (HomeManager.Instance.progress == 0)
        {
            tutorialObj.SetActive(false);
            HomeManager.Instance.SetProgress(1);
        }
    }
}