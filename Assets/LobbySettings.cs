using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.Android;
using Unity.Netcode;

public class LobbySettings : MonoBehaviour
{

    [SerializeField] Image readyButton;
    [SerializeField] TMP_Dropdown dropDown;
    private PlayerData _playerData;
    public Color trueColour;
    public Color falseColour;
    private NetworkConnect _networkConnect;
  

    private bool _ready = false;
    private int _playersReady;
    public int numberOfPlayers;



    public void ReadyNow()
    {
        _playerData = FindObjectOfType<PlayerData>();
        _networkConnect = FindObjectOfType<NetworkConnect>();
        _playerData.IsReadyFunction();
        _ready = !_ready;
        numberOfPlayers = _networkConnect.players;
      
        if (_ready)
        {
           readyButton.color = trueColour;
            _playersReady++;
            if (_playersReady == numberOfPlayers)
            {
                GameManager.Instance.UpdateGameState(GameState.FortuneTeller);
            }
        }
        if (!_ready)
        {
            readyButton.color = falseColour;
            _playersReady--;
            if (_playersReady == numberOfPlayers)
            {
                GameManager.Instance.UpdateGameState(GameState.FortuneTeller);
            }
        }
       
    }

    public void ChangeColour()
    {
        _playerData = FindObjectOfType<PlayerData>();
       
        
        int pickedEntryIndex = dropDown.value;

        string colour = dropDown.options[pickedEntryIndex].text;
        _playerData.ChangeColourFunction(colour);

      
    }

    // Update is called once per frame
       
}
