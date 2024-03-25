using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectFortune;
    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;

    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state)
    {
        _selectFortune.SetActive(state == GameState.FortuneTeller);
    }
}
