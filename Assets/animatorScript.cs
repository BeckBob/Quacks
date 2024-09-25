using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
   
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartWalking()
    {
        animator.SetBool("isWalking", true);
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void StartTalking()
    {
        animator.SetBool("isTalking", true);
    }

    public void StopTalking()
    {
        animator.SetBool("isTalking", false);
    }

    public void StopDramaticTalking()
    {
        animator.SetBool("isTalkingDramatic", false);
    }
    public void StartDramaticTalking()
    {
        animator.SetBool("isTalkingDramatic", true);
    }

    public void GoodJobAnimation()
    {
        animator.SetTrigger("goodJobTrigger");
    }
}
