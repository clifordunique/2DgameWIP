using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaperBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    ReaperAttack attack;
    float attackRange;
    float distanceTeleport;

    private float distanceX;
    private float distanceY;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BasicEnemy>();
        attack = animator.GetComponent<ReaperAttack>();
        rb = animator.GetComponent<Rigidbody2D>();

        attackRange = attack.attackRange;
        distanceTeleport = attack.distanceTeleport;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();

        distanceX = enemy.DistanceFromPlayerXAxis();
        distanceY = enemy.DistanceFromPlayerYAxis();

        if (distanceX < attackRange && distanceY < 3f)
        {
            animator.SetTrigger("Attack");
        }
        else if (distanceY > 3f && enemy.playerTransform.GetComponent<PlayerController>().grounded)
        {
            //tp when player on like a paltform
            animator.SetTrigger("Teleport");
        }
        else if(distanceX > attackRange && distanceX < distanceTeleport)
        {
            enemy.MoveTowardsPlayer();
        }
        else if (distanceX > distanceTeleport)
        {
            Debug.Log(distanceX);
            Debug.Log(distanceY);
            animator.SetTrigger("Teleport");
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Teleport");
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
