using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Basic melee: simplest melee enemy, only walk up and attacks
public class EnemyOgreBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    OgreAttack attack;
    float attackRange;
    private float distanceX;
    private float distanceY;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BasicEnemy>();
        attack = animator.GetComponent<OgreAttack>();
        rb = animator.GetComponent<Rigidbody2D>();

        attackRange = attack.attackRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
        
        distanceX = enemy.DistanceFromPlayerXAxis();
        distanceY = enemy.DistanceFromPlayerYAxis();

        if (distanceX < attackRange && distanceY < 1f)
        {
            animator.SetTrigger("Attack");
        }
        else if (distanceX < attackRange && distanceY > 1f)
        {
            return;
        }
        else
        {
            enemy.MoveTowardsPlayer();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }



}
