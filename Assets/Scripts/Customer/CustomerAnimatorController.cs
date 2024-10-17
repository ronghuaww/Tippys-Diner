using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimatorController : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {

    }

    public void SetWalkingAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void SetEatingAnimation(bool isEating)
    {
        animator.SetBool("isEating", isEating);
    }
}

