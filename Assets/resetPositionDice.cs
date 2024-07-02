using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPositionDice : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;
    void Start()
    {
        originalPosition = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;
    }

    private void OnEnable()
    {
        originalPosition = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;
    }
    private void OnDisable()
    {
        transform.rotation = originalRotation;
        transform.position = originalPosition;
    }
}
