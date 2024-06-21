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

    PlayerData playerData;
    ChipPoints chipPoints;
    GrabIngredient grabIngredient;
    WinnerManager winnerManager;
    Vector3 speed;
    public bool alreadyCalled = false;
    // Start is called before the first frame update

    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Side")){ 
        playerData = FindObjectOfType<PlayerData>();
        grabIngredient = FindObjectOfType<GrabIngredient>();
        chipPoints = FindObjectOfType<ChipPoints>();
        winnerManager = FindObjectOfType<WinnerManager>();
        Vector3 velocity = other.attachedRigidbody.velocity;
            speed = velocity;
        
        if (speed.x == 0f && speed.y == 0f && speed.z == 0f && !alreadyCalled)
        {
                switch (other.gameObject.name)
                {
                    case "Side1":
                        Debug.Log("plus one victory point");
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 1;
                        FunctionTimer.Create(() => DeactivateDice(), 5f);

                        break;
                    case "Side2":
                        Debug.Log("Add two victory points");
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 2;
                        FunctionTimer.Create(() => DeactivateDice(), 5f);
                        break;
                    case "Side3":
                        Debug.Log("Added pumpkin to bag");
                        alreadyCalled = true;
                        grabIngredient.AddToBagPermanantly(14);
                        FunctionTimer.Create(() => DeactivateDice(), 5f);
                        break;
                    case "Side4":
                        Debug.Log("Add ruby");
                        alreadyCalled = true;
                        playerData.Rubies.Value += 1;
                        FunctionTimer.Create(() => DeactivateDice(), 5f);
                        break;
                    case "Side5":
                        Debug.Log("Add droplet");
                        alreadyCalled = true;
                        chipPoints.AddDroplet();
                        FunctionTimer.Create(() => DeactivateDice(), 5f);
                        break;
                    case "Side6":
                        Debug.Log("Add one victory point");
                        alreadyCalled = true;
                        playerData.VictoryPoints.Value += 1;
                        FunctionTimer.Create(() => DeactivateDice(), 5f);
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

    private async void DeactivateDice()
    {
        yellowDice.SetActive(false);
        blueDice.SetActive(false);
        redDice.SetActive(false);
        purpleDice.SetActive(false);

        purpleWinnerCanvas.SetActive(false);
        redWinnerCanvas.SetActive(false);
        yellowWinnerCanvas.SetActive(false);
        blueWinnerCanvas.SetActive(false);
        //deactivate dice floor.
        alreadyCalled = false;
        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();
        chipPoints.CheckMoths();
    }
}
