using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPositionPurifier : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnDisable()
    {
        transform.rotation = originalRotation;
        transform.position = originalPosition;
    }
}
