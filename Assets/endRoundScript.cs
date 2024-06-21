using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndRoundScript : MonoBehaviour
{
    [SerializeField] GameObject futureScore;
    [SerializeField] GameObject potExplodedCanvas;
    
    private PlayerData _playerData;
    
    private VictoryPointComponenet _victoryPointComponenet;
    private CoinComponenet _coinComponenet;
    private ChipPoints _chipPoints;

    //TWO FUNCTIONS
    public void PotExplosionEndRound()
    {
        futureScore.SetActive(false);
        potExplodedCanvas.SetActive(true);
        Debug.Log("potionExplosionendround");

        //afterRoundChipEffects
    }
    //end round from pot explosions, activate buttons so player chooses between coins and victory points, after choice send what they've chosen to player data. check if all players are done and move to after round effects. THEY CANNOT WIN EVEN WITH HIGHEST SCORE

    public void ChooseVictoryPoints()
    {
        _playerData = FindObjectOfType<PlayerData>();
        
        _victoryPointComponenet = FindObjectOfType<VictoryPointComponenet>();
        int victoryPoints = _victoryPointComponenet._victoryPoints;
    
        Debug.Log(victoryPoints);
        _playerData.VictoryPoints.Value += victoryPoints; 
        //NetworkManager.Singleton.ConnectedClients[]
        Debug.Log(_playerData.VictoryPoints.Value);
        potExplodedCanvas.SetActive(false);
    }

    public void ChooseCoins()
    {
        _playerData = FindObjectOfType<PlayerData>();
        _coinComponenet = FindObjectOfType<CoinComponenet>();
        _chipPoints = FindObjectOfType<ChipPoints>();

        int coinstwo = _chipPoints.Coins;
        Debug.Log(coinstwo);
        int coins = _coinComponenet._coins;
        Debug.Log(_coinComponenet._coins);
        _playerData.Coins.Value += coins;
        Debug.Log(coins);
        potExplodedCanvas.SetActive(false);
    }


    public void EndRoundSafely()
    {
        //afterRoundfFortuneEffects
        //afterRoundChipEffects

        _playerData = FindObjectOfType<PlayerData>();
        _victoryPointComponenet = FindObjectOfType<VictoryPointComponenet>();
        _coinComponenet = FindObjectOfType<CoinComponenet>();

        int victoryPoints = _victoryPointComponenet._victoryPoints;
        _playerData.VictoryPoints.Value += victoryPoints;
        int coins = _coinComponenet._coins;
        _playerData.Coins.Value += coins;
        Debug.Log("endedRoundSafely");

    }
    //end round from filling bottle with potion - send what they get to player data - compare to other, non exploded players and whoever has top score gets to roll dice. end round and move on to after round effects.
}
