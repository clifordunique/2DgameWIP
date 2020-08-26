using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBehavior : StateMachineBehaviour
{
    PlayerController player;
    Rigidbody2D body2d;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<PlayerController>();
        body2d = animator.GetComponent<Rigidbody2D>();
        player.staggered = true;
        player.ResetMeleeAttack();
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {    
        player.staggered = false;
        player.speedMod = 1;
        body2d.drag = 1;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
