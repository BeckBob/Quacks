using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Renderer[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {

                item.enabled = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            root.position = VRrigReferences.Singleton.root.position;
            root.rotation = VRrigReferences.Singleton.root.rotation;

            head.position = VRrigReferences.Singleton.head.position;
            head.rotation = VRrigReferences.Singleton.head.rotation;

            leftHand.position = VRrigReferences.Singleton.leftHand.position;
            leftHand.rotation = VRrigReferences.Singleton.leftHand.rotation;

            rightHand.position = VRrigReferences.Singleton.rightHand.position;
            rightHand.rotation = VRrigReferences.Singleton.rightHand.rotation;
        }
    }
}
