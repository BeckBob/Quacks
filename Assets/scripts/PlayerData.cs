using UnityEngine;
using Unity.Netcode;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Threading.Tasks;
using Unity.Collections;
using static Oculus.Platform.PlatformInternal;
using System.Collections.Generic;

public class PlayerData : NetworkBehaviour
{
    public NetworkVariable<int> VictoryPoints = new NetworkVariable<int>();
    public NetworkVariable<FixedString128Bytes> Name = new NetworkVariable<FixedString128Bytes>();
    public NetworkVariable<int> RatTails = new NetworkVariable<int>();
    public NetworkVariable<int> Rubies = new NetworkVariable<int>();
    public NetworkVariable<FixedString128Bytes> Colour = new NetworkVariable<FixedString128Bytes>();
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>();
    public NetworkVariable<int> Coins = new NetworkVariable<int>();
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public NetworkVariable<bool> PurifierFull = new NetworkVariable<bool>();


    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        isReady.Value = false;
        VictoryPoints.Value = 0;
        PurifierFull.Value = true;
        Colour.Value = "Random";
       
        RatTails.Value = 0;
        GetUserName();
    }

    public void AddVictoryPoints(int victoryPoints)
    {
        VictoryPoints.Value += victoryPoints;
        Debug.Log(VictoryPoints.Value);
    }

    public void AddCoins(int coins)
    {
        Coins.Value += coins;
    }

    public void IsReadyFunction()
    {
        isReady.Value = !isReady.Value;   
    }

    private void GetUserName()
    {
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
    }

    
    private void GetLoggedInUserCallback(Message message)
    {
        if (!message.IsError)
        {
            User user = message.GetUser();
            string userId = user.OculusID;
            

            Name.Value = userId;
        }
            if (message.IsError)
            {
                Debug.Log("Cannot get user info");
                return;
            }


        }
}