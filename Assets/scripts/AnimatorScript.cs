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

    private void Update()
    {
        if (isWalking)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
           
        };
    }
}
