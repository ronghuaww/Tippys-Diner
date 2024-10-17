using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {

    }

    public void SetWalkingAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    // public void SetEatingAnimation(bool isEating)
    // {
    //     animator.SetBool("isEating", isEating);
    // }

    public void TriggerPickupAnimation()
    {
        animator.SetTrigger("PickUp");
    }

    public void TriggerPutdownAnimation()
    {
        animator.SetTrigger("PutDown");
    }
}
