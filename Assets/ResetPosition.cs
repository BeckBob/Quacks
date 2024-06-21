using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    Vector3 originalPosition;
    void Start()
    {
        originalPosition = gameObject.transform.position;
    }

    private void OnDisable()
    {
        transform.position = originalPosition;
    }
}
