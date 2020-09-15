using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBossBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    BasicEnemy enemy;
    NecromancerMasterAttack attack;
    float attackRange;
    private float distanceX;
    private float distanceY;

    readonly float startSpellcaspCooldown = 5.0f;
    public float spellcastCooldown = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BasicEnemy>();
        attack = animator.GetComponent<NecromancerMasterAttack>();
        rb = animator.GetComponent<Rigidbody2D>();

        attackRange = attack.meleeAttackRange;
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
        }else if (distanceX > attackRange && spellcastCooldown <= 0)
        {
            animator.SetTrigger("SpellCast");
            spellcastCooldown = startSpellcaspCooldown;
        }
        else
        {
            enemy.MoveTowardsPlayer();
            spellcastCooldown -= Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
