using Oculus.Interaction;
using Oculus.Voice.Windows;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log(colour);
         
            Debug.Log(other.transform.tag);
            winnerManager = FindObjectOfType<WinnerManager>();
            Vector3 velocity = other.attachedRigidbody.velocity;
            speed = velocity;
            Debug.Log(velocity);

            if (speed.x == 0f && speed.y == 0f && speed.z == 0f && !alreadyCalled && other.transform.tag == colour)
            {
                Debug.Log("past if in dice");
                switch (other.gameObject.name)
                {
                    case "Side1":

                        Debug.Log("plus one victory point");
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 1;
                        await chipPoints.MessageAboveCauldron("You get 1 Victory point!!");
                         DeactivateDice(colour);

                        break;
                    case "Side2":
                        Debug.Log("Add two victory points");
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 2;
                        await chipPoints.MessageAboveCauldron("You get 2 Victory points!");
                        DeactivateDice(colour);
                        break;
                    case "Side3":
                        Debug.Log("Added pumpkin to bag");
                        alreadyCalled = true;
                        grabIngredient.AddToBagPermanantly(14);
                        chipPoints.ResetScore();
                        chipPoints.resetScoreText();
                        await chipPoints.MessageAboveCauldron("Pumpkin added to your bag!");
                        DeactivateDice(colour);
                        break;
                    case "Side4":
                        Debug.Log("Add ruby");
                        alreadyCalled = true;
                        playerData.Rubies.Value += 1;
                        chipPoints.ChangeRubyUI();
                        await chipPoints.MessageAboveCauldron("You get a Ruby!!");
                        DeactivateDice(colour);
                        break;
                    case "Side5":
                        Debug.Log("Add droplet");
                        alreadyCalled = true;
                        chipPoints.AddDroplet();
                        chipPoints.ResetScore();
                        chipPoints.resetScoreText();
                        await chipPoints.MessageAboveCauldron("Added a droplet to your pot!");
                         DeactivateDice(colour);
                        break;
                    case "Side6":
                        Debug.Log("Add one victory point");
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 1;
                        await chipPoints.MessageAboveCauldron("You get 1 Victory point!");
                        DeactivateDice(colour);
                        break;
                }
            }
        }



    }

    private void CheckVelocity(Collider other)
    {
        Vector3 velocity = other.attachedRigidbody.velocity;
        speed = velocity;
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
            return; 
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
