using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Player : Character,IMatchTarget
{
    public enum AttackType
    {
        Normal,
        Skill_Tackle
    }
    // Start is called before the first frame update
    Vector3 velocity;
    public Rigidbody m_rig;
    Animator animator;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] My_VirtualPad pad;
    [SerializeField] float dashSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float moveAmount;

    [SerializeField] Transform target;
    Collider targetCollider;
    public bool dontMove;
    void Awake()
    {
        TryGetComponent(out animator);
        if(target!=null)
        target.TryGetComponent(out targetCollider);

        animator.keepAnimatorControllerStateOnDisable = true;

        foreach(var smb in animator.GetBehaviours<MatchPositionSMB>())
        {
            smb.target = this;
        }
        foreach (var smb in animator.GetBehaviours<Player_LookAtTargetSMB>())
        {
            smb.player = this;
        }
    }
    void Start()
    {
        //イベントの登録
        OnDamage += DamageCalc;
        m_rig = GetComponent<Rigidbody>();
    }
    void Update()
    {
        DebugUpdate();
        Move();
        AnimationUpdate();

    }
    private void FixedUpdate()
    {

    }
    private void Move()
    {
        if (dontMove) return;
        moveAmount = pad.GetMoveAmount();//入力移動量

        if (moveAmount < 0.1f)
        {
            m_rig.velocity = Vector3.zero;
            return;//入力ベクトルが極端に小さければ移動しない
        }

        speed = moveAmount < 0.6 ? walkSpeed : dashSpeed; //入力量によって移動速度を変更
        Vector2 direction = pad.GetDirection();//移動ベクトル(正規化済み)
        m_rig.velocity = (new Vector3(direction.x, 0, direction.y) * speed);
        Rotation();
    }
    private void Rotation()
    {
        Vector3 lookAtPos = transform.position + m_rig.velocity;

        transform.LookAt(new Vector3(lookAtPos.x, transform.position.y, lookAtPos.z));
    }
    public void LookAtTarget()
    {
        if (target == null) return;
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }
    private void AnimationUpdate()
    {
        animator.SetFloat("Speed",moveAmount < 0.1f ? 0 : moveAmount);
    }
    public void Attack(int type)
    {
        animator.SetInteger("AttackType", type);
        animator.SetTrigger("Attack");
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z)) Attack((int)AttackType.Normal);
        if (Input.GetKeyDown(KeyCode.X)) Attack((int)AttackType.Skill_Tackle);
    }

    public Vector3 TargetPosition => targetCollider  == null ?new Vector3(Mathf.Infinity,Mathf.Infinity) : targetCollider.ClosestPoint(transform.position);
}
