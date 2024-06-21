using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly_CSharp.boardPlacement;
using pointSystem;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;
using System.Threading.Tasks;



public class ChipPoints : MonoBehaviour
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

    private Program programInstance;
    [SerializeField] TextMeshProUGUI cauldronScoreFront;
    [SerializeField] TextMeshProUGUI cauldronScoreBack;

    public PotionQuality quality;

    [SerializeField] GameObject futureScore;
    [SerializeField] GameObject potExplodedCanvas;

    private PlayerData _playerData;

    [SerializeField] GameObject leftOverIngredient;
    [SerializeField] GameObject rubylocationSphere;
    [SerializeField] GameObject buttonsToAddLeftover;
    Vector3 leftOverIngredientLocation;
    Vector3 rubyInstantiationLocation;
    [SerializeField] GameObject rubyForBowl;

    NetworkConnect _networkConnect;
    public List<GameObject> ingredients;
    WinnerManager winnerManager;
    GrabIngredient grabIngredient;
    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    [SerializeField] GameObject button3;
    [SerializeField] GameObject button4;
    [SerializeField] GameObject button5;
    [SerializeField] TextMeshProUGUI aboveCauldronText;
    [SerializeField] TextMeshProUGUI choiceOne;
    [SerializeField] TextMeshProUGUI choiceTwo;
    [SerializeField] TextMeshProUGUI choiceThree;
    [SerializeField] TextMeshProUGUI choiceFour;
    [SerializeField] TextMeshProUGUI choiceFive;
    [SerializeField] TextMeshProUGUI rubyNumber;

    public Material Round1Sky;
    public Material Round2Sky;
    public Material Round3Sky;
    public Material Round4Sky;
    public Material Round5Sky;
    public Material Round6Sky;
    public Material Round7Sky;
    public Material Round8Sky;

    public bool choiceOneCauldron = false;
    public bool choiceTwoCauldron = false;
    public bool choiceThreeCauldron = false;
    public bool choiceFourCauldron = false;
    public bool choiceFiveCauldron = false;
    private int numberOfPlayers;

    public static int InitialScore = 0;

    public int Score = InitialScore;
    public int Coins { get; private set; }
    public int VictoryPoints { get; private set; }
    public bool RubiesThisRound { get; private set; }


    public int FutureCoins { get; private set; }
    public int FutureVictoryPoints { get; private set; }
    public bool FutureRubiesThisRound { get; private set; }


    List<string> ingredientsList = new();
    List<string> ingredientsPutToSide = new();
    List<string> ingredientsPutToSideForNextRound = new();

    public int mushroomRule = 1;
    public int mandrakeRule = 1;
    public int crowSkullRule = 1;
    public int gardenSpiderRule = 1;
    public int hawkMothRule = 1;
    public int ghostsBreathRule = 1;

    public int extraPoints;
    public int extraRubies = 0;
    public int extraVictoryPoints = 0;

    private int pumpkins = 0;
    private int mandrakes = 0;

    [SerializeField] TextMeshProUGUI pumpkinAmount;
    [SerializeField] TextMeshProUGUI toadstallAmount;
    [SerializeField] TextMeshProUGUI crowSkullAmount;
    [SerializeField] TextMeshProUGUI mothAmount;
    [SerializeField] TextMeshProUGUI spiderAmount;
    [SerializeField] TextMeshProUGUI mandrakeAmount;
    [SerializeField] TextMeshProUGUI ghostsAmount;
    [SerializeField] TextMeshProUGUI cherryBombAmount;

    private void Start()
    {
        // Initialize programInstance
        programInstance = new Program();
        ingredients = new List<GameObject>(Resources.LoadAll<GameObject>("ingredients"));
        CountIngredientsInPot();
        

    }
    //TWO FUNCTIONS
    public async void PotExplosionEndRound()
    {
        if (crowSkullRule == 3)
        {
            
            if (ingredientsList[ingredientsList.Count] == "crowSkullOne")
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "small crow skull last in cauldron saved your potion, you get your victory points and coins";
              
                _playerData.Coins.Value = Coins;
                _playerData.VictoryPoints.Value = VictoryPoints;
                _playerData.Score.Value = 0;
                await CheckWhichChoice();
                ResetChoices();
            }
            if (ingredientsList[ingredientsList.Count - 1] == "crowSkullTwo" || ingredientsList[ingredientsList.Count] == "crowSkullTwo")
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "medium crow skull one of the last two ingredients in cauldron saved your potion, you get your victory points and coins";
                
                _playerData.Coins.Value = Coins;
                _playerData.VictoryPoints.Value = VictoryPoints;
                Score = 0;
                await CheckWhichChoice();
                ResetChoices();
            }
            if (ingredientsList[ingredientsList.Count - 1] == "crowSkullFour" || ingredientsList[ingredientsList.Count - 2] == "crowSkullFour" || ingredientsList[ingredientsList.Count] == "crowSkullFour")
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "Large crow skull one of the last three ingredients in cauldron saved your potion, you get your victory points and coins";
              
                _playerData.Coins.Value = Coins;
                _playerData.VictoryPoints.Value = VictoryPoints;
                _playerData.Score.Value = 0;
                await CheckWhichChoice();
                ResetChoices();
            }
            else
            {
                futureScore.SetActive(false);
                potExplodedCanvas.SetActive(true);
                Score = 0;
            }

        }
        else
        {

            futureScore.SetActive(false);
            potExplodedCanvas.SetActive(true);
            Score = 0;

        }
        AfterRoundChipEffects();
        
        Debug.Log("pot exploded");
        GameManager.Instance.UpdateGameState(GameState.RollDice);

     
    }
    public void ResetInsidePot()
    {
        ingredientsList.Clear();
    }
    public void ResetScore() { Score = InitialScore; }

    public async void OnTriggerEnter(Collider other)
    {

        ingredientsList.Add(other.gameObject.tag);
        extraPoints = 0; extraRubies = 0;


        if (mushroomRule == 1)
        {
            if (other.gameObject.tag.Contains("mushroom") && ingredientsList.Count >= 2 && ingredientsList[ingredientsList.Count - 1].Contains("cherryBomb"))
            {

                if (ingredientsList[ingredientsList.Count - 1].Contains("One"))
                {
                    
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Small cherrybomb was right before mushroom! Add 1 to score!";
                    //sound effect
                    Score += 1;
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                else if (ingredientsList[ingredientsList.Count - 1].Contains("Two"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Medium cherrybomb was right before mushroom! Add 2 to score!";
                    //sound effect
                    Score += 2;
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                else if (ingredientsList[ingredientsList.Count - 1].Contains("Three"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Large cherrybomb was right before mushroom! Add 3 to score!";
                    //sound effect
                    Score += 3;
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                ResetChoices();
            }
        }

        if (mushroomRule == 2)
        {
            if (other.gameObject.tag.Contains("cherryBombOne") && (ingredientsList.Contains("mushroomOne") || ingredientsList.Contains("mushroomTwo") || ingredientsList.Contains("mushroomFour")))
            {

                
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "mushroom in potion adds one to score when small cherrybomb is added!";
                //sound effect
                Score += 1;
                await CheckWhichChoice();
                buttonsToAddLeftover.SetActive(false);
                ResetChoices();
            }
            
        }

        if (mushroomRule == 3)
        {
            if (other.gameObject.CompareTag("pumpkinOne"))
            { pumpkins += 1; }

            if (other.gameObject.tag.Contains("mushroom") && pumpkins < 3)
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "pumpkin in potion adds 1 to score when adding mushroom!";
                //sound effect
                Score += 1;
                await CheckWhichChoice();
                buttonsToAddLeftover.SetActive(false);
                ResetChoices();
                
            }
            if (other.gameObject.tag.Contains("mushroom") && pumpkins >= 3)
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "3 or more pumpkins in potion adds 2 to score when adding mushroom!";
                //sound effect
                Score += 2;
                await CheckWhichChoice();
                buttonsToAddLeftover.SetActive(false);
                ResetChoices();
                
            }
        }

        if (mushroomRule == 4)
        {
            if (other.gameObject.tag.Contains("mushroom"))
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = "Put mushroom to the side and decide whether to add to pot at the end or keep to the side for a future round!";
                await CheckWhichChoice();
                //sound effect
                buttonsToAddLeftover.SetActive(false);
                ResetChoices();
                ingredientsPutToSide.Add(other.gameObject.tag);
                ingredientsPutToSide.RemoveAt(ingredientsPutToSide.Count);
            }
        }
       

        if (mandrakeRule == 1)
        {
            if (ingredientsList.Count >= 2 && ingredientsList[ingredientsList.Count - 2].Contains("mandrake"))
            {
                if (other.gameObject.tag.Contains("One"))
                {
                    Score += 1;
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley before doubles this ingredient!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                else if (other.gameObject.tag.Contains("Two"))
                {
                    Score += 2;
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley before doubles this ingredient!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                else if (other.gameObject.tag.Contains("Three"))
                {
                    Score += 3;
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley before doubles this ingredient!";
                    //some sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                else if (other.gameObject.tag.Contains("Four"))
                {
                    Score += 4;
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley before doubles this ingredient!";
                    //some sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                ResetChoices();
            }
        }

        if (mandrakeRule == 2)
            if (other.gameObject.tag.Contains("mandrake") && ingredientsList[ingredientsList.Count - 1].Contains("cherryBomb"))
            {
                if (ingredientsList[ingredientsList.Count - 1].Contains("One")) {
                    ingredientsList.RemoveAt(ingredientsList.Count - 1);

                    grabIngredient.AddToBagThisRound(0);
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley after cherrybomb! Removing cherrybomb from potion and putting back in your bag";
                    //having some sort of animation here to visulaise it might help.
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                if (ingredientsList[ingredientsList.Count - 1].Contains("Two"))
                {
                    ingredientsList.RemoveAt(ingredientsList.Count - 1);

                    grabIngredient.AddToBagThisRound(2);
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley after cherrybomb! Removing cherrybomb from potion and putting back in your bag";
                    //having some sort of animation here to visulaise it might help.
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                if (ingredientsList[ingredientsList.Count - 1].Contains("Three"))
                {
                    ingredientsList.RemoveAt(ingredientsList.Count - 1);

                    grabIngredient.AddToBagThisRound(1);
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Mandrake immediatley after cherrybomb! Removing cherrybomb from potion and putting back in your bag";
                    //having some sort of animation here to visulaise it might help.
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);
                }
                ResetChoices();
                // also need to add back to players bag but haven't created that yet.
            }

        if (mandrakeRule == 3)
        {
            if (other.gameObject.tag.Contains("mandrake"))
            {

                mandrakes++;
                if (mandrakes == 1)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "One mandrake in potion! Limit of cherry bombs increased to 8!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false); quality.AddToCherryBombLimit(); }
                if (mandrakes == 3)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "One mandrake in potion! Limit of cherry bombs increased to 9!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false); quality.AddToCherryBombLimit();
                }
                ResetChoices();
            }
        }

        if (mandrakeRule == 4)
        {
            if (other.gameObject.tag.Contains("mandrake"))
            {
                mandrakes++;
                if (mandrakes == 1)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "One mandrake in potion! Add 1 extra point to score!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false);  Score += 1; }
                if (mandrakes == 2)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Two mandrakes in potion! Add 2 extra points to score!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false); Score += 2;
                }
                
                if (mandrakes >= 3)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Three or more mandrakes in potion! Add 3 extra points to score!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false); Score += 3;
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
            FutureCoins = boardPlacement[Score].Coins;
            FutureVictoryPoints = boardPlacement[Score].VictoryPoints;
            FutureRubiesThisRound = boardPlacement[Score].Ruby;

            if (crowSkullRule == 1)
            {
                if (other.gameObject.tag.Contains("crowSkull") && RubiesThisRound == true)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    aboveCauldronText.text = "Crow Skull on score with ruby! Immediatley recieve one ruby!";
                    //sound effect
                    await CheckWhichChoice();
                    buttonsToAddLeftover.SetActive(false); 
                    _playerData.Rubies.Value++;
                    ChangeRubyUI();
                    ResetChoices();
                }
            }

            if (crowSkullRule == 2)
            {
                if (other.gameObject.tag.Contains("crowSkull") && RubiesThisRound == true)
                {
                    if (other.gameObject.tag.Contains("One"))
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        aboveCauldronText.text = "Small Crow Skull on score with ruby! Immediatley recieve 1 Victory Point!";
                        //sound effect
                        await CheckWhichChoice();
                        buttonsToAddLeftover.SetActive(false);
                        _playerData.VictoryPoints.Value++;
                    }
                    
                    

                    if (other.gameObject.tag.Contains("Two"))
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        aboveCauldronText.text = "Medium Crow Skull on score with ruby! Immediatley recieve 2 Victory Points!";
                        //sound effect
                        await CheckWhichChoice();
                        buttonsToAddLeftover.SetActive(false);
                        _playerData.VictoryPoints.Value += 2;
                    }
                    if (other.gameObject.tag.Contains("Four"))
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        aboveCauldronText.text = "Medium Crow Skull on score with ruby! Immediatley recieve 4 Victory Point!";
                        //sound effect
                        await CheckWhichChoice();
                        buttonsToAddLeftover.SetActive(false);
                        _playerData.VictoryPoints.Value += 4;
                    }
                    ResetChoices();
                }
            }


            if (crowSkullRule == 4)
            {
                if (other.gameObject.tag.Contains("crowSkullOne"))
                {


                    buttonsToAddLeftover.SetActive(true);
                    button1.SetActive(true);
                    button2.SetActive(true);
                    int choiceOneNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    choiceOne.text = getNameOfIngredientFromNumber(choiceOneNumber);
                    choiceTwo.text = "Skip";
                    aboveCauldronText.text = $"Small crow skull! Pick one random ingredient from your bag to add to your pot!";
                    await CheckWhichChoice();

                    if (choiceOneCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceOneNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    buttonsToAddLeftover.SetActive(false);
                    ResetChoices();
                }
                if (other.gameObject.tag.Contains("crowSkullTwo"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    button1.SetActive(true);
                    button2.SetActive(true);
                    button3.SetActive(true);
                    int choiceOneNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    int choiceTwoNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    choiceOne.text = getNameOfIngredientFromNumber(choiceOneNumber);
                    choiceTwo.text = getNameOfIngredientFromNumber(choiceTwoNumber);
                    choiceThree.text = "Skip";
                    aboveCauldronText.text = $"Medium crow skull! Pick one random ingredient from your bag to add to your pot!";
                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceOneNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    if (choiceTwoCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceTwoNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    buttonsToAddLeftover.SetActive(false);
                    ResetChoices();
                }
                if (other.gameObject.tag.Contains("crowSkullFour"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    button1.SetActive(true);
                    button2.SetActive(true);
                    button3.SetActive(true);
                    button4.SetActive(true);
                    button5.SetActive(true);
                    int choiceOneNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    int choiceTwoNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    int choiceThreeNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    int choiceFourNumber = grabIngredient.RandomlyDrawSeveralIngredients();
                    choiceOne.text = getNameOfIngredientFromNumber(choiceOneNumber);
                    choiceTwo.text = getNameOfIngredientFromNumber(choiceTwoNumber);
                    choiceThree.text = getNameOfIngredientFromNumber(choiceThreeNumber);
                    choiceThree.text = getNameOfIngredientFromNumber(choiceFourNumber);
                    choiceFive.text = "Skip";
                    aboveCauldronText.text = $"Large crow skull! Pick one random ingredient from your bag to add to your pot!";
                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceOneNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    if (choiceTwoCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceTwoNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    if (choiceThreeCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceThreeNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    if (choiceFourCauldron)
                    {
                        leftOverIngredientLocation = leftOverIngredient.transform.position;
                        Instantiate(ingredients[choiceFourNumber], leftOverIngredientLocation, Quaternion.identity);
                    }
                    buttonsToAddLeftover.SetActive(false);
                    ResetChoices();
                }
                //Draw 1 / 2 / 4 chips from your bad. You MAY place 1 of them in your pot. S = 5, M = 10, L = 19. - need to impliment bag system before i can add this rule.
            }



            if (ghostsBreathRule == 4)
            {

                if (other.gameObject.tag.Contains("ghost"))

                {

                    if (Coins >= 10 && Coins < 20)
                    { 
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        aboveCauldronText.text = "ghosts breath adds 1 point to extra points."; 
                        extraVictoryPoints++;
                        await CheckWhichChoice();
                        //buttonsToAddLeftover.SetActive(false);
                        ResetChoices();
                    }
                    if (Coins >= 20 && Coins < 30)
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        aboveCauldronText.text = "ghosts breath adds 2 points to extra points.";
                        await CheckWhichChoice();
                        //buttonsToAddLeftover.SetActive(false);
                        ResetChoices();
                  
                        extraVictoryPoints += 2; }
                    if (Coins >= 30)
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        aboveCauldronText.text = "ghosts breath adds 3 points to extra points.";
                       
                        extraVictoryPoints += 3;
                        await CheckWhichChoice();
                        //buttonsToAddLeftover.SetActive(false);
                        ResetChoices();
                    }
                }
            }

            cauldronScoreFront.text = Score.ToString();
            cauldronScoreBack.text = Score.ToString();
            CountIngredientsInPot();
        }
    }

    public void ResetStuffInBook()
    {
        Chips[] boardPlacement = programInstance.GetBoardPlacement();
        Coins = boardPlacement[Score].Coins;
        VictoryPoints = boardPlacement[Score].VictoryPoints;
        RubiesThisRound = boardPlacement[Score].Ruby;
    }

    public string getNameOfIngredientFromNumber(int nextIngredient)
    {
        if (nextIngredient == 0) { return "Small CherryBomb"; }
        if (nextIngredient == 1) { return "Large CherryBomb"; }
        if (nextIngredient == 2) { return "Medium CherryBomb"; }
        if (nextIngredient == 3) { return "Large Crow Skull"; }
        if (nextIngredient == 4) { return "Small Crow Skull"; }
        if (nextIngredient == 5) { return "Medium Crow Skull"; }
        if (nextIngredient == 6) { return "Ghosts Breath"; }
        if (nextIngredient == 7) { return "Large Mandrake"; }
        if (nextIngredient == 8) { return "Small Mandrake"; }
        if (nextIngredient == 9) { return "Medium Mandrake"; }
        if (nextIngredient == 10) { return "HawkMoth"; }
        if (nextIngredient == 11) { return "Large Mushroom"; }
        if (nextIngredient == 12) { return "Small Mushroom"; }
        if (nextIngredient == 13) { return "Medium Mushroom"; }
        if (nextIngredient == 14) { return "Pumpkin"; }
        if (nextIngredient == 15) { return "Large Spider"; }
        if (nextIngredient == 16) { return "Small Spider"; }
        if (nextIngredient == 17) { return "Medium Spider"; }

        else { return "No more ingredients in bag"; }
    }
    public async void ChooseVictoryPoints()
    {
        ResetChoices();
        _playerData = FindObjectOfType<PlayerData>();

        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = $"added {VictoryPoints} Victory Points";
        await CheckWhichChoice();
       
        ResetChoices();

        
        Coins = 0;  
        

        potExplodedCanvas.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.RollDice);

    }

    public async void ChooseCoins()
    {
        _playerData = FindObjectOfType<PlayerData>();

        //buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = $"added {Coins} Coins";
        await CheckWhichChoice();
        //buttonsToAddLeftover.SetActive(false);
        ResetChoices();

        
        VictoryPoints = 0;



        potExplodedCanvas.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.RollDice);

    }


    public async void EndRoundSafely()
    {
        //afterRoundfFortuneEffects
        AfterRoundChipEffects();

        _playerData = FindObjectOfType<PlayerData>();
        

        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = "Round Over!";

        //buttonsToAddLeftover.SetActive(false);
        await CheckWhichChoice();
          
            
        ResetChoices();
        _playerData.Score.Value = Score;
        ResetScore();
        cauldronScoreFront.text = Score.ToString();
        cauldronScoreBack.text = Score.ToString();

        GameManager.Instance.UpdateGameState(GameState.RollDice);

    }


    public async void AfterRoundChipEffects()
    {
        pumpkins = 0;
        if (mushroomRule == 4 && ingredientsPutToSide.Count > 0)
        {
            if (ingredientsPutToSide[0] == "mushroomOne")
            {
                buttonsToAddLeftover.SetActive(true);
                aboveCauldronText.text = "small mushroom put to side left over. Add now or leave for future round";
                button1.SetActive(true);
                button2.SetActive(true);
                choiceOne.text = "Add to pot";
                choiceTwo.text = "don't add";
               
                
                await CheckWhichChoice();
                if (choiceOneCauldron)
                {
                    ingredientsPutToSide.RemoveAt(0);
                    leftOverIngredientLocation = leftOverIngredient.transform.position;
                    Instantiate(ingredients[12], leftOverIngredientLocation, Quaternion.identity);
                    AfterRoundChipEffects();
                }
                if (choiceTwoCauldron)
                {
                    ingredientsPutToSideForNextRound.Add(ingredientsPutToSide[0]);
                    ingredientsPutToSide.RemoveAt(0);
                    AfterRoundChipEffects();
                }

            }
            if (ingredientsPutToSide[0] == "mushroomTwo")
            {
                buttonsToAddLeftover.SetActive(true);
                aboveCauldronText.text = "medium mushroom put to side left over. Add now or leave for future round";
                button1.SetActive(true);
                button2.SetActive(true);
                choiceOne.text = "Add to pot";
                choiceTwo.text = "don't add";
                

                await CheckWhichChoice();
                if (choiceOneCauldron)
                {
                    ingredientsPutToSide.RemoveAt(0);
                    leftOverIngredientLocation = leftOverIngredient.transform.position;
                    Instantiate(ingredients[12], leftOverIngredientLocation, Quaternion.identity);
                    AfterRoundChipEffects();
                }
                if (choiceTwoCauldron)
                {
                    ingredientsPutToSideForNextRound.Add(ingredientsPutToSide[0]);
                    ingredientsPutToSide.RemoveAt(0);
                    AfterRoundChipEffects();
                }

            }
            if (ingredientsPutToSide[0] == "mushroomFour")
            {
                buttonsToAddLeftover.SetActive(true);
                aboveCauldronText.text = "large mushroom put to side left over. Add now or leave for future round";
                button1.SetActive(true);
                button2.SetActive(true);
                choiceOne.text = "Add to pot";
                choiceTwo.text = "don't add";
               
                await CheckWhichChoice();
                if (choiceOneCauldron)
                {
                    ingredientsPutToSide.RemoveAt(0);
                    leftOverIngredientLocation = leftOverIngredient.transform.position;
                    Instantiate(ingredients[12], leftOverIngredientLocation, Quaternion.identity);
                    AfterRoundChipEffects();
                }
                if (choiceTwoCauldron)
                {
                    ingredientsPutToSideForNextRound.Add(ingredientsPutToSide[0]);
                    ingredientsPutToSide.RemoveAt(0);
                    AfterRoundChipEffects();
                }
            }
            ResetChoices();
        }
        else return;
       
    }

    public void AddDroplet()
    {
        InitialScore += 1;
    }

  
    public async void CheckMoths()
    {
        ResetChoices();
        //_networkConnect = FindObjectOfType<NetworkConnect>();
        //numberOfPlayers = _networkConnect.players;
        //if (numberOfPlayers > 2) { hawkMothRule = 2; }
        winnerManager = FindObjectOfType<WinnerManager>();
        int moths = 0;
        foreach (string ingredient in ingredientsList)
        {
            if (ingredient == "mothOne") { moths++; }
        }
        if (moths > 0)
        {
            winnerManager.AmountOfMoths(moths);

        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            button5.SetActive(true);
            aboveCauldronText.text = "No moths in your potion";
            await CheckWhichChoice();
            Debug.Log("no moths in cauldron");

        }
        Debug.Log("checkingMoths");
        ResetChoices();
        CheckSpiders();

    }

    public async void CheckSpiders()
    {
        ResetChoices();
        int spiders = 0;
        foreach (string ingredient in ingredientsList)
        {
            if (ingredient == "spiderOne") { spiders++; }
            if (ingredient == "spiderTwo") { spiders += 2; }
            if (ingredient == "spiderFour") { spiders += 4; }
        }
        if (spiders > 0)
        {

            if (gardenSpiderRule == 1)
            {
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") || ingredientsList[ingredientsList.Count].Contains("spider"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had a spider last or next to last in your pot, you may pay one ruby to to add a droplet to your pot permanatly";
                    button1.SetActive(true);
                    button2.SetActive(true);
                    choiceOne.text = "buy droplet";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        _playerData.Rubies.Value = -1;
                        ChangeRubyUI();
                        AddDroplet();
                        ResetScore();
                        CheckGhostsBreath();
                        cauldronScoreFront.text = Score.ToString();
                        cauldronScoreBack.text = Score.ToString();
                    }

                    ResetChoices();
                }
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") && ingredientsList[ingredientsList.Count].Contains("spider"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had spiders both last or next to last in your pot, you may pay one ruby to to add a droplet to your pot permanatly for each spider";
                    int turn = 1;
                    button1.SetActive(true);
                    button2.SetActive(true);
                    choiceOne.text = "buy droplet";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        if (_playerData.Rubies.Value >= 0)
                        {
                            _playerData.Rubies.Value = -1;
                            ChangeRubyUI();
                            AddDroplet();
                            ResetScore() ;
                            cauldronScoreFront.text = Score.ToString();
                            cauldronScoreBack.text = Score.ToString();
                            if (turn == 1)
                            {
                                turn += 1;
                                CheckSpiders();
                            }
                            else
                            {
                                turn = 1;

                            }
                            cauldronScoreFront.text = Score.ToString();
                            cauldronScoreBack.text = Score.ToString();
                        }
                    }

                    if (choiceTwoCauldron)
                    {
                        if (turn == 1)
                        {
                            turn += 1;
                            CheckSpiders();
                        }
                        else
                        {
                            turn = 1;
                            CheckGhostsBreath();
                        }

                    }

                }

            }

            if (gardenSpiderRule == 2)
            {
                if (quality.GetCherryBombs() == 7 && ingredientsList.Contains("spider"))
                {
                    int totalSpiders = 0;
                    foreach (string ingredient in ingredientsList)
                    {
                        if (ingredient == "spiderOne")
                        {
                            totalSpiders++;
                        }
                        if (ingredient == "spiderTwo")
                        {
                            totalSpiders += 2;
                        }
                        if (ingredient == "spiderFour")
                        {
                            totalSpiders += 4;
                        }
                    }

                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = $"you have a spider in your potion and  also the Cherrybombs equal 7 so you get to choose, do you want to add total of spiders to pot ({totalSpiders})";
                    button1.SetActive(true);
                    button2.SetActive(true);
                    choiceOne.text = "add to pot";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        Score += totalSpiders;
                    }


                }


            }
            if (gardenSpiderRule == 3)
            {
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") || ingredientsList[ingredientsList.Count].Contains("spider"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had a spider last or next to last in your pot, you get a ruby!";
                    button5.SetActive(true);
                    //some sound effect for rubies
                    _playerData.Rubies.Value++;
                    
                    await CheckWhichChoice();
                    ChangeRubyUI();

                }
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") && ingredientsList[ingredientsList.Count].Contains("spider"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had a spider last AND next to last in your pot, you get TWO rubies!";
                    button5.SetActive(true);
                    //some sound effect for rubies
                    _playerData.Rubies.Value += 2;
                   
                    await CheckWhichChoice();
                    ChangeRubyUI();
                }

            }

            if (gardenSpiderRule == 4)
            {
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider"))
                {
                    grabIngredient = FindObjectOfType<GrabIngredient>();
                    if (ingredientsList[ingredientsList.Count - 1] == "spiderOne")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a small spider next to last in your pot, added pumpkin to bag!";
                        button5.SetActive(true);
                        //some sound effect for Pumpkin
                        grabIngredient.AddToBagPermanantly(14);
                        await CheckWhichChoice();
                    }
                    if (ingredientsList[ingredientsList.Count - 1] == "spiderTwo")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a medium spider next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "Crow skull";
                        choiceTwo.text = "Mushroom";

                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            //sound effect of crow
                            grabIngredient.AddToBagPermanantly(4);
                        }
                        if (choiceOneCauldron)
                        {
                            //sound effect of mushroom
                            grabIngredient.AddToBagPermanantly(12);
                        }

                    }
                    if (ingredientsList[ingredientsList.Count - 1] == "spiderFour")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a large spider next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "Mandrake";
                        choiceTwo.text = "GhostsBreath";

                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            //sound effect of screaming? like nice screaming?
                            grabIngredient.AddToBagPermanantly(8);
                        }
                        if (choiceOneCauldron)
                        {
                            //sound effect of a "BOO"
                            grabIngredient.AddToBagPermanantly(6);
                        }//text saying large spider second to last in pot, choose to add small mandrake or ghostsbreath
                    }
                }
                if (ingredientsList[ingredientsList.Count].Contains("spider"))
                {
                    grabIngredient = FindObjectOfType<GrabIngredient>();

                    if (ingredientsList[ingredientsList.Count] == "spiderOne")
                    {

                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a small spider next to last in your pot, added pumpkin to bag!";
                        button5.SetActive(true);
                        //some sound effect for Pumpkin
                        grabIngredient.AddToBagPermanantly(14);
                        await CheckWhichChoice();


                    }
                    if (ingredientsList[ingredientsList.Count] == "spiderTwo")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a medium spider next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "Crow skull";
                        choiceTwo.text = "Mushroom";

                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            //sound effect of crow
                            grabIngredient.AddToBagPermanantly(4);
                        }
                        if (choiceOneCauldron)
                        {
                            //sound effect of mushroom
                            grabIngredient.AddToBagPermanantly(12);
                        }

                    }
                    if (ingredientsList[ingredientsList.Count] == "spiderFour")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a large spider next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "Mandrake";
                        choiceTwo.text = "GhostsBreath";

                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            //sound effect of screaming? like nice screaming?
                            grabIngredient.AddToBagPermanantly(8);
                        }
                        if (choiceOneCauldron)
                        {
                            //sound effect of a "BOO"
                            grabIngredient.AddToBagPermanantly(6);
                        }
                    }

                }


            }

        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            aboveCauldronText.text = "You had no spiders in your pot";
            button5.SetActive(true);
            await CheckWhichChoice();
            Debug.Log("no spiders");
        }
        Debug.Log("checkSpiders");
        ResetChoices();
        CheckGhostsBreath();
    }




    public async Task CheckWhichChoice()
    {
        while (!ChoiceChosem())
        {
            Debug.Log("no choice chosen");
          
            await Task.Yield();
        }
        Debug.Log("choice MADE");

    }

    public bool ChoiceChosem()
    {
        
            if (!choiceOneCauldron && !choiceTwoCauldron && !choiceThreeCauldron && !choiceFourCauldron && !choiceFiveCauldron)
            {
               return false;
            }
            else
            {
                return true; 
            }
      
    }

    public void ResetChoices()
    {
        choiceTwoCauldron = false;
        choiceOneCauldron = false;
        choiceFourCauldron = false;
        choiceThreeCauldron = false;
        choiceFiveCauldron = false;
    }


    public async void CheckGhostsBreath()
    {
        int ghosts = 0;
        foreach (string ingredient in ingredientsList)
        {
            if (ingredient == "ghostOne") { ghosts++; }
 
        }
        if (ghosts > 0)
        {
            if (ghostsBreathRule == 1)
            {
                if (ghosts == 1)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had one GhostsBreath in your potion and get one extra victory point";
                    button5.SetActive(true);
                    VictoryPoints++;
                    await CheckWhichChoice(); 
                }
                if (ghosts == 2)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had two GhostsBreath in your potion and get one extra victory point and one extra ruby!";
                    button5.SetActive(true);
                    VictoryPoints++;
                    _playerData.Rubies.Value++;
                    ChangeRubyUI();
                    await CheckWhichChoice();
                }
                if (ghosts >= 3)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = $"You had {ghosts} GhostsBreath in your potion and get two extra victory points and one droplet";
                    button5.SetActive(true);
                    VictoryPoints += 2;
                    InitialScore++;
                    await CheckWhichChoice();
                }
                //For 1, 2 or 3 purple chips, you receive the indicated bonus.  1 = 1 victory point 2 = victory point and ruby 3 = 2 victory points and teardrop forward one space.COSTS 9. - END OF ROUND
            }
            if (ghostsBreathRule == 2)
            {
                buttonsToAddLeftover.SetActive(true);
                aboveCauldronText.text = "You may trade the ghosts breath in your potion for these things. One for a Moth, Victory point and a Ruby. Two for a small spider, a medium crow skull, 3 victory points and one droplet. 3 for a large Mandrake, 6 victory points, a ruby and two droplets";
                button1.SetActive(true);
                button2.SetActive(true);
                if (ghosts == 1) {
                    aboveCauldronText.text = "You have one ghostsbreath in your potion, you may permanantly trade the ghostsbreath in your potion for these things. One for a HawkMoth, Victory point and a Ruby.";
                    choiceOne.text = "trade one";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        _playerData.VictoryPoints.Value += 1;
                        _playerData.Rubies.Value += 1;
                        grabIngredient.AddToBagPermanantly(10);
                    }
                  
                }
                if (ghosts == 2) {
                    button3.SetActive(true);
                    aboveCauldronText.text = "You have two ghostsbreath in your potion, you may permanantly trade the ghosts breath in your potion for these things. One for a Moth, Victory point and a Ruby. Trade two for a small spider, a medium crow skull, 3 victory points and one droplet.";
                    choiceOne.text = "trade one";
                    choiceTwo.text = "trade two";
                    choiceThree.text = "skip";

                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        _playerData.VictoryPoints.Value += 1;
                        _playerData.Rubies.Value += 1;
                        grabIngredient.AddToBagPermanantly(10);
                        ChangeRubyUI();
                    }
                    if (choiceTwoCauldron)
                    {
                        _playerData.VictoryPoints.Value += 3;
                        AddDroplet();
                        ResetScore();
                        grabIngredient.AddToBagPermanantly(16);
                        grabIngredient.AddToBagPermanantly(5);
                    }
                    ResetChoices();
                }
                if (ghosts >= 3)
                {
                    button3.SetActive(true);
                    button4.SetActive(true);
                    aboveCauldronText.text = "You have three or more ghostsbreath in your potion, you may trade the ghosts breath in your potion for these things. One for a Moth, Victory point and a Ruby. Trade two for a small spider, a medium crow skull, 3 victory points and one droplet. Trade three for a large Mandrake, 6 victory points, a ruby and two droplets";
                    choiceOne.text = "trade one";
                    choiceTwo.text = "trade two";
                    choiceThree.text = "trade three";
                    choiceFour.text = "skip";

                    await CheckWhichChoice();
                    if (choiceOneCauldron)
                    {
                        //add ui and soundeffects to show these things added
                        _playerData.VictoryPoints.Value += 1;
                        _playerData.Rubies.Value += 1;
                        grabIngredient.AddToBagPermanantly(10);
                    }
                    if (choiceTwoCauldron)
                    {
                        _playerData.VictoryPoints.Value += 3;
                        AddDroplet();
                        ResetScore();
                        cauldronScoreFront.text = Score.ToString();
                        cauldronScoreBack.text = Score.ToString();
                        grabIngredient.AddToBagPermanantly(16);
                        grabIngredient.AddToBagPermanantly(5);
                    }
                    if (choiceThreeCauldron)
                    {
                        _playerData.VictoryPoints.Value += 6;
                        AddDroplet();
                        AddDroplet();
                        ResetScore();
                        cauldronScoreFront.text = Score.ToString();
                        cauldronScoreBack.text = Score.ToString();
                        grabIngredient.AddToBagPermanantly(7);
                    }
                    ResetChoices();
                }

                //add two more buttons for choice 3 and skip - also add ifs so buttons only appear in there are enough ghosts to trade that much.
                //You may exchange the purple chips in your pot for the indicated bonus 1 = Moth, victory point and ruby 2 = small spider medium skull 3 victory points and space forward. 3 = large mandrake 6 victory points a ruby and 2 spaces forward. COSTS 12 - END OF ROUND
            }
            if (ghostsBreathRule == 3)
            {
                List<string> smallIngredients = new();
                List<string> mediumIngredients = new();
                foreach (string ingredient in ingredientsList)
                {
                    if (ingredient == "mandrakeOne" && !smallIngredients.Contains("mandrakeOne")|| ingredient == "spiderOne" && !smallIngredients.Contains("spiderOne") || ingredient == "mushroomOne" && !smallIngredients.Contains("mushroomOne") || ingredient == "crowSkullOne" && !smallIngredients.Contains("crowSkullOne")) { smallIngredients.Add(ingredient); }
                    if (ingredient == "mandrakeTwo" && !mediumIngredients.Contains("mandrakeTwo") || ingredient == "spiderTwo" && !mediumIngredients.Contains("spiderTwo") || ingredient == "mushroomTwo" && !mediumIngredients.Contains("mushroomTwo") || ingredient == "crowSkullTwo" && !mediumIngredients.Contains("crowSkullTwo")) { mediumIngredients.Add(ingredient); }

                }
                buttonsToAddLeftover.SetActive(true);
                if (ghosts == 1)
                {
                    if(smallIngredients.Count == 1)
                    {
                        aboveCauldronText.text = "You had one ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";
                        
                        button1.SetActive(true);
                      
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                   
                    }
                    if (smallIngredients.Count == 2)
                    {
                        aboveCauldronText.text = "You had one ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                     
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (smallIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }

                    }
                    if (smallIngredients.Count == 3)
                    {
                        aboveCauldronText.text = "You had one ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                   
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (smallIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                        if (choiceThreeCauldron)
                        {
                            if (smallIngredients[2].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[2].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[2].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[2].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                    }
                    if (smallIngredients.Count == 4)
                    {
                        aboveCauldronText.text = "You had one ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                        button4.SetActive(true);
                       
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        choiceFour.text = smallIngredients[3].Substring(0, smallIngredients[3].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (smallIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                        if (choiceThreeCauldron)
                        {
                            if (smallIngredients[2].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[2].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[2].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[2].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                        if (choiceFourCauldron)
                        {
                            if (smallIngredients[3].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(9);
                            }
                            if (smallIngredients[3].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(17);
                            }
                            if (smallIngredients[3].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(13);
                            }
                            if (smallIngredients[3].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(5);
                            }
                        }
                    }
                }
                if (ghosts == 2)
                {
                    if (mediumIngredients.Count == 1)
                    {
                        aboveCauldronText.text = "You had two ghostsbreath in your potion, you can trade one medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);

                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (mediumIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                    }
                    if (mediumIngredients.Count == 2)
                    {
                        aboveCauldronText.text = "You had two ghostsbreath in your potion, you can trade one medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);

                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        choiceTwo.text = mediumIngredients[1].Substring(0, mediumIngredients[1].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (mediumIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (mediumIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                    }
                    if (mediumIngredients.Count == 3)
                    {
                        aboveCauldronText.text = "You had two ghostsbreath in your potion, you can trade one medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);

                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        choiceTwo.text = mediumIngredients[1].Substring(0, mediumIngredients[1].Length - 3);
                        choiceThree.text = mediumIngredients[2].Substring(0, mediumIngredients[2].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (mediumIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (mediumIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceThreeCauldron)
                        {
                            if (mediumIngredients[2].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[2].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[2].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[2].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                    }
                    if (mediumIngredients.Count == 4)
                    {
                        aboveCauldronText.text = "You had two ghostsbreath in your potion, you can trade one medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                        button4.SetActive(true);
                        button5.SetActive(true);
                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        choiceTwo.text = mediumIngredients[1].Substring(0, mediumIngredients[1].Length - 3);
                        choiceThree.text = mediumIngredients[2].Substring(0, mediumIngredients[2].Length - 3);
                        choiceFour.text = mediumIngredients[3].Substring(0, mediumIngredients[3].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (mediumIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (mediumIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceThreeCauldron)
                        {
                            if (mediumIngredients[2].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[2].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[2].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[2].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceFourCauldron)
                        {
                            if (mediumIngredients[3].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(9);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (mediumIngredients[3].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(17);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (mediumIngredients[3].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(13);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (mediumIngredients[3].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(5);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                    }
                }
                if (ghosts >= 3)
                {
                    if (smallIngredients.Count == 1)
                    {
                        aboveCauldronText.text = "You had one ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);

                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }

                    }
                    if (smallIngredients.Count == 2)
                    {
                        aboveCauldronText.text = "You had three ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);

                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (smallIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                    }
                    if (smallIngredients.Count == 3)
                    {
                        aboveCauldronText.text = "You had three ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);

                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (smallIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceThreeCauldron)
                        {
                            if (smallIngredients[2].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[2].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[2].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[2].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }

                    }
                    if (smallIngredients.Count == 4)
                    {
                        aboveCauldronText.text = "You had three ghostsbreath in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                        button4.SetActive(true);
                        button5.SetActive(true);
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        choiceFour.text = smallIngredients[3].Substring(0, smallIngredients[3].Length - 3);
                        await CheckWhichChoice();
                        if (choiceOneCauldron)
                        {
                            if (smallIngredients[0].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[0].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[0].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[0].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceTwoCauldron)
                        {
                            if (smallIngredients[1].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[1].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[1].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[1].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceThreeCauldron)
                        {
                            if (smallIngredients[2].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[2].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[2].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[2].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                        if (choiceFourCauldron)
                        {
                            if (smallIngredients[3].Contains("mandrake"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(8);
                                grabIngredient.AddToBagPermanantly(7);
                            }
                            if (smallIngredients[3].Contains("spider"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(16);
                                grabIngredient.AddToBagPermanantly(15);
                            }
                            if (smallIngredients[3].Contains("mushroom"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(12);
                                grabIngredient.AddToBagPermanantly(11);
                            }
                            if (smallIngredients[3].Contains("crowSkull"))
                            {
                                grabIngredient.RemoveItemFromBagPermanantly(4);
                                grabIngredient.AddToBagPermanantly(3);
                            }
                        }
                    }
                }
           
                // Trade 1 chip from the pot for another chip of the same colour with a greater value according to the chart. 1 = small to medium 2 = medium to large 3 = small to large COSTS 11 - END OF ROUND WOULD NNED 4 BUTTONS TOO.
            }
            if (ghostsBreathRule == 4)
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                aboveCauldronText.text = $"ghosts breath in potion adds {extraPoints} extra points";
                Score += extraPoints;
                await CheckWhichChoice();


            }
        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            aboveCauldronText.text = "You had no ghosts breath in your pot";
            button5.SetActive(true);
            await CheckWhichChoice();
            Debug.Log("no ghosts breath");
           
        }
        ResetChoices();
        Debug.Log("check ghosts breath end");
        cauldronScoreFront.text = Score.ToString();
        cauldronScoreBack.text = Score.ToString();

        AddRubiesCoinsAndVp();
    }

    public async void AddRubiesCoinsAndVp()
    {
        if (RubiesThisRound)
        {
            buttonsToAddLeftover.SetActive(true);
            button5.SetActive(true);
            aboveCauldronText.text = "You get one ruby this round!";
            _playerData.Rubies.Value += 1;
            await CheckWhichChoice();
            ResetChoices();
            Debug.Log("added rubies");
            ChangeRubyUI();
        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            button5.SetActive(true);
            aboveCauldronText.text = "You don't get a ruby this round";
            await CheckWhichChoice();
            ResetChoices();
            Debug.Log("no rubies to add");
        }
        
        aboveCauldronText.text = $"You get {VictoryPoints} Victory Points this round!";
        button5.SetActive(true);
        _playerData.VictoryPoints.Value += VictoryPoints;
        await CheckWhichChoice();
        ResetChoices();
        Debug.Log("Added victory points");


        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = $"You get {Coins} Coins this round!";
        _playerData.Coins.Value += Coins;
        await CheckWhichChoice();
        ResetChoices();
        Debug.Log("Added coins");


        buttonsToAddLeftover.SetActive(false);
        quality.ResetCherryBombs();
        quality.SetCherryBombText();
        ResetInsidePot();

        if (winnerManager.round == 8)
        {
            GameManager.Instance.UpdateGameState(GameState.DeclareWinner);
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.BuyIngredients);
        }
        //timer then add victory points with UI And then same with coins - then gamemanager set state to buyingredients
    }


    public void resetScoreText()
    {
        cauldronScoreFront.text = Score.ToString();
        cauldronScoreBack.text = Score.ToString();
    }


    public void CountIngredientsInPot()
    {
        int bombs = 0;
        int crowSkull = 0;
        int ghost = 0;
        int mandrake = 0;
        int moth = 0;
        int toadstall = 0;
        int pumpkin = 0;
        int spider = 0;

        foreach (string ingredient in ingredientsList)
        {
            if (ingredient.Contains("cherryBomb")) { bombs++; }
            if (ingredient.Contains("crowSkull"))
            { crowSkull++; }
            if (ingredient.Contains("ghost"))
            { ghost++; }
            if (ingredient.Contains("mandrake"))
            { mandrake++; }
            if (ingredient.Contains("moth"))
            { moth++; }
            if (ingredient.Contains("mushroom"))
            { toadstall++; }
            if (ingredient.Contains("pumpkin"))
            { pumpkin++; }
            if (ingredient.Contains("spider"))
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

    public async void SpendRubiesUI()
    {
        Debug.Log("start spend rubies ui");
        if (_playerData.Rubies.Value >= 2)
        {
            button1.SetActive(true);
            choiceOne.text = "Buy droplet";
            button2.SetActive(true);
            int rubiesToSpend = _playerData.Rubies.Value;
            if (_playerData.PurifierFull.Value == false)
            {
                button3.SetActive(true);
                aboveCauldronText.text = $"You have {rubiesToSpend} rubies, do you want to buy a droplet or refill your purifier potion?";
                choiceTwo.text = "Buy refill of purifier potion";
                choiceThree.text = "Skip";
                await CheckWhichChoice();
                if (choiceOneCauldron)
                {
                    BuyDrop();
                    SpendRubiesUI();
                }
                if (choiceTwoCauldron)
                {
                    FillPurifier();
                    SpendRubiesUI();
                }

                ResetChoices();

            }
            else
            {
                aboveCauldronText.text = $"You have {rubiesToSpend} rubies, do you want to buy a droplet?";
                choiceTwo.text = "Skip";

                await CheckWhichChoice();
                if (choiceOneCauldron)
                {
                    BuyDrop();
                    SpendRubiesUI();
                }
                ResetChoices();

            }
        }
        Debug.Log("end spend rubies ui");
        ReadyForNextRound();
    }
    public async void ReadyForNextRound()
    {
        Debug.Log("ready for next round function");
        buttonsToAddLeftover.SetActive(true);
        ResetScore();
        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();
        ChangeSceneryDependingOnRound();


        //RAT TAILS - ADD RAT TAILS BECK MY GOD
        await winnerManager.CalculateRatTails();

        int ratTails = _playerData.RatTails.Value;
        button5.SetActive(true);
        aboveCauldronText.text = $"You have {ratTails} rat tails to add to your pot this round";
        Score += ratTails;
        
        await CheckWhichChoice();
        aboveCauldronText.text = "";
        resetScoreText();
        ResetChoices();
        ResetStuffInBook();
        Debug.Log("about to go into fortune teller");
        GameManager.Instance.UpdateGameState(GameState.FortuneTeller);
    }



    public void BuyDrop()
    {
       AddDroplet();
       _playerData.Rubies.Value -= 2;
       ResetScore();
       resetScoreText();
       ChangeRubyUI(); 
    }

    public void FillPurifier()
    {
        _playerData.Rubies.Value -= 2;
        _playerData.PurifierFull.Value = true;
        ChangeRubyUI();
        //when have proper 3d model fill with the liquid.
    }

    public void ChangeSceneryDependingOnRound()
    {
        if (winnerManager.round == 2)
        {
            RenderSettings.skybox = Round2Sky;

        }
        if (winnerManager.round == 3)
        {
            RenderSettings.skybox = Round3Sky;

        }
        if (winnerManager.round == 4)
        {
            RenderSettings.skybox = Round4Sky;

        }
        if (winnerManager.round == 5)
        {
            RenderSettings.skybox = Round5Sky;

        }
        if (winnerManager.round == 6)
        {
            RenderSettings.skybox = Round6Sky;

        }
        if (winnerManager.round == 7)
        {
            RenderSettings.skybox = Round7Sky;

        }
        if (winnerManager.round == 8)
        {
            RenderSettings.skybox = Round8Sky;
        }
    }

    public void ResetSky()
    {
        RenderSettings.skybox = Round1Sky;
    }

    public void ChangeRubyUI()
    {
        rubyNumber.text = _playerData.Rubies.Value.ToString();
        rubyInstantiationLocation = rubylocationSphere.transform.position;

        Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
    }
}




