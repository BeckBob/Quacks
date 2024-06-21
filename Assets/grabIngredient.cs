using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using Oculus.VoiceSDK.UX;

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
    List<int> bagContents = new() { 14, 16, 1, 2, 2, 0, 0, 0};
    List<int> resetBagContents = new() { 14, 16, 1, 2, 2, 0, 0, 0 };

    public List<GameObject> ingredients;

    [SerializeField]
    private int _cherryBombs;
    private int _cherryBombLimit;

    [SerializeField] TextMeshProUGUI pumpkinAmount;
    [SerializeField] TextMeshProUGUI toadstallAmount;
    [SerializeField] TextMeshProUGUI crowSkullAmount;
    [SerializeField] TextMeshProUGUI mothAmount;
    [SerializeField] TextMeshProUGUI spiderAmount;
    [SerializeField] TextMeshProUGUI mandrakeAmount;
    [SerializeField] TextMeshProUGUI ghostsAmount;
    [SerializeField] TextMeshProUGUI cherryBombAmount;

    [SerializeField] GameObject insideBag;

    Vector3 InsideBagLocation;

    private PotionQuality _potionQuality;

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

    public int RandomlyDrawSeveralIngredients()
    {
       
            System.Random rand = new();
            int num = rand.Next(0, bagContents.Count);
            int nextIngredient = bagContents[num];
        return nextIngredient;
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (bagContents.Count > 0 && _cherryBombs <= _cherryBombLimit)
            {
          
            if (other.gameObject.CompareTag("hand") && _potionQuality.nextIngredientTime)
                {

                System.Random rand = new();
                    int num = rand.Next(0, bagContents.Count);
                    int nextIngredient = bagContents[num];
                  InsideBagLocation = insideBag.transform.position;

                Instantiate(ingredients[nextIngredient], InsideBagLocation, Quaternion.identity);
                // new Vector3((float)-1.379, (float)4.927, (float)-5.763)
                bagContents.RemoveAt(num);
                _potionQuality.FalseNextIngredientMethod();
                CountIngredientsInBag();   
                }
            }
        }

    public void ResetBagContents()
    {
        bagContents = resetBagContents;
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
            {ghost++; }
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
    // Update is called once per frame
    public void Update()
    {
        _cherryBombs = _potionQuality.GetCherryBombs();
        _cherryBombLimit = _potionQuality.cherryBombLimit;
        
    }

  
}
