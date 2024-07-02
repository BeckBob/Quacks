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

    
    private string _fortuneText;
   
   //ublic ChoiceOne _choiceOne;
   //lic ChoiceTwo _choiceTwo;
    void Awake()
    {
      
    

    }

    public int fortuneNumber;

    List<string> fortunes = new List<string> { "Choose", "If you reach a scoring space with a ruby this round you get an extra ruby.", "If your pot explodes this round the player to your left gets any medium ingredient.", "In this round you may put the first white chip you draw back into the bag.", "Choose: Use your rat tails normally OR pass up on 1-3 rat tails and take that many rubies instead.", "The threshold for white chips is raised from 7 to 9 for this round.", "Double the number of rat tails this round.", "Take 4 victory points OR remove 1 white chip from your bag.", "Any player who gets to roll the die this round rolls it twice.", "The player(s) with the fewest victory points receive 1 small spider.", "If you reach a scoring space with a ruby this round you get two extra victory points, even if your pot explodes.", "Choose", "Add droplet to potion.", "Choose", "Beginning with the start player, if you stop without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.", "All players draw 5 chips. The player with the lowest sum takes a medium skull, everyone else gets a ruby. Put all ingredients back in the bag.", "You can trade 1 ruby for any small ingredient.", "In this round, every pumpkin add an extra 1 to potion.", "At the end of the round all flasks get a free refill.", "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.", "Everyone rolls the victory die once and get the bonus shown.", "The player with the fewest rubies receive one ruby.", "After you have placed the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once.", "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." };

  
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
           
            choiceOne.text = "Pick one ingredient worth 4 points";
            choiceTwo.text = "trade rat tails for victory points";
            Debug.Log(choiceOne.text);

            //PRE- ROUND FUNCTION let them pick a 1 4 chip for one button that leads to shop layout AND other one adds victory point for every rat tail
        }
        if (fortuneNum == 1)
        {
            _button3.SetActive(true);
            //close fortune button
            //AFTER-ROUND FUNCTION give extra ruby is end on ruby space
        }
        if (fortuneNum == 2)
        {
            _button3.SetActive(true);
            //close fortune button
            //AFTER-ROUND FUNCTION if pot explodes player++ gets a level 2 ingredient.
        }
        if (fortuneNum == 3)
        {
            _button3.SetActive(true);
            //close fortune buttom
            //DURING-ROUND FUNCTION first cherry bomb out multiple choice to remove ingredient or leave it. - if remove - add back to bag contents list and undo score changes.
        }
        if (fortuneNum == 4) 
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
           

            choiceOne.text = "trade rat tails for rubies";
            choiceTwo.text = "keep rat tails";
            //multiple choice buttons - keep rat tails OR trade each one for a ruby instead
            
            //PRE-ROUND - if trade add rubys if not add rat tails
        }
        if(fortuneNum == 5)
        {
            _button3.SetActive(true);
            //close fortune button
        
            //PRE-ROUND add 2 to cherrybomb limit END OF ROUND Minus two.
        }
        if(fortuneNum==6)
        {
            _button3.SetActive(true);
            //PRE ROUND Double the number of rat tails this round
        }
        if(fortuneNum==7)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
           //choiceOne = FindObjectOfType<ChoiceOne>();
           //choiceTwo = FindObjectOfType<ChoiceTwo>();

            choiceOne.text = "take 4 victory points";
            choiceTwo.text = "remove one white chip from bag";
            //PRE ROUND - take 4 victory points or remove one white chip from your bag
        }
        if(fortuneNum==8)
        {
            _button3.SetActive(true);
            //AFTER ROUND - any player that gets to roll the die this round rolls it twice.
        }
        if (fortuneNum == 9)
        {
            _button3.SetActive(true);
            //PRE ROUND - the player with the fewest victory points recieves one small spider
        }
        if (fortuneNum == 10)
        {
            _button3.SetActive(true);
            //AFTER ROUND - if you reach a scoring space with a ruby this round you get 2 extra victory points, even if your pot explodes.
        }
        if (fortuneNum == 11)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
         

            choiceOne.text = "take 1 moth or any medium ingredient";
            choiceTwo.text = "take 3 rubies";
            //PRE ROUND - either open up shop with only medium ingredients and moth or add 3 rubies to players inventory
        }
        if (fortuneNum == 12)
        {
            _button3.SetActive(true);
            //PRE ROUND - adds droplet to everyones potion.
        }
        if (fortuneNum == 13)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
        

            choiceOne.text = "add 2 droplets";
            choiceTwo.text = "take one ghosts breath";
            //PREROUND
        }
        if (fortuneNum == 14)
        {
            _button3.SetActive(true);
            //AFTER ROUND you stopped without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.
        }
        if (fortuneNum == 15)
        {
            _button3.SetActive(true);
            //Pre Round All players draw 5 ingredients.The player with the lowest sum takes a medium skull, everyone else gets a ruby.
        }
        if (fortuneNum == 16)
        {
            _button1.SetActive(true);
            _button2.SetActive(true);
            //hoiceOne = FindObjectOfType<ChoiceOne>();
            //hoiceTwo = FindObjectOfType<ChoiceTwo>();

            choiceOne.text = "trade";
            choiceTwo.text = "keep rubies";
            //PRE ROUND - You can trade 1 ruby for any small ingredient - open shop if choose trade but with only small ingredients
        }
        if (fortuneNum == 17)
        {
            _button3.SetActive(true);
            //DURING ROUND In this round, every pumpkin add an extra 1 to potion. -have this set a boolean true in chippoints that adds 1 in pumpkin if statements
        }
        if (fortuneNum == 18)
        {
            _button3.SetActive(true);
            //AFTERROUND-the end of the round all flasks get a free refill.
        }
        if(fortuneNum == 19)
        {
            _button3.SetActive(true);
            // END OF ROUND - "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.
        }
        if(fortuneNum== 20)
        {
            _button3.SetActive(true);
            // PRE ROUND DICE FUNCTION - "Everyone rolls the victory die once and get the bonus shown."
        }
        if(fortuneNum == 21)
        {
            _button3.SetActive(true) ;
            // PRE ROUND - "The player with the fewest rubies receive one ruby."
        }
        if(fortuneNum == 22)
        {
            _button3.SetActive(true);
            //DURING ROUND MULTIPLE CHOICE BUTTONS APPEAR - After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once."
        }
        if(fortuneNum == 23)
        {
            _button3.SetActive(true);
            //PRE ROUND - "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." - shop pops up with all the icons of the ingredients drawn.
        }
        Debug.Log("buttons should be active");
    }

    // do ifs for each index and what the fortune does, if multiple choice if leads to function for all multiple choice ones to let you choose otherwise just a button to press okay. functions for fortunes that effect pre game, mid game or after game that we then call in the game manager.



}
