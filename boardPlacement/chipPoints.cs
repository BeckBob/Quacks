using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly_CSharp.boardPlacement;
using pointSystem;

public class chipPoints : MonoBehaviour
{
    private Program programInstance;
    private void Start()
    {
        // Initialize programInstance
        programInstance = new Program();
    
}
    private int _score;
    private void OnTriggerEnter(Collider other)
    {
        Chips[] boardPlacement = programInstance.GetBoardPlacement();

        if (other.gameObject.tag.Contains("One"))
        {
            _score += 1;
            Console.WriteLine(boardPlacement[5]);

        }
        if (other.gameObject.tag.Contains("Two"))
        {
            _score += 2;
        }
        if (other.gameObject.tag.Contains("Three"))
        {
            _score += 3;
        }
        if (other.gameObject.tag.Contains("Four"))
        {
            _score += 4;
        }

    }

}



