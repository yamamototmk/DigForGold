using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneGate : MonoBehaviour
{
    [SerializeField] string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        DataManager.Instance.Save();
        My_SceneManager.Instance.LoadScene(sceneName);
    }
}
