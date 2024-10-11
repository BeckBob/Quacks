using Oculus.Platform.Models;
using Oculus.Platform;
using System.Collections.Generic;
using System;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine.UI;
using UnityEngine;

public class NetworkConnect : MonoBehaviour
{
    private LobbyCodeText _lobbyCodeText;
    private LobbytextVBscript _lobbyTextVBscript;
    [SerializeField] GameObject _inputLobbyCode;
    private NetworkManager _networkManagerInstance;
    public int players;
    public string hostname;
    private Lobby currentLobby;
    public int maxConnection = 20;
    public UnityTransport transport;

    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _lobbyMenu;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Start()
    {
        _lobbyCodeText = FindObjectOfType<LobbyCodeText>();
        _lobbyTextVBscript = FindObjectOfType<LobbytextVBscript>();
        _networkManagerInstance = NetworkManager.Singleton;
    }

    public async void Create()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
            string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            _lobbyCodeText.SetLobbyCode(newJoinCode);
            _lobbyTextVBscript.SetLobbyCode(newJoinCode);

            transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>
                {
                    { "JOIN_CODE", new DataObject(DataObject.VisibilityOptions.Public, newJoinCode) }
                }
            };

            currentLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby Name", maxConnection, lobbyOptions);
            NetworkManager.Singleton.StartHost();
            players = _networkManagerInstance.ConnectedClients.Count;

            _startMenu.SetActive(false);
            GameManager.Instance.UpdateGameState(GameState.Lobby);
            GetUserName();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to create lobby: " + e);
        }
    }

    public async void Join()
    {
        try
        {
            currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            string relayJoinCode = currentLobby.Data["JOIN_CODE"].Value;

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

            transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();
            _startMenu.SetActive(false);
            players = _networkManagerInstance.ConnectedClients.Count;
            GameManager.Instance.UpdateGameState(GameState.Lobby);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to join lobby: " + e);
        }
    }

    public async void JoinWithCode()
    {
        try
        {
            string inputCode = _inputLobbyCode.GetComponent<InputField>().text;
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(inputCode);

            transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();
            _startMenu.SetActive(false);
            players = _networkManagerInstance.ConnectedClients.Count;
            GameManager.Instance.UpdateGameState(GameState.Lobby);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to join lobby with code: " + e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, playerId);
            _lobbyMenu.SetActive(false);
            _startMenu.SetActive(true);
            _lobbyCodeText.SetLobbyCode("");
            _lobbyTextVBscript.SetLobbyCode("");
            GameManager.Instance.UpdateGameState(GameState.StartMenu);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Failed to leave lobby: " + e);
        }
    }

    private void GetUserName()
    {
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
    }

    private void GetLoggedInUserCallback(Message<User> message)
    {
        if (!message.IsError)
        {
            User user = message.Data;
            hostname = user.OculusID;
        }
        else
        {
            Debug.LogError("Failed to get Oculus user info.");
        }
    }
}

