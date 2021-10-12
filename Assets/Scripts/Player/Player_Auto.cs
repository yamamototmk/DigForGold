using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Player_Auto : Character
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
    [SerializeField] My_VirtualPad pad;
    [SerializeField] float dashSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float moveAmount;

    public bool dontMove;
    [SerializeField] public GameObject target;
    [SerializeField] float rotateSpeed;
    [SerializeField] HpBar hpBar;

    public PlayerData playerData;
    public ArmorData armorData;
    public PickAxe pickAxe;
    ActiveSwicher swicher;
    void Awake()
    {
        UpdateStatus();
        TryGetComponent(out animator);
        TryGetComponent(out swicher);
        armorData = new ArmorData();
        armorData.Copy(DataManager.Instance.saveData.playerData.armorData);
        if (armorData.currentArmor == 0)
        {
            armorData.currentArmor = 1;
            DataManager.Instance.saveData.playerData.armorData.Copy(armorData);
        }
    }
    void Start()
    {
        //イベントの登録
        OnDamage += MyDamageCalc;
        Dead += () => { };
        m_rig = GetComponent<Rigidbody>();
    }
    void Update()
    {
        AnimationUpdate();
        armorData.Copy(DataManager.Instance.saveData.playerData.armorData);
        hpBar.HpBarUpdate(armorData.GetArmorMax(), armorData.currentArmor, transform.position);

        if (isDead) return;
        Move();

    }
    void MyDamageCalc(float damage, GameObject other)
    {
        armorData.Copy(DataManager.Instance.saveData.playerData.armorData);
        armorData.currentArmor -= (int)damage;
        if (armorData.currentArmor <= 0)
        {
            armorData.currentArmor = 0;
            speed = 0;
            m_rig.velocity = Vector3.zero;
            animator.SetTrigger("IsDead");
            ApplyDead();
        }
        DataManager.Instance.saveData.playerData.armorData.Copy(armorData);
    }
    public void AidArmor(float aid)
    {
        armorData.currentArmor += (int)aid;
        swicher.GameObjectSetActive_True(2);
        if (armorData.currentArmor > armorData.GetArmorMax()) armorData.currentArmor = armorData.GetArmorMax();
        Debug.Log("AID:" + aid);
        DataManager.Instance.saveData.playerData.armorData.Copy(armorData);
    }
    /// <summary>
    /// 装備の更新等変化があった時に呼ぶ
    /// </summary>
    private void UpdateStatus()
    {
        playerData = DataManager.Instance.saveData.playerData;


    }
    private void Move()
    {
        if (dontMove) return;
        moveAmount = pad.GetMoveAmount();//入力移動量

        if (moveAmount < 0.1f)
        {
            //ターゲットがいればそっちを向く
            //if (target != null)
            //{
            //    Vector3 lookPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            //    Quaternion targetRotaion = Quaternion.LookRotation(target.transform.position - transform.position);
            //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, rotateSpeed * Time.deltaTime);
            //}
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

    private void AnimationUpdate()
    {
        animator.SetFloat("Speed", moveAmount < 0.1f ? 0 : moveAmount);
    }
    public void Attack(bool flg)
    {
        animator.SetBool("IsAttack", flg);
    }

}

