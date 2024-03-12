using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryPointComponenet : MonoBehaviour
{
    [SerializeField]
    private int _victoryPoints;

    private chipPoints _chipPoints;
    private TMP_Text _victoryPointText;

    private void Awake()
    {
        _chipPoints = FindObjectOfType<chipPoints>();
        _victoryPointText = GetComponent<TMP_Text>();

    }

    public void Update()
    {
        _victoryPoints = _chipPoints.VictoryPoints;
        _victoryPointText.text = $"= {_victoryPoints}";
    }

}
