using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FortuneNumber : NetworkBehaviour
{
    NetworkVariable <int> NumberGenerated = new NetworkVariable<int>();
    List<int> fortuneNumbers = new();
    public int fortuneNum;
    fortuneTeller _fortuneTeller;
    WinnerManager _winnerManager;

  

    List<string> fortunes = new List<string> { "Choose", "If you reach a scoring space with a ruby this round you get an extra ruby.", "If your pot explodes this round the player to your left gets any medium ingredient.", "In this round you may put the first white chip you draw back into the bag.", "Choose: Use your rat tails normally OR pass up on 1-3 rat tails and take that many rubies instead.", "The threshold for white chips is raised from 7 to 9 for this round.", "Double the number of rat tails this round.", "Take 4 victory points OR remove 1 white chip from your bag.", "Any player who gets to roll the die this round rolls it twice.", "The player(s) with the fewest victory points receive 1 small spider.", "If you reach a scoring space with a ruby this round you get two extra victory points, even if your pot explodes.", "Choose", "Add droplet to potion.", "Choose", "Beginning with the start player, if you stop without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.", "All players draw 5 chips. The player with the lowest sum takes a medium skull, everyone else gets a ruby. Put all ingredients back in the bag.", "You can trade 1 ruby for any small ingredient.", "In this round, every pumpkin add an extra 1 to potion.", "At the end of the round all flasks get a free refill.", "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.", "Everyone rolls the victory die once and get the bonus shown.", "The player with the fewest rubies receive one ruby.", "After you have places the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once.", "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." };
    public async void FortuneNumberGenerator()
    {
        _fortuneTeller = FindObjectOfType<fortuneTeller>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        if (IsHost)
        {
            System.Random rand = new System.Random();
            NumberGenerated.Value = rand.Next(0, fortunes.Count - 1);
            if (fortuneNumbers.Contains(NumberGenerated.Value))
            {
                FortuneNumberGenerator();
                fortunes.RemoveAt(NumberGenerated.Value);
            }
            else
            {
                fortuneNumbers.Add(NumberGenerated.Value);  
            }
        }
        _winnerManager.ReadyUp();
        await _winnerManager.CheckAllPlayersReady();
        fortuneNum = NumberGenerated.Value;
        _winnerManager.ResetReady();
       
        _fortuneTeller.ReadFortune(NumberGenerated.Value);

    }

    public void RefreshFortunes()
    {
        fortunes = new() { "Choose", "If you reach a scoring space with a ruby this round you get an extra ruby.", "If your pot explodes this round the player to your left gets any medium ingredient.", "In this round you may put the first white chip you draw back into the bag.", "Choose: Use your rat tails normally OR pass up on 1-3 rat tails and take that many rubies instead.", "The threshold for white chips is raised from 7 to 9 for this round.", "Double the number of rat tails this round.", "Take 4 victory points OR remove 1 white chip from your bag.", "Any player who gets to roll the die this round rolls it twice.", "The player(s) with the fewest victory points receive 1 small spider.", "If you reach a scoring space with a ruby this round you get two extra victory points, even if your pot explodes.", "Choose", "Add droplet to potion.", "Choose", "Beginning with the start player, if you stop without an explosion, draw up to 5 chips from your bag. You may place 1 of them in your pot.", "All players draw 5 chips. The player with the lowest sum takes a medium skull, everyone else gets a ruby. Put all ingredients back in the bag.", "You can trade 1 ruby for any small ingredient.", "In this round, every pumpkin add an extra 1 to potion.", "At the end of the round all flasks get a free refill.", "If your white chips total exactly 7 at the end of the round you get to add a droplet to your potion.", "Everyone rolls the victory die once and get the bonus shown.", "The player with the fewest rubies receive one ruby.", "After you have placed the first 5 ingredients in your pot, choose to continue OR begin the round all over again – but you get this choice only once.", "Draw 4 chips from your bag/ You may trade in 1 of them for a chip of the same colour with the next higher value. If you can’t make a trade take a small spider. Put all chips back in the bag." };
    }




}
