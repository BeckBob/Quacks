using Oculus.Interaction;
using Oculus.Voice.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DiceScript : MonoBehaviour
{

    [SerializeField] GameObject purpleDice;
    [SerializeField] GameObject blueDice;
    [SerializeField] GameObject yellowDice;
    [SerializeField] GameObject redDice;
    [SerializeField] GameObject redWinnerCanvas;
    [SerializeField] GameObject blueWinnerCanvas;
    [SerializeField] GameObject yellowWinnerCanvas;
    [SerializeField] GameObject purpleWinnerCanvas;
    [SerializeField] GameObject diceFloor;

    PlayerData playerData;
    ChipPoints chipPoints;
    GrabIngredient grabIngredient;
    WinnerManager winnerManager;
    FortuneNumber _fortuneNumber;
    FortuneManager fortuneManager;
    private bool RollTwiceAllowed = true;
    Vector3 speed;
    Vector3 speed2;
    public bool alreadyCalled = false;
    // Start is called before the first frame update



    private async void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Side"))
        {
            playerData = FindObjectOfType<PlayerData>();
            grabIngredient = FindObjectOfType<GrabIngredient>();
            chipPoints = FindObjectOfType<ChipPoints>();
            string colour = playerData.Colour.Value.ToString();
          
            winnerManager = FindObjectOfType<WinnerManager>();
            StartCoroutine((CheckVelocity(other)));
            
            

            if (speed.x == 0f && speed.y == 0f && speed.z == 0f && speed2.x == 0f && speed2.y == 0f && speed2.z == 0f && !alreadyCalled && other.transform.tag == colour)
            {
          
                switch (other.gameObject.name)
                {
                    case "Side1":

                        
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 1;
                        await chipPoints.MessageAboveCauldron("You get 1 Victory point!!");
                         DeactivateDice(colour);

                        break;
                    case "Side2":
                       
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 2;
                        await chipPoints.MessageAboveCauldron("You get 2 Victory points!");
                        DeactivateDice(colour);
                        break;
                    case "Side3":
                        
                        alreadyCalled = true;
                        grabIngredient.AddToBagPermanantly(14);
                        chipPoints.ResetScore();
                        chipPoints.resetScoreText();
                        await chipPoints.MessageAboveCauldron("Pumpkin added to your bag!");
                        DeactivateDice(colour);
                        break;
                    case "Side4":
                        
                        alreadyCalled = true;
                        playerData.Rubies.Value += 1;
                        chipPoints.ChangeRubyUI();
                        await chipPoints.MessageAboveCauldron("You get a Ruby!!");
                        DeactivateDice(colour);
                        break;
                    case "Side5":
                       
                        alreadyCalled = true;
                        chipPoints.AddDroplet();
                        chipPoints.ResetScore();
                        chipPoints.resetScoreText();
                        await chipPoints.MessageAboveCauldron("Added a droplet to your pot!");
                         DeactivateDice(colour);
                        break;
                    case "Side6":
                        
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 1;
                        await chipPoints.MessageAboveCauldron("You get 1 Victory point!");
                        DeactivateDice(colour);
                        break;
                }
            }
        }



    }

    IEnumerator CheckVelocity(Collider other)
    {
        Vector3 velocity = other.attachedRigidbody.velocity;
        speed = velocity;

        yield return new WaitForSeconds(2);

        Vector3 velocity1 = other.attachedRigidbody.velocity;
        speed2 = velocity1;
    }

    private async void DeactivateDice(string DiceColour)
    {
        _fortuneNumber = FindObjectOfType<FortuneNumber>();
        fortuneManager = FindObjectOfType<FortuneManager>();
        if (DiceColour == "Purple")
            {
                purpleDice.SetActive(false);
            }
            if (DiceColour == "Red")
            {
                redDice.SetActive(false);
            }
            if (DiceColour == "Yellow")
            {
                yellowDice.SetActive(false);
            }
            if (DiceColour == "Blue")
            {
                blueDice.SetActive(false);
            }


            purpleWinnerCanvas.SetActive(false);
            redWinnerCanvas.SetActive(false);
            yellowWinnerCanvas.SetActive(false);
            blueWinnerCanvas.SetActive(false);
            //deactivate dice floor.
            alreadyCalled = false;
            if (_fortuneNumber.fortuneNum == 20)
        {
            winnerManager.ReadyUp();
        }
            if (_fortuneNumber.fortuneNum == 8 && RollTwiceAllowed)
            {
                await fortuneManager.RollDiceTwice();
                RollTwiceAllowed = false;
            }
            else
            {
                RollTwiceAllowed = true;
                winnerManager.ReadyUp();
            }
           
        }
    
}
