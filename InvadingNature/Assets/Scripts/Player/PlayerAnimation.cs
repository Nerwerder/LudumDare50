using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject inactiveAxe;
    public GameObject activeAxe;
    public Animator animator;


    const string isChopping = "IsChopping";
    const string isLifting = "IsLifting";
    const string isThrowing = "IsThrowing";

    private void Start() {
        Debug.Assert(inactiveAxe);
        Debug.Assert(activeAxe);
        Debug.Assert(animator);
    }

    public void Lift () {
        animator.SetTrigger(isLifting);
        animator.ResetTrigger(isChopping);
        animator.ResetTrigger(isThrowing);
    }

    public void Drop() {
        animator.ResetTrigger(isLifting);
        animator.ResetTrigger(isChopping);
        animator.ResetTrigger(isThrowing);
    }

    public void Throw() {
        animator.ResetTrigger(isLifting);
        animator.ResetTrigger(isChopping);
        animator.SetTrigger(isThrowing);
    }
}
