using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageContainer : MonoBehaviour
{
    public enum DamageDirection
    {
        Up, Down, Right, Left, Forward
    }
    public enum DamageStrength
    {
        Ligtht, Midium, Heavy
    }
    public DamageDirection direction;
    public DamageStrength strength;
    // Start is called before the first frame update
    public float Damage;
    [SerializeField]UnityEvent enterEvent;
    public void SetStatus(int damage, DamageDirection direction = DamageDirection.Forward, DamageStrength strength = DamageStrength.Midium)
    {
        this.Damage = damage;
        this.direction = direction;
        this.strength = strength;

    }

    private void OnTriggerEnter(Collider other)
    {
        Character character;
        if (other.TryGetComponent(out character))
        {
            character.Damage(this,gameObject);
            enterEvent.Invoke();
        }

    }
}
