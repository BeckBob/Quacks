using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.FortuneTeller);
    }
    public void UpdateGameState(GameState newState) 
    {  
        State = newState;

        switch (newState)
        {
            case GameState.FortuneTeller:
                HandleFortune();
                break;
            case GameState.RatTails:
                break;
            case GameState.PotionMaking:
                break;
            case GameState.EndOfRoundEffects:
                break;
            case GameState.ifPotExploded:
                break;
            case GameState.RollDice:
                break;
            case GameState.AddStuffFromRound:
                break;
            case GameState.BuyIngredients:
                break;
            case GameState.RepeatFor8Rounds:
                break;
            case GameState.DeclareWinner:
                break;
                
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleFortune()
    {

    }

}

public enum GameState
{
    FortuneTeller,
    RatTails,
    PotionMaking,
    EndOfRoundEffects,
    ifPotExploded,
    RollDice,
    AddStuffFromRound,
    BuyIngredients,
    RepeatFor8Rounds,
    DeclareWinner
}
