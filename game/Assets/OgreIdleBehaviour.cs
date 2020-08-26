using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreIdleBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    OgreAttack attack;
    float attackRange;
    private float distanceX;
    private float distanceY;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BasicEnemy>();
        attack = animator.GetComponent<OgreAttack>();
        rb = animator.GetComponent<Rigidbody2D>();

        attackRange = attack.attackRange;

        if (distanceX < attackRange && distanceY < 1f)
        {
            animator.SetTrigger("Attack");
        }else if (distanceX > attackRange)
        {
            animator.SetBool("Idle", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
