using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlueCauldronScoreText : MonoBehaviour
{
    public int _score;

    private TMP_Text _scoreText;

    private void Awake()
    {



        _scoreText = GetComponent<TMP_Text>();
        if (_scoreText == null)
        {
            Debug.LogError("TMP_Text component not found on the GameObject.");
        }
        _scoreText.text = "0";

    }

    public void UpdateCauldronScore(int score)
    {

        _score = score;

        UpdateText(_score);


    }

    public void UpdateText(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = score.ToString();

            Debug.Log($"Displaying score: {score}");
        }
        else
        {
            Debug.LogError("scoreText is null. TMP_Text component might be missing or not assigned.");
        }
    }
}
