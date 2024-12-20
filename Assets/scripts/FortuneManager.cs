using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FortuneManager : MonoBehaviour
{

    private fortuneTeller _fortuneTeller;
    private PotionQuality _quality;
    private onClickFortune _onClickFortune;
    private GrabIngredient _grabIngredient;
    private PlayerData _playerData;
    private ChipPoints _chipPoints;
 
    private BuyIngredients _buyIngredients;
    private WinnerManager _winnerManager;

    [SerializeField] GameObject purpleSphere;
    [SerializeField] GameObject blueSphere;
    [SerializeField] GameObject yellowSphere;
    [SerializeField] GameObject redSphere;

    [SerializeField] GameObject purpleGrabSphere;
    [SerializeField] GameObject blueGrabSphere;
    [SerializeField] GameObject yellowGrabSphere;
    [SerializeField] GameObject redGrabSphere;

    [SerializeField] GameObject purpleDice;
    [SerializeField] GameObject blueDice;
    [SerializeField] GameObject yellowDice;
    [SerializeField] GameObject redDice;
    [SerializeField] GameObject DiceFloor;

    private bool firstCherryBombHappened = false;
    public bool firstFiveIngredientsHappened = false;
    public bool fortuneShopDone = false;
    // Start is called before the first frame update


    private void Start()
    {
        
        _quality = FindObjectOfType<PotionQuality>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        
        _playerData = FindObjectOfType<PlayerData>();
        if (purpleGrabSphere == null)
        {
            Debug.LogError("SomeVariable has not been assigned.", this);
        }
    }

    public async Task CheckWhichChoice()
    {
        while (!ChoiceChosem())
        {
            Debug.Log("no choice chosen");

            await Task.Delay(100);
        }
        Debug.Log("choice MADE");

    }

    public bool ChoiceChosem()
    {

        if (!fortuneShopDone)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public void ResetChoices()
    {
        fortuneShopDone = false;
    }
    public async Task PreRoundFortuneEffects()
    {
        EnableSpheres();
        _onClickFortune = FindObjectOfType<onClickFortune>();
        _fortuneTeller = FindObjectOfType<fortuneTeller>();
        _grabIngredient = FindAnyObjectByType<GrabIngredient>();
        _chipPoints = FindObjectOfType<ChipPoints>();
       
        _buyIngredients = FindObjectOfType<BuyIngredients>();
        _quality = FindObjectOfType<PotionQuality>();
        Debug.Log("pre round fortune task");
        Debug.Log(_fortuneTeller.fortuneNumber);
        _grabIngredient.fortuneDrawTime = false;

        if (_fortuneTeller.fortuneNumber == 0)
        {
            if(_onClickFortune.buttonOne == true)
            {
                //teleport to shop
               _buyIngredients.SetUpShopForFortune0();
                await CheckWhichChoice();
                ResetChoices();
                //pick ingredient worth 4 points
            }
            if(_onClickFortune.buttonTwo == true)
            {

                _playerData.VictoryPoints.Value += _playerData.RatTails.Value;
                await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to <color=#006400>Victory Points!</color> <sprite name=\"VP\">");
                _playerData.RatTails.Value = 0;
                
                //add same number of victory points as rat tails and then remove rat tail
            }

            
        }
        if (_fortuneTeller.fortuneNumber == 4)
        {if(_onClickFortune.buttonOne == true)
            {
                //nothing changes
            }
            
            if(_onClickFortune.buttonTwo== true)
            {
                 
                await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to <color=#FF0000>Rubies</color> <sprite name=\"ruby\">");
                _playerData.RatTails.Value = 0;
                for (int i = 0; i < _playerData.RatTails.Value; i++)
                {
                    _playerData.Rubies.Value++;
                    _chipPoints.ChangeRubyUI();
                }
                
                //trade rat tails for rubies
            }
        }
        if (_fortuneTeller.fortuneNumber == 5)
        {
            if( _onClickFortune.buttonThree == true)
            {
                _quality.AddToCherryBombLimit();
                _quality.AddToCherryBombLimit();
                _quality.SetCherryBombText();
            }
            //PRE-ROUND add 2 to cherrybomb limit END OF ROUND Minus two.
            
        }
        if (_fortuneTeller.fortuneNumber == 6)
        {
            if (_onClickFortune.buttonThree == true)
            {
                //Double the number of rat tails this round

                _playerData.RatTails.Value += _playerData.RatTails.Value;}
            await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to <color=#FF1493>Rat Tails!</color> <sprite name=\"ratTail\"> ");

        }
         if(_fortuneTeller.fortuneNumber == 7)
        {

            if (_onClickFortune.buttonOne == true)
            {
               
                await _chipPoints.MessageAboveCauldron($"Added <color=#006400>4 Victory Points</color> <sprite name=\"VP\">");
                _playerData.VictoryPoints.Value += 4;
                //add 4 victory points to players score
            }
            if (_onClickFortune.buttonTwo == true)
            {
                Debug.Log("hello");
                _grabIngredient.RemoveItemFromBagPermanantly(0);
                _grabIngredient.ResetBagContents();
                _grabIngredient.CountIngredientsInBag();
                await _chipPoints.MessageAboveCauldron($"Removed <color=#FFFFFF>small cherrybomb</color> <sprite name=\"cherrybomb\">  from bag PERMANANTLY!");
                //remove white chip from players bag
            }
            
        }
        if (_fortuneTeller.fortuneNumber == 9)
        {
           if( _onClickFortune.buttonThree == true)
            {
                await _winnerManager.CalculateLowestVictoryPoints();
                if (_playerData.VictoryPoints.Value == _winnerManager.LowestVictoryPoints)
                {
                    _grabIngredient.AddToBagPermanantly(16);
                    await _chipPoints.MessageAboveCauldron($"You have the <color=#006400>lowest Victory Points</color> <sprite name=\"VP\">  Added <color=#006400>small spider</color> <sprite name=\"spider\">  to bag!");
                    _grabIngredient.ResetBagContents();
                    _grabIngredient.CountIngredientsInBag();
                }
                else
                {
                    await _chipPoints.MessageAboveCauldron($"You DON'T have the lowest <color=#006400>Victory Points</color> <sprite name=\"VP\">  No <color=#006400>spider</color> <sprite name=\"spider\">  for yooou!");
                }
                _winnerManager.LowestVictoryPoints = 0;
                ResetChoices();
                //the player with the fewest victory points recieves one small spider
            }
          
        }
        if (_fortuneTeller.fortuneNumber == 11)
        {
           if(_onClickFortune.buttonOne== true)
            {
                _buyIngredients.SetUpShopForFortune11(11); //take 1 moth or any mediun ingredient - maybe tekeoirt to shop and set up differently?
                await CheckWhichChoice();
               
            }
           if(_onClickFortune.buttonTwo == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    _playerData.Rubies.Value++;
                    _chipPoints.ChangeRubyUI();
                }

                await _chipPoints.MessageAboveCauldron($"Added <color=#FF0000>3 Rubies</color> <sprite name=\"ruby\">");
            }
            ResetChoices();
            //PRE ROUND - either open up shop with only medium ingredients and moth or add 3 rubies to players inventory
        }
        if (_fortuneTeller.fortuneNumber == 12)
        { 
            _chipPoints.AddDroplet();
            _chipPoints.ResetScore();
            _chipPoints.resetScoreText();
            await _chipPoints.MessageAboveCauldron($"Added <color=#800080>droplets</color> <sprite name=\"droplet\">  to potion!");
            //PRE ROUND - adds droplet to everyones potion.
        }
        if (_fortuneTeller.fortuneNumber == 13)
        {
            if (_onClickFortune.buttonOne == true)
            {
               
                _chipPoints.ResetScore();
                _chipPoints.resetScoreText();
                await _chipPoints.MessageAboveCauldron($"Added <color=#800080>2 droplets</color> <sprite name=\"droplet\">  to potion!");
                _chipPoints.AddDroplet();
                FunctionTimer.Create(() => _chipPoints.AddDroplet(), 2f);
                _chipPoints.ResetScore();
                _chipPoints.resetScoreText();
            }
            if (_onClickFortune.buttonTwo == true)
            {
                _grabIngredient.AddToBagPermanantly(6);
                _grabIngredient.ResetBagContents();
                _grabIngredient.CountIngredientsInBag();
                await _chipPoints.MessageAboveCauldron($"Added <color=#800080>ghosts breath</color> <sprite name=\"ghostsbreath\">  to your bag!");
            }
            ResetChoices();


            //PREROUND
        }
        if (_fortuneTeller.fortuneNumber == 15)
        {
            _grabIngredient.fortuneDrawAmount = 5;

            _grabIngredient.fortuneDrawTime = true;
            _grabIngredient.fortunedrawpulls = true;
            _grabIngredient.SetCauldronMessage("Draw 5 ingredients from your bag!");
            await _grabIngredient.CheckDrawnRightAmount();
        
            _grabIngredient.ResetChoices();
            await _winnerManager.CalculateLowestDrawnIngredients();
            if (_grabIngredient.totalOfFortuneIngredients == _winnerManager.LowestDrawn)
            {
                await _chipPoints.MessageAboveCauldron("You drew the lowest total! You get a <color=#708090>medium crow skull</color> <sprite name=\"crowSkull\">  Yippee! A dead things skull!");
                _grabIngredient.AddToBagPermanantly(5);
                _grabIngredient.ResetBagContents();
                _grabIngredient.CountIngredientsInBag();
                


            }
            else
            {
                await _chipPoints.MessageAboveCauldron("You didn't have the lowest total, you loser? You get a <color=#FF0000>Ruby</color> <sprite name=\"ruby\">");
                _playerData.Rubies.Value++;
                _chipPoints.ChangeRubyUI();
                
            }

   
            _grabIngredient.ResetChoices();
            _chipPoints.ResetScore();
            _chipPoints.resetScoreText();
            _quality.nextIngredientTime = true;

            //Pre Round All players draw 5 ingredients.The player with the lowest sum takes a medium skull, everyone else gets a ruby.
        }
        if (_fortuneTeller.fortuneNumber == 16)
        {
            if (_onClickFortune.buttonOne == true)
            {
                _buyIngredients.SetUpShopForFortune16();
                await CheckWhichChoice();
                ResetChoices();
            }

           
            
        }
        if (_fortuneTeller.fortuneNumber == 20)
        {
            if (_winnerManager.RedExists.Value) { redDice.SetActive(true); }
            if (_winnerManager.BlueExists.Value) { blueDice.SetActive(true); }
            if (_winnerManager.YellowExists.Value) { yellowDice.SetActive(true); }
            if (_winnerManager.PurpleExists.Value) { purpleDice.SetActive(true); }
            DiceFloor.SetActive(true);
            await _winnerManager.CheckAllPlayersReady();
            _winnerManager.ResetReady();
            DiceFloor.SetActive(false);
            // PRE ROUND DICE FUNCTION - "Everyone rolls the victory die once and get the bonus shown."
        }
        if (_fortuneTeller.fortuneNumber == 21)
        {
            await _winnerManager.CalculateLowestRubies();
            if(_playerData.Rubies.Value == _winnerManager.LowestRubies)
            {
                _playerData.Rubies.Value++;

                await _chipPoints.MessageAboveCauldron("You have the <color=#FF0000>lowest rubies</color><sprite name=\"ruby\">  You get a <color=#FF0000>Ruby</color> <sprite name=\"ruby\">");
                _chipPoints.ChangeRubyUI();
            }
            _winnerManager.LowestRubies = 0;
            // PRE ROUND - "The player with the fewest rubies receive one ruby."
        }
        if (_fortuneTeller.fortuneNumber == 23)
        {
            _grabIngredient.SetCauldronMessage("Draw 4 ingredients from your bag!");
            _grabIngredient.fortuneDrawAmount = 4;
            _grabIngredient.fortunedrawpulls = true;
            _grabIngredient.fortuneDrawTime = true;
            await _grabIngredient.CheckDrawnRightAmount();
           
            _grabIngredient.ResetChoices();

            await _grabIngredient.SendDrawnIngredientsInfo();
            _grabIngredient.ResetBagContents();
            _grabIngredient.CountIngredientsInBag();
           
            
            

           
            
            //PRE ROUND - "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can�t make a trade take a small spider. Put all chips back in the bag." - shop pops up with all the icons of the ingredients drawn.
        }
        _grabIngredient.fortuneDrawTime = true;
        ResetChoices();
    }

    public async Task DuringRoundFortuneEffects(string ingredient)
    {
        EnableSpheres();
        if (_fortuneTeller.fortuneNumber == 3)
        {
            if (ingredient.Contains("cherryBomb") && firstCherryBombHappened == false)
            {
                await _chipPoints.MessageAboveCauldronMultipleChoice(2, "Your first <color=#FFFFFF>cherrybomb</color> <sprite name=\"cherrybomb\">  Do you want to Remove it and put back in you bag?", "Remove", "Leave in pot", "", "", "");
                if (_chipPoints.choiceOneCauldron)
                {
                    if (ingredient.Contains("One"))
                    {
                        _grabIngredient.AddToBagThisRound(0);

                    }
                    if (ingredient.Contains("Two"))
                    {
                        _grabIngredient.AddToBagThisRound(2);
                    }
                    if (ingredient.Contains("Three"))
                    {
                        _grabIngredient.AddToBagThisRound(1);
                     
                    }
                    _chipPoints.RemoveLastIngredient();
                    _grabIngredient.CountIngredientsInBag();
                    _chipPoints.CountIngredientsInPot();
                    _chipPoints.resetScoreText();
                    _quality.SetCherryBombText();
                    _chipPoints.ResetStuffInBook();
                }
                _chipPoints.ResetChoices();
                firstCherryBombHappened = true;
            }
            
            //DURING-ROUND FUNCTION first cherry bomb out multiple choice to remove ingredient or leave it. - if remove - add back to bag contents list and undo score changes.
        }
        if (_fortuneTeller.fortuneNumber == 17)
        {
            if (ingredient.Contains("pumpkin"))
            {
                await _chipPoints.MessageAboveCauldron("The fortune means <color=#CD7F32>Pumpkin</color> <sprite name=\"pumpkin\">  adds extra <color=#800080>droplet</color> <sprite name=\"droplet\">  to pot!");
                _chipPoints.Score++;
            }
            //DURING ROUND In this round, every pumpkin add an extra 1 to potion. -have this set a boolean true in chippoints that adds 1 in pumpkin if statements
        }
        if (_fortuneTeller.fortuneNumber == 22)
        {
            //this broke it too
            if (firstFiveIngredientsHappened == false && _chipPoints.ingredients.Count == 5)
            {
                await _chipPoints.MessageAboveCauldronMultipleChoice(2, "You've had your first 5 ingredients, do you want to carry on or restart?", "Carry on", "Restart", "", "", "");
                if (_chipPoints.choiceTwoCauldron)
                {
                    _grabIngredient.ResetBagContents();
                    _chipPoints.ResetScore();
                    _quality.ResetCherryBombs();
                    _quality.SetCherryBombText();
                    _grabIngredient.CountIngredientsInBag();
                    _chipPoints.CountIngredientsInPot();
                    _chipPoints.ResetStuffInBook();
                }
                _chipPoints.ResetChoices();
                firstFiveIngredientsHappened = true;
            }
            //DURING ROUND MULTIPLE CHOICE BUTTONS APPEAR - After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again � but you get this choice only once."
        }
    }


    public async Task PostRoundFortuneEffects()
    {
        _winnerManager = FindObjectOfType<WinnerManager>();
        firstCherryBombHappened = false;
        firstFiveIngredientsHappened = false;
        EnableSpheres();
        if (_fortuneTeller.fortuneNumber == 1)
        {
            
            if (_chipPoints.RubiesThisRound)
            {
                await _chipPoints.MessageAboveCauldron("You reached a <color=#FF0000>Ruby</color> <sprite name=\"ruby\"> level this round and due to the fortune you get an extra <color=#FF0000>Ruby</color> <sprite name=\"ruby\">!");
                _playerData.Rubies.Value++;
                _chipPoints.ChangeRubyUI();
            }
            else
            {
                await _chipPoints.MessageAboveCauldron("Boooohooooo! You didn't reach a <color=#FF0000>Ruby</color> <sprite name=\"ruby\"> level, no extra <color=#FF0000>Ruby</color> from fortune <sprite name=\"ruby\"> for you");
            }
        
        }
        if (_fortuneTeller.fortuneNumber == 2)
        {
            if (_playerData.Colour.Value == "Purple")
            {
                if (_winnerManager.YellowExists.Value)
                {
                    if (_winnerManager.yellowExploded.Value)
                    {
                        
                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.RedExists.Value)
                {
                    if (_winnerManager.redExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.BlueExists.Value)
                {
                    if (_winnerManager.blueExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
            }
            if (_playerData.Colour.Value == "Red")
            {
                if (_winnerManager.BlueExists.Value)
                {
                    if (_winnerManager.blueExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.PurpleExists.Value)
                {
                    if (_winnerManager.purpleExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.YellowExists.Value)
                {
                    if (_winnerManager.yellowExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
            
                
            }
            if (_playerData.Colour.Value == "Blue")
            {
                 if (_winnerManager.PurpleExists.Value)
                {
                    if (_winnerManager.purpleExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.YellowExists.Value)
                {
                    if (_winnerManager.yellowExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.RedExists.Value)
                {
                    if (_winnerManager.redExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
            }
            if (_playerData.Colour.Value == "Yellow")
            {
                if (_winnerManager.RedExists.Value)
                {
                    if (_winnerManager.redExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.BlueExists.Value)
                {
                    if (_winnerManager.blueExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
                else if (_winnerManager.PurpleExists.Value)
                {
                    if (_winnerManager.purpleExploded.Value)
                    {

                        await getMediumIngredient();
                    }
                }
            }
            //close fortune button
            //AFTER-ROUND FUNCTION if pot to left IMPLIMENT POT EXPLODED BOOL TO WINNER MANAGER BIT explodes player++ gets a level 2 ingredient.
        }
       

        if (_fortuneTeller.fortuneNumber == 10)
        {
            if (_chipPoints.RubiesThisRound)
            {
                await _chipPoints.MessageAboveCauldron("<color=#FF0000>Ruby</color> <sprite name=\"ruby\"> on this level, you get <color=#006400>2 victory points</color> <sprite name=\"VP\">");
                _playerData.VictoryPoints.Value += 2;
            }
            //AFTER ROUND - if you reach a scoring space with a ruby this round you get 2 extra victory points, even if your pot explodes.
        }
        if (_fortuneTeller.fortuneNumber == 14)
        {
            Debug.Log(_winnerManager.purpleExploded.Value);
            
            if (_playerData.Colour.Value == "Purple" && !_winnerManager.purpleExploded.Value)
           {
                await DrawIngredientsAndPutOneInPot();
                _chipPoints.ResetChoices();

            }
            if (_playerData.Colour.Value == "Red" && !_winnerManager.redExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _chipPoints.ResetChoices();

            }
            if (_playerData.Colour.Value == "Blue" && !_winnerManager.blueExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _chipPoints.ResetChoices();

            }
            if (_playerData.Colour.Value == "Yellow" && !_winnerManager.yellowExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _chipPoints.ResetChoices();
              
            }
            //AFTER ROUND you stopped without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.
        }
        if (_fortuneTeller.fortuneNumber == 18)
        {
            await _chipPoints.MessageAboveCauldron("Everyone gets a free refill of their purifier bottle this round!!");
            _playerData.PurifierFull.Value = true;
            //AFTERROUND-the end of the round all flasks get a free refill.
        }
        if (_fortuneTeller.fortuneNumber == 19)
        {
            if (_quality.GetCherryBombs() == 7)
            {
                await _chipPoints.MessageAboveCauldron("Your <color=#FFFFFF>Cherrybombs</color> <sprite name=\"cherrybomb\">  total exactly 7! You get another <color=#800080>droplet</color> <sprite name=\"droplet\">  in your potion!");
                _chipPoints.AddDroplet();
              
            }
            else
            {
                await _chipPoints.MessageAboveCauldron("Ohhh nooo! You didn't get a total of <color=#FFFFFF>7 Cherrybombs</color> <sprite name=\"cherrybomb\">  no <color=#800080>droplet</color> <sprite name=\"droplet\">  for you! YOU LOSER!");
            }
            // END OF ROUND - "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.  
        }
        if (_playerData.Colour.Value == "Purple")
        {
            _winnerManager.purpleExploded.Value = false;
        }
        if (_playerData.Colour.Value == "Red")
        {
            _winnerManager.redExploded.Value = false;
        }
        if (_playerData.Colour.Value == "Blue")
        {
            _winnerManager.blueExploded.Value = false;
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            _winnerManager.yellowExploded.Value = false;
        }
  
    }




    public void EnableSpheres()
    {
        _playerData = FindObjectOfType<PlayerData>();
        if (_playerData.Colour.Value == "Purple")
        {
            purpleGrabSphere.SetActive(true);
            purpleSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            blueGrabSphere.SetActive(true);
            blueSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Red")
        {
            redGrabSphere.SetActive(true);
            redSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            yellowGrabSphere.SetActive(true);
            yellowSphere.SetActive(true);
        }
    }

    public void DisableSpheres()
    {
        if (_playerData.Colour.Value == "Purple")
        {
            purpleGrabSphere.SetActive(false);
            purpleSphere.SetActive(false);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            blueGrabSphere.SetActive(false);
            blueSphere.SetActive(false);
        }
        if (_playerData.Colour.Value == "Red")
        {
            redGrabSphere.SetActive(false);
            redSphere.SetActive(false);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            yellowGrabSphere.SetActive(false);
            yellowSphere.SetActive(false);
        }
       
    }


    public async Task RollDiceTwice()
    {
       
        if (_fortuneTeller.fortuneNumber == 8)
        {
            EnableSpheres();
            await _chipPoints.MessageAboveCauldron("The fortune means the winner gets to roll twice this round!");
            

       
            string winnerColor = DetermineWinnerColor();

            if (!string.IsNullOrEmpty(winnerColor))
            {
            
                ActivateDiceForWinner(winnerColor);
            }
        }

    }

    private string DetermineWinnerColor()
    {
  
        if (_winnerManager.RoundWinnerScore.Value == _winnerManager.RedPoints.Value)
        {
            return "Red";
        }
        if (_winnerManager.RoundWinnerScore.Value == _winnerManager.YellowPoints.Value)
        {
            return "Yellow";
        }
        if (_winnerManager.RoundWinnerScore.Value == _winnerManager.BluePoints.Value)
        {
            return "Blue";
        }
        if (_winnerManager.RoundWinnerScore.Value == _winnerManager.PurplePoints.Value)
        {
            return "Purple";
        }
        else
        {
            return string.Empty;
        }
    }

    private void ActivateDiceForWinner(string color)
    {
        GameObject diceToActivate = null;
        Debug.Log(color);
        switch (color)
        {
            case "Red":
                diceToActivate = redDice;
                break;
            case "Yellow":
                diceToActivate = yellowDice;
                break;
            case "Blue":
                diceToActivate = blueDice;
                break;
            case "Purple":
                diceToActivate = purpleDice;
                break;
        }
        Debug.Log(diceToActivate);

        if (diceToActivate != null)
        {
            diceToActivate.SetActive(true);
            if (_playerData.Colour.Value == color)
            {
                DiceFloor.SetActive(true);
            }
        }
    }

    private async Task getMediumIngredient()
    {
        await _chipPoints.MessageAboveCauldron($"The player to your right had their pot explode! This means you get to pick a medium ingredient!");
        _buyIngredients.SetUpShopForFortune11(2);
        await CheckWhichChoice();
        ResetChoices();
    }

    private async Task DrawIngredientsAndPutOneInPot()
    {
        _grabIngredient.fortuneDrawTime = true;
        _grabIngredient.fortunedrawpulls = true;

        Debug.Log("fortune 14 after round effects - drawing ingredients");
        _grabIngredient.fortuneDrawAmount = 5;  // Set the number of ingredients to draw
        _grabIngredient.SetCauldronMessage("Your pot didn't explode! Draw 5 ingredients from bag!");

        
            // Check that we have drawn the right amount, then send the ingredients to the pot
            await _grabIngredient.CheckDrawnRightAmount();
            await _grabIngredient.SendDrawnIngredientsInfoToAddToPot();

            // Once the ingredients are added, immediately reset the state to avoid duplicates
            _grabIngredient.ResetBagContents();
            _grabIngredient.CountIngredientsInBag();
            _grabIngredient.ResetChoices();
       
       
            _grabIngredient.fortuneDrawTime = false;
     
    }

    public async Task AtTheVeryEndOfRound()
    {
        if (_fortuneTeller.fortuneNumber == 5)
        {
            await _chipPoints.MessageAboveCauldron("Putting <color=#FFFFFF>Cherrybomb</color> <sprite name=\"cherrybomb\">  limit back to <color=#FFFFFF>7</color>");
            _quality.RemoveFromCherryBombLimit();
            _quality.RemoveFromCherryBombLimit();
        }
    }
}
