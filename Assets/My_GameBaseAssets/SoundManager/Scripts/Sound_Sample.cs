using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Sample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BGM_FadeTest());
        StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator test(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlayJungle("Notice");
        yield return new WaitForSeconds(3f);
        //SoundManager.Instance.PlaySe("Heal", Vector3.zero);
        //yield return new WaitForSeconds(3f);
        //SoundManager.Instance.PlaySe("Heal", new Vector3(-10,0,0));
        //yield return new WaitForSeconds(3f);
        //SoundManager.Instance.PlaySe("Heal", new Vector3(10, 0, 0));
        //yield return new WaitForSeconds(3f);
        //SoundManager.Instance.PlaySe("Heal", new Vector3(0, 0, 10));
        //yield return new WaitForSeconds(3f);
        //SoundManager.Instance.PlaySe("Heal", new Vector3(0, 0, -10));
        yield return new WaitForSeconds(3f);

    }
    IEnumerator BGM_FadeTest()
    {
        SoundManager.Instance.PlayBGMWithFade("Dysipe");
        yield return new WaitForSeconds(5f);
        SoundManager.Instance.PlayBGMWithFade("Fepu");
        yield return new WaitForSeconds(5f);
        SoundManager.Instance.StopBGMWithFade(2f);

    }
}
