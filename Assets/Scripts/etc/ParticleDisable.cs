using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisable : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Debug.Log("�A�j���I��");
        gameObject.SetActive(false);
    }
}
