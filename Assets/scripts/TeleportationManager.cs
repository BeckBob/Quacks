using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] GameObject bluePlayerSpace;
    [SerializeField] GameObject yellowPlayerSpace;
    [SerializeField] GameObject redPlayerSpace;
    [SerializeField] GameObject purplePlayerSpace;

    [SerializeField] GameObject bluetoFace;
    [SerializeField] GameObject yellowtoFace;
    [SerializeField] GameObject redtoFace;
    [SerializeField] GameObject purpletoFace;

    [SerializeField] GameObject shopDirection;
    [SerializeField] GameObject stallSphere;

    PlayerData _playerData;
    LobbySettings _settings;

    private Vector3 shopDirectionLocation;
    private Vector3 stallLocation;

    private void Start()
    {
        // Cache direction and stall location
        shopDirectionLocation = shopDirection.transform.position;
        stallLocation = stallSphere.transform.position;
    }

    public void StartGameTeleportation()
    {
        _playerData = FindObjectOfType<PlayerData>();
        _settings = FindObjectOfType<LobbySettings>();

        // Set player color randomly if needed
        var color = _playerData.Colour.Value == "Random" ? _settings.GetRandomColour() : _playerData.Colour.Value;
        _playerData.Colour.Value = color;

        // Teleport and face direction based on player color
        TeleportAndRotatePlayer(color.ToString());
    }

    private void TeleportAndRotatePlayer(string color)
    {
        GameObject targetSpace = null;
        Vector3 faceDirection = Vector3.zero;

        switch (color)
        {
            case "Red":
                targetSpace = redPlayerSpace;
                faceDirection = redtoFace.transform.position;
                break;
            case "Blue":
                targetSpace = bluePlayerSpace;
                faceDirection = bluetoFace.transform.position;
                break;
            case "Yellow":
                targetSpace = yellowPlayerSpace;
                faceDirection = yellowtoFace.transform.position;
                break;
            case "Purple":
                targetSpace = purplePlayerSpace;
                faceDirection = purpletoFace.transform.position;
                break;
        }

        if (targetSpace != null)
        {
            gameObject.transform.position = targetSpace.transform.position;
            RotateTowards(faceDirection);
        }
    }

    public void ShopTeleportation()
    {
        // Teleport to the stall and face shop direction
        gameObject.transform.position = stallLocation;
        RotateTowards(shopDirectionLocation);
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        // Calculate direction to face, ignoring y-axis for a flat rotation
        Vector3 direction = (targetPosition + transform.position).normalized;
        direction.y = 0;

        // Rotate immediately to face the direction
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
