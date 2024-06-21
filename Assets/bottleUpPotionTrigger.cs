using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottleUpPotionTrigger : MonoBehaviour
{

    public ChipPoints chipPoints;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("potionBottle"))
        { 
            Debug.Log("bottled up");
            chipPoints.EndRoundSafely();
            gameObject.SetActive(false);
        }
    }
}
