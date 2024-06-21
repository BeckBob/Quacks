using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveCauldronButtons : MonoBehaviour
{
    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    [SerializeField] GameObject button3;
    [SerializeField] GameObject button4;
    [SerializeField] GameObject button5;
   

    private ChipPoints _chipPoints;


    // Start is called before the first frame update
    void Start()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();

    }



    public void ClickButton1()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.choiceOneCauldron = true;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
       
    }

    public void ClickButton2()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.choiceTwoCauldron = true;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
            
       
    }

    public void ClickButton3()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.choiceThreeCauldron = true;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        
    }

    public void ClickButton4()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.choiceFourCauldron = true;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
       
    }
    public void ClickButton5()
    {
        _chipPoints = FindObjectOfType<ChipPoints>();
        _chipPoints.choiceFiveCauldron = true;
        button5.SetActive(false);
     

    }

}
