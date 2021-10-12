using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPoolObject
{
    Rigidbody rb;
    Collider coll;
    TrailRenderer trail;
    [SerializeField] float speed;
    [SerializeField] float inactiveTime;
    [SerializeField] bool ifHitDisable;
    void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out coll);
        TryGetComponent(out trail);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.useGravity = true;
        }
        Hit();
    }
    public void Hit()
    {
        rb.velocity = Vector3.zero;
        coll.enabled = false;
        if (ifHitDisable)
            gameObject.SetActive(false);
    }
    public void Initialize()
    {
        transform.parent = null;
        rb.velocity = transform.forward * speed;
        StartCoroutine(DoInactive());
        rb.useGravity = false;
        coll.enabled = true;
        if (trail != null)
            trail.Clear();
    }
    IEnumerator DoInactive()
    {
        yield return new WaitForSeconds(inactiveTime);
        gameObject.SetActive(false);
    }
}
