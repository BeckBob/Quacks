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
    private TeleportationManager _teleportationManager;
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

    public bool fortuneShopDone = false;
    // Start is called before the first frame update

    private void Start()
    {
        
        _quality = FindObjectOfType<PotionQuality>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        
        _playerData = FindObjectOfType<PlayerData>();
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
        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _buyIngredients = FindObjectOfType<BuyIngredients>();

        if (_fortuneTeller.fortuneNum == 0)
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
        if (_fortuneTeller.fortuneNum == 4)
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
        if (_fortuneTeller.fortuneNum == 5)
        {
            if( _onClickFortune.buttonThree == true)
            {
                _quality.AddToCherryBombLimit();
                _quality.AddToCherryBombLimit();
                _quality.SetCherryBombText();
            }
            //PRE-ROUND add 2 to cherrybomb limit END OF ROUND Minus two.
            
        }
        if (_fortuneTeller.fortuneNum == 6)
        {
            if (_onClickFortune.buttonThree == true)
            {
                //Double the number of rat tails this round

                _playerData.RatTails.Value += _playerData.RatTails.Value;}
            await _chipPoints.MessageAboveCauldron($"Added {_playerData.RatTails.Value} to Rat Tails!");

        }
         if(_fortuneTeller.fortuneNum==7)
        {

            if (_onClickFortune.buttonOne == true)
            {
                _playerData.VictoryPoints.Value *= 4;
                await _chipPoints.MessageAboveCauldron($"Added 4 Victory Points!");
                //add 4 victory points to players score
            }
            if (_onClickFortune.buttonTwo == true)
            {
                Debug.Log("hello");
                _grabIngredient.RemoveItemFromBagPermanantly(0);
                _grabIngredient.CountIngredientsInBag();
                await _chipPoints.MessageAboveCauldron($"Removed small cherry bomb from bag PERMANANTLY!");
                //remove white chip from players bag
            }
            
        }
        if (_fortuneTeller.fortuneNum == 9)
        {
           if( _onClickFortune.buttonThree == true)
            {
                await _winnerManager.CalculateLowestVictoryPoints();
                if (_playerData.VictoryPoints.Value == _winnerManager.LowestVictoryPoints)
                {
                    _grabIngredient.AddToBagPermanantly(16);
                    await _chipPoints.MessageAboveCauldron($"You have the lowest Victory Points! Added small spider to bag!");
                }
                _winnerManager.LowestVictoryPoints = 0;

                //the player with the fewest victory points recieves one small spider
            }
          
        }
        if (_fortuneTeller.fortuneNum == 11)
        {
           if(_onClickFortune.buttonOne== true)
            {
                _buyIngredients.SetUpShopForFortune11(); //take 1 moth or any mediun ingredient - maybe tekeoirt to shop and set up differently?
                await CheckWhichChoice();
                ResetChoices();
            }
           if(_onClickFortune.buttonTwo == true)
            {
                _playerData.Rubies.Value *= 3;
                _chipPoints.ChangeRubyUI();
                await _chipPoints.MessageAboveCauldron($"Added 3 Rubies!");
            }
        
            //PRE ROUND - either open up shop with only medium ingredients and moth or add 3 rubies to players inventory
        }
        if (_fortuneTeller.fortuneNum == 12)
        { 
            _chipPoints.AddDroplet();
            _chipPoints.ResetScore();
            _chipPoints.resetScoreText();
            await _chipPoints.MessageAboveCauldron($"Added droplet to potion!");
            //PRE ROUND - adds droplet to everyones potion.
        }
        if (_fortuneTeller.fortuneNum == 13)
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


          
            //PREROUND
        }
        if (_fortuneTeller.fortuneNum == 15)
        {
            _grabIngredient.fortuneDrawAmount = 5;

            _grabIngredient.fortuneDrawTime = true;
            await _grabIngredient.CheckDrawnRightAmount();
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
        if (_fortuneTeller.fortuneNum == 16)
        {
            if (_onClickFortune.buttonOne == true)
            {
                _buyIngredients.SetUpShopForFortune16();
                await CheckWhichChoice();
                ResetChoices();
            }

           
            
        }
        if (_fortuneTeller.fortuneNum == 20)
        {
            redDice.SetActive(true);
            blueDice.SetActive(true);
            yellowDice.SetActive(true);
            purpleDice.SetActive(true);
            DiceFloor.SetActive(true);
            await _winnerManager.CheckAllPlayersReady();
            DiceFloor.SetActive(false);
            // PRE ROUND DICE FUNCTION - "Everyone rolls the victory die once and get the bonus shown."
        }
        if (_fortuneTeller.fortuneNum == 21)
        {
            await _winnerManager.CalculateLowestRubies();
            if(_playerData.Rubies.Value == _winnerManager.LowestRubies)
            {
                _playerData.Rubies.Value++;
                await _chipPoints.MessageAboveCauldron("You have the lowest rubies? You get a ruby!");
            }
            _winnerManager.LowestRubies = 0;
            // PRE ROUND - "The player with the fewest rubies receive one ruby."
        }
        if (_fortuneTeller.fortuneNum == 23)
        {
            _grabIngredient.fortuneDrawAmount = 4;

            _grabIngredient.fortuneDrawTime = true;
            await _grabIngredient.CheckDrawnRightAmount();
            await _grabIngredient.SendDrawnIngredientsInfo();
            _grabIngredient.ResetBagContents();
            _grabIngredient.CountIngredientsInBag();
            _grabIngredient.DeleteInstantiatedIngredients();
            _grabIngredient.ResetChoices();
            

           
            
            //PRE ROUND - "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." - shop pops up with all the icons of the ingredients drawn.
        }
        DisableSpheres();
    }

    public async void DuringRoundFortuneEffects()
    {
        if (_fortuneTeller.fortuneNum == 3)
        {
            
            //close fortune buttom
            //DURING-ROUND FUNCTION first cherry bomb out multiple choice to remove ingredient or leave it. - if remove - add back to bag contents list and undo score changes.
        }
        if (_fortuneTeller.fortuneNum == 17)
        {
           
            //DURING ROUND In this round, every pumpkin add an extra 1 to potion. -have this set a boolean true in chippoints that adds 1 in pumpkin if statements
        }
        if (_fortuneTeller.fortuneNum == 22)
        {
            
            //DURING ROUND MULTIPLE CHOICE BUTTONS APPEAR - After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once."
        }
    }


    public async void PostRoundFortuneEffects()
    {
        if (_fortuneTeller.fortuneNum == 1)
        {
            await _chipPoints.MessageAboveCauldron("You have the lowest rubies? You get a ruby!");
            if (_chipPoints.RubiesThisRound)
            {
                await _chipPoints.MessageAboveCauldron("You landed on a ruby this round and according to the fortune you get an extra ruby!");
                _playerData.Rubies.Value++;
                _chipPoints.ChangeRubyUI();
            }
            //close fortune button
            //AFTER-ROUND FUNCTION give extra ruby is end on ruby space
        }
        if (_fortuneTeller.fortuneNum == 2)
        {

            //close fortune button
            //AFTER-ROUND FUNCTION if pot explodes player++ gets a level 2 ingredient.
        }
        if (_fortuneTeller.fortuneNum == 5)
        {
            await _chipPoints.MessageAboveCauldron("Putting Cherry Bomb limit back to 7!");
            _quality.RemoveFromCherryBombLimit();
            _quality.RemoveFromCherryBombLimit();
        }

        if (_fortuneTeller.fortuneNum == 10)
        {
            
            //AFTER ROUND - if you reach a scoring space with a ruby this round you get 2 extra victory points, even if your pot explodes.
        }
        if (_fortuneTeller.fortuneNum == 14)
        {
            
            //AFTER ROUND you stopped without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.
        }
        if (_fortuneTeller.fortuneNum == 18)
        {
           
            //AFTERROUND-the end of the round all flasks get a free refill.
        }
        if (_fortuneTeller.fortuneNum == 19)
        {
            
            // END OF ROUND - "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.
        }
    }
    // Update is called once per frame



    public void EnableSpheres()
    {
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


    public async void RollDiceTwice()
    {
        if (_fortuneTeller.fortuneNum == 8)
        {
            await _chipPoints.MessageAboveCauldron("The fortune means the winner gets to roll twice this round!");
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
}
