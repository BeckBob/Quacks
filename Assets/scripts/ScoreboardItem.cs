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

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI RatTails;
    [SerializeField] TextMeshProUGUI Rubies;


    public void TrackPlayer(GameObject player)
    {
        player.GetComponent<PlayerData>().Name.OnValueChanged += OnNameChanged;
        player.GetComponent<PlayerData>().VictoryPoints.OnValueChanged += OnVictoryPointsChanged;
        player.GetComponent<PlayerData>().Colour.OnValueChanged += OnColourChanged;
        player.GetComponent<PlayerData>().RatTails.OnValueChanged += OnRatTailsChanged;
        player.GetComponent<PlayerData>().Rubies.OnValueChanged += OnRubiesChanged;

        OnVictoryPointsChanged(0, player.GetComponent<PlayerData>().VictoryPoints.Value);
        OnNameChanged("", player.GetComponent <PlayerData>().Name.Value);
        OnColourChanged("Random", player.GetComponent<PlayerData>().Colour.Value);
        OnRatTailsChanged(0, player.GetComponent<PlayerData>().RatTails.Value);
        OnRubiesChanged(0, player.GetComponent<PlayerData>().Rubies.Value);

    }

    private void OnVictoryPointsChanged(int previousValue, int newValue)
    {
        VictoryPointUI.text = newValue.ToString();
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
      
        NameUI.text = newValue.ToString();
    }

    private void OnColourChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        switch (newValue.ToString())
        {
            case "Red":
                image.color = Color.red;
                NameUI.color = Color.white;
                VictoryPointUI.color = Color.white;
                Rubies.color = Color.white;
                RatTails.color = Color.white;
                break;
            case "Yellow":
                image.color = Color.yellow;
                break;
            case "Purple":
                image.color = Color.magenta;
                NameUI.color = Color.white;
                VictoryPointUI.color = Color.white;
                Rubies.color = Color.white;
                RatTails.color = Color.white;
                break;
            case "Blue":
                image.color = Color.blue;
                NameUI.color = Color.white;
                VictoryPointUI.color = Color.white;
                Rubies.color = Color.white;
                RatTails.color = Color.white;
                break;
            default:
                Debug.LogWarning($"Unknown color: {newValue}");
                break;
        }
    }
    private void OnRatTailsChanged(int previousValue, int newValue)
    {
        RatTails.text = newValue.ToString();
    }

    private void OnRubiesChanged(int previousValue, int newValue)
    {
        Rubies.text = newValue.ToString();
    }
}
