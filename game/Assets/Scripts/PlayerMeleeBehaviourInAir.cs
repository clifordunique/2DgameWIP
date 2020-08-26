using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeBehaviourInAir : StateMachineBehaviour
{
    PlayerController player;
    Rigidbody2D rb;
    float originalGravityScale;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<PlayerController>();
        rb = animator.GetComponent<Rigidbody2D>();

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = originalGravityScale / 4f;

        player.attacking = true;

        player.speedMod = player.jumpMeleeSpeedModValue;    //0.5
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.attacking = false;
        player.GetComponent<Rigidbody2D>().gravityScale = 1f;
        player.speedMod = player.defaultSpeedModValue;
        rb.gravityScale = originalGravityScale;
    }
}
