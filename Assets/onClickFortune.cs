using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class onClickFortune : MonoBehaviour
{
    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    [SerializeField] GameObject button3;
    [SerializeField] GameObject fortune;
    [SerializeField] GameObject BottleUpCollider;
    private PlayerData _playerData;

    public bool buttonOne = false;
    public bool buttonTwo = false;
    public bool buttonThree = false;
    private FortuneManager _fortuneManager;

    private WinnerManager _winnerManager;
    

    // Start is called before the first frame update
    void Start()
    {
        _fortuneManager = FindAnyObjectByType<FortuneManager>();
        _playerData = FindAnyObjectByType<PlayerData>();
       
        _winnerManager = FindObjectOfType<WinnerManager>();
    }

   

    public async void ClickButton1() 
    {
      
        buttonOne = true;
        button1.SetActive(false);
        button2.SetActive(false);
        fortune.SetActive(false);

        await _fortuneManager.PreRoundFortuneEffects();
        _winnerManager.ReadyUp();
        await _winnerManager.CheckAllPlayersReady();
        _winnerManager.ResetReady();
        
        BottleUpCollider.gameObject.SetActive(true);   
            GameManager.Instance.UpdateGameState(GameState.PotionMaking);
        
    }

    public async void ClickButton2()
    {
        
        buttonTwo = true;
        button1.SetActive(false);
        button2.SetActive(false);
        fortune.SetActive(false);
        
        await _fortuneManager.PreRoundFortuneEffects();
        _winnerManager.ReadyUp();
        await _winnerManager.CheckAllPlayersReady();
        _winnerManager.ResetReady();

        BottleUpCollider.gameObject.SetActive(true);
        GameManager.Instance.UpdateGameState(GameState.PotionMaking);
    }

    public async void ClickButton3()
    {
        
        buttonThree = true;
        button3.SetActive(false);
        fortune.SetActive(false);
        
        await _fortuneManager.PreRoundFortuneEffects();

        _winnerManager.ReadyUp();
        await _winnerManager.CheckAllPlayersReady();
        _winnerManager.ResetReady();

        BottleUpCollider.gameObject.SetActive(true);
        GameManager.Instance.UpdateGameState(GameState.PotionMaking);
    }



}
