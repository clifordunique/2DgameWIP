using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhoulBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    GhoulAttack attack;
    float attackRange;
    private float distanceFromPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BasicEnemy>();
        attack = animator.GetComponent<GhoulAttack>();
        rb = animator.GetComponent<Rigidbody2D>();

        attackRange = attack.attackRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
        enemy.MoveTowardsPlayer();
        distanceFromPlayer = enemy.getDistanceFormPlayer();
        if (distanceFromPlayer < attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }


}
