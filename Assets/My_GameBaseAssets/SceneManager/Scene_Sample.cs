using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649
public class Scene_Sample : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image fadeImage;
    void Awake()
    {
        My_SceneManager.Instance.SetFadeAction(canvasFade);
        //My_SceneManager.Instance.SetFadeAction(canvasFade_Fill);
        //My_SceneManager.Instance.LoadScene("Scene_A");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //    My_SceneManager.Instance.LoadScene("Scene_A");
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    My_SceneManager.Instance.LoadScene("Scene_B");
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    My_SceneManager.Instance.LoadScene("Scene_C");

    }
    public void canvasFade()
    {
        canvasGroup.alpha = My_SceneManager.Instance.fadeValue;
    }
    public void canvasFade_Fill()
    {
       fadeImage.fillAmount = My_SceneManager.Instance.fadeValue;
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(10f);
        //My_SceneManager.Instance.LoadScene("Scene_B");
        //yield return new WaitForSeconds(10f);
        //My_SceneManager.Instance.LoadScene("Scene_C");
    }
}
