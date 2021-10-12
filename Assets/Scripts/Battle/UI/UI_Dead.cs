using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Dead : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] float showSpeed;
    [SerializeField] bool isActive;
    Player_Auto p;
    public void Start()
    {
        isActive = false;
        TryGetComponent(out canvasGroup);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;

        p = GameObject.FindWithTag("Player").GetComponent<Player_Auto>();
    }
    private void Update()
    {
        if (isActive) return;
        if (p.armorData.currentArmor <= 0)
            StartCoroutine(DoShow());
    }

    IEnumerator DoShow()
    {
        while(canvasGroup.alpha<1)
        {
            canvasGroup.alpha += showSpeed * Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void Return()
    {
        DataManager.Instance.DataDeathReset();
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("Home");
    }
    public void Retry()
    {
        DataManager.Instance.saveData.playerData.armorData.currentArmor = DataManager.Instance.saveData.playerData.armorData.GetArmorMax();
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene("BattleScene0");
    }

}
