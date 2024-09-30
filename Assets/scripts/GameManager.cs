using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using System;
using Unity.Services.Lobbies.Models;
using System.Runtime.CompilerServices;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject _fortuneTextPurple;
    [SerializeField] private GameObject _fortuneTextBlue;
    [SerializeField] private GameObject _fortuneTextYellow;
    [SerializeField] private GameObject _fortuneTextRed;

    [SerializeField] private GameObject _purplePresentScore;
    [SerializeField] private GameObject _yellowPresentScore;
    [SerializeField] private GameObject _redPresentScore;
    [SerializeField] private GameObject _bluePresentScore;
    [SerializeField] private GameObject _purpleFutureScore;
    [SerializeField] private GameObject _redFutureScore;
    [SerializeField] private GameObject _blueFutureScore;
    [SerializeField] private GameObject _yellowFutureScore;
    [SerializeField] private GameObject _purpleSphere;
    [SerializeField] private GameObject _redSphere;
    [SerializeField] private GameObject _yellowSphere;
    [SerializeField] private GameObject _blueSphere;
    [SerializeField] private GameObject _purpleCauldronScores;
    [SerializeField] private GameObject _redCauldronScores;
    [SerializeField] private GameObject _blueCauldronScores;
    [SerializeField] private GameObject _yellowCauldronScores;
    [SerializeField] private GameObject _purpleIngredientSphere;
    [SerializeField] private GameObject _yellowIngredientSphere;
    [SerializeField] private GameObject _blueIngredientSphere;
    [SerializeField] private GameObject _redIngredientSphere;
    [SerializeField] private GameObject _LobbySettings;
    [SerializeField] private GameObject _LobbyCodeText;
    [SerializeField] private GameObject _bigBook;
    [SerializeField] private GameObject _purplePlayerSpace;
    [SerializeField] private GameObject _yellowPlayerSpace;
    [SerializeField] private GameObject _BluePlayerSpace;
    [SerializeField] private GameObject _redPlayerSpace;
    private TeleportationManager _teleportationManager;

    AnimatorScript _animationScript;

    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource shopMusic;
    [SerializeField] private AudioSource morningMusic;
    [SerializeField] private AudioSource noonMusic;
    [SerializeField] private AudioSource afternoonMusic;
    [SerializeField] private AudioSource eveningMusic;

    [SerializeField] private GameObject wizardLocation;
    [SerializeField] private GameObject wizardCharacter;

    private FortuneNumber _fortuneNumber;
    private PlayerData _playerData;
    private WinnerManager _winnerManager;
    private BuyIngredients _buyIngredients;
    private ChipPoints _chipPoints;
    private LobbySettings lobbySettings;
  
    fortuneTeller _fortuneTeller;
    public GameState State;

    private int gameRound = 0;

    Vector3 centralSpot;
    

    public static event Action<GameState> OnGameStateChanged;
    void Awake()
    {
        Instance = this;

        _fortuneNumber = FindObjectOfType<FortuneNumber>();
        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        _buyIngredients = FindObjectOfType<BuyIngredients>();
        _chipPoints = FindObjectOfType<ChipPoints>();
        lobbySettings = FindObjectOfType<LobbySettings>();
        _animationScript = FindObjectOfType<AnimatorScript>();
        centralSpot = wizardLocation.transform.position;
    }

    void Start()
    {
        UpdateGameState(GameState.StartMenu);
    }
    public void UpdateGameState(GameState newState) 
    {  
        State = newState;

        switch (newState)
        {
            case GameState.StartMenu:
                HandleStartMenu();
                break;
            case GameState.Lobby:
                HandleLobby();
                break;
            case GameState.FortuneTeller:
                HandleFortune(); 
                break;
            case GameState.PotionMaking:
                HandlePotionMaking();
                break;
            case GameState.RollDice:
                RollDice();
                break;
            case GameState.BuyIngredients:
                BuyIngredients();
                break;
            case GameState.SpendRubies:
                SpendRubies();
                    break;
            case GameState.DeclareWinner:
                DeclareWinner();
                break;
                
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log(State.ToString());

    }

    private async void DeclareWinner()
    {
        await _chipPoints.calculateEndGameExtraPoints();
        _winnerManager.GameWinner();
    }
    private void BuyIngredients()
    {

        if (_playerData.Colour.Value == "Purple")
        {
            _purpleSphere.SetActive(false);

            _purpleIngredientSphere.SetActive(false);
            _purplePresentScore.SetActive(false);
            _purpleFutureScore.SetActive(false);
        }
        if (_playerData.Colour.Value == "Red")
        {
            _redSphere.SetActive(false);

            _redIngredientSphere.SetActive(false);
            _redPresentScore.SetActive(false);
            _redFutureScore.SetActive(false);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            _yellowSphere.SetActive(false);
            _yellowIngredientSphere.SetActive(false);
            _yellowPresentScore.SetActive(false);
            _yellowFutureScore.SetActive(false);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            _blueSphere.SetActive(false);

            _blueIngredientSphere.SetActive(false);
            _bluePresentScore.SetActive(false);
            _blueFutureScore.SetActive(false);
        }
        SetMusicForShop();
        _buyIngredients.SetUpStall();
    }

    private void SpendRubies()
    {
      
        if (_playerData.Colour.Value == "Purple")
        {
            _purpleSphere.SetActive(true);

        }
        if (_playerData.Colour.Value == "Red")
        {
            _redSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            _yellowSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            _blueSphere.SetActive(true);
        }
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.SpendRubiesUI();

    }
    
    private void RollDice()
    {
        
            _winnerManager.RoundWinner();
            _animationScript.StopWalking();
            wizardCharacter.transform.position = centralSpot;

    }

    private void HandleLobby()
    {
        _LobbyCodeText.SetActive(true);
        _LobbySettings.SetActive(true);
        lobbySettings = FindObjectOfType<LobbySettings>();
        lobbySettings.AddRuleDropdowns();
    }

    private void HandleStartMenu()
    {
        
    }

    private async void HandlePotionMaking()
    {
        _animationScript.StartWalking();
        _playerData = FindObjectOfType<PlayerData>();

        _winnerManager.ReadyUp();
        await _winnerManager.CheckAllPlayersReady();
        _winnerManager.ResetReady();

        if (_playerData.Colour.Value == "Purple")
        {
            _purpleSphere.SetActive(true);

            _purpleIngredientSphere.SetActive(true);
            _purplePresentScore.SetActive(true);
            _purpleFutureScore.SetActive(true);
        }
        if (_playerData.Colour.Value == "Red")
        {
            _redSphere.SetActive(true);

            _redIngredientSphere.SetActive(true);
            _redPresentScore.SetActive(true);
            _redFutureScore.SetActive(true);
        }
        if (_playerData.Colour.Value == "Yellow") { 
            _yellowSphere.SetActive(true);
        _yellowIngredientSphere.SetActive(true);
        _yellowPresentScore.SetActive(true);
        _yellowFutureScore.SetActive(true);
    }
        if (_playerData.Colour.Value == "Blue")
        {
            _blueSphere.SetActive(true);
           
            _blueIngredientSphere.SetActive(true);
            _bluePresentScore.SetActive(true);
            _blueFutureScore.SetActive(true);
        }
        _yellowCauldronScores.SetActive(true);
        _blueCauldronScores.SetActive(true);
        _redCauldronScores.SetActive(true);
        _purpleCauldronScores.SetActive(true);
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.SetRules();
        _chipPoints.ResetStuffInBook();
    }

    private void HandleFortune()
    {
        gameRound++;
        SetMusicForRound();
        _fortuneTeller = FindObjectOfType<fortuneTeller>();
        _playerData = FindObjectOfType<PlayerData>();
        NewRoundWiz();
        _purplePlayerSpace.SetActive(true);
        _yellowPlayerSpace.SetActive(true);
        _redPlayerSpace.SetActive(true);
        _BluePlayerSpace.SetActive(true);
     
        
        if (_playerData.Colour.Value == "Purple")
        {
            _fortuneTextPurple.SetActive(true);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            _fortuneTextBlue.SetActive(true);
        }
        if (_playerData.Colour.Value == "Red")
        {
            _fortuneTextRed.SetActive(true);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            _fortuneTextYellow.SetActive(true);
        }

        _fortuneNumber.FortuneNumberGenerator();
        
        
    }

    private void NewRoundWiz()
    {
        wizardCharacter.transform.position = centralSpot;

        _animationScript.StartDramaticTalking(3);

        //do if statements for which sound bite to use for round 1, 2, etc.
    }

    private void SetMusicForRound()
    {
        shopMusic.Stop();
        if (gameRound == 1 || gameRound == 2)
        {
            menuMusic.Stop();
            morningMusic.Play();
        }
        if (gameRound == 3 || gameRound == 4)
        {
            
            noonMusic.Play();
        }
        if (gameRound == 5 || gameRound == 6)
        {
            
            afternoonMusic.Play();
        }
        if (gameRound == 7 || gameRound == 8)
        {
            
            eveningMusic.Play();
        }
    }

    private void SetMusicForShop()
    {
        if (gameRound == 1 || gameRound == 2)
        {
            morningMusic.Stop();
        }
        if (gameRound == 3 || gameRound == 4)
        {
            noonMusic.Stop();
        }
        if (gameRound == 5 || gameRound == 6)
        {
            afternoonMusic.Stop();
        }
        if (gameRound == 7)
        {
            eveningMusic.Stop();
        }
        shopMusic.Play();
    }
}



public enum GameState
{
    StartMenu,
    Lobby,
    FortuneTeller,
    PotionMaking,
    RollDice,
    BuyIngredients,
    SpendRubies,
    DeclareWinner
}
