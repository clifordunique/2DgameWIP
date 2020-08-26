using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = animator.GetComponent<PlayerController>();
        Rigidbody2D body2d = animator.GetComponent<Rigidbody2D>();
        player.attacking = true;
    }

}
