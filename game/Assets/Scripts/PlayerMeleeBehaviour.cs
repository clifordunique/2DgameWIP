using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = animator.GetComponent<PlayerController>();
        player.attacking = true;
        player.speedMod = player.meleeSpeedModValue;    //0.5
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = animator.GetComponent<PlayerController>();
        player.attacking = false;
        player.speedMod = player.defaultSpeedModValue;
    }
}
