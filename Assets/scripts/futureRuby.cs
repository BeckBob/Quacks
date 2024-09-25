using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class futureRuby : MonoBehaviour
{
    private bool _ruby;
    [SerializeField] GameObject FutureTick;
    [SerializeField] GameObject FutureCross;

    private ChipPoints _chipPoints;
  

    private void Awake()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
     

    }

    public void Update()
    {
        _ruby = _chipPoints.FutureRubiesThisRound;
        if (_ruby == false)
        {
            FutureCross.SetActive(true);
            FutureTick.SetActive(false);
        }
        if (_ruby == true)
        {
            
            FutureCross.SetActive(false);
            FutureTick.SetActive(true);
        }
    }
}
