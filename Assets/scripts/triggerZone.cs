using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
   
    public UnityEvent<GameObject> OnEnterEvent;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Contains("One") || other.gameObject.tag.Contains("Two") || other.gameObject.tag.Contains("Three") || other.gameObject.tag.Contains("Four"))
        {
            OnEnterEvent.Invoke(other.gameObject);
        }
    }
}
