using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fortuneTeller : MonoBehaviour
{
   
    private TMP_Text _victoryPointText;
    private string _fortuneText;
    void Awake()
    {
      
        _victoryPointText = GetComponent<TMP_Text>();

    }

    public int fortuneNum;

    List<string> fortunes = new List<string> { "Choose: Take any one 4 chip OR 1 victory point for each rat tail you would get this turn", "If you reach a scoring space with a ruby this round you get an extra ruby.", "If your pot explodes this round the player to your left gets any medium ingredient.", "In this round you may put the first white chip you draw back into the bag.", "Choose: Use your rat tails normally OR pass up on 1-3 rat tails and take that many rubies instead.", "The threshold for white chips is raised from 7 to 9 for this round.", "Double the number of rat tails this round.", "Take 4 victory points OR remove 1 white chip from your bag.", "Any player who gets to roll the die this round rolls it twice.", "The player(s) with the fewest victory points receive 1 small spider.", "If you reach a scoring space with a ruby this round you get two extra victory points, even if your pot explodes.", "Choose: take 1 moth or any medium ingredient OR 3 rubies.", "Add droplet to potion.", "Choose: add 2 droplets OR take one Ghosts breath.", "Beginning with the start player, if you stopped without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.", "All players draw 5 chips. The player with the lowest sum takes a medium skull, everyone else gets a ruby. Put all ingredients back in the bag.", "You can trade 1 ruby for any small ingredient.", "In this round, every pumpkin add an extra 1 to potion.", "At the end of the round all flasks get a free refill.", "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.", "Everyone rolls the victory die once and get the bonus shown.", "The player with the fewest rubies receive one ruby.", "After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once.", "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." };
    
  
    private void Start()
    {
        System.Random rand = new System.Random();
        fortuneNum = rand.Next(0, fortunes.Count);

        _fortuneText = fortunes[fortuneNum];

        if (fortuneNum == 0)
        {
            //multipleChoice buttons
            
            //PRE- ROUND FUNCTION let them pick a 1 4 chip for one button that leads to shop layout AND other one adds victory point for every rat tail
        }
        if (fortuneNum == 1)
        {
            //close fortune button
            //AFTER-ROUND FUNCTION give extra ruby is end on ruby space
        }
        if (fortuneNum == 2)
        {
            //close fortune button
            //AFTER-ROUND FUNCTION if pot explodes player++ gets a level 2 ingredient.
        }
        if (fortuneNum == 3)
        {
            //close fortune buttom
            //DURING-ROUND FUNCTION first cherry bomb out multiple choice to remove ingredient or leave it. - if remove - add back to bag contents list and undo score changes.
        }
        if (fortuneNum == 4) 
        { 
            //multiple choice buttons - keep rat tails OR trade each one for a ruby instead
            //PRE-ROUND - if trade add rubys if not add rat tails
        }
        if(fortuneNum == 5)
        {
            //close fortune button
            //PRE-ROUND add 2 to cherrybomb limit END OF ROUND Minus two.
        }

    }

    // do ifs for each index and what the fortune does, if multiple choice if leads to function for all multiple choice ones to let you choose otherwise just a button to press okay. functions for fortunes that effect pre game, mid game or after game that we then call in the game manager.

    
    public void Update()
    {
        
        _victoryPointText.text = _fortuneText;
    }

}
