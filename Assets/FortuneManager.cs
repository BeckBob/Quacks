using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneManager : MonoBehaviour
{

    private fortuneTeller _fortuneTeller;
    private PotionQuality _quality;
    private onClickFortune _onClickFortune;
    private GrabIngredient _grabIngredient;

    // Start is called before the first frame update

    private void Start()
    {
        
        _quality = FindObjectOfType<PotionQuality>();
        
        _grabIngredient = FindAnyObjectByType<GrabIngredient>();
    }
    public void PreRoundFortuneEffects()
    {
        _onClickFortune = FindObjectOfType<onClickFortune>();
        _fortuneTeller = FindObjectOfType<fortuneTeller>();
        
        if (_fortuneTeller.fortuneNum == 0)
        {
            if(_onClickFortune.buttonOne == true)
            {
                //pick ingredient worth 4 points
            }
            if(_onClickFortune.buttonTwo == true)
            {
                //add same number of victory points as rat tails and then remove rat tail
            }

            
        }
        if (_fortuneTeller.fortuneNum == 4)
        {
            if(_onClickFortune.buttonOne == true)
            {
                //nothing changes
            }
            if(_onClickFortune.buttonTwo== true)
            {
                //trade rat tails for rubies
            }
        }
        if (_fortuneTeller.fortuneNum == 5)
        {
            if( _onClickFortune.buttonThree == true)
            {
                _quality.AddToCherryBombLimit();
                _quality.AddToCherryBombLimit();
            }
            //PRE-ROUND add 2 to cherrybomb limit END OF ROUND Minus two.
            
        }
        if (_fortuneTeller.fortuneNum == 6)
        {
            if (_onClickFortune.buttonThree == true)
            {
                //Double the number of rat tails this round
            }
            
        }
         if(_fortuneTeller.fortuneNum==7)
        {

            if (_onClickFortune.buttonOne == true)
            {
                //add 4 victory points to players score
            }
            if (_onClickFortune.buttonTwo == true)
            {
                Debug.Log("hello");
                _grabIngredient.RemoveItemFromBag(0);
                //remove white chip from players bag
            }
            
        }
        if (_fortuneTeller.fortuneNum == 9)
        {
           if( _onClickFortune.buttonThree == true)
            {
                //the player with the fewest victory points recieves one small spider
            }
            //PRE ROUND - the player with the fewest victory points recieves one small spider
        }
        //Change game state
    }

    public void DuringRoundFortuneEffects()
    {

    }


    public void PostRoundFortuneEffects()
    {
        if (_fortuneTeller.fortuneNum == 5)
        {
            _quality.RemoveFromCherryBombLimit();
            _quality.RemoveFromCherryBombLimit();
        }
    }
    // Update is called once per frame
   
}
