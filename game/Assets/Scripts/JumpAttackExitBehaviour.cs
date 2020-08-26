using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackExitBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = animator.GetComponent<PlayerController>();
        Rigidbody2D body2d = animator.GetComponent<Rigidbody2D>();
        player.speedMod = 0;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = animator.GetComponent<PlayerController>();
        Rigidbody2D body2d = animator.GetComponent<Rigidbody2D>();
        player.attacking = false;
        player.speedMod = player.defaultSpeedModValue;
    }
}
