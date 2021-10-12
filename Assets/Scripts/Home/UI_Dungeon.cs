using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dungeon : Window_UI
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] Text arrivalText;
    [SerializeField] GameObject tutorialObj;
    private void Start()
    {
        if (HomeManager.Instance.progress == (int)TutorialID.GotoDangeon)
        {
            tutorialObj.SetActive(true);
        }

        int arrivalFloor = DataManager.Instance.saveData.arrivalFloor;
        arrivalText.text = "Arrival Floor:" + (arrivalFloor == 0 ? "F1" : "B" + arrivalFloor);
        foreach (GameObject g in buttons)
        {
            g.SetActive(false);
        }
        int num = (arrivalFloor + 1) / 5;
        for (int i = 0; i <= num; i++)
        {
            buttons[i].SetActive(true);
        }
    }
   
}
