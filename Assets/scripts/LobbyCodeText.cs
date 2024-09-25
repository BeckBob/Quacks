using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyCodeText : NetworkBehaviour
{
    private TMP_Text _lobbyCode;
    private string _code;

 
    void Awake()
    {

        _lobbyCode = GetComponent<TMP_Text>();

    }

    public void SetLobbyCode(string code) {  _code = code;} 

    public void Update()
    {

        _lobbyCode.text = _code;
    }
}
