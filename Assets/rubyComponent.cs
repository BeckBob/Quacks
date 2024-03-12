using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class rubyComponenet : MonoBehaviour
{
    [SerializeField]
    private bool _ruby;

    private chipPoints _chipPoints;
    private TMP_Text _rubyText;

    private void Awake()
    {
        _chipPoints = FindObjectOfType<chipPoints>();
        _rubyText = GetComponent<TMP_Text>();

    }

    public void Update()
    {
        _ruby = _chipPoints.RubiesThisRound;
        if (_ruby == false)
        {
            _rubyText.text = "= N";
        }
        if (_ruby == true)
        {
            _rubyText.text = "= Y";
        }
    }

}

