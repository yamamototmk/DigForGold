using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Enemy_Golem : Character, IPoolObject
{
    Player_Auto player;
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] float attackDistance0;
    [SerializeField] float attackDistance1;

    [SerializeField] float setDistInterval = 0.6f;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] HpBar hpBar;
    UnityAction action;

    public void Awake()
    {
        defaultPos = transform.position;
        TryGetComponent(out agent);
        TryGetComponent(out animator);
        InitAction += () => { agent.speed = moveSpeed; isDead = false; StartCoroutine(DoSetiDistination()); EnemyManager.Instance.AddEnemyList(this); };
        Init();

        OnDamage += DamageCalc;
        Dead += () => { IsDead(); };
    }
    public void Initialize()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    void Action_Chase()
    {

        if (agent.remainingDistance < attackDistance1)
        {
            Action_Attack(1);
        }
        else if (agent.remainingDistance < attackDistance0)
        {
            Action_Attack(0);
        }
    }
    void Action_Attack(int id)
    {
        animator.SetInteger("AttackID", id);
        animator.SetBool("IsAttack", true);
        agent.speed = 0;
        Quaternion targetRotaion = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, rotateSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {

        action.Invoke();

        if (isDead)
        {
            agent.speed = 0;
            return;
        }

        hpBar.HpBarUpdate(maxHp, currentHp, transform.position);

        //目的地に到達した
        if (agent.remainingDistance <= agent.radius + 0.1f)
        {
            animator.SetBool("IsMove", false);
        }
        else
        {
            animator.SetBool("IsMove", true);

        }

        //プレイヤーまでの距離がattaciDistanceより長い
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance0)
        {
            animator.SetBool("IsAttack", false);
        }
        else
        {
            animator.SetBool("IsAttack", true);
            agent.speed = 0;
            Quaternion targetRotaion = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, rotateSpeed * Time.deltaTime);
        }
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
        
        yield return new WaitForSeconds(2f);

        while (true)
        {
            yield return new WaitForSeconds(setDistInterval);
            agent.SetDestination(player.transform.position);
        }
    }
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
    public void SetDefaultSpeed()
    {
        agent.speed = moveSpeed;
    }
}
