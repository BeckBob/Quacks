using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BuyIngredients : MonoBehaviour
{
    [SerializeField] GameObject initialIngredients;
    [SerializeField] GameObject MandrakeStallStuff;
    [SerializeField] GameObject ghostsBreathStallStuff;
    [SerializeField] GameObject coinsBoard;
    [SerializeField] GameObject bagBoard;
    [SerializeField] GameObject purpleSphere;
    [SerializeField] GameObject purpleBagSphere;

    [SerializeField] TextMeshProUGUI mandrakeSmallPrice;
    [SerializeField] TextMeshProUGUI mandrakeMediumPrice;
    [SerializeField] TextMeshProUGUI mandrakeLargePrice;
    [SerializeField] TextMeshProUGUI toadstallSmallPrice;
    [SerializeField] TextMeshProUGUI toadstallMediumPrice;
    [SerializeField] TextMeshProUGUI toadstallLargePrice;
    [SerializeField] TextMeshProUGUI pumpkinPrice;
    [SerializeField] TextMeshProUGUI ghostsBreathPrice;
    [SerializeField] TextMeshProUGUI crowSkullSmallPrice;
    [SerializeField] TextMeshProUGUI crowSkullMediumPrice;
    [SerializeField] TextMeshProUGUI crowSkullLargePrice;
    [SerializeField] TextMeshProUGUI mothPrice;
    [SerializeField] TextMeshProUGUI gardenSpiderSmallPrice;
    [SerializeField] TextMeshProUGUI gardenSpiderMediumPrice;
    [SerializeField] TextMeshProUGUI gardenSpiderLargePrice;

    [SerializeField] TextMeshProUGUI coinsLeft;
    TeleportationManager _teleportationManager;
    GrabIngredient _grabIngredient;
    PlayerData _playerData;

    [SerializeField] GameObject stallSphere;

    [SerializeField] TextMeshProUGUI pumpkinAmount;
    [SerializeField] TextMeshProUGUI toadstallAmount;
    [SerializeField] TextMeshProUGUI crowSkullAmount;
    [SerializeField] TextMeshProUGUI mothAmount;
    [SerializeField] TextMeshProUGUI spiderAmount;
    [SerializeField] TextMeshProUGUI mandrakeAmount;
    [SerializeField] TextMeshProUGUI ghostsAmount;
    [SerializeField] TextMeshProUGUI cherryBombAmount;

    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    [SerializeField] GameObject button3;
  
    [SerializeField] TextMeshProUGUI aboveCauldronText;
    [SerializeField] TextMeshProUGUI choiceOne;
    [SerializeField] TextMeshProUGUI choiceTwo;
    [SerializeField] TextMeshProUGUI choiceThree;

    [SerializeField] GameObject aboveCauldronCanvas;

    private ChipPoints _chipPoints;
    private WinnerManager _winnerManager;

    // Start is called before the first frame update
    void Start()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _winnerManager = FindObjectOfType<WinnerManager>();

        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _playerData = FindObjectOfType<PlayerData>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpStall()
    {
        purpleSphere.SetActive(true);
        purpleBagSphere.SetActive(true);
        _chipPoints = FindObjectOfType<ChipPoints>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        
        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _playerData = FindObjectOfType<PlayerData>();
        initialIngredients.SetActive(true);
        coinsBoard.SetActive(true);
        bagBoard.SetActive(true);
        UpdateAmountInBag();
        if (_winnerManager.round >= 3)
        {
            MandrakeStallStuff.SetActive(true);
        }
        if (_winnerManager.round >= 4)
        {
           ghostsBreathStallStuff.SetActive(true);
        }
        if (_chipPoints.mushroomRule == 1)
        {
            toadstallSmallPrice.text = "5";
            toadstallMediumPrice.text = "9";
            toadstallLargePrice.text = "15";
        }
        if (_chipPoints.mushroomRule == 2)
        {
            toadstallSmallPrice.text = "7";
            toadstallMediumPrice.text = "11";
            toadstallLargePrice.text = "17";
        }
        if (_chipPoints.mushroomRule == 3)
        {
            toadstallSmallPrice.text = "6";
            toadstallMediumPrice.text = "10";
            toadstallLargePrice.text = "16";
        }
        if (_chipPoints.mushroomRule == 4)
        {
            toadstallSmallPrice.text = "4";
            toadstallMediumPrice.text = "8";
            toadstallLargePrice.text = "14";
        }


        pumpkinPrice.text = "3";


        if (_chipPoints.ghostsBreathRule == 1)
        {
            ghostsBreathPrice.text = "9";

        }
        if (_chipPoints.ghostsBreathRule == 2)
        {
            ghostsBreathPrice.text = "12";

        }
        if (_chipPoints.ghostsBreathRule == 3)
        {
            ghostsBreathPrice.text = "11";

        }
        if (_chipPoints.ghostsBreathRule == 4)
        {
            ghostsBreathPrice.text = "10";

        }

        if (_chipPoints.crowSkullRule == 1)
        {
            crowSkullSmallPrice.text = "4";
            crowSkullMediumPrice.text = "8";
            crowSkullLargePrice.text = "14";
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4)
        {
            crowSkullSmallPrice.text = "5";
            crowSkullMediumPrice.text = "10";
            crowSkullLargePrice.text = "19";
        }

        mothPrice.text = "10";

        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3)
        {
            gardenSpiderSmallPrice.text = "4";
            gardenSpiderMediumPrice.text = "8";
            gardenSpiderLargePrice.text = "14";

        }
        if (_chipPoints.gardenSpiderRule == 2)
        {
            gardenSpiderSmallPrice.text = "6";
            gardenSpiderMediumPrice.text = "11";
            gardenSpiderLargePrice.text = "21";

        }
        if (_chipPoints.gardenSpiderRule == 4)
        {
            gardenSpiderSmallPrice.text = "6";
            gardenSpiderMediumPrice.text = "11";
            gardenSpiderLargePrice.text = "18";

        }
        if (_chipPoints.mandrakeRule == 1)
        {
            toadstallSmallPrice.text = "9";
            toadstallMediumPrice.text = "13";
            toadstallLargePrice.text = "19";
        }
        if (_chipPoints.mandrakeRule >= 2)
        {
            toadstallSmallPrice.text = "8";
            toadstallMediumPrice.text = "12";
            toadstallLargePrice.text = "18";
        }
        UpdateCoinText();
        _teleportationManager.ShopTeleportation();
    }

    public void BuySmallToadstall()
    {
        _grabIngredient.AddToBagPermanantly(12);
        if (_chipPoints.mushroomRule == 1 && _playerData.Coins.Value >= 5)
        {
            _playerData.Coins.Value -= 5;
        }
        if (_chipPoints.mushroomRule == 2 && _playerData.Coins.Value >= 7)
        {
            _playerData.Coins.Value -= 7;
        }
        if (_chipPoints.mushroomRule == 3 && _playerData.Coins.Value >= 6)
        {
            _playerData.Coins.Value -= 6;
        }
        if (_chipPoints.mushroomRule == 4 && _playerData.Coins.Value >= 4)
        {
            _playerData.Coins.Value -= 4;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player

        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuyMediumToadstall()
    {
        _grabIngredient.AddToBagPermanantly(13);
        if (_chipPoints.mushroomRule == 1 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 9;
        }
        if (_chipPoints.mushroomRule == 2 && _playerData.Coins.Value >= 11)
        {
            _playerData.Coins.Value -= 11;
        }
        if (_chipPoints.mushroomRule == 3 && _playerData.Coins.Value >= 10)
        {
            _playerData.Coins.Value -= 10;
        }
        if (_chipPoints.mushroomRule == 4 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuyLargeToadstall()
    {
        _grabIngredient.AddToBagPermanantly(11);
        if (_chipPoints.mushroomRule == 1 && _playerData.Coins.Value >= 15)
        {
            _playerData.Coins.Value -= 15;
        }
        if (_chipPoints.mushroomRule == 2 && _playerData.Coins.Value >= 17)
        {
            _playerData.Coins.Value -= 17;
        }
        if (_chipPoints.mushroomRule == 3 && _playerData.Coins.Value >= 16)
        {
            _playerData.Coins.Value -= 16;
        }
        if (_chipPoints.mushroomRule == 4 && _playerData.Coins.Value >= 14)
        {
            _playerData.Coins.Value -= 14;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuyPumpkin()
    {
        _grabIngredient.AddToBagPermanantly(14);
        if (_playerData.Coins.Value >= 3)
        {
            _playerData.Coins.Value -= 3;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuySmallCrowSkull()
    {
        _grabIngredient.AddToBagPermanantly(4);
        if (_chipPoints.crowSkullRule == 1 && _playerData.Coins.Value >= 4)
        {
            _playerData.Coins.Value -= 4;
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4 && _playerData.Coins.Value >= 5)
        {
            _playerData.Coins.Value -= 5;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuyMediumCrowSkull()
    {
        _grabIngredient.AddToBagPermanantly(5);
        if (_chipPoints.crowSkullRule == 1 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4 && _playerData.Coins.Value >= 10)
        {
            _playerData.Coins.Value -= 10;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }
    public void BuyLargeCrowSkull()
    {
        _grabIngredient.AddToBagPermanantly(3);
        if (_chipPoints.crowSkullRule == 1 && _playerData.Coins.Value >= 14)
        {
            _playerData.Coins.Value -= 14;
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4 && _playerData.Coins.Value >= 19)
        {
            _playerData.Coins.Value -= 19;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }
    public void BuyMoth()
    {
        _grabIngredient.AddToBagPermanantly(10);
        if (_playerData.Coins.Value >= 10)
        {
            _playerData.Coins.Value -= 10;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }


    public void BuySmallSpider()
    {
        _grabIngredient.AddToBagPermanantly(16);

        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3 && _playerData.Coins.Value >= 4)
        {
            _playerData.Coins.Value -= 4;

        }
        if (_chipPoints.gardenSpiderRule == 2 || _chipPoints.gardenSpiderRule == 4 && _playerData.Coins.Value >= 6)
        {
            _playerData.Coins.Value -= 6;

        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();

    }

    public void BuyMediumSpider()
    {
        _grabIngredient.AddToBagPermanantly(17);
        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;

        }
        if (_chipPoints.gardenSpiderRule == 2 || _chipPoints.gardenSpiderRule == 4 && _playerData.Coins.Value >= 11)
        {
            _playerData.Coins.Value -= 11;

        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();

    }
    public void BuyLargeSpider()
    {
        _grabIngredient.AddToBagPermanantly(15);
        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3 && _playerData.Coins.Value >= 14)
        {
            _playerData.Coins.Value -= 14;

        }
        if (_chipPoints.gardenSpiderRule == 2 && _playerData.Coins.Value >= 21)
        {
            _playerData.Coins.Value -= 21;

        }
        if (_chipPoints.gardenSpiderRule == 4 && _playerData.Coins.Value >= 18)
        {
            _playerData.Coins.Value -= 18;
        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuySmallMandrake()
    {
        _grabIngredient.AddToBagPermanantly(8);
        if (_chipPoints.mandrakeRule == 1 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 9;

        }
        if (_chipPoints.mandrakeRule >= 2 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;

        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuyMediumMandrake()
    {
        _grabIngredient.AddToBagPermanantly(9);
        if (_chipPoints.mandrakeRule == 1 && _playerData.Coins.Value >= 13)
        {
            _playerData.Coins.Value -= 13;

        }
        if (_chipPoints.mandrakeRule >= 2 && _playerData.Coins.Value >= 12)
        {
            _playerData.Coins.Value -= 12;


        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }
    public void BuyLargeMandrake()
    {
        _grabIngredient.AddToBagPermanantly(7);
        if (_chipPoints.mandrakeRule == 1 && _playerData.Coins.Value >= 19)
        {
            _playerData.Coins.Value -= 19;


        }
        if (_chipPoints.mandrakeRule >= 2 && _playerData.Coins.Value >= 19)
        {
            _playerData.Coins.Value -= 19;


        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void BuyGhostsBreath()
    {
        _grabIngredient.AddToBagPermanantly(6);
        if (_chipPoints.ghostsBreathRule == 1 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 9;


        }
        if (_chipPoints.ghostsBreathRule == 2 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 12;


        }
        if (_chipPoints.ghostsBreathRule == 3 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 11;


        }
        if (_chipPoints.ghostsBreathRule == 4 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 10;

        }
        else
        {
            Debug.Log("Can't afford");
            //add ui to tell player
        }
        UpdateCoinText();
        UpdateAmountInBag();
    }

    public void FinishShopping()
    {
        purpleBagSphere.SetActive(true);
        _teleportationManager.StartGameTeleportation();
        _grabIngredient.ResetBagContents();
        _chipPoints.CountIngredientsInPot();
        initialIngredients.SetActive(false);
        coinsBoard.SetActive(false);
        bagBoard.SetActive(false);
        MandrakeStallStuff.SetActive(false);
        ghostsBreathStallStuff.SetActive(false);
        purpleBagSphere.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.SpendRubies);
    }

    public void UpdateAmountInBag()
    {
        
        purpleBagSphere.SetActive(true);
        _grabIngredient.ResetBagContents();
        _grabIngredient.CountIngredientsInBag();
        cherryBombAmount.text = _grabIngredient.bombs.ToString();
        crowSkullAmount.text = _grabIngredient.crowSkull.ToString();
        ghostsAmount.text = _grabIngredient.ghost.ToString();
        mandrakeAmount.text = _grabIngredient.mandrake.ToString();
        mothAmount.text = _grabIngredient.moth.ToString();
        toadstallAmount.text = _grabIngredient.toadstall.ToString();
        pumpkinAmount.text = _grabIngredient.pumpkin.ToString();
        spiderAmount.text = _grabIngredient.spider.ToString();
        
    }

    private void UpdateCoinText()
    {
        coinsLeft.text = _playerData.Coins.Value.ToString();
    }

   

  

   
}
