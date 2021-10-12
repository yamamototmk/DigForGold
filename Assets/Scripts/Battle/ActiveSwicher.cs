using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSwicher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField, Header("ColliderとDamageContainerをアタッチしたオブジェクトを登録")] GameObject[] gameObjects;
    [SerializeField] List<Collider> colls;
    [SerializeField] List<DamageContainer> dc;
    [SerializeField] List<GameObject> go;
    [SerializeField] bool isEnable;
    void Start()
    {
        foreach (GameObject go in gameObjects)
        {
            colls.Add(go.GetComponent<Collider>());
            dc.Add(go.GetComponent<DamageContainer>());
        }

    }
    public void SetDamage(int index, float damage)
    {
        dc[index].Damage = damage;
    }
    public void Enable(int index)
    {
        isEnable = true;
        colls[index].enabled = isEnable;
    }
    public void Disable(int index)
    {
        isEnable = false;
        colls[index].enabled = isEnable;
    }
    public void GameObjectSetActive_True(int i)
    {
        go[i].SetActive(false);
        go[i].SetActive(true);
    }
    public void GameObjectSetActive_False(int i)
    {
        go[i].SetActive(false);
    }
}
