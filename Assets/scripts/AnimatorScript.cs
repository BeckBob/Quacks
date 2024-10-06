using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

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

    public bool isWalkingToTurn = false;

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

    public async Task TurnToWalk()
    {
       isWalkingToTurn = true;
        await Task.Delay(10 * 1000);
        isWalkingToTurn = false;
    }

    private void Update()
    {
        if (isWalking)
        {
            // Translate only on the XZ plane (horizontal movement)
            Vector3 moveDirection = Vector3.back * Time.deltaTime * speed;
            moveDirection.y = 0;  // Ensure no vertical movement
            transform.Translate(moveDirection);
        }

        if (!isWalking && !isWalkingToTurn)
        {
            Vector3 playerDirection = transform.position - player.position; 
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

            // Calculate the angle between the current rotation and the target rotation
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            // Rotate the NPC smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Stop rotating if the angle is within the tolerance
            
        }
        if (isWalkingToTurn)
        {
            Vector3 spotDirection = transform.position - firstWalkSpot.position;
            spotDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(spotDirection);


            float angle = Quaternion.Angle(transform.rotation, targetRotation);


            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
