using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTurnCollolider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject firstSpot;

    AnimatorScript _animatorScript;
    void Start()
    {
        _animatorScript = GetComponent<AnimatorScript>();
    }
    public bool firstTurn = false;
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Contains("wizard"))
        {
            _animatorScript.TurnWizard();
            if (!firstTurn)
            {
                firstTurn = true;
                firstSpot.SetActive(false);
            }
        }
    }

    public void resetFirstTurn()
    {
        firstTurn = false;
        firstSpot.SetActive(true);
    }
}
