using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttack : StateMachineBehaviour {

    public static bool hasHit = false; 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Collider weaponCollider = animator.gameObject.GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<CapsuleCollider>();

        if(hasHit)
        {
            weaponCollider.enabled = false;
            Debug.Log("disabled");
        }
        else
        {
            weaponCollider.enabled = true;
            Debug.Log("enabled");
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Collider weaponCollider = animator.gameObject.GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<CapsuleCollider>();

        //animator.SetBool("IsAttacking", false);

        if (animator.GetBool("AttackQued")==false)
        {
            animator.SetBool("IsAttacking", false);
            animator.applyRootMotion = false;
        }

        animator.SetBool("AttackQued", false);

        weaponCollider.enabled = false;

        hasHit = false;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsAttacking", true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
