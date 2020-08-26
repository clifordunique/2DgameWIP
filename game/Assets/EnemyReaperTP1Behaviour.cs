using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaperTP1Behaviour : StateMachineBehaviour
{
    public float originalScale;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //cannot be attacked, will not move

        animator.GetComponent<CapsuleCollider2D>().enabled = false;
        animator.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

        // so that the collider can be disabled without it falling under the floor/easy way to fix something in place
        animator.GetComponent<Rigidbody2D>().isKinematic = true;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
