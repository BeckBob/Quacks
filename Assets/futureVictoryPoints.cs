using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class futureVictoryPoints : MonoBehaviour
{
    private int _victoryPoints;

    private ChipPoints _chipPoints;
    private TMP_Text _victoryPointText;

    private void Awake()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _victoryPointText = GetComponent<TMP_Text>();

    }

    public void Update()
    {
        _victoryPoints = _chipPoints.FutureVictoryPoints;
        _victoryPointText.text = $"= {_victoryPoints}";
    }
}
