using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceTwo : MonoBehaviour
{
    private TMP_Text _choiceTwo;
    private string _choiceTwoText;
    void Awake()
    {

        _choiceTwo = GetComponent<TMP_Text>();

    }
    public void ChoiceTwoText(string choiceTwoText)
    {
        _choiceTwoText = choiceTwoText;
        
        Debug.Log(choiceTwoText);
    }

    public void Update()
    {
        if(_choiceTwo == null) { Debug.Log("choiceTwo is null"); }
        if (_choiceTwo != null) { _choiceTwo.text = _choiceTwoText; }
    }

}
