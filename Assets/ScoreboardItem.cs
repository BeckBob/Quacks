using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using System;
using Unity.Collections;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI NameUI;
    [SerializeField]
    TextMeshProUGUI VictoryPointUI;
   
    
    public void TrackPlayer(GameObject player)
    {
        player.GetComponent<PlayerData>().Name.OnValueChanged += OnNameChanged;
        player.GetComponent<PlayerData>().VictoryPoints.OnValueChanged += OnVictoryPointsChanged;
        
        OnVictoryPointsChanged(0, player.GetComponent<PlayerData>().VictoryPoints.Value);
        OnNameChanged("", player.GetComponent <PlayerData>().Name.Value);

    }

    private void OnVictoryPointsChanged(int previousValue, int newValue)
    {
        VictoryPointUI.text = newValue.ToString();
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
      
        NameUI.text = newValue.ToString();
    }
}
