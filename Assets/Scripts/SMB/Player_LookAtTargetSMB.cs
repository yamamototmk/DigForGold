using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LookAtTargetSMB : StateMachineBehaviour
{
    public Player player;
    [SerializeField] Vector2 effectiveRange;
    [SerializeField] Vector2 dontMoveEffectiveRange;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > effectiveRange.x &&
            stateInfo.normalizedTime < effectiveRange.y)
            player.LookAtTarget();
        if (stateInfo.normalizedTime > dontMoveEffectiveRange.x &&
            stateInfo.normalizedTime < dontMoveEffectiveRange.y)
            player.dontMove = true;
        else
            player.dontMove = false;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.dontMove = false;
    }
}
