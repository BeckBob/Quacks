using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class AnimatorScript : MonoBehaviour
{
   
    Animator animator;

    WizardTurnCollolider turnCollolider;

    public bool isWalking = false;

    [SerializeField] private float speed = 2.0f;

    public Transform player;
   
    [SerializeField] Transform firstWalkSpot;

    public float rotationSpeed = 3.0f;
    public float rotationTolerance = 5.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        turnCollolider = FindObjectOfType<WizardTurnCollolider>();
    }

    public void StartWalking()
    {
        animator.SetBool("isWalking", true);
        isWalking = true;
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
        isWalking = false;
        turnCollolider.resetFirstTurn();
    }

    public void StartTalking(int time)
    {
        animator.SetBool("isTalking", true);
        FunctionTimer.Create(() => StopTalking(), time);
    }

    public void StopTalking()
    {
        animator.SetBool("isTalking", false);

    }

    public void StopDramaticTalking()
    {
        animator.SetBool("isTalkingDramatic", false);
    }
    public void StartDramaticTalking(int time)
    {
        animator.SetBool("isTalkingDramatic", true);
        
        FunctionTimer.Create(() => StopDramaticTalking(), time);
    }

    public void GoodJobAnimation()
    {
        animator.SetTrigger("goodJobTrigger");
    }

    public void TurnWizard()
    {
        transform.rotation *= Quaternion.Euler(0, 90, 0);
    }

    public void TurnToWalk()
    { 
    }

    private void Update()
    {
        if (isWalking)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
           
        };

        if (!isWalking)
        {
            Vector3 playerDirection = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

            // Calculate the angle between the current rotation and the target rotation
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            // Update the Animator parameter to control the turning animation
            animator.SetFloat("TurnSpeed", angle);

            // Rotate the NPC smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Stop rotating if the angle is within the tolerance
            if (angle <= rotationTolerance)
            {
                animator.SetFloat("TurnSpeed", 0f);
            }
        }
    }
}
