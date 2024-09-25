using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayers : MonoBehaviour
{
    
    [SerializeField] Transform container;
    [SerializeField] GameObject LobbyItemPrefab;


    void OnEnable()
    {
        NetworkPlayer.OnPlayerSpawn += OnPlayerSpawned;
    }

    void OnDisable()
    {
        NetworkPlayer.OnPlayerDespawn -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        GameObject PlayerUI = Instantiate(LobbyItemPrefab, container);
        PlayerUI.GetComponent<LobbyItem>().TrackPlayer(player);
    }

}
