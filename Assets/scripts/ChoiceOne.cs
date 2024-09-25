using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceOne : MonoBehaviour
{


    private TMP_Text _choiceOne;
    private string _choiceOneText;
    void Awake()
    {

        _choiceOne = GetComponent<TMP_Text>();

    }
   public void ChoiceOneText(string choiceOneText)
    {
        _choiceOneText = choiceOneText;
    }

    public void Update()
    {
        if (_choiceOne != null) { _choiceOne.text = _choiceOneText; }
    }
}
