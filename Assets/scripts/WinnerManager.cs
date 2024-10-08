using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

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

    private NetworkVariable<int> PurpleRubies = new NetworkVariable<int>();
    private NetworkVariable<int> RedRubies = new NetworkVariable<int>();
    private NetworkVariable<int> BlueRubies = new NetworkVariable<int>();
    private NetworkVariable<int> YellowRubies = new NetworkVariable<int>();

    private NetworkVariable<int> PurpleDrawnIngredients = new NetworkVariable<int>();
    private NetworkVariable<int> RedDrawnIngredients = new NetworkVariable<int>();
    private NetworkVariable<int> BlueDrawnIngredients = new NetworkVariable<int>();
    private NetworkVariable<int> YellowDrawnIngredients = new NetworkVariable<int>();

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

    public int round = 1;

    public NetworkVariable<int> ToadstallRule = new NetworkVariable<int>();
    public NetworkVariable<int> SpiderRule = new NetworkVariable<int>();
    public NetworkVariable<int> CrowskullRule = new NetworkVariable<int>();
    public NetworkVariable<int> MandrakeRule = new NetworkVariable<int>();
    public NetworkVariable<int> MothRule = new NetworkVariable<int>();
    public NetworkVariable<int> GhostsbreathRule = new NetworkVariable<int>();

    public NetworkVariable<bool> redExploded = new();
    public NetworkVariable<bool> yellowExploded = new();
    public NetworkVariable<bool> blueExploded = new();
    public NetworkVariable<bool> purpleExploded = new();


    private NetworkConnect _networkConnect;
    private PlayerData _playerData;
    private ChipPoints _chipPoints;
    private GrabIngredient _grabIngredient;
    private AnimatorScript _animatorScript;

    private int numberOfPlayers;
    private int MothNumbersBeaten;
    private bool TwoPlayerDraw = false;

    public int LowestVictoryPoints;
    public int LowestRubies;
    public int LowestDrawn;

    [SerializeField] GameObject PurpleRoundWinCanvas;
    [SerializeField] GameObject RedRoundWinCanvas;
    [SerializeField] GameObject BlueRoundWinCanvas;
    [SerializeField] GameObject YellowRoundWinCanvas;
    [SerializeField] GameObject PurpleFutureCanvas;
    [SerializeField] GameObject RedFutureCanvas;
    [SerializeField] GameObject BlueFutureCanvas;
    [SerializeField] GameObject YellowFutureCanvas;

    [SerializeField] GameObject purpleConfetti;
    [SerializeField] GameObject redConfetti;
    [SerializeField] GameObject blueConfetti;
    [SerializeField] GameObject yellowConfetti;

    [SerializeField] GameObject RedDice;
    [SerializeField] GameObject BlueDice;
    [SerializeField] GameObject YellowDice;
    [SerializeField] GameObject PurpleDice;
    [SerializeField] GameObject DiceFloor;

    [SerializeField] GameObject aboveCauldronUIPurple;
    [SerializeField] GameObject aboveCauldronButtonPurple;
    [SerializeField] TextMeshProUGUI aboveCauldronTextPurple;


    [SerializeField] GameObject aboveCauldronUIRed;
    [SerializeField] GameObject aboveCauldronButtonRed;
    [SerializeField] TextMeshProUGUI aboveCauldronTextRed;


    [SerializeField] GameObject aboveCauldronUIBlue;
    [SerializeField] GameObject aboveCauldronButtonBlue;
    [SerializeField] TextMeshProUGUI aboveCauldronTextBlue;

   


    [SerializeField] GameObject aboveCauldronUIYellow;
    [SerializeField] GameObject aboveCauldronButtonYellow;
    [SerializeField] TextMeshProUGUI aboveCauldronTextYellow;
    [SerializeField] TextMeshProUGUI winnerText;


    List<string> winners = new();
    // Start is called before the first frame update
    void Start()
    {

        _networkConnect = FindObjectOfType<NetworkConnect>();
        _animatorScript = FindObjectOfType<AnimatorScript>();
        YellowExists.Value = false;
        BlueExists.Value = false;
        RedExists.Value = false;
        PurpleExists.Value = false;
        ToadstallRule.Value = 1;
        SpiderRule.Value = 1;
        CrowskullRule.Value = 1;
        MandrakeRule.Value = 1;
        MothRule.Value = 1;
        GhostsbreathRule.Value = 1;
        redExploded.Value = false;
        purpleExploded.Value = false;
        yellowExploded.Value = false;
        blueExploded.Value = false;

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
    public async Task CalculateLowestVictoryPoints()
    {
        Debug.Log("Calculating lowest Victory Points");

        // Find PlayerData component
        _playerData = FindObjectOfType<PlayerData>();
        if (_playerData == null)
        {
            Debug.LogError("PlayerData not found!");
            return;
        }

        string colourValue = _playerData.Colour.Value.ToString();

        switch (colourValue)
        {
            case "Red":
                RedVictoryPoints.Value = _playerData.VictoryPoints.Value;
                break;
            case "Yellow":
                YellowVictoryPoints.Value = _playerData.VictoryPoints.Value;
                break;
            case "Blue":
                BlueVictoryPoints.Value = _playerData.VictoryPoints.Value;
                break;
            case "Purple":
                PurpleVictoryPoints.Value = _playerData.VictoryPoints.Value;
                break;
            default:
                Debug.LogWarning("Unexpected player color: " + colourValue);
                return;
        }
        ReadyUp();

        // Await other async methods
        await CheckAllPlayersReady();
        ResetReady();

        // Calculate the lowest victory points
        int[] victoryPoints = {
            RedVictoryPoints.Value,
            YellowVictoryPoints.Value,
            BlueVictoryPoints.Value,
            PurpleVictoryPoints.Value
        };

        // Ensure we have valid points before calculating minimum
        if (victoryPoints.All(vp => vp >= 0))
        {
            LowestVictoryPoints = victoryPoints.Min();
        }
        else
        {
            Debug.LogError("Victory points array contains invalid data.");
        }

        Debug.Log("Lowest Victory Points: " + LowestVictoryPoints);
    }

    public async Task CalculateLowestRubies()
    {
        Debug.Log("Calculating lowest Victory Points");
        _playerData = FindObjectOfType<PlayerData>();
        // figure out who has the highest score and then get them to roll dice and initiate dice function
        if (_playerData.Colour.Value == "Red")
        {
            RedRubies.Value = _playerData.Rubies.Value;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            YellowRubies.Value = _playerData.Rubies.Value;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Blue")
        {
            BlueRubies.Value = _playerData.Rubies.Value;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Purple")
        {
            PurpleRubies.Value = _playerData.Rubies.Value;
            ReadyUp();
        }
        await CheckAllPlayersReady();
        ResetReady();

        LowestRubies = new[] { RedRubies.Value, BlueRubies.Value, YellowRubies.Value, PurpleRubies.Value }.Min();
    }

    public async Task CalculateLowestDrawnIngredients()
    {
        Debug.Log("Calculating lowest drawn Victory points");
        _playerData = FindObjectOfType<PlayerData>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        // figure out who has the highest score and then get them to roll dice and initiate dice function
        if (_playerData.Colour.Value == "Red")
        {
            RedDrawnIngredients.Value = _grabIngredient.totalOfFortuneIngredients;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            YellowDrawnIngredients.Value = _grabIngredient.totalOfFortuneIngredients;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Blue")
        {
            BlueDrawnIngredients.Value = _grabIngredient.totalOfFortuneIngredients;
            ReadyUp();
        }
        if (_playerData.Colour.Value == "Purple")
        {
            PurpleDrawnIngredients.Value = _grabIngredient.totalOfFortuneIngredients;
            ReadyUp();
        }
        await CheckAllPlayersReady();
        ResetReady();

        LowestDrawn = new[] { RedDrawnIngredients.Value, BlueDrawnIngredients.Value, YellowDrawnIngredients.Value, PurpleDrawnIngredients.Value }.Min();
    }

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
        round++;
        Debug.Log(round);
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
      
        ResetReady();
        RoundWinnerScore.Value = new[] { RedPoints.Value, BluePoints.Value, YellowPoints.Value, PurplePoints.Value }.Max();
        Debug.Log(RoundWinnerScore.Value);
        _chipPoints = FindObjectOfType<ChipPoints>();

        if (RedExists.Value && RoundWinnerScore.Value == RedPoints.Value)
        {
            
            RedDice.SetActive(true);
            if (_playerData.Colour.Value == "Red")
            {
                _animatorScript.GoodJobAnimation();
                RedFutureCanvas.SetActive(false);
                RedRoundWinCanvas.SetActive(true);
                DiceFloor.SetActive(true);
                await CheckAllPlayersReady();

                ResetReady();
                _chipPoints.CheckMoths();
            }
            // UI announcing they won - in book and above head?
            //instantiate dice in front of them and add whatever it lands on to their player Data
        }
        if (YellowExists.Value && RoundWinnerScore.Value == YellowPoints.Value)
        {
        
            YellowDice.SetActive(true);
            if (_playerData.Colour.Value == "Yellow")
            {
                _animatorScript.GoodJobAnimation();
                YellowFutureCanvas.SetActive(false);
                YellowRoundWinCanvas.SetActive(true);
                DiceFloor.SetActive(true);
                await CheckAllPlayersReady();

                ResetReady();
                _chipPoints.CheckMoths();
            }
 
        }
        if (BlueExists.Value && RoundWinnerScore.Value == BluePoints.Value)
        {
         
            BlueDice.SetActive(true);
            if (_playerData.Colour.Value == "Blue")
            {
                _animatorScript.GoodJobAnimation();
                BlueFutureCanvas.SetActive(false);
                BlueRoundWinCanvas.SetActive(true);
                DiceFloor.SetActive(true);
                await CheckAllPlayersReady();

                ResetReady();
                _chipPoints.CheckMoths();
            }
        }
        if (PurpleExists.Value && RoundWinnerScore.Value == PurplePoints.Value)
        {
           
            PurpleDice.SetActive(true);
            if (_playerData.Colour.Value == "Purple")
            {
                _animatorScript.GoodJobAnimation();
                PurpleFutureCanvas.SetActive(false);
                PurpleRoundWinCanvas.SetActive(true);
                DiceFloor.SetActive(true);
                await CheckAllPlayersReady();

                ResetReady();
                _chipPoints.CheckMoths();
            }
     
            

        }
        else
        {
            ReadyUp();
            await CheckAllPlayersReady();

            ResetReady();
            _chipPoints.CheckMoths();
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
            redConfetti.SetActive(true);
            if (_playerData.Colour.Value == "Red")
            {
                _animatorScript.GoodJobAnimation();
                aboveCauldronUIRed.SetActive(true);
                aboveCauldronTextRed.text = "YOU WON!";
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
            }
            else
            {
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
                ShowWinnerUI();
            }
           
        }
        if (GameWinnerScore.Value == YellowVictoryPoints.Value)
        {
            yellowConfetti.SetActive(true);
            if (_playerData.Colour.Value == "Yellow")
            {
                _animatorScript.GoodJobAnimation();
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
                aboveCauldronUIYellow.SetActive(true);
                aboveCauldronTextYellow.text = "YOU WON!";
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
            }
            else
            {
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
                ShowWinnerUI();
            }
        }
        if (GameWinnerScore.Value == BlueVictoryPoints.Value)
        {
           blueConfetti.SetActive(true);
            if (_playerData.Colour.Value == "Blue")
            {
                _animatorScript.GoodJobAnimation();
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString());
                aboveCauldronUIBlue.SetActive(true);
                aboveCauldronTextBlue.text = "YOU WON!";
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
            }
            else
            {
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
                ShowWinnerUI();
            }
        }
        if (GameWinnerScore.Value == PurpleVictoryPoints.Value)
        {
           purpleConfetti.SetActive(true);
            if (_playerData.Colour.Value == "Purple")
            {
                _animatorScript.GoodJobAnimation();
                Debug.Log("YOU WON");
                winners.Add(_playerData.Name.Value.ToString()); 
                aboveCauldronUIPurple.SetActive(true);
                aboveCauldronTextPurple.text = "YOU WON!";
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
            }
            else
            {
                ReadyUp();
                await CheckAllPlayersReady();
                ResetReady();
                ShowWinnerUI();
                
            }
        }
        
    }

    public async void ShowWinnerUI()
    {
        if (winners.Count == 1)
        {
            DecideWhichCauldronUI($"{winners[0]} IS THE WINNER!");

        }
        if (winners.Count == 2)
        {
            DecideWhichCauldronUI($"IT'S A DRAW! {winners[0]} AND {winners[1]} ARE THE WINNERS!");

        }
        if (winners.Count == 3)
        {
            DecideWhichCauldronUI($"IT'S A DRAW! {winners[0]} AND {winners[1]} AND {winners[2]} ARE THE WINNERS!");

        }
        if (winners.Count == 4)
        {
            DecideWhichCauldronUI($"How did you all get the same points? what a pointless game");
        }

        await _chipPoints.MessageAboveCauldronMultipleChoice(2, "", "Play Again", "Leave", "", "", "");
        if (_chipPoints.choiceOneCauldron)
        {
            //put them back to the book/ new lobby
        }
        if (_chipPoints.choiceTwoCauldron)
        {
            //leave lobby and go back to home screen
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
            DecideWhichCauldronUI("You have more moths in your pot than both your neighbours, you get a droplet and a ruby");
          
            _playerData.Rubies.Value++;
            _chipPoints.AddDroplet();
            await _chipPoints.CheckWhichChoice();
        }
        if (_chipPoints.hawkMothRule == 1 && MothNumbersBeaten == 1)
        {
            DecideWhichCauldronUI("You have more moths in your pot than ONE of your neighbours, you get a droplet");
          
            _chipPoints.AddDroplet();
       
            await _chipPoints.CheckWhichChoice();
            // UI saying they have more moths than one of their neighbours and get a droplet
        }

        if (_chipPoints.hawkMothRule == 2 && MothNumbersBeaten == 1)
        {
            DecideWhichCauldronUI("You have more moths in your pot than your neighbours, you get a droplet and a ruby");
           
            _playerData.Rubies.Value++;
            _chipPoints.AddDroplet();
            await _chipPoints.CheckWhichChoice();
            // UI saying they have more moths than their neighbours and get a ruby and a droplet
        }
        if (_chipPoints.hawkMothRule == 2 && TwoPlayerDraw)
        {
            DecideWhichCauldronUI("You have same amount of moths in your pot as your neighbour, you get one droplet");
          
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

    public void DecideWhichCauldronUI(string textAboveCauldron)
    {
        if(_playerData.Colour.Value == "Purple")
        {
            aboveCauldronButtonPurple.SetActive(true);
            aboveCauldronUIPurple.SetActive(true);
            aboveCauldronTextPurple.text = textAboveCauldron;
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            aboveCauldronButtonYellow.SetActive(true);
            aboveCauldronUIYellow.SetActive(true);
            aboveCauldronTextYellow.text = textAboveCauldron;
        }
        if (_playerData.Colour.Value == "Blue")
        {
            aboveCauldronButtonBlue.SetActive(true);
            aboveCauldronUIBlue.SetActive(true);
            aboveCauldronTextBlue.text = textAboveCauldron;
        }
        if (_playerData.Colour.Value == "Red")
        {
            aboveCauldronButtonRed.SetActive(true);
            aboveCauldronUIRed.SetActive(true);
            aboveCauldronTextRed.text = textAboveCauldron;
        }
    }

    }


