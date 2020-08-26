using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemyAttackBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BasicEnemy enemy = animator.GetComponent<BasicEnemy>();
        enemy.StopMovement();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
