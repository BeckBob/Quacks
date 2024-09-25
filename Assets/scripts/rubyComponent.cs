using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class rubyComponenet : MonoBehaviour
{
    
    private bool _ruby;
    [SerializeField] GameObject tickPresent;
    [SerializeField] GameObject crossPresent;

    private ChipPoints _chipPoints;
    
    private void Awake()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        

    }

    public void Update()
    {
        _ruby = _chipPoints.RubiesThisRound;
        if (_ruby == false)
        {
            tickPresent.SetActive(false);
            crossPresent.SetActive(true);
        }
        if (_ruby == true)
        {
            tickPresent.SetActive(true);
            crossPresent.SetActive(false);
        }
    }

}

