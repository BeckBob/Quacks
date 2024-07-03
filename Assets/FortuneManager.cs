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
    private bool firstFiveIngredientsHappened = false;
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

            await Task.Yield();
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
                await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to Victory Points!");
                _playerData.RatTails.Value = 0;
                
                //add same number of victory points as rat tails and then remove rat tail
            }

            
        }
        if (_fortuneTeller.fortuneNumber == 4)
        {
            if(_onClickFortune.buttonOne == true)
            {
                //nothing changes
            }
            if(_onClickFortune.buttonTwo== true)
            {
                _playerData.Rubies.Value += _playerData.RatTails.Value;
                await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to Rubies!");
                _playerData.RatTails.Value = 0;

                _chipPoints.ChangeRubyUI();
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
            await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to Rat Tails!");

        }
         if(_fortuneTeller.fortuneNumber == 7)
        {

            if (_onClickFortune.buttonOne == true)
            {
               
                await _chipPoints.MessageAboveCauldron($"Added 4 Victory Points!");
                _playerData.VictoryPoints.Value += 4;
                //add 4 victory points to players score
            }
            if (_onClickFortune.buttonTwo == true)
            {
                Debug.Log("hello");
                _grabIngredient.RemoveItemFromBagPermanantly(0);
                _grabIngredient.ResetBagContents();
                _grabIngredient.CountIngredientsInBag();
                await _chipPoints.MessageAboveCauldron($"Removed small cherry bomb from bag PERMANANTLY!");
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
                    await _chipPoints.MessageAboveCauldron($"You have the lowest Victory Points! Added small spider to bag!");
                    _grabIngredient.ResetBagContents();
                    _grabIngredient.CountIngredientsInBag();
                }
                else
                {
                    await _chipPoints.MessageAboveCauldron($"You DON'T have the lowest Victory Points! No spider for yooou!");
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
                _playerData.Rubies.Value += 3;
                _chipPoints.ChangeRubyUI();
                await _chipPoints.MessageAboveCauldron($"Added 3 Rubies!");
            }
            ResetChoices();
            //PRE ROUND - either open up shop with only medium ingredients and moth or add 3 rubies to players inventory
        }
        if (_fortuneTeller.fortuneNumber == 12)
        { 
            _chipPoints.AddDroplet();
            _chipPoints.ResetScore();
            _chipPoints.resetScoreText();
            await _chipPoints.MessageAboveCauldron($"Added droplet to potion!");
            //PRE ROUND - adds droplet to everyones potion.
        }
        if (_fortuneTeller.fortuneNumber == 13)
        {
            if (_onClickFortune.buttonOne == true)
            {
                _chipPoints.AddDroplet();
                _chipPoints.AddDroplet();
                _chipPoints.ResetScore();
                _chipPoints.resetScoreText();
                await _chipPoints.MessageAboveCauldron($"Added 2 droplets to potion!");
            }
            if (_onClickFortune.buttonTwo == true)
            {
                _grabIngredient.AddToBagPermanantly(6);
                _grabIngredient.ResetBagContents();
                _grabIngredient.CountIngredientsInBag();
                await _chipPoints.MessageAboveCauldron($"Added Ghosts Breath to your bag!");
            }
            ResetChoices();


            //PREROUND
        }
        if (_fortuneTeller.fortuneNumber == 15)
        {
            _grabIngredient.fortuneDrawAmount = 5;

            _grabIngredient.fortuneDrawTime = true;
            await _grabIngredient.CheckDrawnRightAmount();
            _grabIngredient.DeleteInstantiatedIngredients();
            _grabIngredient.ResetChoices();
            await _winnerManager.CalculateLowestDrawnIngredients();
            if (_grabIngredient.totalOfFortuneIngredients == _winnerManager.LowestDrawn)
            {
                await _chipPoints.MessageAboveCauldron("You drew the lowest total! You get a medium crow skull! Yippee! A dead things skull!");
                _grabIngredient.AddToBagPermanantly(5);
                _grabIngredient.ResetBagContents();
                _grabIngredient.CountIngredientsInBag();
                
            }
            else
            {
                await _chipPoints.MessageAboveCauldron("You didn't have the lowest total, you loser? You get a ruby!");
                _playerData.Rubies.Value++;
                _chipPoints.ChangeRubyUI();
                
            }

            _grabIngredient.DeleteInstantiatedIngredients();
            _grabIngredient.ResetChoices();
            _chipPoints.ResetScore();
            _chipPoints.resetScoreText();

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
            redDice.SetActive(true);
            blueDice.SetActive(true);
            yellowDice.SetActive(true);
            purpleDice.SetActive(true);
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

                await _chipPoints.MessageAboveCauldron("You have the lowest rubies! You get a ruby!");
                _chipPoints.ChangeRubyUI();
            }
            _winnerManager.LowestRubies = 0;
            // PRE ROUND - "The player with the fewest rubies receive one ruby."
        }
        if (_fortuneTeller.fortuneNumber == 23)
        {
            _grabIngredient.fortuneDrawAmount = 4;

            _grabIngredient.fortuneDrawTime = true;
            await _grabIngredient.CheckDrawnRightAmount();
            _grabIngredient.DeleteInstantiatedIngredients();
            _grabIngredient.ResetChoices();

            await _grabIngredient.SendDrawnIngredientsInfo();
            _grabIngredient.ResetBagContents();
            _grabIngredient.CountIngredientsInBag();
            _grabIngredient.DeleteInstantiatedIngredients();
            
            

           
            
            //PRE ROUND - "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." - shop pops up with all the icons of the ingredients drawn.
        }
        ResetChoices();
    }

    public async Task DuringRoundFortuneEffects(string ingredient)
    {
        EnableSpheres();
        if (_fortuneTeller.fortuneNumber == 3)
        {
            if (ingredient.Contains("cherryBomb") && firstCherryBombHappened == false)
            {
                await _chipPoints.MessageAboveCauldronMultipleChoice(2, "Your first cherry bomb! Do you want to Remove it and put back in you bag?", "Remove Cherry Bomb", "Leave in pot", "", "", "");
                if (_chipPoints.choiceOneCauldron)
                {
                    if (ingredient.Contains("One"))
                    {
                        _grabIngredient.AddToBagThisRound(0);
                        _quality.RemoveFromCherryBombs();

                    }
                    if (ingredient.Contains("Two"))
                    {
                        _grabIngredient.AddToBagThisRound(2);
                        _quality.RemoveFromCherryBombs();
                        _quality.RemoveFromCherryBombs();
                    }
                    if (ingredient.Contains("Three"))
                    {
                        _grabIngredient.AddToBagThisRound(1);
                        _quality.RemoveFromCherryBombs();
                        _quality.RemoveFromCherryBombs();
                        _quality.RemoveFromCherryBombs();
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
                await _chipPoints.MessageAboveCauldron("The fortune pumpkin adds an extra point to pot!");
                _chipPoints.Score++;
            }
            //DURING ROUND In this round, every pumpkin add an extra 1 to potion. -have this set a boolean true in chippoints that adds 1 in pumpkin if statements
        }
        if (_fortuneTeller.fortuneNumber == 22)
        {
            if (ingredient.Contains("cherryBomb") && firstFiveIngredientsHappened == true)
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
                firstFiveIngredientsHappened = false;
            }
            //DURING ROUND MULTIPLE CHOICE BUTTONS APPEAR - After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once."
        }
    }


    public async Task PostRoundFortuneEffects()
    {
        firstCherryBombHappened = false;
        firstFiveIngredientsHappened = false;
        EnableSpheres();
        if (_fortuneTeller.fortuneNumber == 1)
        {
            
            if (_chipPoints.RubiesThisRound)
            {
                await _chipPoints.MessageAboveCauldron("You reached a ruby level this round and according to the fortune you get an extra ruby!");
                _playerData.Rubies.Value++;
                _chipPoints.ChangeRubyUI();
            }
            else
            {
                await _chipPoints.MessageAboveCauldron("Boooohooooo! You didn't reach a ruby level, no extra fortune ruby for you");
            }
            //close fortune button
            //AFTER-ROUND FUNCTION give extra ruby is end on ruby space
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
                await _chipPoints.MessageAboveCauldron("Ruby on this level, you get 2 extra victory points because of fortune!");
                _playerData.VictoryPoints.Value += 2;
            }
            //AFTER ROUND - if you reach a scoring space with a ruby this round you get 2 extra victory points, even if your pot explodes.
        }
        if (_fortuneTeller.fortuneNumber == 14)
        {
            if (_playerData.Colour.Value == "Purple" && !_winnerManager.purpleExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _grabIngredient.ResetChoices();
                _grabIngredient.DeleteInstantiatedIngredients();
            }
            if (_playerData.Colour.Value == "Red" && !_winnerManager.redExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _grabIngredient.ResetChoices();
                _grabIngredient.DeleteInstantiatedIngredients();
            }
            if (_playerData.Colour.Value == "Blue" && !_winnerManager.blueExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _grabIngredient.ResetChoices();
                _grabIngredient.DeleteInstantiatedIngredients();
            }
            if (_playerData.Colour.Value == "Yellow" && !_winnerManager.yellowExploded.Value)
            {
                await DrawIngredientsAndPutOneInPot();
                _grabIngredient.ResetChoices();
                _grabIngredient.DeleteInstantiatedIngredients();
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
                await _chipPoints.MessageAboveCauldron("Your Cherry Bombs total exactly 7! You get another droplet in your potion!");
                _chipPoints.AddDroplet();
                _chipPoints.ResetScore();
                _chipPoints.resetScoreText();
            }
            else
            {
                await _chipPoints.MessageAboveCauldron("Ohhh nooo! You didn't get a total of 7 cherry bombs, no droplet for you! YOU LOSER!");
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
            DisableSpheres();
            if (_winnerManager.RoundWinnerScore.Value == _winnerManager.RedPoints.Value)
            {

                redDice.SetActive(true);
                if (_playerData.Colour.Value == "Red")
                {
                    DiceFloor.SetActive(true);
                }
                // UI announcing they won - in book and above head?
                //instantiate dice in front of them and add whatever it lands on to their player Data
            }
            if (_winnerManager.RoundWinnerScore.Value == _winnerManager.YellowPoints.Value)
            {

                yellowDice.SetActive(true);
                if (_playerData.Colour.Value == "Yellow")
                {
                    DiceFloor.SetActive(true);
                }
            }
            if (_winnerManager.RoundWinnerScore.Value == _winnerManager.BluePoints.Value)
            {

                blueDice.SetActive(true);
                if (_playerData.Colour.Value == "Blue")
                {
                    DiceFloor.SetActive(true);
                }
            }
            if (_winnerManager.RoundWinnerScore.Value == _winnerManager.PurplePoints.Value)
            {

                purpleDice.SetActive(true);
                if (_playerData.Colour.Value == "Purple")
                {
                    DiceFloor.SetActive(true);
                }

                //AFTER ROUND - any player that gets to roll the die this round rolls it twice.
            }
        }
        }

    private async Task getMediumIngredient()
    {
        _buyIngredients.SetUpShopForFortune11(2);
        await CheckWhichChoice();
        ResetChoices();
    }

    private async Task DrawIngredientsAndPutOneInPot()
    {
        _grabIngredient.fortuneDrawAmount = 5;

        _grabIngredient.fortuneDrawTime = true;
        await _grabIngredient.CheckDrawnRightAmount();
        await _grabIngredient.SendDrawnIngredientsInfoToAddToPot();
        _grabIngredient.ResetBagContents();
        _grabIngredient.CountIngredientsInBag();
        _grabIngredient.DeleteInstantiatedIngredients();
        _grabIngredient.ResetChoices();
    }

    public async Task AtTheVeryEndOfRound()
    {
        if (_fortuneTeller.fortuneNumber == 5)
        {
            await _chipPoints.MessageAboveCauldron("Putting Cherry Bomb limit back to 7!");
            _quality.RemoveFromCherryBombLimit();
            _quality.RemoveFromCherryBombLimit();
        }
    }
}
