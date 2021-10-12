using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IPoolObject
{
    public enum ItemType { Gold, Sapphire, ruby, common, Aid }
    Rigidbody m_rig;
    [SerializeField, Range(0, 10)] float randomX;
    [SerializeField, Range(0, 10)] float yPow;
    [SerializeField, Range(0, 10)] float randomZ;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;
    [SerializeField, Header("追いかけを開始する距離")] float chaseDistance;
    Vector3 v;
    Player_Auto player;
    [SerializeField] ItemType itemType;
    [SerializeField,Header("複数取得判定ないなら1固定")] int oreRandomMax;

    Vector3 initScale;
    [SerializeField]int randNum;
    void Awake()
    {

        if (!TryGetComponent(out m_rig)) Debug.LogError("Rigidbodyが無い");
        initScale = transform.localScale;
    }
    private void Start()
    {
        //Pop();
    }
    public void Pop()
    {
        GameObject.FindWithTag("Player").TryGetComponent(out player);

        v = new Vector3();
        v.x = Random.Range(-randomX, randomX);
        v.y = yPow;
        v.z = Random.Range(-randomZ, randomZ);

        m_rig.AddForce(v, ForceMode.Impulse);
        StartCoroutine(DoChasePlayer());
        m_rig.useGravity = true;

        randNum= Random.Range(1, oreRandomMax);
        transform.localScale = initScale * (1 + randNum * 0.2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //これプレイヤーに書くべきでは
        if (!collision.gameObject.CompareTag("Player")) return;
        switch (itemType)
        {
            case ItemType.common:
                DataManager.Instance.AddOre(randNum);
                break;
            case ItemType.ruby: DataManager.Instance.AddRuby(); break;
            case ItemType.Sapphire: DataManager.Instance.AddSapphire(); break;
            case ItemType.Gold: DataManager.Instance.AddGold(); break;
            case ItemType.Aid:
                Player_Auto player;
                collision.gameObject.TryGetComponent(out player);
                float aidValue = DataManager.Instance.saveData.playerData.armorData.GetArmorMax() * 0.1f;
                player.AidArmor((aidValue < 1 ? 1 : aidValue));
                player.pickAxe.Repair();
                break;
        }

        gameObject.SetActive(false);
    }

    IEnumerator DoChasePlayer()
    {
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            yield return null;

            if (Vector3.Distance(transform.position, player.GetTargetPos().position) > chaseDistance) continue;
            transform.position = Vector3.Lerp(transform.position, player.GetTargetPos().position, moveSpeed * Time.deltaTime);
        }
    }
    void IPoolObject.Initialize()
    {
        Pop();
    }
}
