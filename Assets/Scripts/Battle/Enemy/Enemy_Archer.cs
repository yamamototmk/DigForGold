using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy_Archer : Character, IPoolObject
{
    Player_Auto player;
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] float attackDistance;
    [SerializeField] float setDistInterval = 1;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] HpBar hpBar;
    [SerializeField] Transform muzzleTransform;
    public void Awake()
    {
        defaultPos = transform.position;
        TryGetComponent(out agent);
        TryGetComponent(out animator);
        player = GameObject.FindWithTag("Player").GetComponent<Player_Auto>();

        InitAction += () => { agent.speed = moveSpeed; isDead = false; StartCoroutine(DoSetiDistination()); EnemyManager.Instance.AddEnemyList(this); };
        Init();

        OnDamage += DamageCalc;
        Dead += () => { IsDead(); ItemDrop(GetRandomDropObjecNamet()); };
    }
    void Update()
    {
        Aim();

        if (isDead)
        {
            agent.speed = 0;
            return;
        }
        //ƒvƒŒƒCƒ„[‚Ü‚Å‚Ì‹——£‚ªattaciDistance‚æ‚è’·‚¢
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            animator.SetBool("IsAttack", false);
        }
        else
        {
            animator.SetBool("IsAttack", true);
            agent.speed = 0;
            Quaternion targetRotaion = Quaternion.LookRotation(player.GetTargetPos().position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, rotateSpeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        hpBar.HpBarUpdate(maxHp, currentHp, transform.position);
    }
    private void IsDead()
    {
        BattleLevelManager.Instance.AddObj(defaultPos);
        isDead = true;
        animator.SetTrigger("Dead");
    }
    public void DeadAnimationEvent()
    {
        player.target = null;
        player.Attack(false);
        gameObject.SetActive(false);
    }
    IEnumerator DoSetiDistination()
    {
        while (true)
        {
            yield return new WaitForSeconds(setDistInterval);
            try
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("Idle", false);
            }
            catch { }
        }
    }

    void IPoolObject.Initialize()
    {
        Init();
    }
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
    public void SetDefaultSpeed()
    {
        agent.speed = moveSpeed;
    }
    void Aim()
    {
        Quaternion targetRotaion = Quaternion.LookRotation(player.GetTargetPos().position - muzzleTransform.position);
        muzzleTransform.rotation = targetRotaion;
    }

    public void Shot()
    {
        GameObject arrow = ObjectPoolManager.Instance.GetObject("Arrow");
        arrow.transform.position = muzzleTransform.position;
        arrow.transform.rotation = muzzleTransform.rotation;
        arrow.GetComponent<Arrow>().Initialize();
    }
    private string GetRandomDropObjecNamet()
    {
        float rand = Random.Range(0f, 100f);
        if (rand > 95) return "Item_Gold";
        if (rand > 50) return "Item_Aid";
        return "";
    }
}
