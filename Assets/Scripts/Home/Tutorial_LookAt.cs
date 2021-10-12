using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_LookAt : MonoBehaviour
{
    [SerializeField] Transform camera_LookAtTransform;
    [SerializeField] int progress;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoCameraControll());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoCameraControll()
    {
        yield return new WaitForSeconds(1.5f);

        MyCamera.Instance.SetTargetTransform(camera_LookAtTransform);
        yield return new WaitForSeconds(2f);
        MyCamera.Instance.SetTargetTransform(GameObject.FindWithTag("Player").transform);
    }
    public void ProgressNext()
    {
        if (HomeManager.Instance.progress == progress)
            HomeManager.Instance.SetProgress(progress + 1);
    }
}
