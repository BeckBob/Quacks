using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    public static event Action<GameObject> OnPlayerSpawn;
    public static event Action<GameObject> OnPlayerDespawn;


    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Renderer[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnPlayerSpawn?.Invoke(this.gameObject);
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {

                item.enabled = false;
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnPlayerDespawn?.Invoke(this.gameObject);

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
