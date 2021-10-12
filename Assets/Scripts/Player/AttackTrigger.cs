using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]Player_Auto player;
    private void OnTriggerEnter(Collider other)
    {
        player.Attack(true);
        if (player.target == null) player.target = other.gameObject;
    }
    private void OnTriggerStay(Collider other)
    {
        player.Attack(true);
        if (player.target == null) player.target = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        player.Attack(false);
        player.target = null;
    }
}
