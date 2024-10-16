using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottleUpPotionTrigger : MonoBehaviour
{
    [SerializeField] private Renderer potionBottle;
    [SerializeField] private Renderer purifier;
    [SerializeField] private AudioSource bigPlop;
    [SerializeField] private AudioSource smallPlop;
    [SerializeField] private AudioSource mediumPlop;
    public ChipPoints chipPoints;
    PlayerData playerData;
    PotionQuality quality;
    
    public async void OnTriggerEnter(Collider other)
    {

        chipPoints = FindObjectOfType<ChipPoints>();
        playerData = FindObjectOfType<PlayerData>();
        quality = FindObjectOfType<PotionQuality>();
        if (other.gameObject.CompareTag("potionBottle") && !quality.IsPotExploded() && quality.nextIngredientTime)
        { 
            Debug.Log("bottled up");
            chipPoints.EndRoundSafely();
            
            potionBottle.material.SetFloat("_Fill", 0.081f);
        }
        if (other.gameObject.CompareTag("potionBottle") && !quality.IsPotExploded() && !quality.nextIngredientTime)
        {
            await chipPoints.MessageAboveCauldron("You MUST put ingredient you pulled from bag into pot before you can bottle it!");
        }
        if (other.gameObject.CompareTag("purifier") && chipPoints.lastIngredient.Contains("cherryBomb") && playerData.PurifierFull.Value == true)
        {
            Debug.Log("purified");
            playerData.PurifierFull.Value = false;
            chipPoints.RemoveLastIngredient();
            purifier.material.SetFloat("_Fill", 0);
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

            await chipPoints.MessageAboveCauldron("The last ingredient in the pot wasn't a cherrybomb AND the purifier bottle is empty, YOU IDIOT.");
        }
        if (other.gameObject.tag.Contains("One") && !other.gameObject.tag.Contains("pumpkin"))
        {
            smallPlop.Play();
        }
        if (other.gameObject.tag.Contains("Two") || other.gameObject.tag.Contains("Three"))
        {
            mediumPlop.Play();
        }
        if (other.gameObject.tag.Contains("Four") || other.gameObject.tag.Contains("pumpkin"))
        {
            bigPlop.Play();
        }
    }

    public void ReserPotionBottle()
    {
        potionBottle.material.SetFloat("_Fill", 0);
    }

    public void ReserPurifier()
    {
        potionBottle.material.SetFloat("_Fill", 0.081f);
    }


}
