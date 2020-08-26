using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaperTP2Behaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    CapsuleCollider2D col;

    float originalGravityScale;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        col = animator.GetComponent<CapsuleCollider2D>();
        rb = animator.GetComponent<Rigidbody2D>();

        //look towards player
        enemy = animator.GetComponent<BasicEnemy>();
        enemy.LookAtPlayer();
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.isKinematic = false;
        col.enabled = true;
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
