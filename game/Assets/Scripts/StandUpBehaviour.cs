using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class StandUpBehaviour : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController player = animator.GetComponent<PlayerController>();
            player.speedMod = player.slideStandUpSpeedMod;    
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController player = animator.GetComponent<PlayerController>();
            player.speedMod = player.defaultSpeedModValue;
        }
    }
