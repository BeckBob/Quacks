using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottleUpPotionTrigger : MonoBehaviour
{

    public ChipPoints chipPoints;
    PlayerData playerData;
    public async void OnTriggerEnter(Collider other)
    {

        chipPoints = FindObjectOfType<ChipPoints>();
        playerData = FindObjectOfType<PlayerData>();
        if (other.gameObject.CompareTag("potionBottle"))
        { 
            Debug.Log("bottled up");
            chipPoints.EndRoundSafely();
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("purifier") && chipPoints.lastIngredient.Contains("cherryBomb") && playerData.PurifierFull.Value == true)
        {
            Debug.Log("purified");
            playerData.PurifierFull.Value = false;
            chipPoints.RemoveLastIngredient();
            await chipPoints.MessageAboveCauldron("You removed the last cherrybomb in the pot");

        }
        if (other.gameObject.CompareTag("purifier") && !chipPoints.lastIngredient.Contains("cherryBomb") && playerData.PurifierFull.Value == true)
        {

            await chipPoints.MessageAboveCauldron("SILLY CHILD! The last ingredient in the pot wasn't a cherrybomb");
        }
        if (other.gameObject.CompareTag("purifier") && chipPoints.lastIngredient.Contains("cherryBomb") && playerData.PurifierFull.Value == false)
        {

            await chipPoints.MessageAboveCauldron("FOOL! Purifier bottle is empty!");
        }
        if (other.gameObject.CompareTag("purifier") && !chipPoints.lastIngredient.Contains("cherryBomb") && playerData.PurifierFull.Value == false)
        {

            await chipPoints.MessageAboveCauldron("The last ingredient in the pot wasn't a cherrybomb and purifier bottle is empty, YOU IDIOT.");
        }
    }
}
