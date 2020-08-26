using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    NecromancerAttack attack;

    float attackRangeMin;
    float attackRangeMax;
    private float distanceX;
    private float distanceY;
    public float summonFrequency;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        enemy = animator.GetComponent<BasicEnemy>();
        rb = animator.GetComponent<Rigidbody2D>();
        attack = animator.GetComponent<NecromancerAttack>();

        attackRangeMin = attack.attackRangeMin;
        attackRangeMax = attack.attackRangeMax;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
        distanceX = enemy.DistanceFromPlayerXAxis();
        distanceY = enemy.DistanceFromPlayerYAxis();

        if (distanceX < attackRangeMax && distanceX > attackRangeMin && distanceY < 3f)
        {
            if (UnityEngine.Random.value <= summonFrequency)
            {
                animator.SetTrigger("Summon");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }
        else if (distanceX >= attackRangeMax)
        {
            enemy.MoveTowardsPlayer();
        }
        else if (distanceX <= attackRangeMin)
        {
            enemy.MoveAwayFromPlayer();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger();
    }
}
