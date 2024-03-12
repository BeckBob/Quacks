using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRrigReferences : MonoBehaviour
{
    public static VRrigReferences Singleton;


    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;


    private void Awake()
    {
        Singleton = this;
    }
}
