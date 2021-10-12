using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandom : MonoBehaviour
{
    [SerializeField] bool rotateX;
    [SerializeField] bool rotateY;
    [SerializeField] bool rotateZ;
    [SerializeField] Vector3 rotation;
    // Start is called before the first frame update
    void Start()
    {
        float x = rotateX ? Random.Range(0f, 360f) : 0;
        float y = rotateY ? Random.Range(0f, 360f) : 0;
        float z = rotateZ ? Random.Range(0f, 360f) : 0;
        rotation = new Vector3(x, y, z);

        transform.Rotate(rotation);
    }


}
