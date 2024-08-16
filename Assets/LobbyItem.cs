using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI NameUI;
    [SerializeField]
    GameObject Tick;
    [SerializeField]
    GameObject Cross;

    [SerializeField]
    Image image;
    [SerializeField]
    TextMeshProUGUI victoryPoint;
    [SerializeField] TextMeshProUGUI rubies;
    [SerializeField] TextMeshProUGUI ratTails;


    public void TrackPlayer(GameObject player)
    {
        player.GetComponent<PlayerData>().Name.OnValueChanged += OnNameChanged;
        player.GetComponent<PlayerData>().isReady.OnValueChanged += IsReadyChanged;
        player.GetComponent<PlayerData>().Colour.OnValueChanged += ColourChanged;

        IsReadyChanged(false, player.GetComponent<PlayerData>().isReady.Value);
        OnNameChanged("", player.GetComponent<PlayerData>().Name.Value);
        ColourChanged("white", player.GetComponent <PlayerData>().Colour.Value);

    }

    private void ColourChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue){

        if (newValue == "Red")
        {
            image.color = Color.red;

        }
        if (newValue == "Yellow")
        {
            image.color = Color.yellow;
        }
        if (newValue == "Purple")
        {
            image.color = Color.magenta;
            
        }
        if (newValue == "Blue")
        {
            image.color = Color.blue;
       
        }
        if (newValue == "Random")
        {
            image.color = Color.white;
        }
    }

    private void IsReadyChanged(bool previousValue, bool newValue)
    {
        if (newValue == true)
        {
            Cross.SetActive(false);
            Tick.SetActive(true);
        }
        if (newValue == false)
        {
            Cross.SetActive(true);
            Tick.SetActive(false);
        }
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        Debug.Log(newValue);
        NameUI.text = newValue.ToString();
    }
}
