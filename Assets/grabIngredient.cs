using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;

public class grabIngredient : MonoBehaviour
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
    List<int> bagContents = new List<int> {14, 16, 1, 2, 2, 0, 0, 0};
    
    public List<GameObject> ingredients;

    [SerializeField]
    private int _cherryBombs;
    private int _cherryBombLimit;

    private PotionQuality _potionQuality;
    public bool nextIngredientTime = true;

    private void Awake()
    {
        _potionQuality = FindObjectOfType<PotionQuality>();
        

    }
    
    void Start()
    {
        ingredients = new List<GameObject>(Resources.LoadAll<GameObject>("ingredients"));
       
    }

    public void ingredientInPotion()
    {
        nextIngredientTime = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bagContents.Count > 0 && _cherryBombs <= _cherryBombLimit)
        {
            System.Random rand = new System.Random();
            int num = rand.Next(0, bagContents.Count);

            int nextIngredient = bagContents[num];


            if (other.gameObject.CompareTag("hand") && nextIngredientTime)
            {


                Instantiate(ingredients[nextIngredient], new Vector3((float)2.391, (float)5.216, (float)-3.113), Quaternion.identity);
                nextIngredientTime = false;
                Debug.Log(ingredients);
                bagContents.RemoveAt(num);
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        _cherryBombs = _potionQuality.GetCherryBombs();
        _cherryBombLimit = _potionQuality.cherryBombLimit;
        
    }
}
