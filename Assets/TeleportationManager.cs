using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{

    [SerializeField] GameObject bluePlayerSpace;
    [SerializeField] GameObject yellowPlayerSpace;
    [SerializeField] GameObject redPlayerSpace;
    [SerializeField] GameObject purplePlayerSpace;
    PlayerData _playerData;
    LobbySettings _settings;

    Vector3 blueSpaceLocation;
    Vector3 redSpaceLocation;
    Vector3 yellowSpaceLocation;
    Vector3 purpleSpaceLocation;

    private FixedString128Bytes colour;

    [SerializeField] GameObject stallSphere;

    Vector3 stallLocations;



    // Update is called once per frame
    public void StartGameTeleportation()
    {
        blueSpaceLocation = bluePlayerSpace.transform.position;
        redSpaceLocation = redPlayerSpace.transform.position;
        yellowSpaceLocation = yellowPlayerSpace.transform.position;
        purpleSpaceLocation = purplePlayerSpace.transform.position;
        _playerData = FindObjectOfType<PlayerData>();
        _settings = FindObjectOfType<LobbySettings>();
        colour = _playerData.Colour.Value;
        
        if (colour == "Random")
        {
           
            colour = _settings.GetRandomColour();
            _playerData.Colour.Value = colour;
            
        }

        if (colour == "Red")
        {
            gameObject.transform.position = redSpaceLocation;
        }
        if (colour == "Blue")
        {
            gameObject.transform.position = blueSpaceLocation;
        }
        if (colour == "Yellow")
        {
            gameObject.transform.position = yellowSpaceLocation;
        }
        if (colour == "Purple")
        {
            gameObject.transform.position = purpleSpaceLocation;
        }

    }

    public void ShopTeleportation()
    {
        stallLocations = stallSphere.transform.position;

        gameObject.transform.position = stallLocations;
    }
}
