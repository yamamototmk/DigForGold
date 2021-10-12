using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Character : MonoBehaviour
{
    public event Action<float, GameObject> OnDamage;
    public event Action Dead;
    public event Action InitAction;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    public bool isDead = false;
    [SerializeField, Header("アイテムとかの目標座標")] private Transform targetPos;
    public Transform GetTargetPos() { return targetPos; }

    public Vector3 defaultPos;
    /// <summary>
    /// 生成時に呼ぶ(死んだオブジェクトを再生成しないように)
    /// </summary>
    /// <param name="pos"></param>
    public void SetInitPos(Vector3 pos)
    {
        defaultPos = pos;
    }
    protected void Init()
    {
        currentHp = maxHp;
        if (InitAction == null) InitAction += () => { };
        InitAction.Invoke();
    }
    public void ApplyDamage(float damage, GameObject obj = null)
    {
        OnDamage.Invoke(damage, obj);
    }
    public void ApplyDead()
    {
        if (isDead) return;

        Dead += () => { isDead = true; };
        Dead.Invoke();
    }
    public void DamageCalc(float damage, GameObject other)
    {
        currentHp -= damage;
        if (currentHp <= 0) ApplyDead();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    DamageContainer container;
    //    other.TryGetComponent(out container);

    //    if (container == null) return;

    //    ApplyDamage(container.Damage);

    //}
    public void Damage(DamageContainer container, GameObject other)
    {
        ApplyDamage(container.Damage, other);
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }
    public float GetMaxHp()
    {
        return maxHp;
    }

    //エネミー共通
    public void ItemDrop(string poolName)
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject(poolName);
        if (obj == null) return;
        obj.transform.position = this.transform.position;
    }

}
