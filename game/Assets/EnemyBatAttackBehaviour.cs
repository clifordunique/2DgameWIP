﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatAttackBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    FlyingEnemy enemy;
    BatAttack attack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponentInParent<Rigidbody2D>();
        enemy = animator.GetComponentInParent<FlyingEnemy>();
        attack = animator.GetComponent<BatAttack>();
        attack.EnableAttackCollider();
        attack.DashAttack();
        rb.drag = attack.attackDrag;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attack.DisableAttackCollider();
        rb.drag = attack.originalDrag;
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
