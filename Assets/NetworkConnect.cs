using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Oculus.Platform;
using Oculus.Platform.Models;





public class NetworkConnect : MonoBehaviour
{

    private LobbyCodeText _lobbyCodeText;
    private LobbytextVBscript _lobbyTextVBscript;
    [SerializeField] GameObject _inputLobbyCode;
    private NetworkManager _networkManagerInstance;
    public int players;
    public string hostname;


    // Start is called before the first frame update

    private void Start()
    {
        _lobbyCodeText = FindObjectOfType<LobbyCodeText>();
        _lobbyTextVBscript = FindObjectOfType<LobbytextVBscript>();
        _networkManagerInstance = NetworkManager.Singleton;

    }
    [SerializeField] private GameObject _startMenu;
    public int maxConnection = 20;
    public UnityTransport transport;
   


    private Lobby currentLobby;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync(); 
    }
    public async void Create()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
        string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        
        
       

        _lobbyCodeText.SetLobbyCode(newJoinCode);
        _lobbyTextVBscript.SetLobbyCode(newJoinCode);

        transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
        
        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
        lobbyOptions.IsPrivate = false;
        lobbyOptions.Data = new Dictionary<string, DataObject>();
        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, newJoinCode);
        lobbyOptions.Data.Add("JOIN_CODE", dataObject);

        currentLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby Name", maxConnection, lobbyOptions);
       

    NetworkManager.Singleton.StartHost();
        GetUserName();
        players = _networkManagerInstance.ConnectedClients.Count;
        _startMenu.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.Lobby);
    }

    public async void Join()
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

    public async void JoinWithCode()
    {

        Debug.Log(_inputLobbyCode.GetComponent<InputField>().text);
        string InputCode = _inputLobbyCode.GetComponent<InputField>().text;
        
        string relayJoinCode = InputCode;

        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

        transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
        allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

        NetworkManager.Singleton.StartClient();
        players = _networkManagerInstance.ConnectedClients.Count;
        _startMenu.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.Lobby);
    }

    private void GetUserName()
    {
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
    }


    private void GetLoggedInUserCallback(Message message)
    {
        if (!message.IsError)
        {
            User user = message.GetUser();
            string userId = user.OculusID;


            hostname = userId;
        }
        if (message.IsError)
        {
            Debug.Log("Cannot get user info");
            return;
        }


    }
}

