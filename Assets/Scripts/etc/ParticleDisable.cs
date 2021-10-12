using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisable : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Debug.Log("ƒAƒjƒI—¹");
        gameObject.SetActive(false);
    }
}
