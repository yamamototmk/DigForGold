using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChashExchenger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UI_CashExchanger ui;
    void Start()
    {
        ui.tutorial3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
