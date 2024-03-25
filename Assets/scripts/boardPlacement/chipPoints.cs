using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly_CSharp.boardPlacement;
using pointSystem;
using UnityEngine.Events;
using TMPro;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation.XRDeviceSimulator;
using static UnityEditor.VersionControl.Asset;

public class chipPoints : MonoBehaviour
{
    private Program programInstance;

    public PotionQuality quality;
    public UnityEvent OnScoreChanged;

    private void Start()
    {
        // Initialize programInstance
        programInstance = new Program();

        

    }
    public int Score;
    public int Coins { get; private set; }
    public int VictoryPoints { get; private set; }
    public bool RubiesThisRound { get; private set; }

    List<string> ingredientsList = new List<string>();

    public int mushroomRule = 3;
    public int mandrakeRule = 4;
    public int crowSkullRule = 2;
    public int gardenSpiderRule = 1;
    public int hawkMothRule = 1;
    public int ghostsBreathRule = 4;

    public int extraPoints;
    public int extraRubies = 0;
    public int extraVictoryPoints = 0;

    private int pumpkins = 0;
    private int mandrakes = 0;

    public void OnTriggerEnter(Collider other)
    {
        ingredientsList.Add(other.gameObject.tag);
        extraPoints = 0; extraRubies = 0;
        if (mushroomRule == 1)
        {
            if (other.gameObject.tag.Contains("mushroom") && ingredientsList.Count >= 2 && ingredientsList[ingredientsList.Count - 2].Contains("cherryBomb"))
            {

                if (ingredientsList[ingredientsList.Count - 2].Contains("One"))
                {
                    extraPoints = 1;
                }
                else if (ingredientsList[ingredientsList.Count - 2].Contains("Two"))
                {
                    extraPoints = 2;
                }
                else if (ingredientsList[ingredientsList.Count - 2].Contains("Three"))
                {
                    extraPoints = 3;
                }
                
            }
        }

        else if (mushroomRule == 2)
        {
            if (other.gameObject.tag.Contains("cherryBombOne") && (ingredientsList.Contains("mushroomOne") || ingredientsList.Contains("mushroomTwo") || ingredientsList.Contains("mushroomFour")))
            {
                extraPoints = 1;
                
            }
        }

        else if (mushroomRule == 3)
        {
            if (other.gameObject.CompareTag("pumpkinOne"))
            { pumpkins += 1; }
           
            if (other.gameObject.tag.Contains("mushroom") && pumpkins < 3)
                {
                extraPoints = 1;
            }
            if (other.gameObject.tag.Contains("mushroom") && pumpkins >= 3)
            {
                extraPoints = 2;
            }
        }
        //mushroom rule 4 put mushroom chip to side TO remove from end of array put in special pot that gets called at the end of the round and player gets given a yes or no question to put it on - also maybe put in thing to show what the player gets if they move one more space

        if (mandrakeRule == 1)
        {
            if (ingredientsList.Count >= 2 && ingredientsList[ingredientsList.Count - 2].Contains("mandrake"))
            {
                if (other.gameObject.tag.Contains("One"))
                {
                    extraPoints = 1;
                }
                else if (other.gameObject.tag.Contains("Two"))
                {
                    extraPoints = 2;
                }
                else if(other.gameObject.tag.Contains("Three"))
                {
                    extraPoints = 3;
                }
                else if (other.gameObject.tag.Contains("Four"))
                {
                    extraPoints = 4;
                }
            }
        }

        if (mandrakeRule == 2)
            if (other.gameObject.tag.Contains("mandrake") && ingredientsList[ingredientsList.Count - 2].Contains("cherryBomb"))
            {
                ingredientsList.RemoveAt(ingredientsList.Count - 2);
                // also need to add back to players bag but haven't created that yet.
            }

        if (mandrakeRule == 3)
        {
            if (other.gameObject.tag.Contains("mandrake"))
            {
                mandrakes++;
                if (mandrakes == 1 || mandrakes == 3)
                { quality.AddToCherryBombLimit();  }

            }
        }

        if (mandrakeRule == 4)
        {
            if (other.gameObject.tag.Contains("mandrake"))
            {
                mandrakes++;
                if (mandrakes == 1)
                { extraPoints = 1; }
                if (mandrakes == 2)
                { extraPoints = 2; }
                if (mandrakes >= 3)
                { extraPoints = 3; }

            }
        }


        Chips[] boardPlacement = programInstance.GetBoardPlacement();
     

        if (boardPlacement != null)
        {
            if (other.gameObject.tag.Contains("One"))
            {
                Score += 1;
                Score += extraPoints;
              
            }
            else if (other.gameObject.tag.Contains("Two"))
            {
                Score += 2;
                Score += extraPoints;
               
            }
            else if (other.gameObject.tag.Contains("Three"))
            {
                Score += 3;
                Score += extraPoints;

                
            }
            else if (other.gameObject.tag.Contains("Four"))
            {
                Score += 4;
                Score += extraPoints;

                
            }

            Coins = boardPlacement[(Score - 1)].Coins;
            VictoryPoints = boardPlacement[(Score - 1)].VictoryPoints;
            RubiesThisRound = boardPlacement[(Score - 1)].Ruby;


            if (crowSkullRule == 1)
            {
                if (other.gameObject.tag.Contains("crowSkull") && RubiesThisRound == true)
                { extraRubies++;}
            }

            if (crowSkullRule == 2)
            {
                if (other.gameObject.tag.Contains("crowSkull") && RubiesThisRound == true)
                {
                    if (other.gameObject.tag.Contains("One"))
                    {
                        extraVictoryPoints += 1;
                    }

                    if (other.gameObject.tag.Contains("Two"))
                    {
                        extraVictoryPoints += 2;
                    }
                    if (other.gameObject.tag.Contains("Four"))
                    {
                        extraVictoryPoints += 4;
                    }
                }
            }

            if (crowSkullRule == 3)
            {
               //If the pot explodes within the next 1 / 2 / 4 chips, you get victory points AND money during the evaluation phase(but no victory die roll) S = 5, M = 10, L = 19. - need to impliment rounds system before doing this one.
            }

            if (crowSkullRule == 4)
            {
                //Draw 1 / 2 / 4 chips from your bad. You MAY place 1 of them in your pot. S = 5, M = 10, L = 19. - need to impliment bag system before i can add this rule.
            }

            if (gardenSpiderRule == 1)
            {
                //For each green chip that is the LAST or NEXT - TO - LAST chip in your pot, you may pay 1 ruby to move your droplet 1 space.S COSTS 4, M = 8, L = 14 - need to impliment end of round mechanisims first
            }

            if (gardenSpiderRule == 2)
            {
                //If your white chips add up to exactly 7, add up the value the green chips in your pot and move the last chip forward that many spaces.S COSTS 6, M = 11, L = 21. - need to impliment after round mechanics first
            }

            if (gardenSpiderRule == 3)
            {
                //For each green chip that is the LAST or NEXT - TO - LAST chip in your pot, you receive one ruby.S COSTS 4, M = 8, L = 14. - need to impliment after round mechanics first
            }

            if (gardenSpiderRule == 4)
            {
                //For each green chip that is the LAST or NEXT-TO-LAST chip In your post take one of the indicated ingredients – 1 = pumpkin 2= skull/mushroom 3=mandrake/GhostsBreath. S COSTS 6, M = 11 L = 18 - need to impliment round mechanics first
            }
            if (hawkMothRule == 1)
            {
                //If you draw more black chips than one of the players sitting next to you tear drop moves forward – more than both players next to you is teardrop and ruby - need to impliment end of round mechanics first
            }
            if (hawkMothRule == 2)
            {
                //If you draw as many as the other player teardrop forward(WHEN ONLY TWO PLAYERS)-more than them teardrop and ruby.
            }

            if (ghostsBreathRule == 1)
            {
                //For 1, 2 or 3 purple chips, you receive the indicated bonus.  1 = 1 victory point 2 = victory point and ruby 3 = 2 victory points and teardrop forward one space.COSTS 9. - END OF ROUND
            }   
            if(ghostsBreathRule == 2)
            {
                //You may exchange the purple chips in your pot for the indicated bonus 1 = Moth, victory point and ruby 2 = small spider medium skull 3 victory points and space forward. 3 = large mandrake 6 victory points a ruby and 2 spaces forward. COSTS 12 - END OF ROUND
            }
            if(ghostsBreathRule == 3)
            {
                // Trade 1 chip from the pot for another chip of the same colour with a greater value according to the chart. 1 = small to medium 2 = medium to large 3 = small to large COSTS 11 - END OF ROUND
            }
            if (ghostsBreathRule == 4)
            {

                if (other.gameObject.tag.Contains("ghost"))

                {
                    
                    if (Coins >= 10 && Coins < 20)
                    { extraVictoryPoints++; }
                    if (Coins >= 20 && Coins < 30)
                    { extraVictoryPoints += 2; }
                    if (Coins >= 30)
                    { extraVictoryPoints += 3; }
                }
            }


            OnScoreChanged.Invoke();
        }
    }
}



