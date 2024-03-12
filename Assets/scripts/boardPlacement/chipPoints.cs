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

    private ingredientEffects _ingredientEffects;

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

    public int mushroomRule = 1;

    public int extraPoints;

    public int extraRubies = 0;

    public void OnTriggerEnter(Collider other)
    {
        ingredientsList.Add(other.gameObject.tag);
        extraPoints = 0; extraRubies = 0;
        if (other.gameObject.tag.Contains("mushroom") && mushroomRule == 1 && ingredientsList.Count >= 2 && ingredientsList[ingredientsList.Count - 2].Contains("cherryBomb"))
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



