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

public class chipPoints : MonoBehaviour
{
    private Program programInstance;


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
    public int mandrakeRule = 1;

    public int extraPoints;

    public int extraRubies = 0;

    private int pumpkins = 0;

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
                UnityEngine.Debug.Log(extraPoints);
            }
        }

        else if (mushroomRule == 2)
        {
            if (other.gameObject.tag.Contains("cherryBombOne") && (ingredientsList.Contains("mushroomOne") || ingredientsList.Contains("mushroomTwo") || ingredientsList.Contains("mushroomFour")))
            {
                extraPoints = 1;
                UnityEngine.Debug.Log(extraPoints);
            }
        }

        else if (mushroomRule == 3)
        {
            if (other.gameObject.CompareTag("pumpkinOne"))
            { pumpkins += 1; }
            UnityEngine.Debug.Log(pumpkins);
            if (other.gameObject.tag.Contains("mushroom") && pumpkins < 3)
                {
                extraPoints = 1;
            }
            if (other.gameObject.tag.Contains("mushroom") && pumpkins >= 3)
            {
                extraPoints = 2;
            }
        }
        //mushroom rule 3 put mushroom chip to side SO remove from end of array put in special pot that gets called at the end of the round and player gets given a yes or no question to put it on - also maybe put in thing to show what the player gets if they move one more space

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

        Chips[] boardPlacement = programInstance.GetBoardPlacement();
     

        if (boardPlacement != null)
        {
            if (other.gameObject.tag.Contains("One"))
            {
                Score += 1;
                Score += extraPoints;
                UnityEngine.Debug.Log(extraPoints);
            }
            else if (other.gameObject.tag.Contains("Two"))
            {
                Score += 2;
                Score += extraPoints;
                UnityEngine.Debug.Log(extraPoints);
            }
            else if (other.gameObject.tag.Contains("Three"))
            {
                Score += 3;
                Score += extraPoints;

                UnityEngine.Debug.Log(extraPoints);
            }
            else if (other.gameObject.tag.Contains("Four"))
            {
                Score += 4;
                Score += extraPoints;

                UnityEngine.Debug.Log(extraPoints);
            }

            Coins = boardPlacement[(Score - 1)].Coins;
            VictoryPoints = boardPlacement[(Score - 1)].VictoryPoints;
            RubiesThisRound = boardPlacement[(Score - 1)].Ruby;
            
           
            OnScoreChanged.Invoke();
        } 
    }

    

 

}



