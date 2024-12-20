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
using UnityEditor.SceneManagement;
using Meta.WitAi;



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
    private FortuneManager _fortuneManager;

    [SerializeField] GameObject leftOverIngredient;
    [SerializeField] GameObject rubylocationSphere;
    [SerializeField] GameObject buttonsToAddLeftover;

    [SerializeField] AudioSource ghostClip;
    [SerializeField] AudioSource spiderClip;
    [SerializeField] AudioSource potionEffectClip;

    Vector3 leftOverIngredientLocation;
    Vector3 rubyInstantiationLocation;
    [SerializeField] GameObject rubyForBowl;
    [SerializeField] GameObject droplet;

    [SerializeField] GameObject bottleUp;

    NetworkConnect _networkConnect;
    public List<GameObject> ingredients;
    WinnerManager winnerManager;
    GrabIngredient grabIngredient;
    AnimatorScript _animatorScript;

    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    [SerializeField] GameObject button3;
    [SerializeField] GameObject button4;
    [SerializeField] GameObject button5;
    [SerializeField] GameObject button6;
    [SerializeField] GameObject button7;
    [SerializeField] TextMeshProUGUI aboveCauldronText;
    [SerializeField] TextMeshProUGUI choiceOne;
    [SerializeField] TextMeshProUGUI choiceTwo;
    [SerializeField] TextMeshProUGUI choiceThree;
    [SerializeField] TextMeshProUGUI choiceFour;
    [SerializeField] TextMeshProUGUI choiceFive;
    [SerializeField] TextMeshProUGUI choiceSix;
    [SerializeField] TextMeshProUGUI choiceSeven;
    [SerializeField] TextMeshProUGUI rubyNumber;
    [SerializeField] GameObject bagSphere;

    private bool isCheckingChoice = false;



    private GameObject rubyOne;
    private GameObject rubyTwo;
    private GameObject rubyThree;
    private GameObject rubyFour;
    private GameObject rubyFive;
    private GameObject rubySix;
    private GameObject rubySeven;
    private GameObject rubyEight;

    private int RubyobjNum = 0;

    [SerializeField] GameObject ratTailObject;

    public Material Round1Sky;
   
    public Material Round3Sky;

    public Material Round5Sky;

    public Material Round7Sky;

    public Material Round9Sky;

    [SerializeField] private GameObject potionOne;

    public string lastIngredient;

    public bool choiceOneCauldron = false;
    public bool choiceTwoCauldron = false;
    public bool choiceThreeCauldron = false;
    public bool choiceFourCauldron = false;
    public bool choiceFiveCauldron = false;
    public bool choiceSixCauldron =false;
    private int numberOfPlayers;

    private float startHeightPotion;

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
        _fortuneManager = FindObjectOfType<FortuneManager>();
        CountIngredientsInPot();
        startHeightPotion = potionOne.transform.position.y;
        winnerManager = FindObjectOfType<WinnerManager>();
        _animatorScript = FindObjectOfType<AnimatorScript>();

    }
    //TWO FUNCTIONS


    public async void PotExplosionEndRound()
    {
        ResetChoices();

        _playerData = FindObjectOfType<PlayerData>();
        if (crowSkullRule == 3)
        {
            
            if (ingredientsList[ingredientsList.Count - 1] == "crowSkullOne")
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                potionEffectClip.Play();
                aboveCauldronText.text = "<color=#89CFF0>Small</color> <sprite name=\"crowSkull\">  last in cauldron saved your potion, you get to keep your <sprite name=\"VP\">  and <sprite name=\"coin\"> ";
              
                _playerData.Coins.Value = Coins;
                _playerData.VictoryPoints.Value = VictoryPoints;
                _playerData.Score.Value = 0;
                await CheckWhichChoice(aboveCauldronText.text);
                ResetChoices();
                EndRoundSafely();
            }
            if (ingredientsList[ingredientsList.Count - 1] == "crowSkullTwo" || ingredientsList[ingredientsList.Count - 2] == "crowSkullTwo")
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                potionEffectClip.Play();
                aboveCauldronText.text = "<color=#89CFF0>Medium</color> <sprite name=\"crowSkull\">  one of the last two ingredients in cauldron saved your potion, you get to keep your <sprite name=\"VP\">  and <sprite name=\"coin\"> ";
                
                _playerData.Coins.Value = Coins;
                _playerData.VictoryPoints.Value = VictoryPoints;
                Score = 0;
                await CheckWhichChoice(aboveCauldronText.text);
                ResetChoices();
                EndRoundSafely();
            }
            if (ingredientsList[ingredientsList.Count - 1] == "crowSkullFour" || ingredientsList[ingredientsList.Count - 2] == "crowSkullFour" || ingredientsList[ingredientsList.Count - 3] == "crowSkullFour")
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                potionEffectClip.Play();
                aboveCauldronText.text = "<color=#89CFF0>Large</color> <sprite name=\"crowSkull\">  one of the last three ingredients in cauldron saved your potion, you get to keep your <sprite name=\"VP\">  and <sprite name=\"coin\"> ";
              
                _playerData.Coins.Value = Coins;
                _playerData.VictoryPoints.Value = VictoryPoints;
                _playerData.Score.Value = 0;
                await CheckWhichChoice(aboveCauldronText.text);
                ResetChoices();
                EndRoundSafely();
            }
            else
            {
                buttonsToAddLeftover.SetActive(true);
                button1.SetActive(true);
                button2.SetActive(true);
                
                aboveCauldronText.text = "POT EXPLODED!!!!      WHAT DO YOU WANT TO KEEP?";
                choiceOne.text = "COiNS";
                choiceTwo.text = "VICTORY POINTS";
            }

        }
        else
        {

            buttonsToAddLeftover.SetActive(true);
            button1.SetActive(true);
            button2.SetActive(true);

            aboveCauldronText.text = "POT EXPLODED!!!!      WHAT DO YOU WANT TO KEEP?";
            choiceOne.text = "<sprite name=\"coin\">";
            choiceTwo.text = "<sprite name=\"VP\">";


        }
        if (_playerData.Colour.Value == "Purple")
        {
            winnerManager.purpleExploded.Value = true;
        }
        if (_playerData.Colour.Value == "Red")
        {
            winnerManager.redExploded.Value = true;
        }
        if (_playerData.Colour.Value == "Blue")
        {
            winnerManager.blueExploded.Value = true;
        }
        if (_playerData.Colour.Value == "Yellow")
        {
            winnerManager.yellowExploded.Value = true;
        }
        await CheckWhichChoice(aboveCauldronText.text);
        if (choiceOneCauldron)
        {
            ChooseCoins();
            ResetChoices();
        }
        if (choiceOneCauldron)
        {
            ChooseVictoryPoints();
            ResetChoices();
        }
        
        
        

     
    }
    public void ResetInsidePot()
    {
        ingredientsList.Clear();
    }
    public void ResetScore() { Score = InitialScore;
        
    }

    public async void OnTriggerEnter(Collider other)
    {

        if (other.tag.Contains("Untagged") || other.tag.Contains("potion") || other.tag.Contains("purifier") || other.tag.Contains("droplet"))
        {
            Debug.Log(other.gameObject.tag);
            return;
        }
        else
        {



            
            ingredientsList.Add(other.gameObject.tag);
            extraPoints = 0; extraRubies = 0;
            lastIngredient = other.gameObject.tag;

            if (other.gameObject.tag.Contains("ghost"))
            {
                ghostClip.Play();
            }
            if (other.gameObject.tag.Contains("spider"))
            {
                spiderClip.Play();
            }

            if (mushroomRule == 1)
            {
                if (other.gameObject.tag.Contains("mushroom") && ingredientsList.Count >= 2 && ingredientsList[ingredientsList.Count - 2].Contains("cherryBomb"))
                {

                    if (ingredientsList[ingredientsList.Count - 2].Contains("One"))
                    {

                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFFFFF>Small</color> <sprite name=\"cherrybomb\">  was right before <sprite name=\"toadstool\"> ! Add <color=#800080>1</color> <sprite name=\"droplet\"> !";
                        //sound effect
                        Score += 1;
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    else if (ingredientsList[ingredientsList.Count - 2].Contains("Two"))
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFFFFF>Medium</color> <sprite name=\"cherrybomb\">  was right before <sprite name=\"toadstool\"> ! Add <color=#800080>2</color> <sprite name=\"droplet\"> !";
                        //sound effect
                        Score += 2;
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    else if (ingredientsList[ingredientsList.Count - 2].Contains("Three"))
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFFFFF>Large</color> <sprite name=\"cherrybomb\">  was right before <sprite name=\"toadstool\">  ! Add <color=#800080>3</color> <sprite name=\"droplet\"> !";
                        //sound effect
                        Score += 3;
                        await CheckWhichChoice(aboveCauldronText.text);
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
                    potionEffectClip.Play();
                    aboveCauldronText.text = "<sprite name=\"toadstool\">  in potion adds one to score when <color=#FFFFFF>small</color> <sprite name=\"cherrybomb\">  is added!";
                    //sound effect
                    Score += 1;
                    await CheckWhichChoice(aboveCauldronText.text);
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
                    potionEffectClip.Play();
                    aboveCauldronText.text = "<sprite name=\"pumpkin\">  in potion adds <color=#800080>1</color> <sprite name=\"droplet\">  when adding <sprite name=\"toadstool\"> !";
                    //sound effect
                    Score += 1;
                    await CheckWhichChoice(aboveCauldronText.text);
                    buttonsToAddLeftover.SetActive(false);
                    ResetChoices();

                }
                if (other.gameObject.tag.Contains("mushroom") && pumpkins >= 3)
                {
                    buttonsToAddLeftover.SetActive(true);
                    button5.SetActive(true);
                    potionEffectClip.Play();
                    aboveCauldronText.text = "<color=#CD7F32>3 or more</color> <sprite name=\"pumpkin\">  in potion adds <color=#800080>2</color> <sprite name=\"droplet\">  when adding <sprite name=\"toadstool\"> !";
                    //sound effect
                    Score += 2;
                    await CheckWhichChoice(aboveCauldronText.text);
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
                    potionEffectClip.Play();
                    aboveCauldronText.text = "Put <sprite name=\"toadstool\">  to the side and decide whether to add to pot at the end or keep to the side for a future round!";
                    await CheckWhichChoice(aboveCauldronText.text);
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
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley before doubles this ingredient!";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    else if (other.gameObject.tag.Contains("Two"))
                    {
                        Score += 2;
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley before doubles this ingredient!";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    else if (other.gameObject.tag.Contains("Three"))
                    {
                        Score += 3;
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley before doubles this ingredient!";
                        //some sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    else if (other.gameObject.tag.Contains("Four"))
                    {
                        Score += 4;
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley before doubles this ingredient!";
                        //some sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    ResetChoices();
                }
            }

            if (mandrakeRule == 2)
                if (other.gameObject.tag.Contains("mandrake") && ingredientsList[ingredientsList.Count - 1].Contains("cherryBomb"))
                {
                    if (ingredientsList[ingredientsList.Count - 1].Contains("One"))
                    {
                        ingredientsList.RemoveAt(ingredientsList.Count - 1);

                        grabIngredient.AddToBagThisRound(0);
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley after <sprite name=\"cherrybomb\"> ! Removing <sprite name=\"cherrybomb\">  from potion and putting back in your bag";
                        //having some sort of animation here to visulaise it might help.
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    if (ingredientsList[ingredientsList.Count - 1].Contains("Two"))
                    {
                        ingredientsList.RemoveAt(ingredientsList.Count - 1);

                        grabIngredient.AddToBagThisRound(2);
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley after <sprite name=\"cherrybomb\"> ! Removing <sprite name=\"cherrybomb\">  from potion and putting back in your bag";
                        //having some sort of animation here to visulaise it might help.
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                    }
                    if (ingredientsList[ingredientsList.Count - 1].Contains("Three"))
                    {
                        ingredientsList.RemoveAt(ingredientsList.Count - 1);

                        grabIngredient.AddToBagThisRound(1);
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<sprite name=\"mandrake\">  immediatley after <sprite name=\"cherrybomb\"> ! Removing <sprite name=\"cherrybomb\">  from potion and putting back in your bag";
                        //having some sort of animation here to visulaise it might help.
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFD700>1</color> <sprite name=\"mandrake\">  in potion! Limit of <sprite name=\"cherrybomb\"> increased to 8!";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);

                        buttonsToAddLeftover.SetActive(false); quality.AddToCherryBombLimit();
                        quality.SetCherryBombText();
                    }
                    if (mandrakes == 3)
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFD700>3</color> <sprite name=\"mandrake\">  in potion! Limit of <sprite name=\"cherrybomb\"> increased to 9!";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false);
                        quality.AddToCherryBombLimit();
                        quality.SetCherryBombText();
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
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFD700>1</color>  <sprite name=\"mandrake\">  in potion! Add <color=#800080>1</color> <sprite name=\"droplet\"> !";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false); Score += 1;
                    }
                    if (mandrakes == 2)
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFD700>2</color>  <sprite name=\"mandrake\">  in potion! Add <color=#800080>2</color> <sprite name=\"droplet\"> !";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
                        buttonsToAddLeftover.SetActive(false); Score += 2;
                    }

                    if (mandrakes >= 3)
                    {
                        buttonsToAddLeftover.SetActive(true);
                        button5.SetActive(true);
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#FFD700>3 or more</color> <sprite name=\"mandrake\">  in potion! Add <color=#800080>3</color> <sprite name=\"droplet\"> !";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        potionEffectClip.Play();
                        aboveCauldronText.text = "<color=#F0F8FF>Crow Skull</color> <sprite name=\"crowSkull\">  on score with <sprite name=\"ruby\"> ! Immediatley recieve <color=#FF0000>1</color> <sprite name=\"ruby\"> !";
                        //sound effect
                        await CheckWhichChoice(aboveCauldronText.text);
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
                            potionEffectClip.Play();
                            aboveCauldronText.text = "<color=#F0F8FF>Small Crow Skull</color> <sprite name=\"crowSkull\">  on score with <sprite name=\"ruby\"> ! Immediatley recieve <color=#006400>1</color> <sprite name=\"VP\"> !";
                            //sound effect
                            await CheckWhichChoice(aboveCauldronText.text);
                            buttonsToAddLeftover.SetActive(false);
                            _playerData.VictoryPoints.Value++;
                        }



                        if (other.gameObject.tag.Contains("Two"))
                        {
                            buttonsToAddLeftover.SetActive(true);
                            button5.SetActive(true);
                            potionEffectClip.Play();
                            aboveCauldronText.text = "<color=#F0F8FF>Medium Crow Skull</color> <sprite name=\"crowSkull\">  on score with <sprite name=\"ruby\"> ! Immediatley recieve <color=#006400>2</color> <sprite name=\"VP\"> !";
                            //sound effect
                            await CheckWhichChoice(aboveCauldronText.text);
                            buttonsToAddLeftover.SetActive(false);
                            _playerData.VictoryPoints.Value += 2;
                        }
                        if (other.gameObject.tag.Contains("Four"))
                        {
                            buttonsToAddLeftover.SetActive(true);
                            button5.SetActive(true);
                            potionEffectClip.Play();
                            aboveCauldronText.text = "<color=#F0F8FF>Medium Crow Skull</color><sprite name=\"crowSkull\">  on score with <sprite name=\"ruby\"> ! Immediatley recieve <color=#006400>4</color> <sprite name=\"VP\"> !";
                            //sound effect
                            await CheckWhichChoice(aboveCauldronText.text);
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
                        potionEffectClip.Play();
                        aboveCauldronText.text = $"<color=#F0F8FF>Small</color> <sprite name=\"crowSkull\"> ! Pick one random ingredient from your bag to add to your pot!";
                        await CheckWhichChoice(aboveCauldronText.text);

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
                        potionEffectClip.Play();
                        aboveCauldronText.text = $"<color=#F0F8FF>Medium</color> <sprite name=\"crowSkull\"> ! Pick one random ingredient from your bag to add to your pot!";
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        potionEffectClip.Play();
                        aboveCauldronText.text = $"<color=#F0F8FF>Large</color> <sprite name=\"crowSkull\"> ! Pick one random ingredient from your bag to add to your pot!";
                        await CheckWhichChoice(aboveCauldronText.text);
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
                            potionEffectClip.Play();
                            aboveCauldronText.text = "<color=#800080>Ghosts Breath</color> <sprite name=\"ghostsbreath\"> adds <color=#00FF00>1</color> <sprite name=\"VP\"> ";
                            extraVictoryPoints++;
                            await CheckWhichChoice(aboveCauldronText.text);
                            //buttonsToAddLeftover.SetActive(false);
                            ResetChoices();
                        }
                        if (Coins >= 20 && Coins < 30)
                        {
                            buttonsToAddLeftover.SetActive(true);
                            button5.SetActive(true);
                            potionEffectClip.Play();
                            aboveCauldronText.text = "<color=#800080>Ghosts Breath</color> <sprite name=\"ghostsbreath\"> adds <color=#00FF00>2</color> <sprite name=\"VP\"> ";
                            await CheckWhichChoice(aboveCauldronText.text);
                            //buttonsToAddLeftover.SetActive(false);
                            ResetChoices();

                            extraVictoryPoints += 2;
                        }
                        if (Coins >= 30)
                        {
                            buttonsToAddLeftover.SetActive(true);
                            button5.SetActive(true);
                            potionEffectClip.Play();
                            aboveCauldronText.text = "<color=#800080>Ghosts Breath</color> <sprite name=\"ghostsbreath\"> adds <color=#00FF00>3</color> <sprite name=\"VP\">.";

                            extraVictoryPoints += 3;
                            await CheckWhichChoice(aboveCauldronText.text);
                            //buttonsToAddLeftover.SetActive(false);
                            ResetChoices();
                        }
                    }
                }
     
                await _fortuneManager.DuringRoundFortuneEffects(other.gameObject.tag);

                cauldronScoreFront.text = Score.ToString();
                cauldronScoreBack.text = Score.ToString();
                CountIngredientsInPot();
                ChangePotionHeight();
                quality.nextIngredientTime = true;

               


            }
        }
    }

    public void ResetStuffInBook()
    {
        programInstance = new Program();
        Chips[] boardPlacement = programInstance.GetBoardPlacement();
        if (Score > 0)
        {
            Coins = boardPlacement[Score - 1].Coins;
            VictoryPoints = boardPlacement[Score - 1].VictoryPoints;
            RubiesThisRound = boardPlacement[Score - 1].Ruby;
        }
        else
        {
            Coins = boardPlacement[0].Coins;
            VictoryPoints = boardPlacement[0].VictoryPoints;
            RubiesThisRound = boardPlacement[0].Ruby;
        }

        FutureCoins = boardPlacement[Score].Coins;
        FutureVictoryPoints = boardPlacement[Score].VictoryPoints;
        FutureRubiesThisRound = boardPlacement[Score].Ruby;
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
        AfterRoundChipEffects();
        await _fortuneManager.PostRoundFortuneEffects();
        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = $"added <color=#006400>{VictoryPoints}</color> <sprite name=\"VP\">";
        await CheckWhichChoice(aboveCauldronText.text);
        Coins = 0;
        ResetChoices();
        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();
   


            
            GameManager.Instance.UpdateGameState(GameState.RollDice);
        
    }

    public async void ChooseCoins()
    {
        _playerData = FindObjectOfType<PlayerData>();
        AfterRoundChipEffects();
        await _fortuneManager.PostRoundFortuneEffects();
        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = $"added <color=#FFA500>{Coins}</color> <sprite name=\"coin\">";
        await CheckWhichChoice(aboveCauldronText.text);
        buttonsToAddLeftover.SetActive(false);
        ResetChoices();

        
        VictoryPoints = 0;

        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();

        
            GameManager.Instance.UpdateGameState(GameState.RollDice);
        

    }


    public async void EndRoundSafely()
    {
        Debug.Log(winnerManager.purpleExploded.Value);
        //afterRoundfFortuneEffects
        AfterRoundChipEffects();
        await _fortuneManager.PostRoundFortuneEffects();
        _playerData = FindObjectOfType<PlayerData>();
        

        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = "Round Over!";

        //buttonsToAddLeftover.SetActive(false);
        await CheckWhichChoice(aboveCauldronText.text);
          
            
        ResetChoices();
        _playerData.Score.Value = Score;
        ResetScore();
        cauldronScoreFront.text = Score.ToString();
        cauldronScoreBack.text = Score.ToString();
        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();
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
                aboveCauldronText.text = "<color=#FF0000>Small</color> <sprite name=\"toadstool\">  put to side left over. Add now or leave for future round";
                button1.SetActive(true);
                button2.SetActive(true);
                choiceOne.text = "Add to pot";
                choiceTwo.text = "don't add";
               
                
                await CheckWhichChoice(aboveCauldronText.text);
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
                aboveCauldronText.text = "<color=#FF0000>Medium</color> <sprite name=\"toadstool\">  put to side left over. Add now or leave for future round";
                button1.SetActive(true);
                button2.SetActive(true);
                choiceOne.text = "Add to pot";
                choiceTwo.text = "don't add";
                

                await CheckWhichChoice(aboveCauldronText.text);
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
                aboveCauldronText.text = "<color=#FF0000>Large</color> <sprite name=\"toadstool\">  put to side left over. Add now or leave for future round";
                button1.SetActive(true);
                button2.SetActive(true);
                choiceOne.text = "Add to pot";
                choiceTwo.text = "don't add";
               
                await CheckWhichChoice(aboveCauldronText.text);
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
        InitialScore+=1;
        leftOverIngredientLocation = leftOverIngredient.transform.position;
        
        GameObject dropletObj = Instantiate(droplet, leftOverIngredientLocation, Quaternion.identity);
        FunctionTimer.Create(() => Destroy(dropletObj), 1f);
    }

  
    public async void CheckMoths()
    {
        ResetChoices();
        //_networkConnect = FindObjectOfType<NetworkConnect>();
        //numberOfPlayers = _networkConnect.players;
        //if (numberOfPlayers > 2) { hawkMothRule = 2; }
       
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
            await MessageAboveCauldron("<color=#708090>0</color> <sprite name=\"moth\">  in your pot");
            //this currently doesn't show up when playing as blue player, not sure why because other things using the same function do.
            Debug.Log("no moths in cauldron");

        }
        Debug.Log("checkingMoths");
        //maybe make these checks tasks that happen in add VP and coins? 

        if (!isCheckingChoice)
        {
            ResetChoices();
            CheckSpiders();
        }


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
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") && _playerData.Rubies.Value >= 1 || ingredientsList[ingredientsList.Count - 2].Contains("spider") && _playerData.Rubies.Value >= 1)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had a <sprite name=\"spider\">  last or next to last in your pot, you may pay <color=#FF0000>1</color> <sprite name=\"ruby\">  to";
                    button1.SetActive(true);
                    button2.SetActive(true);
                    choiceOne.text = "buy <sprite name=\"droplet\">";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice(aboveCauldronText.text);
                    if (choiceOneCauldron)
                    {
                        _playerData.Rubies.Value--;
                        ChangeRubyUI();
                        AddDroplet();
                       
                        
                        cauldronScoreFront.text = Score.ToString();
                        cauldronScoreBack.text = Score.ToString();
                    }

                    ResetChoices();
                }
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") && ingredientsList[ingredientsList.Count - 2].Contains("spider") && _playerData.Rubies.Value >= 1)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had <sprite name=\"spider\">  both last AND next to last in your pot, you may pay <color=#FF0000>1</color> <sprite name=\"ruby\">  to";
                    int turn = 1;
                    button1.SetActive(true);
                    button2.SetActive(true);
                    choiceOne.text = "buy <sprite name=\"droplet\">";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice(aboveCauldronText.text);
                    if (choiceOneCauldron)
                    {
                        if (_playerData.Rubies.Value >= 0)
                        {
                            _playerData.Rubies.Value--;
                            ChangeRubyUI();
                            AddDroplet();
                         
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
                    aboveCauldronText.text = $"you have a <sprite name=\"spider\">  in your potion and the <sprite name=\"cherrybomb\">  <color=#00FF00>equal 7</color> so you CAN add <color=#006400>total of</color> <sprite name=\"spider\">  to pot ({totalSpiders})";
                    button1.SetActive(true);
                    button2.SetActive(true);
                    choiceOne.text = "add to pot";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice(aboveCauldronText.text);
                    if (choiceOneCauldron)
                    {
                        Score += totalSpiders;
                    }


                }


            }
            if (gardenSpiderRule == 3)
            {
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") || ingredientsList[ingredientsList.Count - 2].Contains("spider"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had a <sprite name=\"spider\">  last or next to last in your pot, you get a <sprite name=\"ruby\"> !";
                    button5.SetActive(true);
                    //some sound effect for rubies
                    _playerData.Rubies.Value++;
                    
                    await CheckWhichChoice(aboveCauldronText.text);
                    ChangeRubyUI();

                }
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider") && ingredientsList[ingredientsList.Count - 2].Contains("spider"))
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had a <sprite name=\"spider\">  last AND next to last in your pot, you get <color=#FF0000>2</color> <sprite name=\"ruby\"> !";
                    button5.SetActive(true);
                    //some sound effect for rubies
                  

                    await CheckWhichChoice(aboveCauldronText.text);

                    for (int i = 0; i <= 2; i++)
                    {
                        _playerData.Rubies.Value++;
                        ChangeRubyUI();
                    }
                }

            }

            if (gardenSpiderRule == 4)
            {
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider"))
                {
                    grabIngredient = FindObjectOfType<GrabIngredient>();
                    if (ingredientsList[ingredientsList.Count - 2] == "spiderOne")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a <color=#006400>small</color> <sprite name=\"spider\">  next to last in your pot, added <sprite name=\"pumpkin\">  to bag!";
                        button5.SetActive(true);
                        //some sound effect for Pumpkin
                        grabIngredient.AddToBagPermanantly(14);
                        await CheckWhichChoice(aboveCauldronText.text);
                    }
                    if (ingredientsList[ingredientsList.Count - 2] == "spiderTwo")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a <color=#006400>medium</color> <sprite name=\"spider\">  next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "<sprite name=\"crowSkull\">";
                        choiceTwo.text = "<sprite name=\"toadstool\">";

                        await CheckWhichChoice(aboveCauldronText.text);
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
                    if (ingredientsList[ingredientsList.Count - 2] == "spiderFour")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "You had a <color=#006400>large</color> <sprite name=\"spider\">  next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "<sprite name=\"mandrake\">";
                        choiceTwo.text = "<sprite name=\"ghostsbreath\">";

                        await CheckWhichChoice(aboveCauldronText.text);
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
                if (ingredientsList[ingredientsList.Count - 1].Contains("spider"))
                {
                    grabIngredient = FindObjectOfType<GrabIngredient>();

                    if (ingredientsList[ingredientsList.Count] == "spiderOne")
                    {

                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "<color=#006400>Small</color> <sprite name=\"spider\">  next to last in your pot, added <sprite name=\"pumpkin\">  to bag!";
                        button5.SetActive(true);
                        //some sound effect for Pumpkin
                        grabIngredient.AddToBagPermanantly(14);
                        await CheckWhichChoice(aboveCauldronText.text);


                    }
                    if (ingredientsList[ingredientsList.Count - 1] == "spiderTwo")
                    {
                        buttonsToAddLeftover.SetActive(true);
                        aboveCauldronText.text = "<color=#006400>Medium</color> <sprite name=\"spider\">  next to last in your pot, choose small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "<sprite name=\"crowSkull\">";
                        choiceTwo.text = "<sprite name=\"toadstool\">";

                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "<color=#006400>Large</color> <sprite name=\"spider\">  next to last in your pot, choose which small ingredient to add to bag!";
                        button1.SetActive(true);
                        button2.SetActive(true);
                        choiceOne.text = "<sprite name=\"mandrake\">";
                        choiceTwo.text = "<sprite name=\"ghostsbreath\">";

                        await CheckWhichChoice(aboveCauldronText.text);
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
           
            await MessageAboveCauldron("<color=#006400>0</color> <sprite name=\"spider\"> in your pot");
            Debug.Log("no spiders");
        }
        Debug.Log("checkSpiders");
        ResetChoices();
        CheckGhostsBreath();
    }




    public async Task CheckWhichChoice(string message)
    {
        if (isCheckingChoice) return; // Prevent multiple instances from running

        isCheckingChoice = true;
        while (!IsChoiceMade())
        {
            Debug.Log("no choice chosen: " + message + " - " + gameObject.name);
            await Task.Delay(100);
        }
        Debug.Log("choice MADE");
        isCheckingChoice = false;  // Reset the flag when done

    }

    public bool IsChoiceMade()
    {
       
        return choiceOneCauldron || choiceTwoCauldron || choiceThreeCauldron || choiceFourCauldron || choiceFiveCauldron || choiceSixCauldron;
    }

    public void ResetChoices()
    {
        choiceTwoCauldron = false;
        choiceOneCauldron = false;
        choiceFourCauldron = false;
        choiceThreeCauldron = false;
        choiceFiveCauldron = false;
        choiceSixCauldron = false;
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
                    aboveCauldronText.text = "You had <color=#800080>1</color> <sprite name=\"ghostsbreath\">  in your potion and get <color=#006400>1</color> <sprite name=\"VP\" >";
                    button5.SetActive(true);
                    VictoryPoints++;
                    await CheckWhichChoice(aboveCauldronText.text); 
                }
                if (ghosts == 2)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = "You had <color=#800080>2</color> <sprite name=\"ghostsbreath\">  in your potion and get <color=#006400>1</color> <sprite name=\"VP\">  and <color=#FF0000>1</color> <sprite name=\"ruby\"> !";
                    button5.SetActive(true);
                    VictoryPoints++;
                    _playerData.Rubies.Value++;
                    ChangeRubyUI();
                    await CheckWhichChoice(aboveCauldronText.text);
                }
                if (ghosts >= 3)
                {
                    buttonsToAddLeftover.SetActive(true);
                    aboveCauldronText.text = $"You had <color=#800080>{ghosts}</color> <sprite name=\"ghostsbreath\">  in your potion and get <color=#006400>2</color> <sprite name=\"VP\">  points and <color=#800080>1</color> <sprite name=\"droplet\"> ";
                    button5.SetActive(true);
                    VictoryPoints += 2;
                    AddDroplet();
                    await CheckWhichChoice(aboveCauldronText.text);
                }
                //For 1, 2 or 3 purple chips, you receive the indicated bonus.  1 = 1 victory point 2 = victory point and ruby 3 = 2 victory points and teardrop forward one space.COSTS 9. - END OF ROUND
            }
            if (ghostsBreathRule == 2)
            {
                buttonsToAddLeftover.SetActive(true);
               
                button1.SetActive(true);
                button2.SetActive(true);
                if (ghosts == 1) {
                    aboveCauldronText.text = "<color=#800080>1</color> <sprite name=\"ghostsbreath\">  in your potion, you may permanantly trade the <sprite name=\"ghostsbreath\">  in your potion for <color=#708090>1</color> <sprite name=\"moth\"> , <color=#006400>1</color> <sprite name=\"VP\">  and <color=#FF0000>1</color> <sprite name=\"ruby\"> ";
                    choiceOne.text = "Trade 1";
                    choiceTwo.text = "skip";
                    await CheckWhichChoice(aboveCauldronText.text);
                    if (choiceOneCauldron)
                    {
                        _playerData.VictoryPoints.Value += 1;
                        _playerData.Rubies.Value += 1;
                        grabIngredient.AddToBagPermanantly(10);
                    }
                  
                }
                if (ghosts == 2) {
                    button3.SetActive(true);
                    aboveCauldronText.text = "<color=#800080>2</color> <sprite name=\"ghostsbreath\">  in your potion, you may permanantly trade <color=#800080>1</color> <sprite name=\"ghostsbreath\">  for <color=#708090>1</color> <sprite name=\"moth\"> , <color=#006400>1</color> <sprite name=\"VP\">  and <color=#FF0000>1</color> <sprite name=\"ruby\">. Trade 2 for a <color=#006400>small</color> <sprite name=\"spider\"> , a <color=#708090>medium</color> <sprite name=\"crowSkull\"> , <color=#006400>3</color> <sprite name=\"VP\">  and <color=#800080>1</color> <sprite name=\"droplet\"> ";
                    choiceOne.text = "trade 1";
                    choiceTwo.text = "trade 2";
                    choiceThree.text = "skip";

                    await CheckWhichChoice(aboveCauldronText.text);
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
                       
                        grabIngredient.AddToBagPermanantly(16);
                        grabIngredient.AddToBagPermanantly(5);
                    }
                    ResetChoices();
                }
                if (ghosts >= 3)
                {
                    button3.SetActive(true);
                    button4.SetActive(true);
                    aboveCauldronText.text = "You have <color=#800080>3 or more</color> <sprite name=\"ghostsbreath\">  in your potion, you may trade <color=#800080>1</color> <sprite name=\"ghostsbreath\">  for <color=#708090>1</color> <sprite name=\"moth\"> , <color=#00FF00>1</color> <sprite name=\"VP\">  and <color=#FF0000>1</color> <sprite name=\"ruby\"> . Trade 2 for a <color=#006400>small</color> <sprite name=\"spider\"> , a <color=#708090>medium</color> <sprite name=\"crowSkull\"> , <color=#006400>3</color> <sprite name=\"VP\">  and <color=#800080>1</color> <sprite name=\"droplet\"> . Trade 3 for a <color=#FFA500>large</color> <sprite name=\"mandrake\"> , <color=#00FF00>6</color> <sprite name=\"VP\"> , <color=#FF0000>1</color> <sprite name=\"ruby\"> and <color=#800080>2</color> <sprite name=\"droplet\"> ";
                    choiceOne.text = "Trade 1";
                    choiceTwo.text = "Trade 2";
                    choiceThree.text = "Trade 3";
                    choiceFour.text = "skip";

                    await CheckWhichChoice(aboveCauldronText.text);
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
                        
                        cauldronScoreFront.text = Score.ToString();
                        cauldronScoreBack.text = Score.ToString();
                        grabIngredient.AddToBagPermanantly(16);
                        grabIngredient.AddToBagPermanantly(5);
                    }
                    if (choiceThreeCauldron)
                    {
                        _playerData.VictoryPoints.Value += 6;
                        AddDroplet();
                        FunctionTimer.Create(() => AddDroplet(), 3f);
                        
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
                        aboveCauldronText.text = "You had <color=#800080>1</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";
                        
                        button1.SetActive(true);
                      
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>1</color>  <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                     
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>1</color>  <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                   
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>1</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a medium version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                        button4.SetActive(true);
                       
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        choiceFour.text = smallIngredients[3].Substring(0, smallIngredients[3].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>2</color>  <sprite name=\"ghostsbreath\">  in your potion, you can trade 1 medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);

                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>2</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade 1 medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);

                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        choiceTwo.text = mediumIngredients[1].Substring(0, mediumIngredients[1].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>2</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade 1 medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);

                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        choiceTwo.text = mediumIngredients[1].Substring(0, mediumIngredients[1].Length - 3);
                        choiceThree.text = mediumIngredients[2].Substring(0, mediumIngredients[2].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>2</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one medium ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                        button4.SetActive(true);
                        button5.SetActive(true);
                        choiceOne.text = mediumIngredients[0].Substring(0, mediumIngredients[0].Length - 3);
                        choiceTwo.text = mediumIngredients[1].Substring(0, mediumIngredients[1].Length - 3);
                        choiceThree.text = mediumIngredients[2].Substring(0, mediumIngredients[2].Length - 3);
                        choiceFour.text = mediumIngredients[3].Substring(0, mediumIngredients[3].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>3</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);

                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>3</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);

                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>3</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);

                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                        aboveCauldronText.text = "You had <color=#800080>3</color> <sprite name=\"ghostsbreath\">  in your potion, you can trade one small ingredient that was in your pot for a large version of it!";

                        button1.SetActive(true);
                        button2.SetActive(true);
                        button3.SetActive(true);
                        button4.SetActive(true);
                        button5.SetActive(true);
                        choiceOne.text = smallIngredients[0].Substring(0, smallIngredients[0].Length - 3);
                        choiceTwo.text = smallIngredients[1].Substring(0, smallIngredients[1].Length - 3);
                        choiceThree.text = smallIngredients[2].Substring(0, smallIngredients[2].Length - 3);
                        choiceFour.text = smallIngredients[3].Substring(0, smallIngredients[3].Length - 3);
                        await CheckWhichChoice(aboveCauldronText.text);
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
                aboveCauldronText.text = $"<sprite name=\"ghostsbreath\">  in potion adds <color=#800080>{extraPoints}</color> <sprite name=\"droplet\"> ";
                Score += extraPoints;
                await CheckWhichChoice(aboveCauldronText.text);


            }
        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            aboveCauldronText.text = "You had <color=#800080>0</color> <sprite name=\"ghostsbreath\">  in your pot";
            button5.SetActive(true);
            await CheckWhichChoice(aboveCauldronText.text);
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
            aboveCauldronText.text = "You get <color=#FF0000>1</color> <sprite name=\"ruby\"> this round!";
            _playerData.Rubies.Value += 1;
            await CheckWhichChoice(aboveCauldronText.text);
            ResetChoices();
            Debug.Log("added rubies");
            ChangeRubyUI();
        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            button5.SetActive(true);
            aboveCauldronText.text = "You get <color=#FF0000>0</color> <sprite name=\"ruby\"> this round";
            await CheckWhichChoice(aboveCauldronText.text);
            ResetChoices();
            Debug.Log("no rubies to add");
        }
        
        aboveCauldronText.text = $"You get <color=#006400>{VictoryPoints}</color> <sprite name=\"VP\">  this round!";
        button5.SetActive(true);
        _playerData.VictoryPoints.Value += VictoryPoints;
        await CheckWhichChoice(aboveCauldronText.text);
        ResetChoices();
        Debug.Log("Added victory points");


        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = $"You get <color=#FFA500>{Coins}</color> <sprite name=\"coin\">  this round!";
        _playerData.Coins.Value += Coins;
        await CheckWhichChoice(aboveCauldronText.text);
        ResetChoices();
        Debug.Log("Added coins");
        await _fortuneManager.AtTheVeryEndOfRound();

        aboveCauldronText.text = "";
        quality.ResetCherryBombs();
        quality.SetCherryBombText();
        ResetInsidePot();
       
        

        if (winnerManager.round == 9)
        {
            GameManager.Instance.UpdateGameState(GameState.DeclareWinner);
        }
        else
        {
            quality.ResetPotionColour();
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
            choiceOne.text = "Buy <sprite name=\"droplet\">";
            button2.SetActive(true);
            int rubiesToSpend = _playerData.Rubies.Value;

            if (_playerData.PurifierFull.Value == false)
            {
                buttonsToAddLeftover.SetActive(true);
                button5.SetActive(true);
                _animatorScript.StartTalking(5);
                aboveCauldronText.text = $"You have <color=#FF0000>0{rubiesToSpend}</color> <sprite name=\"ruby\">, do you want to buy <sprite name=\"droplet\"> or refill your purifier potion?";
                choiceTwo.text = "refill purifier";
                if (_playerData.Rubies.Value >= 4)
                {
                    button3.SetActive(true);
                    choiceThree.text = "Buy 2 <sprite name=\"droplet\">";
                    button4.SetActive(true);
                    choiceFour.text = "Refill and 1 <sprite name=\"droplet\">";
                }
             
                await CheckWhichChoice(aboveCauldronText.text);
                if (choiceOneCauldron)
                {
                    BuyDrop();
                   
                }
                if (choiceTwoCauldron)
                {
                    FillPurifier();
                 
                }
                if (choiceThreeCauldron)
                {
                    BuyDrop();
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                }
                if (choiceFourCauldron)
                {
                    BuyDrop();
                    FillPurifier();
                }

                ResetChoices();

            }
            else if(_playerData.Rubies.Value >= 2)
            {
                buttonsToAddLeftover.SetActive(true);
                _animatorScript.StartTalking(4);
                aboveCauldronText.text = $"You have <color=#FF0000>{rubiesToSpend}</color> <sprite name=\"ruby\">, do you want to buy a <sprite name=\"droplet\"> ?";
                choiceTwo.text = "Skip";
                if (_playerData.Rubies.Value >= 4)
                {
                    button3.SetActive(true);
                    choiceThree.text = "Buy 2 <sprite name=\"droplet\">";
                }
                if (_playerData.Rubies.Value >= 6)
                {
                    button4.SetActive(true);
                    choiceFour.text = "Buy 3 <sprite name=\"droplet\">";
                }
                if (_playerData.Rubies.Value >= 8)
                {
                    button5.SetActive(true);
                    choiceFive.text = "Buy 4 <sprite name=\"droplet\">";
                }
                if (_playerData.Rubies.Value >= 10)
                {
                    button6.SetActive(true);
                    choiceSix.text = "Buy 5 <sprite name=\"droplet\">";
                }

                await CheckWhichChoice(aboveCauldronText.text);
                if (choiceOneCauldron)
                {
                    BuyDrop();
                }
                if (choiceThreeCauldron)
                {
                    BuyDrop();
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                }
                if (choiceFourCauldron)
                {
                    BuyDrop();
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                }
                if (choiceFiveCauldron)
                {
                    BuyDrop();
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                }
                if (choiceSixCauldron)
                {
                    BuyDrop();
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                    FunctionTimer.Create(() => BuyDrop(), 1f);
                }
                ResetChoices();

            }
        }
        else
        {
            buttonsToAddLeftover.SetActive(true);
            button5.SetActive(true);
            _animatorScript.StartTalking(4);
            await MessageAboveCauldron("You don't have enough <sprite name=\"ruby\"> to spend!");
            
            ResetChoices();
        }
        Debug.Log("end spend rubies ui");
        ReadyForNextRound();
    }
    public async void ReadyForNextRound()
    {
        
        bagSphere.SetActive(true);
        grabIngredient = FindObjectOfType<GrabIngredient>();
        Debug.Log("ready for next round function");
        grabIngredient.ResetBombs();
        ResetScore();
        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();
        
        

        
        await winnerManager.CalculateRatTails();

        int ratTails = _playerData.RatTails.Value;
        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        _animatorScript.StartTalking(4);
        aboveCauldronText.text = $"You have <color=#FF1493>{ratTails} Rat Tails</color> <sprite name=\"ratTail\">  to add to your pot";
        Score += ratTails;
        

        await CheckWhichChoice(aboveCauldronText.text);
        aboveCauldronText.text = "";
        await InstantiateRatTails(ratTails);
        quality.ResetPotionColour();
        quality.ResetCherryBombLimit();
        quality.SetCherryBombText();
        grabIngredient.ResetBagContents();
        bagSphere.SetActive(false);
        resetScoreText();
        ResetChoices();
        ResetStuffInBook();
       
        winnerManager.ReadyUp();
        await winnerManager.CheckAllPlayersReady();
        winnerManager.ResetReady();
        ChangeSceneryDependingOnRound();
        ChangePotionHeight();
        bottleUp.GetComponent<bottleUpPotionTrigger>().ReserPotionBottle();
        quality.SetSmokeColor("green");
        ResetPotionHeight();
        GameManager.Instance.UpdateGameState(GameState.FortuneTeller);
    }

    private async Task InstantiateRatTails(int ratTails)
    {
        int newRatTails = ratTails;
        while (newRatTails > 0) {
            Instantiate(ratTailObject, rubyInstantiationLocation, Quaternion.identity);
            newRatTails -= 1;
            await Task.Delay(200);
        }

    } 

 
public void BuyDrop()
    {
       AddDroplet();
      
        for (int i = 0; i < 2; i++)
        {
            _playerData.Rubies.Value--;
            ChangeRubyUI();
        }
        ResetScore();
       resetScoreText();
    
       

    }

    public void FillPurifier()
    {
        for (int i = 0; i < 2; i++)
        {
            _playerData.Rubies.Value--;
            ChangeRubyUI();
        }
        _playerData.PurifierFull.Value = true;
        bottleUp.GetComponent<bottleUpPotionTrigger>().ReserPurifier();
      
        _animatorScript.StartTalking(1);
        //when have proper 3d model fill with the liquid.
    }

    public void ChangeSceneryDependingOnRound()
    {
       
        if (winnerManager.round == 2)
        {
            RenderSettings.skybox = Round3Sky;

        }
        if (winnerManager.round == 4)
        {
            RenderSettings.skybox = Round5Sky;

        }
        if (winnerManager.round == 6)
        {
            RenderSettings.skybox = Round7Sky;

        }
        if(winnerManager.round == 8)
        {
            RenderSettings.skybox= Round9Sky;
        }
    
    }

    public void ResetGame()
    {
        RenderSettings.skybox = Round1Sky;
        ChangeRubyUI();
        ResetScore();
        resetScoreText();
        ResetInsidePot();
        ResetPotionHeight();
        bottleUp.GetComponent<bottleUpPotionTrigger>().ReserPotionBottle();
        quality.ResetPotionColour();
        quality.ResetCherryBombLimit();
        quality.SetCherryBombText();

    }

    public void ChangeRubyUI()
    {
        _playerData = FindObjectOfType<PlayerData>();
        rubyNumber.text = _playerData.Rubies.Value.ToString();
        rubyInstantiationLocation = rubylocationSphere.transform.position;

        if (RubyobjNum < _playerData.Rubies.Value)
        {
            if (_playerData.Rubies.Value == 1)
            {
                rubyOne = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 2)
            {
                rubyTwo = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 3)
            {
                rubyThree = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 4)
            {
                rubyFour = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 5)
            {
                rubyFive = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 6)
            {
                rubySix = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 7)
            {
                rubySeven = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }
            if (_playerData.Rubies.Value == 8)
            {
                rubyEight = Instantiate(rubyForBowl, rubyInstantiationLocation, Quaternion.identity);
            }

        }
        else {
            if (_playerData.Rubies.Value <= 7)
            {
                Destroy(rubyEight);
            }
            if (_playerData.Rubies.Value <= 6)
            {
                Destroy(rubySeven);
            }
            if (_playerData.Rubies.Value <= 5)
            {
                Destroy(rubySix);
            }
            if (_playerData.Rubies.Value <= 4)
            {
                Destroy(rubyFive);
            }
            if (_playerData.Rubies.Value <= 3)
            {
                Destroy(rubyFour);
            }
            if (_playerData.Rubies.Value <= 2)
            {
                Destroy(rubyThree);
            }
            if (_playerData.Rubies.Value <= 1)
            {
                Destroy(rubyTwo);
            }
            if (_playerData.Rubies.Value <= 0)
            {
                Destroy(rubyOne);
            }
        }
        RubyobjNum = _playerData.Rubies.Value;
    }

    public void SetRules()
    {
        winnerManager = FindObjectOfType<WinnerManager>();
        mushroomRule = winnerManager.ToadstallRule.Value;
        crowSkullRule = winnerManager.CrowskullRule.Value;
        gardenSpiderRule = winnerManager.SpiderRule.Value;
        mandrakeRule = winnerManager.MandrakeRule.Value;
        hawkMothRule = winnerManager.MothRule.Value;
        ghostsBreathRule = winnerManager.GhostsbreathRule.Value; 
      
    }

    public void RemoveLastIngredient()
    {
        grabIngredient = FindObjectOfType<GrabIngredient>();
        if (lastIngredient == "cherryBombOne")
        { grabIngredient.AddToBagThisRound(0);
            Score -= 1;
            quality.RemoveFromCherryBombs(1);
        }
        if (lastIngredient == "cherryBombTwo")
        { grabIngredient.AddToBagThisRound(2);
           
            Score -= 2;
            quality.RemoveFromCherryBombs(2);
            
        }
        if (lastIngredient == "cherryBombThree")
        { grabIngredient.AddToBagThisRound(1);
           
            quality.RemoveFromCherryBombs(3);
            Score -= 3;
        }
        int ingredientsCount = ingredientsList.Count - 1;

        ingredientsList.RemoveAt(ingredientsCount);
        quality.SetCherryBombText();
        cauldronScoreFront.text = Score.ToString();
        cauldronScoreBack.text = Score.ToString();
    }

    public async Task MessageAboveCauldron(string message)
    {
        
        buttonsToAddLeftover.SetActive(true);
        button5.SetActive(true);
        aboveCauldronText.text = message;

        await CheckWhichChoice(message);
        ResetChoices();
        aboveCauldronText.text = "";
        


    }

    private void ResetPotionHeight()
    {
        ResetScore();
        float Added = (float)(Score * 0.010);

        float newHeight = startHeightPotion + Added;

        float threshold = 0.01f;
        float speed = 10f;




        if (Mathf.Abs(newHeight - potionOne.transform.position.y) > threshold)
        {

            float targetY = Mathf.MoveTowards(potionOne.transform.position.y, newHeight, speed * Time.deltaTime);
            potionOne.transform.position = new Vector3(
                potionOne.transform.position.x,
                targetY,
                potionOne.transform.position.z
            );
            bottleUp.transform.position = new Vector3(
               potionOne.transform.position.x,
               targetY,
               potionOne.transform.position.z
           );
        }
    }

    private void ChangePotionHeight()
    {
        float Added = (float)(Score * 0.010); 
        
        float newHeight = startHeightPotion + Added; 
       
        float threshold = 0.01f;
        float speed = 0.5f;

       

       
        if (Mathf.Abs(newHeight - potionOne.transform.position.y) > threshold)
        { 
           
            float targetY = Mathf.MoveTowards(potionOne.transform.position.y, newHeight, speed * Time.deltaTime);
            potionOne.transform.position = new Vector3(
                potionOne.transform.position.x,
                targetY,
                potionOne.transform.position.z
            );
            bottleUp.transform.position = new Vector3(
               potionOne.transform.position.x,
               targetY,
               potionOne.transform.position.z
           );
        }
    }
    public async Task MessageAboveCauldronMultipleChoice(int num, string message, string choice1, string choice2, string choice3, string choice4, string choice5)
    {
        
        buttonsToAddLeftover.SetActive(true);
        if (num == 0)
        {
            aboveCauldronText.text = message;
            button5.SetActive(true);
        }
        if (num == 1)
        {
            aboveCauldronText.text = message;
            button1.SetActive(true);
            button2.SetActive(true);
            choiceOne.text = choice1;
            choiceTwo.text = "Skip";
        }
        if (num == 2)
        {
            aboveCauldronText.text = message;
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);

            choiceOne.text = choice1;
            choiceTwo.text = choice2;
            choiceThree.text = "Skip";
        }
        if (num == 3)
        {
            aboveCauldronText.text = message;
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            choiceOne.text = choice1;
            choiceTwo.text = choice2;
            choiceThree.text = choice3;
            choiceFour.text = "Skip";
        }
        if (num == 4)
        {
            aboveCauldronText.text = message;

            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            button5.SetActive(true);
            choiceOne.text = choice1;
            choiceTwo.text = choice2;
            choiceThree.text = choice3;
            choiceFour.text = choice4;
            choiceFive.text = "Skip";
        }
        if (num == 5)
        {
            aboveCauldronText.text = message;

            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            button6.SetActive(true);
            button7.SetActive(true);
            choiceOne.text = choice1;
            choiceTwo.text = choice2;
            choiceThree.text = choice3;
            choiceFour.text = choice4;
            choiceSix.text = choice5;
            choiceSeven.text = "Skip";
            
        }


        await CheckWhichChoice(aboveCauldronText.text);
        aboveCauldronText.text = "";
        
    }

    public void InstantiateOverPot(int num)
    {
        leftOverIngredientLocation = leftOverIngredient.transform.position;
        Instantiate(ingredients[num], leftOverIngredientLocation, Quaternion.identity);
    }

    public async Task calculateEndGameExtraPoints()
    {
        await MessageAboveCauldron("END OF GAME! Time to add up extra points!");
        int pointsFromRubies = _playerData.Rubies.Value / 2;
        _animatorScript.StartTalking(5);
        await MessageAboveCauldron($"You get <sprite name=\"VP\">  for every <color=#FF0000>2</color> <sprite name=\"ruby\">, giving you <color=#006400>{pointsFromRubies}</color> <sprite name=\"VP\"> ");
        _playerData.VictoryPoints.Value += pointsFromRubies;
        int pointsFromMoney = _playerData.Coins.Value / 5;
        _animatorScript.StartTalking(5);
        await MessageAboveCauldron($"You get a <sprite name=\"VP\">  for every <color=#FFA500>5</color> <sprite name=\"coin\"> , giving you <color=#006400>{pointsFromMoney}</color> <sprite name=\"VP\"> ");
        _playerData.VictoryPoints.Value += pointsFromMoney;
    }

}




