using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class futureCoin : MonoBehaviour
{
    
    private int _coins;

    private ChipPoints _chipPoints;
    private TMP_Text _coinsText;

    private void Awake()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _coinsText = GetComponent<TMP_Text>();

    }

    public void Update()
    {
        _coins = _chipPoints.FutureCoins;
        _coinsText.text = $"= {_coins}";
    }

}
