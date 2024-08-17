using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using Oculus.VoiceSDK.UX;
using System.Threading.Tasks;
using UnityEngine.InputSystem.OSX;
using UnityEditor.Experimental.GraphView;
using Newtonsoft.Json.Bson;

public class GrabIngredient : MonoBehaviour
{


    // 0 - cherryBombOne
    // 1 - cherryBombThree
    // 2 - cherryBombTwo   
    // 3 - crowSkullFour
    // 4 - crowSkullOne
    // 5 - crowSkullTwo
    // 6 - ghostOne
    // 7 - mandrakeFour
    // 8 - mandrakeOne
    // 9 - mandrakeTwo
    // 10 - mothOne
    // 11 - mushroomFour
    // 12 - mushroomOne
    // 13 - mushroomTwo
    // 14 - pumpkin
    // 15 - spiderFour
    // 16 - spiderOne
    // 17 - spiderTwo
    List<int> bagContents = new() { 14, 16, 1, 2, 2, 0, 0, 0 };
    List<int> resetBagContents = new() { 14, 16, 1, 2, 2, 0, 0, 0 };


    List<int> ingredientsToUpgrade = new();
    List<int> ingredientToAddOneToPot = new();

    public List<GameObject> ingredients;

    [SerializeField]
    private int _cherryBombs;
    private int _cherryBombLimit;

    public int fortuneDrawAmount;
    public int fortuneDrawn;
    public bool fortuneDrawTime = false;

    public int totalOfFortuneIngredients;
   

    private GameObject drawnOne;
    private GameObject drawnTwo;
    private GameObject drawnThree;
    private GameObject drawnFour;
    private GameObject drawnFive;

    [SerializeField] TextMeshProUGUI pumpkinAmount;
    [SerializeField] TextMeshProUGUI toadstallAmount;
    [SerializeField] TextMeshProUGUI crowSkullAmount;
    [SerializeField] TextMeshProUGUI mothAmount;
    [SerializeField] TextMeshProUGUI spiderAmount;
    [SerializeField] TextMeshProUGUI mandrakeAmount;
    [SerializeField] TextMeshProUGUI ghostsAmount;
    [SerializeField] TextMeshProUGUI cherryBombAmount;
    [SerializeField] TextMeshProUGUI aboveCauldronText;

    [SerializeField] GameObject insideBag;

    Vector3 InsideBagLocation;

    private PotionQuality _potionQuality;
    private ChipPoints _chipPoints;

    public int bombs = 0;
    public int crowSkull = 0;
    public int ghost = 0;
    public int mandrake = 0;
    public int moth = 0;
    public int toadstall = 0;
    public int pumpkin = 0;
    public int spider = 0;



    public void RemoveItemFromBag(int ingredientNumber)
    {
        int index = bagContents.IndexOf(ingredientNumber);
        bagContents.RemoveAt(index);

    }

    public void ResetBombs()
    {
        _cherryBombs = 0;

    }

    public void RemoveItemFromBagPermanantly(int ingredientNumber)
    {
        int index = resetBagContents.IndexOf(ingredientNumber);
        resetBagContents.RemoveAt(index);

    }

    public void AddToBagPermanantly(int ingredientNumber)
    {
        resetBagContents.Add(ingredientNumber);
    }

    public void AddToBagThisRound(int ingredientNumber)
    {
        bagContents.Add(ingredientNumber);
    }
    private void Awake()
    {
        _potionQuality = FindObjectOfType<PotionQuality>();


    }

    void Start()
    {
        ingredients = new List<GameObject>(Resources.LoadAll<GameObject>("ingredients"));
        CountIngredientsInBag();
    }
    public async Task CheckDrawnRightAmount()
    {
        while (!EverythingDrawn())
        {
            
            Debug.Log("no choice chosen");
           
            await Task.Yield();
        }
        Debug.Log("choice MADE");
        fortuneDrawTime = false;

    }

    public void DeleteInstantiatedIngredients()
    {
        Destroy(drawnOne);
        Destroy(drawnTwo);
        Destroy(drawnThree);
        Destroy(drawnFour);
        Destroy(drawnFive);
    }

    public bool EverythingDrawn()
    {

        if (fortuneDrawn == fortuneDrawAmount || bagContents.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ResetChoices()
    {
        
        fortuneDrawn = 0;
        fortuneDrawAmount = 0;
        totalOfFortuneIngredients = 0;
        ingredientsToUpgrade.Clear();
    }
    public int RandomlyDrawSeveralIngredients()
    {

        System.Random rand = new();
        int num = rand.Next(0, bagContents.Count);
        int nextIngredient = bagContents[num];
        return nextIngredient;

    }

    private async void OnTriggerEnter(Collider other)
    {
       
        if (fortuneDrawTime && bagContents.Count > 0 && fortuneDrawAmount > fortuneDrawn)
        {
            _potionQuality = FindObjectOfType<PotionQuality>();
            if (other.gameObject.CompareTag("hand"))
            {
                
                System.Random rand = new();
                int num = rand.Next(0, bagContents.Count -1);
                int nextIngredient = bagContents[num];
                InsideBagLocation = insideBag.transform.position;
                fortuneDrawn++;
                if (fortuneDrawn == 1)
                {
                    drawnOne = Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                    await Task.Delay(4000);
                   

                }
                if (fortuneDrawn == 2)
                {
                    drawnTwo = Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                    await Task.Delay(4000);
                    
                }
                if (fortuneDrawn == 3)
                {
                    drawnThree = Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                    await Task.Delay(4000);
                   
                }
                if (fortuneDrawn == 4)
                {
                    drawnFour = Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                    await Task.Delay(4000);
                    
                }
                if (fortuneDrawn == 5)
                {
                    drawnFive = Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                    await Task.Delay(4000);
                  
                }
                ingredientToAddOneToPot.Add(num);
                bagContents.RemoveAt(num);
                CountIngredientsInBag();
                CountDrawnIngredients(num);
                CountDrawnIngredientsForUpgrading(num);
                 
                int leftToDraw = fortuneDrawAmount - fortuneDrawn;
                aboveCauldronText.text = $"You have {leftToDraw} ingredients left to draw!";
              
            }
        }
        if (bagContents.Count > 0 && _cherryBombs <= _cherryBombLimit)
        {
            _potionQuality = FindObjectOfType<PotionQuality>();
            
            if (other.gameObject.CompareTag("hand") && _potionQuality.nextIngredientTime)
            {
                
                
                System.Random rand = new();
                int num = rand.Next(0, bagContents.Count -1);
                int nextIngredient = bagContents[num];
                InsideBagLocation = insideBag.transform.position;

                Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                
                bagContents.RemoveAt(num);
                _potionQuality.FalseNextIngredientMethod();
                CountIngredientsInBag();
            }
        }
    }

    public void ResetBagContents()
    {
        bagContents.Clear();
        foreach (int ingredient in resetBagContents) {
            bagContents.Add(ingredient);
        }
        Debug.Log($"{bagContents.Count} bag contents");
        Debug.Log($"{resetBagContents.Count} reset bag contents");
    }

    public void CountIngredientsInBag()
    {
        pumpkin = 0;
        ghost = 0;
        crowSkull = 0;
        mandrake = 0;
        moth = 0;
        toadstall = 0;
        spider = 0;
        bombs = 0;

        foreach (int ingredient in bagContents)
        {
            if (ingredient >= 0 && ingredient < 3) { bombs++; }
            if (ingredient >= 3 && ingredient <= 5)
            { crowSkull++; }
            if (ingredient == 6)
            { ghost++; }
            if (ingredient >= 7 && ingredient <= 9)
            { mandrake++; }
            if (ingredient == 10)
            { moth++; }
            if (ingredient >= 11 && ingredient <= 13)
            { toadstall++; }
            if (ingredient == 14)
            { pumpkin++; }
            if (ingredient >= 15 && ingredient <= 17)
            { spider++; }
        }

        cherryBombAmount.text = bombs.ToString();
        crowSkullAmount.text = crowSkull.ToString();
        ghostsAmount.text = ghost.ToString();
        mandrakeAmount.text = mandrake.ToString();
        mothAmount.text = moth.ToString();
        toadstallAmount.text = toadstall.ToString();
        pumpkinAmount.text = pumpkin.ToString();
        spiderAmount.text = spider.ToString();
    }

    private void CountDrawnIngredients(int Num) {
       
        if (Num == 0 || Num == 4 || Num == 6 || Num == 8 || Num == 10 || Num == 12 || Num == 14 || Num == 16)
        {
            totalOfFortuneIngredients++;
        }
        if (Num == 2 || Num == 5 || Num == 9 || Num == 13 || Num == 17)
        {
            totalOfFortuneIngredients += 2;
        }
        if (Num == 1)
        {
            totalOfFortuneIngredients += 3;
        }
        if (Num == 3 || Num == 7 || Num == 11 || Num == 15)
        {
            totalOfFortuneIngredients += 4;
        }
    }

    public async Task SendDrawnIngredientsInfo()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        int num = ingredientsToUpgrade.Count;
        if (num == 0)
        {
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "OH NO! Can't upgrade any of the ingredients you drew!", "", "", "", "", "");
            _chipPoints.ResetChoices();
        }
        if (num == 1)
        {
            int ingredientNumber = ingredientsToUpgrade[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, $"Oh dear... you can only upgrade {ingredientName}...", "UPGRADE", "", "", "", "");
            GetNumberOfUpgrade(ingredientNumber);
            RemoveItemFromBagPermanantly(ingredientNumber);
            AddToBagPermanantly(GetNumberOfUpgrade(ingredientNumber));
        }
        if (num == 2)
        {
            int ingredientNumber = ingredientsToUpgrade[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientsToUpgrade[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "Which of these ingredients do you want to upgrade?", ingredientName, ingredientNameTwo, "", "", "");
            if (_chipPoints.choiceOneCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientNumber);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientNumber));
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientTwo);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientTwo));
            }

        }
        if (num == 3)
        {
            int ingredientNumber = ingredientsToUpgrade[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientsToUpgrade[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            int ingredientThree = ingredientsToUpgrade[2];
            string ingredientNameThree = GetNameOFIngredient(ingredientThree);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "Which of these ingredients do you want to upgrade?", ingredientName, ingredientNameTwo, ingredientNameThree, "", "");
            if (_chipPoints.choiceOneCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientNumber);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientNumber));
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientTwo);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientTwo));
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientThree);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientThree));
            }

        }
        if (num == 4)
        {
            int ingredientNumber = ingredientsToUpgrade[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientsToUpgrade[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            int ingredientThree = ingredientsToUpgrade[2];
            string ingredientNameThree = GetNameOFIngredient(ingredientThree);
            int ingredientFour = ingredientsToUpgrade[3];
            string ingredientNameFour = GetNameOFIngredient(ingredientFour);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "OH WOW! you get to choose between 4! Which of these ingredients do you want to upgrade?", ingredientName, ingredientNameTwo, ingredientNameThree, ingredientNameFour, "");
            if (_chipPoints.choiceOneCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientNumber);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientNumber));
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientTwo);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientTwo));
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientThree);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientThree));
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                RemoveItemFromBagPermanantly(ingredientFour);
                AddToBagPermanantly(GetNumberOfUpgrade(ingredientFour));
            }

        }
        _chipPoints.ResetChoices();
        

    }

    public async Task SendDrawnIngredientsInfoToAddToPot()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        int num = ingredientToAddOneToPot.Count;
        if (num == 0)
        {
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "OH NO! You had no ingredients left to draw!", "", "", "", "", "");
            _chipPoints.ResetChoices();
        }
        if (num == 1)
        {
            int ingredientNumber = ingredientToAddOneToPot[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, $"Oh dear... you could only draw one ingredient, {ingredientName}, do you want to put it in your pot?", "put in pot", "", "", "", "");
            if (_chipPoints.choiceOneCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientNumber);
            }
            
            
        }
        if (num == 2)
        {
            int ingredientNumber = ingredientToAddOneToPot[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientToAddOneToPot[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "Which ingredient do you want to add to the pot?", ingredientName, ingredientNameTwo, "", "", "");
            if (_chipPoints.choiceOneCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientNumber);
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientTwo);
            }

        }
        if (num == 3)
        {
            int ingredientNumber = ingredientToAddOneToPot[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientToAddOneToPot[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            int ingredientThree = ingredientToAddOneToPot[2];
            string ingredientNameThree = GetNameOFIngredient(ingredientThree);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "Which ingredient do you want to add to the pot?", ingredientName, ingredientNameTwo, ingredientNameThree, "", "");
            if (_chipPoints.choiceOneCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientNumber);
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientTwo);
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientThree);
            }

        }
        if (num == 4)
        {
            int ingredientNumber = ingredientToAddOneToPot[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientToAddOneToPot[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            int ingredientThree = ingredientToAddOneToPot[2];
            string ingredientNameThree = GetNameOFIngredient(ingredientThree);
            int ingredientFour = ingredientToAddOneToPot[3];
            string ingredientNameFour = GetNameOFIngredient(ingredientFour);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "Whick ingredient do you want to put in the pot?", ingredientName, ingredientNameTwo, ingredientNameThree, ingredientNameFour, "");
            if (_chipPoints.choiceOneCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientNumber);
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientTwo);
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientThree);
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientFour);
            }

        }
        if (num == 4)
        {
            int ingredientNumber = ingredientToAddOneToPot[0];
            string ingredientName = GetNameOFIngredient(ingredientNumber);
            int ingredientTwo = ingredientToAddOneToPot[1];
            string ingredientNameTwo = GetNameOFIngredient(ingredientTwo);
            int ingredientThree = ingredientToAddOneToPot[2];
            string ingredientNameThree = GetNameOFIngredient(ingredientThree);
            int ingredientFour = ingredientToAddOneToPot[3];
            string ingredientNameFour = GetNameOFIngredient(ingredientFour);
            int ingredientFive = ingredientToAddOneToPot[4];
            string ingredientNameFive = GetNameOFIngredient(ingredientFive);
            await _chipPoints.MessageAboveCauldronMultipleChoice(num, "Whick ingredient do you want to put in the pot?", ingredientName, ingredientNameTwo, ingredientNameThree, ingredientNameFour, ingredientNameFive);
            if (_chipPoints.choiceOneCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientNumber);
            }
            if (_chipPoints.choiceTwoCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientTwo);
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientThree);
            }
            if (_chipPoints.choiceThreeCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientFour);
            }
            if (_chipPoints.choiceSixCauldron)
            {
                _chipPoints.InstantiateOverPot(ingredientFive);
            }

        }
        _chipPoints.ResetChoices();


    }

    private void CountDrawnIngredientsForUpgrading(int Num)
    {
        if (Num == 4 && !ingredientsToUpgrade.Contains(4) || Num == 5 && !ingredientsToUpgrade.Contains(5) || Num == 8 && !ingredientsToUpgrade.Contains(8) || Num == 9 && !ingredientsToUpgrade.Contains(9) || Num == 12 && !ingredientsToUpgrade.Contains(12) || Num == 13 && !ingredientsToUpgrade.Contains(13) || Num == 16 && !ingredientsToUpgrade.Contains(16) || Num == 17 && !ingredientsToUpgrade.Contains(17))
        {
            ingredientsToUpgrade.Add(Num);
        }

    }

    public void updateCherryBombs()
    {
        _cherryBombs = _potionQuality.GetCherryBombs();
        _cherryBombLimit = _potionQuality.cherryBombLimit;
    }
   


    private string GetNameOFIngredient(int Num)
    {
        if (Num == 0)
        { return "Small Cherry Bomb"; }
        if (Num == 1)
        { return "Large Cherry Bomb"; }
        if (Num == 2)
        { return "Small Cherry Bomb"; }
        if (Num == 3)
        { return "Large Crow Skull"; }
        if (Num == 4)
        { return "Small Crow Skull"; }
        if (Num == 5)
        { return "Medium Crow Skull"; }
        if (Num == 6)
        { return "Large Crow Skull"; }
        if (Num == 7)
        { return "Large Mandrake"; }
        if (Num == 8)
        { return "Small Mandrake"; }
        if (Num == 9)
        { return "Medium Mandrake"; }
        if (Num == 10)
        { return "HawkMoth"; }
        if (Num == 11)
        { return "Large Mushroom"; }
        if (Num == 12)
        { return "Small Mushroom"; }
        if (Num == 13)
        { return "Medium Mushroom"; }
        if (Num == 14)
        { return "Pumpkin"; }
        if (Num == 15)
        { return "Large Garden Spider"; }
        if (Num == 16)
        { return "Small Garden Spider"; }
        if (Num == 17)
        { return "Medium Garden Spider"; }
        else
        {
            return "";
        }

    }

    private int GetNumberOfUpgrade (int Num)
    {

        if (Num == 4)
        { return 5; }
        if (Num == 5)
        { return 3; }
        if (Num == 8)
        { return 9; }
        if (Num == 9)
        { return 7; }
        if (Num == 12)
        { return 13; }
        if (Num == 13)
        { return 11; }
        if (Num == 16)
        { return 17; }
        if (Num == 17)
        { return 15; }
        else
        {
            return 0;
        }

    }

}
