using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using pointSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class fortuneTeller : MonoBehaviour
{
    [SerializeField] private GameObject _button1;
    [SerializeField] private GameObject _button2;
    [SerializeField] private GameObject _button3;
    [SerializeField] private GameObject _fortuneTextObj;
    [SerializeField] TextMeshProUGUI choiceOne;
    [SerializeField] TextMeshProUGUI choiceTwo;
    
    [SerializeField] TextMeshProUGUI _fortune;
    [SerializeField] GameObject potionBottle;
    [SerializeField] GameObject purifierBottle;

    AnimatorScript _animatorScript; 
    
    private string _fortuneText;
   
   //ublic ChoiceOne _choiceOne;
   //lic ChoiceTwo _choiceTwo;
    void Awake()
    {
      _animatorScript = FindObjectOfType<AnimatorScript>();
    

    }

    public int fortuneNumber;

    List<string> fortunes = new List<string> { "Choose", "If you reach a scoring space with a <color=#FF0000>ruby</color> <sprite name=\"ruby\"> this round you get an <color=#FF0000>extra ruby</color> <sprite name=\"ruby\">", "If your pot explodes this round the player to your left gets any medium ingredient.", "In this round you may put the first <color=#FFFFFF>Cherrybomb</color> <sprite name=\"cherrybomb\">  you draw back into the bag.", "Choose: Use your <color=#FF1493>Rat Tails</color> <sprite name=\"ratTail\">  normally OR pass up on <color=#FF1493>1-3</color> <sprite name=\"ratTail\">  and take <color=#FF0000>that many rubies</color> <sprite name=\"ruby\"> instead.", "The threshold for <color=#FFFFFF>Cherrybomb</color> <sprite name=\"cherrybomb\">  is raised from <color=#FFFFFF>7 to 9</color> for this round.", "Double the number of <color=#FF1493>Rat Tails</color> <sprite name=\"ratTail\">  this round.", "Take <color=#006400>4 Victory Points</color> <sprite name=\"VP\">  OR remove <color=#FFFFFF>1 Cherrybomb</color> <sprite name=\"cherrybomb\">  from your bag.", "Any player who gets to roll the die this round rolls it twice.", "The player(s) with the <color=#006400>fewest Victory Points</color> <sprite name=\"VP\">  receive <color=#006400>1 Small Spider</color> <sprite name=\"spider\"> ", "If you reach a scoring space with a <color=#FF0000>Ruby</color> <sprite name=\"ruby\">  this round you get <color=#006400>2 Victory Points</color> <sprite name=\"VP\"> , even if your pot explodes.", "Choose", "Add <color=#800080>Droplet</color> <sprite name=\"droplet\">  to potion.", "Choose", "Beginning with the start player, if you stop without an explosion, draw up to 5 ingredients from your bag. You may place 1 of them in your pot.", "All players draw 5 ingredients. The player with the lowest sum takes a <color=#ADD8E6>Medium Crow Skull</color> <sprite name=\"crowSkull\"> , everyone else gets a <color=#FF0000>ruby</color> <sprite name=\"ruby\">", "You can trade <color=#FF0000>1 Ruby</color> <sprite name=\"ruby\"> for any small ingredient.", "In this round, every <color=#CD7F32>Pumpkin</color> <sprite name=\"pumpkin\">  adds an extra 1 to score", "At the end of the round all flasks get a free refill.", "If your <color=#FFFFFF>Cherrybomb</color> <sprite name=\"cherrybomb\">  total exactly 7 at the end of the round you get to add a <color=#800080>droplet</color> <sprite name=\"droplet\">  to your potion.", "Everyone rolls the dice once and get the bonus shown", "The player with the <color=#FF0000>fewest Rubies</color> <sprite name=\"ruby\"><sprite name=\"ruby\"> receive <color=#FF0000>1 Ruby</color> <sprite name=\"ruby\">", "After you have placed the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once.", "Draw 4 ingredients from your bag You may trade in 1 of them for the same ingredient one size up. If you can’t make a trade take a <color=#006400>Small Spider</color> <sprite name=\"spider\"> " };

    
    public void ReadFortune(int fortuneNum)
    {
        potionBottle.SetActive(false);
        purifierBottle.SetActive(false);
        potionBottle.SetActive(true);
        purifierBottle.SetActive(true) ;
        
        fortuneNumber = fortuneNum;

       _fortuneTextObj.SetActive(true);
        _fortuneText = fortunes[fortuneNum];
      
        _fortune.text = _fortuneText;
        Debug.Log(fortuneNum);
        if (fortuneNum == 0)
        {
            
            _button1.SetActive(true);
            _button2.SetActive(true);
            //hoiceOne = FindObjectOfType<ChoiceOne>();
            //hoiceTwo = FindObjectOfType<ChoiceTwo>();
           
            choiceOne.text = "Pick one large ingredient worth 4 points";
            choiceTwo.text = "trade rat tails <sprite name=\"ratTail\">  for victory points <sprite name=\"VP\"> ";
            Debug.Log(choiceOne.text);
            _animatorScript.StartTalking(4);
            //PRE- ROUND FUNCTION let them pick a 1 4 chip for one button that leads to shop layout AND other one adds victory point for every rat tail
        }
        if (fortuneNum == 1)
        {
            _button3.SetActive(true);
            //close fortune button
            //AFTER-ROUND FUNCTION give extra ruby is end on ruby space
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 2)
        {
            _button3.SetActive(true);
            //close fortune button
            //AFTER-ROUND FUNCTION if pot explodes player++ gets a level 2 ingredient.
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 3)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);
            //close fortune buttom
            //DURING-ROUND FUNCTION first cherry bomb out multiple choice to remove ingredient or leave it. - if remove - add back to bag contents list and undo score changes.
        }
        if (fortuneNum == 4) 
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
            _animatorScript.StartTalking(6);

            choiceOne.text = "trade <sprite name=\"ratTail\"> for <sprite name=\"ruby\">";
            choiceTwo.text = "keep rat tails <sprite name=\"ratTail\">";
            //multiple choice buttons - keep rat tails OR trade each one for a ruby instead
            
            //PRE-ROUND - if trade add rubys if not add rat tails
        }
        if(fortuneNum == 5)
        {
            _button3.SetActive(true);
            //close fortune button
            _animatorScript.StartTalking(6);

            //PRE-ROUND add 2 to cherrybomb limit END OF ROUND Minus two.
        }
        if(fortuneNum==6)
        {
            _button3.SetActive(true);
            //PRE ROUND Double the number of rat tails this round
            _animatorScript.StartTalking(6);
        }
        if(fortuneNum==7)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
           //choiceOne = FindObjectOfType<ChoiceOne>();
           //choiceTwo = FindObjectOfType<ChoiceTwo>();

            choiceOne.text = "take 4 <sprite name=\"VP\">";
            choiceTwo.text = "remove small <sprite name=\"cherrybomb\"> from bag";
            _animatorScript.StartTalking(6);
            //PRE ROUND - take 4 victory points or remove one white chip from your bag
        }
        if(fortuneNum==8)
        {
            _button3.SetActive(true);
            //AFTER ROUND - any player that gets to roll the die this round rolls it twice.
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 9)
        {
            _button3.SetActive(true);
            //PRE ROUND - the player with the fewest victory points recieves one small spider
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 10)
        {
            _button3.SetActive(true);
            //AFTER ROUND - if you reach a scoring space with a ruby this round you get 2 extra victory points, even if your pot explodes.
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 11)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
         

            choiceOne.text = "take 1 <sprite name=\"moth\"> or any medium ingredient";
            choiceTwo.text = "take 3 <sprite name=\"ruby\">";
            _animatorScript.StartTalking(6);
            //PRE ROUND - either open up shop with only medium ingredients and moth or add 3 rubies to players inventory
        }
        if (fortuneNum == 12)
        {
            _button3.SetActive(true);
            //PRE ROUND - adds droplet to everyones potion.
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 13)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
            _animatorScript.StartTalking(6);

            choiceOne.text = "add 2 <sprite name=\"droplet\">";
            choiceTwo.text = "take one ghosts breath <sprite name=\"ghostsbreath\">";
            //PREROUND
        }
        if (fortuneNum == 14)
        {
            _button3.SetActive(true);
            //AFTER ROUND you stopped without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.
            _animatorScript.StartTalking(6);
        }
        if (fortuneNum == 15)
        {
            _button3.SetActive(true);
            //Pre Round All players draw 5 ingredients.The player with the lowest sum takes a medium skull, everyone else gets a ruby.
            _animatorScript.StartTalking(6);

        }
        if (fortuneNum == 16)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
            //hoiceOne = FindObjectOfType<ChoiceOne>();
            //hoiceTwo = FindObjectOfType<ChoiceTwo>();
            _animatorScript.StartTalking(6);


            choiceOne.text = "trade";
            choiceTwo.text = "keep <sprite name=\"ruby\">";
            //PRE ROUND - You can trade 1 ruby for any small ingredient - open shop if choose trade but with only small ingredients
            _animatorScript.StartTalking(6);

        }
        if (fortuneNum == 17)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);

            //DURING ROUND In this round, every pumpkin add an extra 1 to potion. -have this set a boolean true in chippoints that adds 1 in pumpkin if statements
        }
        if (fortuneNum == 18)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);

            //AFTERROUND-the end of the round all flasks get a free refill.
        }
        if (fortuneNum == 19)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);

            // END OF ROUND - "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.
        }
        if (fortuneNum== 20)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);

            // PRE ROUND DICE FUNCTION - "Everyone rolls the victory die once and get the bonus shown."
        }
        if (fortuneNum == 21)
        {
            _button3.SetActive(true) ;
            _animatorScript.StartTalking(6);

            // PRE ROUND - "The player with the fewest rubies receive one ruby."
        }
        if (fortuneNum == 22)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);

            //DURING ROUND MULTIPLE CHOICE BUTTONS APPEAR - After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once."
        }
        if (fortuneNum == 23)
        {
            _button3.SetActive(true);
            _animatorScript.StartTalking(6);

            //PRE ROUND - "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." - shop pops up with all the icons of the ingredients drawn.
        }
      
    }

    public void RefreshFortunes()
    {
        fortunes = new() { "Choose", "If you reach a scoring space with a ruby this round you get an extra ruby.", "If your pot explodes this round the player to your left gets any medium ingredient.", "In this round you may put the first white chip you draw back into the bag.", "Choose: Use your rat tails normally OR pass up on 1-3 rat tails and take that many rubies instead.", "The threshold for white chips is raised from 7 to 9 for this round.", "Double the number of rat tails this round.", "Take 4 victory points OR remove 1 white chip from your bag.", "Any player who gets to roll the die this round rolls it twice.", "The player(s) with the fewest victory points receive 1 small spider.", "If you reach a scoring space with a ruby this round you get two extra victory points, even if your pot explodes.", "Choose", "Add droplet to potion.", "Choose", "Beginning with the start player, if you stop without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.", "All players draw 5 chips. The player with the lowest sum takes a medium skull, everyone else gets a ruby. Put all ingredients back in the bag.", "You can trade 1 ruby for any small ingredient.", "In this round, every pumpkin add an extra 1 to potion.", "At the end of the round all flasks get a free refill.", "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.", "Everyone rolls the victory die once and get the bonus shown.", "The player with the fewest rubies receive one ruby.", "After you have placed the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once.", "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." };
    }
    // do ifs for each index and what the fortune does, if multiple choice if leads to function for all multiple choice ones to let you choose otherwise just a button to press okay. functions for fortunes that effect pre game, mid game or after game that we then call in the game manager.



}
