using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : SingletonMonoBehaviour<MyCamera>
{
    // Start is called before the first frame update
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 offset;
    [SerializeField] float moveSpeed;
    float defaultMoveSpeed;
    void Awake()
    {
        base.Awake();
        defaultMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, moveSpeed * Time.deltaTime);
    }
    public void ChangeMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public void ChangeMoveSpeedDefault()
    {
        moveSpeed = defaultMoveSpeed;
    }
    /// <summary>
    /// èâä˙âªÇÃéûÇ…égÇ§
    /// </summary>
    /// <param name="pos"></param>
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public void SetTargetTransform(Transform transform)
    {
        targetTransform = transform;
    }
}
