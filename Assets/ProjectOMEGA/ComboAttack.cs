using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : StateMachineBehaviour
{
    bool comboFailed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("attackPressed", false);
        comboFailed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool comboPressValid = animator.GetFloat("comboPressValid") > 0.5f;
        Debug.Log("combo " + comboPressValid);

        if (animator.GetBool("attackPressed"))
        {
            animator.SetBool("attackPressed", false);

            if (comboPressValid && !comboFailed)
            {
                animator.SetTrigger("comboAttack");                
            }
            else
            {
                comboFailed = true;                
            }
        }
    }

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
