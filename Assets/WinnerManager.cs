using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class WinnerManager : NetworkBehaviour
{

    public NetworkVariable<int> PurpleVictoryPoints = new NetworkVariable<int>();
    public NetworkVariable<int> RedVictoryPoints = new NetworkVariable<int>();
    public NetworkVariable<int> BlueVictoryPoints = new NetworkVariable<int>();
    public NetworkVariable<int> YellowVictoryPoints = new NetworkVariable<int>();

    public NetworkVariable<int> PurplePoints = new NetworkVariable<int>();
    public NetworkVariable<int> RedPoints = new NetworkVariable<int>();
    public NetworkVariable<int> BluePoints = new NetworkVariable<int>();
    public NetworkVariable<int> YellowPoints = new NetworkVariable<int>();

    public NetworkVariable<int> RoundWinnerScore = new NetworkVariable<int>();
    public NetworkVariable<int> ratTailTopScore = new NetworkVariable<int>();
    public NetworkVariable<int> GameWinnerScore = new NetworkVariable<int>();

    public NetworkVariable<int> PurpleMoths = new NetworkVariable<int>();
    public NetworkVariable<int> RedMoths = new NetworkVariable<int>();
    public NetworkVariable<int> BlueMoths = new NetworkVariable<int>();
    public NetworkVariable<int> YellowMoths = new NetworkVariable<int>();
    public NetworkVariable<int> MothWinningNumber = new NetworkVariable<int>();

    public NetworkVariable<int> ReadyPlayers = new NetworkVariable<int>();

    public NetworkVariable<bool> YellowExists = new NetworkVariable<bool>();
    public NetworkVariable<bool> BlueExists = new NetworkVariable<bool>();
    public NetworkVariable<bool> RedExists = new NetworkVariable<bool>();
    public NetworkVariable<bool> PurpleExists = new NetworkVariable<bool>();

    public int round = 0;


    private NetworkConnect _networkConnect;
    private PlayerData _playerData;
    private ChipPoints _chipPoints;
    private int numberOfPlayers;
    private int MothNumbersBeaten;
    private bool TwoPlayerDraw = false;

    [SerializeField] GameObject PurpleRoundWinCanvas;
    [SerializeField] GameObject RedRoundWinCanvas;
    [SerializeField] GameObject BlueRoundWinCanvas;
    [SerializeField] GameObject YellowRoundWinCanvas;
    [SerializeField] GameObject PurpleFutureCanvas;
    [SerializeField] GameObject RedFutureCanvas;
    [SerializeField] GameObject BlueFutureCanvas;
    [SerializeField] GameObject YellowFutureCanvas;

    [SerializeField] GameObject RedDice;
    [SerializeField] GameObject BlueDice;
    [SerializeField] GameObject YellowDice;
    [SerializeField] GameObject PurpleDice;
    [SerializeField] GameObject DiceFloor;

    [SerializeField] GameObject aboveCauldronUI;
    [SerializeField] GameObject aboveCauldronButton;
    [SerializeField] TextMeshProUGUI aboveCauldronText;
    [SerializeField] TextMeshProUGUI winnerText;

    List<string> winners = new();
    // Start is called before the first frame update
    void Start()
    {

        _networkConnect = FindObjectOfType<NetworkConnect>();
        YellowExists.Value = false;
        BlueExists.Value = false;
        RedExists.Value = false;
        PurpleExists.Value = false;
       
    }

    public async Task CalculateRatTails()
    {
        Debug.Log("Calculating Rat Tails");
        _playerData = FindObjectOfType<PlayerData>();
        // figure out who has the highest score and then get them to roll dice and initiate dice function
        if (_playerData.Colour.Value == "Red")
        {
            RedVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            RedVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Blue")
        {
            RedVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Purple")
        {
            RedVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyUp();
        }
        await CheckAllPlayersReady();
        RatTailsCalculator();
    }
    // Update is called once per frame
    public void RoundWinner()
    {
        _playerData = FindObjectOfType<PlayerData>();
        // figure out who has the highest score and then get them to roll dice and initiate dice function
        if (_playerData.Colour.Value == "Red")
        {
            RedPoints.Value = _playerData.Score.Value;
            RedExists.Value = true;
            ReadyPlayers.Value += 1;
            RoundWinnerDecider();
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            YellowPoints.Value = _playerData.Score.Value;
            ReadyPlayers.Value += 1;
            YellowExists.Value = true;
            RoundWinnerDecider();

        }
        if (_playerData.Colour.Value == "Blue")
        {
            BluePoints.Value = _playerData.Score.Value;
            ReadyPlayers.Value += 1;
            BlueExists.Value = true;
            RoundWinnerDecider();
        }
        if (_playerData.Colour.Value == "Purple")
        {
            PurplePoints.Value = _playerData.Score.Value;
            ReadyPlayers.Value += 1;
            PurpleExists.Value = true;
            RoundWinnerDecider();
        }
    }

    private async void RatTailsCalculator()
    {
        
        ResetReady();
        int RatTailsToAdd = 0;
        ratTailTopScore.Value = new[] { RedVictoryPoints.Value, BlueVictoryPoints.Value, YellowVictoryPoints.Value, PurpleVictoryPoints.Value }.Max();

        if (_playerData.VictoryPoints.Value == ratTailTopScore.Value) { _playerData.RatTails.Value = 0; }
        else
        {
            for (int i = 0; i < ratTailTopScore.Value; i++)
            {
                if (i == 11|| i == 8|| i == 5|| i==2) { RatTailsToAdd++; }
                else if (i % 2 == 0) {  RatTailsToAdd++; }
            }
            _playerData.RatTails.Value = RatTailsToAdd;
            
        }

        await Task.CompletedTask;

    }

    private async void RoundWinnerDecider()
    {
        await CheckAllPlayersReady();
        round += 1;
        ResetReady();
        RoundWinnerScore.Value = new[] { RedPoints.Value, BluePoints.Value, YellowPoints.Value, PurplePoints.Value }.Max();
        Debug.Log(RoundWinnerScore.Value);
        if (RoundWinnerScore.Value == RedPoints.Value)
        {
            RedFutureCanvas.SetActive(false);
            RedRoundWinCanvas.SetActive(true);
            RedDice.SetActive(true);
            if (_playerData.Colour.Value == "Red")
            {
                DiceFloor.SetActive(true);
            }
            // UI announcing they won - in book and above head?
            //instantiate dice in front of them and add whatever it lands on to their player Data
        }
        if (RoundWinnerScore.Value == YellowPoints.Value)
        {
            YellowFutureCanvas.SetActive(false);
            YellowRoundWinCanvas.SetActive(true);
            YellowDice.SetActive(true);
            if (_playerData.Colour.Value == "Yellow")
            {
                DiceFloor.SetActive(true);
            }
        }
        if (RoundWinnerScore.Value == BluePoints.Value)
        {
            BlueFutureCanvas.SetActive(false);
            BlueRoundWinCanvas.SetActive(true);
            BlueDice.SetActive(true);
            if (_playerData.Colour.Value == "Blue")
            {
                DiceFloor.SetActive(true);
            }
        }
        if (RoundWinnerScore.Value == PurplePoints.Value)
        {
            PurpleFutureCanvas.SetActive(false);
            PurpleRoundWinCanvas.SetActive(true);
            PurpleDice.SetActive(true);
            if (_playerData.Colour.Value == "Purple")
            {
                DiceFloor.SetActive(true);
            }
            else
            {
                ReadyUp();
                await CheckAllPlayersReady();
              
                ResetReady();
                _chipPoints.CheckMoths();
            }
            

        }

    }
    private async void GameWinnerDecider()
    {
        await CheckAllPlayersReady();
        
        ResetReady();
        GameWinnerScore.Value = new[] { RedVictoryPoints.Value, BlueVictoryPoints.Value, YellowVictoryPoints.Value, PurpleVictoryPoints.Value }.Max();
        Debug.Log(GameWinnerScore.Value);
        if (GameWinnerScore.Value == RedVictoryPoints.Value)
        {
            //add name of red player to array
            //leave lobby button on book.
            if (_playerData.Colour.Value == "Red")
            {
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
            }
            // UI announcing they won - in book and above head?
            //instantiate dice in front of them and add whatever it lands on to their player Data
        }
        if (GameWinnerScore.Value == YellowVictoryPoints.Value)
        {
            
            if (_playerData.Colour.Value == "Yellow")
            {
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
            }
        }
        if (GameWinnerScore.Value == BlueVictoryPoints.Value)
        {
           
            if (_playerData.Colour.Value == "Blue")
            {
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
            }
        }
        if (GameWinnerScore.Value == PurpleVictoryPoints.Value)
        {
           
            if (_playerData.Colour.Value == "Purple")
            {
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
            }
        }

    }

    public void ShowWinnerUI()
    {
        if (winners.Count == 1)
        {
            winnerText.text = $"{winners[0]} IS THE WINNER!";
        }
        if (winners.Count == 2)
        {
            winnerText.text = $"IT'S A DRAW! {winners[0]} AND {winners[1]} ARE THE WINNERS!";
        }
        if (winners.Count == 3)
        {
            winnerText.text = $"IT'S A DRAW! {winners[0]} AND {winners[1]} AND {winners[2]} ARE THE WINNERS!";
        }
        if (winners.Count == 4)
        {
            winnerText.text = $"How did you all get the same points? what a pointless game";
        }
    }
    public void ReadyUp()
    {
        ReadyPlayers.Value += 1;
    }

    public void ResetReady()
    {
        ReadyPlayers.Value = 0;
    }

    public async Task CheckAllPlayersReady()
    {
        numberOfPlayers = _networkConnect.players;
        while (ReadyPlayers.Value < numberOfPlayers)
        {
            Debug.Log("all players ARE NOT ready");

            await Task.Yield();
        }
        Debug.Log("players READY");
       
    }
    public void GameWinner()
    {
        _playerData = FindObjectOfType<PlayerData>();
        // figure out who has the highest score and then get them to roll dice and initiate dice function
        if (_playerData.Colour.Value == "Red")
        {
            RedVictoryPoints.Value = _playerData.VictoryPoints.Value;
            RedExists.Value = true;
            ReadyPlayers.Value += 1;
            GameWinnerDecider();
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            YellowVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyPlayers.Value += 1;
            YellowExists.Value = true;
            GameWinnerDecider();

        }
        if (_playerData.Colour.Value == "Blue")
        {
            BlueVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyPlayers.Value += 1;
            BlueExists.Value = true;
            GameWinnerDecider();
        }
        if (_playerData.Colour.Value == "Purple")
        {
            PurpleVictoryPoints.Value = _playerData.VictoryPoints.Value;
            ReadyPlayers.Value += 1;
            PurpleExists.Value = true;
            GameWinnerDecider();
        }
        // person with highest victory points - have a trophy or something rise up from their pot - dancing wizard and shop person - confetti?
    }

    public async void AmountOfMoths(int amount)
    {

        if (_playerData.Colour.Value == "Red")
        {
            RedMoths.Value = amount;
            ReadyPlayers.Value += 1;
            await CheckAllPlayersReady();
            MothEffects(amount);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            YellowMoths.Value = amount;
            ReadyPlayers.Value += 1;
            await CheckAllPlayersReady();
            MothEffects(amount);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            BlueMoths.Value = amount;
            ReadyPlayers.Value += 1;
            await CheckAllPlayersReady();
            MothEffects(amount);
        }
        if (_playerData.Colour.Value == "Purple")
        {
            PurpleMoths.Value = amount;
            ReadyPlayers.Value += 1;
            await CheckAllPlayersReady();
            MothEffects(amount);
        }
    }

    public void MothEffects(int amount)
    {
        ResetReady();
        _chipPoints = FindAnyObjectByType<ChipPoints>();
        if (_chipPoints.hawkMothRule == 1)
        {
            if (_networkConnect.players == 3)
            {


                if (_playerData.Colour.Value == "Red")
                {
                    if (YellowExists.Value && amount > YellowMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (BlueExists.Value && amount > BlueMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (PurpleExists.Value && amount > PurpleMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    AddMothEffects();
                }
                if (_playerData.Colour.Value == "Yellow")
                {
                    if (RedExists.Value && amount > RedMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (BlueExists.Value && amount > BlueMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (PurpleExists.Value && amount > PurpleMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    AddMothEffects();
                }
                if (_playerData.Colour.Value == "Blue")
                {
                    if (YellowExists.Value && amount > YellowMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (RedExists.Value && amount > RedMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (PurpleExists.Value && amount > PurpleMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    AddMothEffects();
                }
                if (_playerData.Colour.Value == "Purple")
                {
                    if (YellowExists.Value && amount > YellowMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (BlueExists.Value && amount > BlueMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (RedExists.Value && amount > RedMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    AddMothEffects();

                }

            }
            if (_networkConnect.players == 4)
            {


                if (_playerData.Colour.Value == "Red")
                {
                    if (YellowExists.Value && amount > YellowMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (BlueExists.Value && amount > BlueMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                 
                    AddMothEffects();
                }
                if (_playerData.Colour.Value == "Yellow")
                {
                    if (RedExists.Value && amount > RedMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                  
                    if (PurpleExists.Value && amount > PurpleMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    AddMothEffects();
                }
                if (_playerData.Colour.Value == "Blue")
                {

                    if (RedExists.Value && amount > RedMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (PurpleExists.Value && amount > PurpleMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    AddMothEffects();
                }
                    if (_playerData.Colour.Value == "Purple")
                    {
                        if (YellowExists.Value && amount > YellowMoths.Value)
                        {
                            MothNumbersBeaten++;
                        }
                        if (BlueExists.Value && amount > BlueMoths.Value)
                        {
                            MothNumbersBeaten++;
                        }
                       
                        AddMothEffects();
                    }
                

            }
           

        }
        if (_chipPoints.hawkMothRule == 2)
        {
            if (_playerData.Colour.Value == "Red")
            {
                if (YellowExists.Value && amount > YellowMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (YellowExists.Value && amount == YellowMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                if (BlueExists.Value && amount > BlueMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (BlueExists.Value && amount == BlueMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                if (PurpleExists.Value && amount > PurpleMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (PurpleExists.Value && amount == PurpleMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                AddMothEffects();
            }
            if (_playerData.Colour.Value == "Yellow")
            {
                if (RedExists.Value && amount > RedMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (RedExists.Value && amount == RedMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                if (BlueExists.Value && amount > BlueMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (BlueExists.Value && amount == BlueMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                if (PurpleExists.Value && amount > PurpleMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (PurpleExists.Value && amount == PurpleMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                AddMothEffects();
            }
            if (_playerData.Colour.Value == "Blue")
            {
                if (YellowExists.Value && amount > YellowMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (YellowExists.Value && amount == YellowMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                if (RedExists.Value && amount > RedMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (BlueExists.Value && amount == BlueMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                if (PurpleExists.Value && amount > PurpleMoths.Value)
                {
                    MothNumbersBeaten++;
                }
                if (PurpleExists.Value && amount == PurpleMoths.Value)
                {
                    TwoPlayerDraw = true;
                }
                AddMothEffects();
                if (_playerData.Colour.Value == "Purple")
                {
                    if (YellowExists.Value && amount > YellowMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (YellowExists.Value && amount == YellowMoths.Value)
                    {
                        TwoPlayerDraw = true;
                    }
                    if (BlueExists.Value && amount > BlueMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (BlueExists.Value && amount == BlueMoths.Value)
                    {
                        TwoPlayerDraw = true;
                    }
                    if (RedExists.Value && amount > RedMoths.Value)
                    {
                        MothNumbersBeaten++;
                    }
                    if (BlueExists.Value && amount == BlueMoths.Value)
                    {
                        TwoPlayerDraw = true;
                    }
                    AddMothEffects();
                }
            }
        }
    }


    public async void AddMothEffects()
    {
        _chipPoints = FindAnyObjectByType<ChipPoints>();
        if (_chipPoints.hawkMothRule == 1 && MothNumbersBeaten == 2)
        {
            aboveCauldronUI.SetActive(true);
            aboveCauldronButton.SetActive(true);
            aboveCauldronText.text = "You have more moths in your pot than both your neighbours, you get a droplet and a ruby";
            _playerData.Rubies.Value++;
            _chipPoints.AddDroplet();
            await _chipPoints.CheckWhichChoice();
        }
        if (_chipPoints.hawkMothRule == 1 && MothNumbersBeaten == 1)
        {
            aboveCauldronUI.SetActive(true);
            aboveCauldronButton.SetActive(true);
            aboveCauldronText.text = "You have more moths in your pot than ONE of your neighbours, you get a droplet";
            _chipPoints.AddDroplet();
       
            await _chipPoints.CheckWhichChoice();
            // UI saying they have more moths than one of their neighbours and get a droplet
        }

        if (_chipPoints.hawkMothRule == 2 && MothNumbersBeaten == 1)
        {
            aboveCauldronUI.SetActive(true);
            aboveCauldronButton.SetActive(true);
            aboveCauldronText.text = "You have more moths in your pot than your neighbours, you get a droplet and a ruby";
            _playerData.Rubies.Value++;
            _chipPoints.AddDroplet();
            await _chipPoints.CheckWhichChoice();
            // UI saying they have more moths than their neighbours and get a ruby and a droplet
        }
        if (_chipPoints.hawkMothRule == 2 && TwoPlayerDraw)
        {
            aboveCauldronUI.SetActive(true);
            aboveCauldronButton.SetActive(true);
            aboveCauldronText.text = "You have same amount of moths in your pot as your neighbour, you get one droplet";
            _chipPoints.AddDroplet();
            await _chipPoints.CheckWhichChoice();
            // UI saying they have more moths than their neighbours and get a ruby and a droplet
        }
        //reset all values
        ResetAllMothValues();
        return;
    }
    public void ResetAllMothValues()
    {
        TwoPlayerDraw = false;
        MothNumbersBeaten = 0;
        PurpleMoths.Value = 0;
        BlueMoths.Value = 0;
        RedMoths.Value = 0;
        YellowMoths.Value = 0;
    }



    }


