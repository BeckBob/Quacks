using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.Interaction.Toolkit;

public class BuyIngredients : MonoBehaviour
{
    [SerializeField] GameObject initialIngredients;
    [SerializeField] GameObject MandrakeStallStuff;
    [SerializeField] GameObject ghostsBreathStallStuff;
    [SerializeField] GameObject coinsBoard;
    [SerializeField] GameObject bagBoard;
    [SerializeField] GameObject purpleSphere;
    [SerializeField] GameObject purpleBagSphere;
    [SerializeField] GameObject redBagSphere;
    [SerializeField] GameObject redSphere;
    [SerializeField] GameObject yellowBagSphere;
    [SerializeField] GameObject yellowSphere;
    [SerializeField] GameObject blueBagSphere;
    [SerializeField] GameObject blueSphere;

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
    FortuneManager _fortuneManager;

    [SerializeField] GameObject stallSphere;

    [SerializeField] TextMeshProUGUI pumpkinAmount;
    [SerializeField] TextMeshProUGUI toadstallAmount;
    [SerializeField] TextMeshProUGUI crowSkullAmount;
    [SerializeField] TextMeshProUGUI mothAmount;
    [SerializeField] TextMeshProUGUI spiderAmount;
    [SerializeField] TextMeshProUGUI mandrakeAmount;
    [SerializeField] TextMeshProUGUI ghostsAmount;
    [SerializeField] TextMeshProUGUI cherryBombAmount;

    [SerializeField] GameObject button1Purple;
    [SerializeField] GameObject button2Purple;
    [SerializeField] GameObject button3Purple;


    [SerializeField] GameObject button1Red;
    [SerializeField] GameObject button2Red;
    [SerializeField] GameObject button3Red;

    [SerializeField] GameObject button1Blue;
    [SerializeField] GameObject button2Blue;
    [SerializeField] GameObject button3Blue;

    [SerializeField] GameObject button1Yellow;
    [SerializeField] GameObject button2Yellow;
    [SerializeField] GameObject button3Yellow;

    [SerializeField] TextMeshProUGUI aboveCauldronTextPurple;
    [SerializeField] TextMeshProUGUI choiceOnePurple;
    [SerializeField] TextMeshProUGUI choiceTwoPurple;
    [SerializeField] TextMeshProUGUI choiceThreePurple;

    [SerializeField] GameObject aboveCauldronCanvasPurple;

    [SerializeField] TextMeshProUGUI aboveCauldronTextRed;
    [SerializeField] TextMeshProUGUI choiceOneRed;
    [SerializeField] TextMeshProUGUI choiceTwoRed;
    [SerializeField] TextMeshProUGUI choiceThreeRed;

    [SerializeField] GameObject aboveCauldronCanvasRed;

    [SerializeField] TextMeshProUGUI aboveCauldronTextBlue;
    [SerializeField] TextMeshProUGUI choiceOneBlue;
    [SerializeField] TextMeshProUGUI choiceTwoBlue;
    [SerializeField] TextMeshProUGUI choiceThreeBlue;

    [SerializeField] GameObject aboveCauldronCanvasBlue;

    [SerializeField] TextMeshProUGUI aboveCauldronTextYellow;
    [SerializeField] TextMeshProUGUI choiceOneYellow;
    [SerializeField] TextMeshProUGUI choiceTwoYellow;
    [SerializeField] TextMeshProUGUI choiceThreeYellow;

    [SerializeField] GameObject aboveCauldronCanvasYellow;

    [SerializeField] GameObject questionIngredients;
    [SerializeField] TextMeshProUGUI questionIngredientText;

    [SerializeField] GameObject DoneWithFortuneButton;
    
    [SerializeField] GameObject spiderStallSmall;
    [SerializeField] GameObject spiderStallmedium;
    [SerializeField] GameObject spiderStallLarge;
    [SerializeField] GameObject mandrakeStallSmall;
    [SerializeField] GameObject mandrakeStallMedium;
    [SerializeField] GameObject mandrakeStallLarge;

    [SerializeField] GameObject crowStallSmall;
    [SerializeField] GameObject crowStallMedium;
    [SerializeField] GameObject crowStallLarge;
  
    [SerializeField] GameObject mushroomSmall;
    [SerializeField] GameObject mushroomMedium;
    [SerializeField] GameObject mushroomLarge;

    [SerializeField] GameObject pumpkinStall;
    [SerializeField] GameObject mothStall;

    private ChipPoints _chipPoints;
    private WinnerManager _winnerManager;
    private bool boughtOne = false;
   
    void Start()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _winnerManager = FindObjectOfType<WinnerManager>();

        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _playerData = FindObjectOfType<PlayerData>();
        
    }

    public void SetUpStall()
    {
        ActivateSpheres();
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
            if (_chipPoints.mandrakeRule == 1)
            {
                mandrakeSmallPrice.text = "9";
                mandrakeMediumPrice.text = "13";
                mandrakeLargePrice.text = "19";
            }
            if (_chipPoints.mandrakeRule >= 2)
            {
                mandrakeSmallPrice.text = "8";
                mandrakeMediumPrice.text = "12";
                mandrakeLargePrice.text = "18";
            }
        }
        if (_winnerManager.round >= 4)
        {
           ghostsBreathStallStuff.SetActive(true);
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
        
        UpdateCoinText();
        _teleportationManager.ShopTeleportation();
    }

    public void BuySmallToadstall()
    {
        if (_chipPoints.mandrakeRule == 5 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(12);
            boughtOne = true;
            _playerData.Rubies.Value--;
        }
        if (_chipPoints.mushroomRule == 1 && _playerData.Coins.Value >= 5)
        {
            _playerData.Coins.Value -= 5;
            _grabIngredient.AddToBagPermanantly(12);
        }
        if (_chipPoints.mushroomRule == 2 && _playerData.Coins.Value >= 7)
        {
            _playerData.Coins.Value -= 7;
            _grabIngredient.AddToBagPermanantly(12);
        }
        if (_chipPoints.mushroomRule == 3 && _playerData.Coins.Value >= 6)
        {
            _playerData.Coins.Value -= 6;
            _grabIngredient.AddToBagPermanantly(12);
        }
        if (_chipPoints.mushroomRule == 4 && _playerData.Coins.Value >= 4)
        {
            _playerData.Coins.Value -= 4;
            _grabIngredient.AddToBagPermanantly(12);
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
        if (_chipPoints.mushroomRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(13);
            boughtOne = true;
        }
        if (_chipPoints.mushroomRule == 1 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 9;
            _grabIngredient.AddToBagPermanantly(13);
        }
        if (_chipPoints.mushroomRule == 2 && _playerData.Coins.Value >= 11)
        {
            _playerData.Coins.Value -= 11;
            _grabIngredient.AddToBagPermanantly(13);
        }
        if (_chipPoints.mushroomRule == 3 && _playerData.Coins.Value >= 10)
        {
            _playerData.Coins.Value -= 10;
            _grabIngredient.AddToBagPermanantly(13);
        }
        if (_chipPoints.mushroomRule == 4 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;
            _grabIngredient.AddToBagPermanantly(13);
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
        if (_chipPoints.mushroomRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(11);
            boughtOne = true;
        }
        if (_chipPoints.mushroomRule == 1 && _playerData.Coins.Value >= 15)
        {
            _playerData.Coins.Value -= 15;
            _grabIngredient.AddToBagPermanantly(11);
        }
        if (_chipPoints.mushroomRule == 2 && _playerData.Coins.Value >= 17)
        {
            _playerData.Coins.Value -= 17;
            _grabIngredient.AddToBagPermanantly(11);
        }
        if (_chipPoints.mushroomRule == 3 && _playerData.Coins.Value >= 16)
        {
            _playerData.Coins.Value -= 16;
            _grabIngredient.AddToBagPermanantly(11);
        }
        if (_chipPoints.mushroomRule == 4 && _playerData.Coins.Value >= 14)
        {
            _playerData.Coins.Value -= 14;
            _grabIngredient.AddToBagPermanantly(11);
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
       
        if (_playerData.Coins.Value >= 3)
        {
            _playerData.Coins.Value -= 3;
            _grabIngredient.AddToBagPermanantly(14);
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
        if (_chipPoints.crowSkullRule == 5 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(4);
            boughtOne = true;
            _playerData.Rubies.Value--;
        }
        if (_chipPoints.crowSkullRule == 1 && _playerData.Coins.Value >= 4)
        {
            _playerData.Coins.Value -= 4;
            _grabIngredient.AddToBagPermanantly(4);
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4 && _playerData.Coins.Value >= 5)
        {
            _playerData.Coins.Value -= 5;
            _grabIngredient.AddToBagPermanantly(4);
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
        if (_chipPoints.crowSkullRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(5);
            boughtOne = true;
        }
        if (_chipPoints.crowSkullRule == 1 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;
            _grabIngredient.AddToBagPermanantly(5);
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4 && _playerData.Coins.Value >= 10)
        {
            _playerData.Coins.Value -= 10;
            _grabIngredient.AddToBagPermanantly(5);
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
        if (_chipPoints.crowSkullRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(3);
            boughtOne = true;
        }
        if (_chipPoints.crowSkullRule == 1 && _playerData.Coins.Value >= 14)
        {
            _playerData.Coins.Value -= 14;
            _grabIngredient.AddToBagPermanantly(3);
        }
        if (_chipPoints.crowSkullRule == 2 || _chipPoints.crowSkullRule == 3 || _chipPoints.crowSkullRule == 4 && _playerData.Coins.Value >= 19)
        {
            _playerData.Coins.Value -= 19;
            _grabIngredient.AddToBagPermanantly(3);
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
        if (_chipPoints.hawkMothRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(10);
            boughtOne = true;
        }
        if (_chipPoints.hawkMothRule > 0 && _playerData.Coins.Value >= 10)
        {
            _playerData.Coins.Value -= 10;
            _grabIngredient.AddToBagPermanantly(10);
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

        if (_chipPoints.gardenSpiderRule == 5 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(16);
            boughtOne = true;
            _playerData.Rubies.Value--;
        }
        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3 && _playerData.Coins.Value >= 4)
        {
            _playerData.Coins.Value -= 4;
            _grabIngredient.AddToBagPermanantly(16);
        }
        if (_chipPoints.gardenSpiderRule == 2 || _chipPoints.gardenSpiderRule == 4 && _playerData.Coins.Value >= 6)
        {
            _playerData.Coins.Value -= 6;
            _grabIngredient.AddToBagPermanantly(16);
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
        if (_chipPoints.gardenSpiderRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(17);
            boughtOne = true;
        }
        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;
            _grabIngredient.AddToBagPermanantly(17);
        }
        if (_chipPoints.gardenSpiderRule == 2 || _chipPoints.gardenSpiderRule == 4 && _playerData.Coins.Value >= 11)
        {
            _playerData.Coins.Value -= 11;
            _grabIngredient.AddToBagPermanantly(17);
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
        if (_chipPoints.gardenSpiderRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(15);
            boughtOne = true;
        }
        if (_chipPoints.gardenSpiderRule == 1 || _chipPoints.gardenSpiderRule == 3 && _playerData.Coins.Value >= 14)
        {
            _playerData.Coins.Value -= 14;
            _grabIngredient.AddToBagPermanantly(15);
        }
        if (_chipPoints.gardenSpiderRule == 2 && _playerData.Coins.Value >= 21)
        {
            _playerData.Coins.Value -= 21;
            _grabIngredient.AddToBagPermanantly(15);
        }
        if (_chipPoints.gardenSpiderRule == 4 && _playerData.Coins.Value >= 18)
        {
            _playerData.Coins.Value -= 18;
            _grabIngredient.AddToBagPermanantly(15);
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
        if (_chipPoints.mandrakeRule == 5 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(8);
            boughtOne = true;
            _playerData.Rubies.Value--;
        }
        if (_chipPoints.mandrakeRule == 1 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 9;
            _grabIngredient.AddToBagPermanantly(8);
        }
        if (_chipPoints.mandrakeRule >= 2 && _playerData.Coins.Value >= 8)
        {
            _playerData.Coins.Value -= 8;
            _grabIngredient.AddToBagPermanantly(8);
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
        if (_chipPoints.mandrakeRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(9);
            boughtOne = true;
        }
        if (_chipPoints.mandrakeRule == 1 && _playerData.Coins.Value >= 13)
        {
            _playerData.Coins.Value -= 13;
            _grabIngredient.AddToBagPermanantly(9);
        }
        if (_chipPoints.mandrakeRule >= 2 && _playerData.Coins.Value >= 12)
        {
            _playerData.Coins.Value -= 12;
            _grabIngredient.AddToBagPermanantly(9);

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
        if (_chipPoints.mandrakeRule == 0 && !boughtOne)
        {
            _grabIngredient.AddToBagPermanantly(7);
            boughtOne = true;
        }
        if (_chipPoints.mandrakeRule == 1 && _playerData.Coins.Value >= 19)
        {
            _playerData.Coins.Value -= 19;

            _grabIngredient.AddToBagPermanantly(7);
        }
        if (_chipPoints.mandrakeRule >= 2 && _playerData.Coins.Value >= 19)
        {
            _playerData.Coins.Value -= 19;
            _grabIngredient.AddToBagPermanantly(7);

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
        
        if (_chipPoints.ghostsBreathRule == 1 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 9;
            _grabIngredient.AddToBagPermanantly(6);

        }
        if (_chipPoints.ghostsBreathRule == 2 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 12;
            _grabIngredient.AddToBagPermanantly(6);

        }
        if (_chipPoints.ghostsBreathRule == 3 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 11;
            _grabIngredient.AddToBagPermanantly(6);

        }
        if (_chipPoints.ghostsBreathRule == 4 && _playerData.Coins.Value >= 9)
        {
            _playerData.Coins.Value -= 10;
            _grabIngredient.AddToBagPermanantly(6);
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
        
        _teleportationManager.StartGameTeleportation();
        _grabIngredient.ResetBagContents();
        _chipPoints.CountIngredientsInPot();
        initialIngredients.SetActive(false);
        coinsBoard.SetActive(false);
        bagBoard.SetActive(false);
        MandrakeStallStuff.SetActive(false);
        ghostsBreathStallStuff.SetActive(false);
        DeactivateSpheres();
        GameManager.Instance.UpdateGameState(GameState.SpendRubies);
    }

    public void ActivateSpheres()
    {
        _playerData = FindObjectOfType<PlayerData>();
        if (_playerData.Colour.Value == "Purple")
        {
            purpleSphere.SetActive(true);
            purpleBagSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Red")
        {
            redSphere.SetActive(true);
            redBagSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            yellowSphere.SetActive(true);
            yellowBagSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            blueSphere.SetActive(true);
            blueBagSphere.SetActive(true);
        }
    }

    public void DeactivateSpheres()
    {
        _playerData = FindObjectOfType<PlayerData>();
        if (_playerData.Colour.Value == "Purple")
        {
            purpleSphere.SetActive(false);
            purpleBagSphere.SetActive(false);
        }
        if (_playerData.Colour.Value == "Red")
        {
            redSphere.SetActive(false);
            redBagSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            yellowSphere.SetActive(false);
            yellowBagSphere.SetActive(true);
        }
        if (_playerData.Colour.Value == "Blue")
        {
            blueSphere.SetActive(false);
            blueBagSphere.SetActive(true);
        }
    }

    public void UpdateAmountInBag()
    {
        
        ActivateSpheres();
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
        DeactivateSpheres();
    }

    private void UpdateCoinText()
    {
        coinsLeft.text = _playerData.Coins.Value.ToString();
    }

    private bool MandrakeAsk = false;
    private bool PumpkinAsk = false;
    private bool SpiderAsk = false;
    private bool ToadstoolAsk = false;
    private bool GhostsbreathAsk = false;
    private bool CrowskullAsk = false;
    private bool MothAsk = false;
    public void WhatIsMandrake()
    {
        if (!MandrakeAsk)
        {
            questionIngredients.SetActive(true);
            MandrakeAsk = true;
            if (_winnerManager.SpiderRule.Value == 1)
            {
                questionIngredientText.text = "Mandrake doubles the points of the next ingredient in the pot.";
            }
            if (_winnerManager.SpiderRule.Value == 2)
            {
                questionIngredientText.text = "Mandrake means If the previously placed ingredient was a cherrybomb, put the cherrybomb back into the bag.";
            }
            if (_winnerManager.SpiderRule.Value == 3)
            {
                questionIngredientText.text = "Mandrakes mean the total value of cherrybombs needed to blow up your pot increases, according to the number of mandrakes in the pot. 1 mandrake = 8. 3 mandrakes = 9";
            }
            if (_winnerManager.SpiderRule.Value == 4)
            {
                questionIngredientText.text = "Mandrakes give you extra points. Your first mandrake adds 1 extra point, the 2nd mandrake 2 extra points and the 3rd mandrake adds 3 extra points.";
            }
        }
        else {
            questionIngredients.SetActive(false);
            MandrakeAsk = false;
        }
    }

    public void WhatIsPumpkin()
    {
        if (PumpkinAsk)
        {
            questionIngredients.SetActive(false);
            PumpkinAsk = false;
        }
        else
        {
            questionIngredients.SetActive(true);
            PumpkinAsk = true;
            questionIngredientText.text = "Pumpkins have no special effects.";
        }
    }

    public void WhatIsSpider()
    {
        if (!SpiderAsk)
        {
            questionIngredients.SetActive(true);
            SpiderAsk = true;
            if (_winnerManager.SpiderRule.Value == 1)
            {
                questionIngredientText.text = "For each spider that is the LAST or NEXT-TO-LAST chip in your pot, you may pay 1 ruby to add one droplet to the pot.";
            }
            if (_winnerManager.SpiderRule.Value == 2)
            {
                questionIngredientText.text = "If your Cherrybombs add up to exactly 7, when the round ends choose to add the collective value of all spiders in your pot.";
            }
            if (_winnerManager.SpiderRule.Value == 3)
            {
                questionIngredientText.text = "For each spider that is the LAST or NEXT-TO-LAST ingredient in your pot, you receive one ruby.";
            }
            if (_winnerManager.SpiderRule.Value == 4)
            {
                questionIngredientText.text = " For each spider that is the LAST or NEXT-TO-LAST In your post take one of the indicated ingredients. 1 = Pumpkin. 2 = Crowskull or Toadstool. 3 = Mandrake or GhostsBreath.";
            }
        }
        else
        {
            questionIngredients.SetActive(false);
            SpiderAsk = false;
        }
    }
    public void WhatIsToadstool()
    {
        if (!ToadstoolAsk)
        {
            questionIngredients.SetActive(true);
            ToadstoolAsk = true;
            if (_winnerManager.SpiderRule.Value == 1)
            {
                questionIngredientText.text = "For each spider that is the LAST or NEXT-TO-LAST chip in your pot, you may pay 1 ruby to add one droplet to the pot.";
            }
            if (_winnerManager.SpiderRule.Value == 2)
            {
                questionIngredientText.text = "If your Cherrybombs add up to exactly 7, when the round ends choose to add the collective value of all spiders in your pot.";
            }
            if (_winnerManager.SpiderRule.Value == 3)
            {
                questionIngredientText.text = "For each spider that is the LAST or NEXT-TO-LAST ingredient in your pot, you receive one ruby.";
            }
            if (_winnerManager.SpiderRule.Value == 4)
            {
                questionIngredientText.text = " For each spider that is the LAST or NEXT-TO-LAST In your post take one of the indicated ingredients. 1 = Pumpkin. 2 = Crowskull or Toadstool. 3 = Mandrake or GhostsBreath.";
            }
        }
        else
        {
            questionIngredients.SetActive(false);
            SpiderAsk = false;
        }
    }

    public void WhatIsGhostsBreath()
    {
        if (!GhostsbreathAsk)
        {
            questionIngredients.SetActive(true);
           GhostsbreathAsk = true;
            if (_winnerManager.SpiderRule.Value == 1)
            {
                questionIngredientText.text = "For 1, 2 or 3 ghosts breath, you receive the indicated bonus.  1 = 1 victory point. 2 = victory point and ruby. 3 = 2 victory points and teardrop forward one space.";
            }
            if (_winnerManager.SpiderRule.Value == 2)
            {
                questionIngredientText.text = "When there is a ghostsbreath in your pot you may tradeit for the indicated bonus 1 = Moth, victory point and ruby 2 = small spider medium skull 3 victory points and space forward. 3 = large mandrake 6 victory points a ruby and 2 spaces forward.";
            }
            if (_winnerManager.SpiderRule.Value == 3)
            {
                questionIngredientText.text = "When Ghostsbreath is in pot you can trade 1 ingredient from the pot for a larger version of the same ingredient. 1 = small to medium 2= medium to large 3= small to large";
            }
            if (_winnerManager.SpiderRule.Value == 4)
            {
                questionIngredientText.text = "For each ghostsbreath, depending on where it is in the pot you receive the following victory points. Less that 9 droplets = 0, more than 10 = 1, more than 20 = 2, more than 30 = 3.";
            }
        }
        else
        {
            questionIngredients.SetActive(false);
            GhostsbreathAsk = false;
        }
    }

    public void WhatIsCrowskull()
    {
        if (!CrowskullAsk)
        {
            questionIngredients.SetActive(true);
            CrowskullAsk = true;
            if (_winnerManager.SpiderRule.Value == 1)
            {
                questionIngredientText.text = "If crowskull is put in the pot on a score you would get a ruby, you immediately receive one ruby.";
            }
            if (_winnerManager.SpiderRule.Value == 2)
            {
                questionIngredientText.text = "If crowskull is put in the pot on a score you would get a ruby, you IMMEDIATLEY receive 1/2/4 victory points.";
            }
            if (_winnerManager.SpiderRule.Value == 3)
            {
                questionIngredientText.text = "If the pot explodes within the next 1/2/4 ingredients, you get victory points AND money during the evaluation phase (but no victory die roll).";
            }
            if (_winnerManager.SpiderRule.Value == 4)
            {
                questionIngredientText.text = "Draw ingredients from your bag. You MAY place 1 of them in your pot. Draw 1 ingredient for small, 2 ingredients for a medium, 4 for a large.";
            }
        }
       else
        {
            questionIngredients.SetActive(false);
            CrowskullAsk = false;
        }
    }


    public void WhatIsMoth()
    {
        if (!MothAsk)
        {
            questionIngredients.SetActive(true);
            MothAsk = true;
            if (_winnerManager.SpiderRule.Value == 1)
            {
                questionIngredientText.text = "If you draw more moths than one of the players sitting next to you get a droplet– more than both players next to you is droplet and ruby.";
            }
            if (_winnerManager.SpiderRule.Value == 2)
            {
                questionIngredientText.text = "If you draw as many moths as the other players you get a droplet, more than them you get a droplet and a ruby.";
            }
          
        }
        else
        {
            questionIngredients.SetActive(false);
           MothAsk = false;
        }
    }



    public void SetUpShopForFortune0()
    {
        ActivateSpheres();
        _chipPoints = FindObjectOfType<ChipPoints>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        _fortuneManager = FindObjectOfType<FortuneManager>();
        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _playerData = FindObjectOfType<PlayerData>();
        _chipPoints.mandrakeRule = 0;
        _chipPoints.mushroomRule = 0;
        _chipPoints.crowSkullRule = 0;
        _chipPoints.gardenSpiderRule = 0;
        initialIngredients.SetActive(true);
        
        spiderStallSmall.SetActive(false);
        spiderStallmedium.SetActive(false);
        
        mushroomMedium.SetActive(false);
        mushroomSmall.SetActive(false);
       
        crowStallSmall.SetActive(false);
        crowStallMedium.SetActive(false);
        DoneWithFortuneButton.SetActive(true);
        
        bagBoard.SetActive(true);
        mothStall.SetActive(false);
        pumpkinStall.SetActive(false);
        UpdateAmountInBag();
        if (_winnerManager.round >= 3)
        {
            MandrakeStallStuff.SetActive(true);
            mandrakeStallMedium.SetActive(false);
            mandrakeStallSmall.SetActive(false);
            mandrakeLargePrice.text = "0";
        }

        mandrakeLargePrice.text = "0";
        toadstallLargePrice.text = "0";
        crowSkullLargePrice.text = "0";
        gardenSpiderLargePrice.text = "0";

        
        _teleportationManager.ShopTeleportation();
    }

    public void DoneWIthFortuneShop()
    {
        _chipPoints.SetRules();
        _grabIngredient.ResetBagContents();
        _chipPoints.CountIngredientsInPot();
       
        boughtOne = false;
        _fortuneManager.fortuneShopDone = true;
        bagBoard.SetActive(false);
        if (_winnerManager.round >= 3)
        {
            mandrakeStallSmall.SetActive(true);
            mandrakeStallMedium.SetActive(true);
            mandrakeStallLarge.SetActive(true);
            MandrakeStallStuff.SetActive(false);

        }
        
        
        spiderStallSmall.SetActive(true);
        spiderStallmedium.SetActive(true);
        spiderStallLarge.SetActive(true);
        
        mushroomMedium.SetActive(true);
        mushroomSmall.SetActive(true);
        mushroomLarge.SetActive(true);
        mothStall.SetActive(true);
        pumpkinStall.SetActive(true);


        crowStallSmall.SetActive(true);
        crowStallMedium.SetActive(true);
        crowStallLarge.SetActive(true);
        initialIngredients.SetActive(false);

        DoneWithFortuneButton.SetActive(false);

        DeactivateSpheres();
        _teleportationManager.StartGameTeleportation();
    }


    public void SetUpShopForFortune11()
    {
        ActivateSpheres();
        _chipPoints = FindObjectOfType<ChipPoints>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        _fortuneManager = FindObjectOfType<FortuneManager>();
        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _playerData = FindObjectOfType<PlayerData>();
        _chipPoints.mandrakeRule = 0;
        _chipPoints.mushroomRule = 0;
        _chipPoints.crowSkullRule = 0;
        _chipPoints.gardenSpiderRule = 0;
        _chipPoints.hawkMothRule = 0;
        initialIngredients.SetActive(true);

        spiderStallSmall.SetActive(false);
        spiderStallLarge.SetActive(false);

        mushroomSmall.SetActive(false);
        mushroomLarge.SetActive(false);
        pumpkinStall.SetActive(false);

        crowStallSmall.SetActive(false);
        crowStallLarge.SetActive(false);
        DoneWithFortuneButton.SetActive(true);

        bagBoard.SetActive(true);
        mothStall.SetActive(true);
        pumpkinStall.SetActive(false);
        UpdateAmountInBag();
        if (_winnerManager.round >= 3)
        {
            MandrakeStallStuff.SetActive(true);
            mandrakeStallLarge.SetActive(false);
            mandrakeStallSmall.SetActive(false);
            mandrakeMediumPrice.text = "0";
        }

        mothPrice.text = "0";
        toadstallMediumPrice.text = "0";
        crowSkullMediumPrice.text = "0";
        gardenSpiderMediumPrice.text = "0";


        _teleportationManager.ShopTeleportation();
    }

    public void SetUpShopForFortune16()
    {
        ActivateSpheres();
        _chipPoints = FindObjectOfType<ChipPoints>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        _fortuneManager = FindObjectOfType<FortuneManager>();
        _teleportationManager = FindObjectOfType<TeleportationManager>();
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _playerData = FindObjectOfType<PlayerData>();
        _chipPoints.mandrakeRule = 5;
        _chipPoints.mushroomRule = 5;
        _chipPoints.crowSkullRule = 5;
        _chipPoints.gardenSpiderRule = 5;
        initialIngredients.SetActive(true);

        spiderStallmedium.SetActive(false);
        spiderStallLarge.SetActive(false);

        mushroomMedium.SetActive(false);
        mushroomLarge.SetActive(false);
        pumpkinStall.SetActive(false);

        crowStallMedium.SetActive(false);
        crowStallLarge.SetActive(false);
        DoneWithFortuneButton.SetActive(true);

        bagBoard.SetActive(true);
        mothStall.SetActive(false);
        pumpkinStall.SetActive(false);
        UpdateAmountInBag();
        if (_winnerManager.round >= 3)
        {
            MandrakeStallStuff.SetActive(true);
            mandrakeStallLarge.SetActive(false);
            mandrakeStallMedium.SetActive(false);
            mandrakeSmallPrice.text = "0";
        }


        toadstallSmallPrice.text = "0";
        crowSkullSmallPrice.text = "0";
        gardenSpiderSmallPrice.text = "0";


        _teleportationManager.ShopTeleportation();
    }

}
