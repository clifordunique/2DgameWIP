using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKoboldBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    KoboldAttack attack;

    float basicAttackRange;
    float dashAttackRangeMin;
    float dashAttackRangeMax;
    public float distanceFromPlayer;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        enemy = animator.GetComponent<BasicEnemy>();
        rb = animator.GetComponent<Rigidbody2D>();
        attack = animator.GetComponent<KoboldAttack>();

        basicAttackRange = attack.basicAttackRange;
        dashAttackRangeMin = attack.dashAttackRangeMin;
        dashAttackRangeMax = attack.dashAttackRangeMax;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
        distanceFromPlayer = enemy.getDistanceFormPlayer();

        if (distanceFromPlayer < dashAttackRangeMax && distanceFromPlayer > dashAttackRangeMin)
        {
            animator.SetTrigger("DashAttack");
        }else if (distanceFromPlayer >= dashAttackRangeMax || (distanceFromPlayer <= dashAttackRangeMin && distanceFromPlayer > basicAttackRange))
        {
            enemy.MoveTowardsPlayer();
        }else if (distanceFromPlayer <= basicAttackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
