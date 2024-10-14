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
        if (!go.name.Contains("Dice"))
            if (go.tag.Contains("ghost"))
            {
                GameObject foo = go.transform.parent.gameObject;
                foo.SetActive(false);
                Destroy(foo, 25);
            }
            else {
                go.SetActive(false);
                Destroy(go, 25);
            }
            }

}
