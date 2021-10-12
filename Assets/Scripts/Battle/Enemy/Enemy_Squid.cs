using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Squid : MonoBehaviour
{
    // イカ野郎　倒せない　一方的に攻撃してくる。
    Player_Auto player;
    [SerializeField] float attackDistance;
    [SerializeField] float rotateSpeed;
    [SerializeField] Transform muzzleTransform;
    [SerializeField] Animator animator;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player_Auto>();
        TryGetComponent(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        //プレイヤーまでの距離がattaciDistanceより長い
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            animator.SetBool("IsAttack", false);
        }
        else
        {
            animator.SetBool("IsAttack", true);
            Quaternion targetRotaion = Quaternion.LookRotation(player.GetTargetPos().position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, rotateSpeed * Time.deltaTime);
        }
    }
    private void Aim()
    {

        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<Player_Auto>();
        Quaternion targetRotation = Quaternion.LookRotation(player.GetTargetPos().position - muzzleTransform.position);
        muzzleTransform.rotation = targetRotation;
        transform.rotation = targetRotation;
    }

    public void Shot()
    {
        GameObject bubble = ObjectPoolManager.Instance.GetObject("BubbleBall",makeIfEmpty:true);
        bubble.transform.position = muzzleTransform.position;
        bubble.transform.rotation = muzzleTransform.rotation;
        bubble.GetComponent<Arrow>().Initialize();
    }
}
