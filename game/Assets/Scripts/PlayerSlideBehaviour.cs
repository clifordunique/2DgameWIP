using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideBehaviour : StateMachineBehaviour
{
    readonly float originalDrag = 1f;
    readonly float slidingDrag = 5.5f;

    PlayerController player;
    Rigidbody2D body2d;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<PlayerController>();
        body2d = animator.GetComponent<Rigidbody2D>();
        player.sliding = true;
        body2d.drag = slidingDrag;

        player.ChangeColliderForSlide();

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.sliding = false;
        body2d.drag = originalDrag;
        player.RevertCollider();
        Debug.Log("SLIDE");
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
