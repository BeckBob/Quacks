using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinComponenet : MonoBehaviour
{
    [SerializeField]
    private int _coins;

    private chipPoints _chipPoints;
    private TMP_Text _coinsText;

    private void Awake()
    {
        _chipPoints = FindObjectOfType<chipPoints>();
        _coinsText = GetComponent<TMP_Text>();
       
    }

    public void Update()
    {
        _coins = _chipPoints.Coins;
        _coinsText.text = $"= {_coins}";
    }

}
