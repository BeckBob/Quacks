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
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

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


    GameManager gameManager;
    PlayerData playerData;
    GrabIngredient grabIngredient;

    public static NetworkConnect Instance { get; private set; }

    [SerializeField] GameObject purpleConfetti;
    [SerializeField] GameObject blueConfetti;
    [SerializeField] GameObject redConfetti;
    [SerializeField] GameObject yellowConfetti;

    WinnerManager winnerManager;

    AnimatorScript animatorScript;
    ChipPoints chipPoints;

    [SerializeField] GameObject winnerManagerObject;
    [SerializeField] GameObject gameManagerObject;
    [SerializeField] GameObject lobbySettingsObject;
    [SerializeField] GameObject fortuneNumObject;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject lobbyMenu;


    [SerializeField] private GameObject _bigBook;

    private Vector3 playerStartPos;
    public GameObject player;

    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _lobbyMenu;
    


    private async void Awake()
    {
        // Check if there is already an instance of NetworkConnect
        if (Instance != null)
        {
            // If the existing instance is not this, destroy it
            Destroy(Instance.gameObject);
        }

        // Set this instance as the current instance
        Instance = this;

        await UnityServices.InitializeAsync();

          
       
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Start()
    {
        if (transport == null)
        {
            transport = FindObjectOfType<UnityTransport>();
        }


        _lobbyCodeText = FindObjectOfType<LobbyCodeText>();
        _lobbyTextVBscript = FindObjectOfType<LobbytextVBscript>();
        animatorScript = FindObjectOfType<AnimatorScript>();
        playerData = FindObjectOfType<PlayerData>();
        _networkManagerInstance = NetworkManager.Singleton;
        playerStartPos = player.transform.position;
    }

    private bool IsNetworkManagerReadyToHost()
    {
        return NetworkManager.Singleton != null && !NetworkManager.Singleton.IsListening;
    }

    public async void Create()
    {

        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogError("Cannot start Host. A network instance is already running.");
            return;
        }

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
            ActivateNetworkScripts();
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
            ActivateNetworkScripts();
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
            ActivateNetworkScripts();
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

            // Remove the player from the lobby
            await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, playerId);

           
           if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
           {
                //Shutdown the network and transport layers
                ShutdownNetworkAndTransport();
            }
        
            
            await Task.Delay(200);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Debug.Log("Successfully left the lobby and reset network.");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Failed to leave lobby: " + e);
        }
    }

    private void ShutdownNetworkAndTransport()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            DontDestroyOnLoad(lobbySettingsObject);

            NetworkManager.Singleton.Shutdown();

            if (NetworkManager.Singleton != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }
        }

       
    }

    private void ActivateNetworkScripts()
    {
        fortuneNumObject.SetActive(true);
        winnerManagerObject.SetActive(true);
    }



    public void ReplayGame()
    {
        playerData = FindObjectOfType<PlayerData>();
        grabIngredient = FindObjectOfType<GrabIngredient>();
        chipPoints = FindObjectOfType<ChipPoints>();
        gameManager = FindObjectOfType<GameManager>();

        player.transform.position = playerStartPos;
        _bigBook.SetActive(true);
        yellowConfetti.SetActive(false);
        redConfetti.SetActive(false);
        blueConfetti.SetActive(false);
        purpleConfetti.SetActive(false);
        animatorScript.ResetGame();
       

        playerData.ResetGame();
        grabIngredient.ResetGame();
        
        chipPoints.ResetGame();
        gameManager.ResetGame();
        GameManager.Instance.UpdateGameState(GameState.Lobby);
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

