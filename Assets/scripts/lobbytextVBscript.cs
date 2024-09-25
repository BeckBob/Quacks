using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbytextVBscript : MonoBehaviour
{
    private TMP_Text _lobbyCode;
    private string _code;


    void Awake()
    {

        _lobbyCode = GetComponent<TMP_Text>();

    }

    public void SetLobbyCode(string code) { _code = code; }

    public void Update()
    {

        _lobbyCode.text = "Lobby Code: " + _code;
    }
}

