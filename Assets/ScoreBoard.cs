
using Unity.Services.Relay;
using UnityEngine;
using Oculus.Platform;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;


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
        GameObject PlayerUI = Instantiate(scoreboardItemPrefab, container);
        PlayerUI.GetComponent<ScoreboardItem>().TrackPlayer(player);
    }

    
}


