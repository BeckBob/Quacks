using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cauldron : MonoBehaviour
{

    private void Start()
    {
        GetComponent<TriggerZone>().OnEnterEvent.AddListener(InsideCauldron);
    }


    public void InsideCauldron(GameObject go)
    {
        Destroy(go, 15);
            }

}
