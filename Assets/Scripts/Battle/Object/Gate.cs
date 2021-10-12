using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] GameObject l_Gate;
    [SerializeField] GameObject r_Gate;
    [SerializeField] float openSpeed;
    [SerializeField] bool isOpen;
    [SerializeField] int id;
    public int GetID() { return id; }
    // Start is called before the first frame update
    void Start()
    {
        l_Gate.transform.position = transform.position + new Vector3(-1.6f, 0);
        r_Gate.transform.position = transform.position + new Vector3(1.6f, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isOpen)
        {
            l_Gate.transform.position = Vector3.MoveTowards(l_Gate.transform.position, transform.position + new Vector3(-2, 0), openSpeed * Time.deltaTime);
            r_Gate.transform.position = Vector3.MoveTowards(r_Gate.transform.position, transform.position + new Vector3(2, 0), openSpeed * Time.deltaTime);

        }
        else
        {
            l_Gate.transform.position = Vector3.MoveTowards(l_Gate.transform.position, transform.position + new Vector3(-6, 0), openSpeed * Time.deltaTime);
            r_Gate.transform.position = Vector3.MoveTowards(r_Gate.transform.position, transform.position + new Vector3(6, 0), openSpeed * Time.deltaTime);
        }
    }
    public void Open()
    {
        isOpen = true;
    }
}
