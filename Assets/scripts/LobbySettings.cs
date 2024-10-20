using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.Android;
using Unity.Netcode;
using System.Security;
using Unity.Collections;

public class LobbySettings : NetworkBehaviour
{

    public NetworkVariable<int> ToadstallRule = new NetworkVariable<int>();
    public NetworkVariable<int> SpiderRule = new NetworkVariable<int>();
    public NetworkVariable<int> CrowskullRule = new NetworkVariable<int>();
    public NetworkVariable<int> MandrakeRule = new NetworkVariable<int>();
    public NetworkVariable<int> MothRule = new NetworkVariable<int>();
    public NetworkVariable<int> GhostsbreathRule = new NetworkVariable<int>();
    
    public NetworkVariable<bool> yellowPicked = new NetworkVariable<bool>();
    public NetworkVariable<bool> purplePicked = new NetworkVariable<bool>();
    public NetworkVariable<bool> redPicked = new NetworkVariable<bool>();
    public NetworkVariable<bool> bluePicked = new NetworkVariable<bool>();

    public NetworkVariable<int> readyPlayers = new NetworkVariable<int>();

    [SerializeField] TextMeshProUGUI ToadstallText;
    [SerializeField] TextMeshProUGUI SpiderText;
    [SerializeField] TextMeshProUGUI CrowSkullText;
    [SerializeField] TextMeshProUGUI MothText;
    [SerializeField] TextMeshProUGUI MandrakeText;
    [SerializeField] TextMeshProUGUI GhostBreathText;

    [SerializeField] GameObject DropDown1;
    [SerializeField] GameObject DropDown2;
    [SerializeField] GameObject DropDown3;
    [SerializeField] GameObject DropDown4;
    [SerializeField] GameObject DropDown5;
    [SerializeField] GameObject DropDown6;

    WinnerManager _winnerManager;

    [SerializeField] Image readyButton;
    [SerializeField] TMP_Dropdown dropDown;
    [SerializeField] TMP_Dropdown toadstoolDropdown;
    [SerializeField] TMP_Dropdown spiderDropdown;
    [SerializeField] TMP_Dropdown crowDropdown;
    [SerializeField] TMP_Dropdown mothDropdown;
    [SerializeField] TMP_Dropdown mandrakeDropdown;
    [SerializeField] TMP_Dropdown ghostDropdown;
    private PlayerData _playerData;
    public Color trueColour;
    public Color falseColour;
    private NetworkConnect _networkConnect;
    private TeleportationManager _teleportationManager;

    [SerializeField] GameObject purpleSphere;
    [SerializeField] GameObject blueSphere;
    [SerializeField] GameObject yellowSphere;
    [SerializeField] GameObject redSphere;

    [SerializeField] GameObject _bigBook;

    private string oldColour = "Random";
    private bool _ready = false;
 

    public int numberOfPlayers;

    public List<string> AvailableColours = new List<string>();
    void Awake()
    {

       


        _networkConnect = FindObjectOfType<NetworkConnect>();
        _winnerManager = FindObjectOfType<WinnerManager>();
        _teleportationManager = FindObjectOfType<TeleportationManager>();
       
       
    
    }

    public void InitiateRules()
    {
        ToadstallRule.Value = 1;
        SpiderRule.Value = 1;
        CrowskullRule.Value = 1;
        MandrakeRule.Value = 1;
        MothRule.Value = 1;
        GhostsbreathRule.Value = 1;
        readyPlayers.Value = 0;
        yellowPicked.Value = false;
        redPicked.Value = false;
        bluePicked.Value = false;
        purplePicked.Value = false;
    }
    
    public void AddRuleDropdowns()
    {
        _networkConnect = FindObjectOfType<NetworkConnect>();
        _playerData = FindObjectOfType<PlayerData>();
        InitiateRules();
        if (_playerData.Name.Value == _networkConnect.hostname)
        {
            DropDown1.SetActive(true);
            DropDown2.SetActive(true);
            DropDown3.SetActive(true);
            DropDown4.SetActive(true);
            DropDown5.SetActive(true);
            DropDown6.SetActive(true);
        }
    }

    public void ChangeColourFunction(FixedString128Bytes NewColour)
    {
        _playerData = FindObjectOfType<PlayerData>();
        _playerData.Colour.Value = NewColour;
        if (NewColour == "Purple")
        {
            purplePicked.Value = true;
            updateOldColourBool();
            oldColour = "Purple";
        }
        if (NewColour == "Yellow")
        {
            yellowPicked.Value = true;
            updateOldColourBool();
            oldColour = "Yellow";
        }
        if (NewColour == "Red")
        {
            redPicked.Value = true;
            updateOldColourBool();
            oldColour = "Red";
        }
        if (NewColour == "Blue")
        {
            bluePicked.Value = true;
            updateOldColourBool();
            oldColour = "Blue";
        }
        if (NewColour == "Random")
        {

            updateOldColourBool();
            oldColour = "Random";
        }
    }

    public void updateOldColourBool() { 
    if (oldColour == "Purple")
        {
            purplePicked.Value = false;
        }
        if (oldColour == "Yellow")
        {
            yellowPicked.Value = false;
        }
        if (oldColour == "Red")
        {
            redPicked.Value = false;
        }
        if (oldColour == "Blue")
        {
            bluePicked.Value = false;
        }
    }
    
    private void BuildRandomColourList()
    {
        if(!purplePicked.Value) { AvailableColours.Add("Purple"); }
        if(!yellowPicked.Value) { AvailableColours.Add("Yellow"); }
        if(!redPicked.Value) { AvailableColours.Add("Red"); }
        if (!bluePicked.Value) { AvailableColours.Add("Blue"); }
    }

    public FixedString128Bytes GetRandomColour()
    {
        System.Random rand = new System.Random();
        int colourNum = rand.Next(0, AvailableColours.Count);
        FixedString128Bytes randomColour = AvailableColours[colourNum];
        return randomColour;
    }
    public void ReadyNow()
    {
        _playerData = FindObjectOfType<PlayerData>();
        _networkConnect = FindObjectOfType<NetworkConnect>();
        _playerData.IsReadyFunction();
        _ready = !_ready;
        numberOfPlayers = _networkConnect.players;
      
        if (_ready)
        {
           readyButton.color = trueColour;
            readyPlayers.Value++;
            
            if (readyPlayers.Value == numberOfPlayers)
            {
                SetRules();
                BuildRandomColourList();
                
                if (_playerData.Colour.Value == "Random")
                {
                    FixedString128Bytes colour;
                    colour = GetRandomColour();
                    _playerData.Colour.Value = colour.Value;
                }
                
                _teleportationManager.StartGameTeleportation();
                GameManager.Instance.UpdateGameState(GameState.FortuneTeller);
                _bigBook.SetActive(false);
                ResetReady();
            }
        }
        if (!_ready)
        {
            readyButton.color = falseColour;
            readyPlayers.Value--;
            if (readyPlayers.Value == numberOfPlayers)
            {
                SetRules();
                BuildRandomColourList();
               
                if (_playerData.Colour.Value == "Random")
                {
                    FixedString128Bytes colour;
                    colour = GetRandomColour();
                    _playerData.Colour.Value = colour.Value;
                }
               
                _teleportationManager.StartGameTeleportation();
                GameManager.Instance.UpdateGameState(GameState.FortuneTeller);
                _bigBook.SetActive(false);
                ResetReady();
            }
        }
       
    }

    public void ResetReady()
    {
        _ready = false;
        readyPlayers.Value = 0;
    }

    public void ChangeColour()
    {
        _playerData = FindObjectOfType<PlayerData>();
       
        
        int pickedEntryIndex = dropDown.value;

        string colour = dropDown.options[pickedEntryIndex].text;
        //dropDown.enabled = false;
        ChangeColourFunction(colour);

      
    }

    public void ChangeToadstallRule()
    {
        int pickedEntryIndex = toadstoolDropdown.value;
        Debug.Log(pickedEntryIndex);
        
        if (pickedEntryIndex == 0)
        {
            ToadstallText.text = "1. If the previously placed chip was white, add its value to the red chip and move the red chip that many spaces.";
            ToadstallRule.Value = 1;
        }
        if (pickedEntryIndex == 1)
        {
            ToadstallText.text = "2. As soon as 1 or more Toadstool are in the pot, each following cherryBomb is moved two spaces. ";
            ToadstallRule.Value = 2;
        }
        if (pickedEntryIndex == 2)
        {
            ToadstallText.text = "3. If there are 1/2 pumpkins in your pot your toasdstool adds 1 extra point. If there are 3+ pumpkins add 2 extra points. ";
            ToadstallRule.Value = 3;
        }
        if (pickedEntryIndex == 3)
        {
            ToadstallText.text = "4. Put the toadstall to the side, after you have stopped drawing, choose to either use it this turn OR save it for the end of a future turn.";
            ToadstallRule.Value = 4;
        }
    }

    public void ChangeSpiderRule()
    {
        int pickedEntryIndex = spiderDropdown.value;
        Debug.Log(pickedEntryIndex);
        if (pickedEntryIndex == 0)
        {
            SpiderText.text = "1. For each spider that is the LAST or NEXT-TO-LAST chip in your pot, you may pay 1 ruby to add one droplet to the pot.";
            SpiderRule.Value = 1;
        }
        if (pickedEntryIndex == 1)
        {
            SpiderText.text = "2. If your Cherrybombs add up to exactly 7, when the round ends choose to add the collective value of all spiders in your pot.";
            SpiderRule.Value = 2;
        }
        if (pickedEntryIndex == 2)
        {
            SpiderText.text = "3. For each spider that is the LAST or NEXT-TO-LAST ingredient in your pot, you receive one ruby.";
            SpiderRule.Value = 3;
        }
        if (pickedEntryIndex == 3)
        {
            SpiderText.text = "4. For each spider that is the LAST or NEXT-TO-LAST In your post take one of the indicated ingredients. 1 = Pumpkin. 2 = Crowskull or Toadstool. 3 = Mandrake or GhostsBreath.";
            SpiderRule.Value = 4;
        }
    }

    public void ChangeCrowskullRule()
    {
        int pickedEntryIndex = crowDropdown.value;
        Debug.Log(pickedEntryIndex);
        if (pickedEntryIndex == 0)
        {
            CrowSkullText.text = "1. If crowskull is put in the pot on a score you would get a ruby, you immediately receive one ruby.";
            CrowskullRule.Value = 1;
        }
        if (pickedEntryIndex == 1)
        {
            CrowSkullText.text = "2. If crowskull is put in the pot on a score you would get a ruby, you IMMEDIATLEY receive 1/2/4 victory points.";
            CrowskullRule.Value = 2;
        }
        if (pickedEntryIndex == 2)
        {
            CrowSkullText.text = "3. If the pot explodes within the next 1/2/4 ingredients, you get victory points AND money during the evaluation phase (but no victory die roll).";
            CrowskullRule.Value = 3;
        }
        if (pickedEntryIndex == 3)
        {
            CrowSkullText.text = "4. Draw ingredients from your bag. You MAY place 1 of them in your pot. Draw 1 ingredient for small, 2 ingredients for a medium, 4 for a large.";
            CrowskullRule.Value = 4;
        }
    }

    public void ChangeMothRule()
    {
        int pickedEntryIndex = mothDropdown.value;
        Debug.Log(pickedEntryIndex);
        if (pickedEntryIndex == 0)
        {
            MothText.text = "1. If you draw more moths than one of the players sitting next to you get a droplet– more than both players next to you is droplet and ruby.";
            MothRule.Value = 1;
        }
        if (pickedEntryIndex == 1)
        {
            MothText.text = "2. If you draw as many moths as the other players you get a droplet, more than them you get a droplet and a ruby.";
            MothRule.Value = 2;
        }
 
    }

    public void ChangeMandrakeRule()
    {
        int pickedEntryIndex = mandrakeDropdown.value;
        Debug.Log(pickedEntryIndex);
        if (pickedEntryIndex == 0)
        {
            MandrakeText.text = "1. Doubles the points of the next ingredient in the pot.";
            MandrakeRule.Value = 1;
        }
        if (pickedEntryIndex == 1)
        {
            MandrakeText.text = "2. If the previously placed ingredient was a cherrybomb, put the white cherrybomb into the bag.";
            MandrakeRule.Value = 2;
        }
        if (pickedEntryIndex == 2)
        {
            MandrakeText.text = "3. Total value of cherrybombs needed to blow up your pot increases, according to the number of mandrakes in the pot. 1 mandrake = 8. 3 mandrakes = 9";
            MandrakeRule.Value = 3;
        }
        if (pickedEntryIndex == 3)
        {
            MandrakeText.text = "4. Your first mandrake adds 1 extra point, the 2nd mandrake 2 extra points and the 3rd mandrake adds 3 extra points.";
            MandrakeRule.Value = 4;
        }
    }

    public void ChangeGhostsbreathRule()
    {
        int pickedEntryIndex = ghostDropdown.value;
        if (pickedEntryIndex == 0)
        {
            GhostBreathText.text = "1. For 1, 2 or 3 ghosts breath, you receive the indicated bonus.  1 = 1 victory point. 2 = victory point and ruby. 3 = 2 victory points and teardrop forward one space.";
            GhostsbreathRule.Value = 1;
            
        }
        if (pickedEntryIndex == 1)
        {
            GhostBreathText.text = "2. You may exchange the GhostsBreath in your pot for the indicated bonus 1 = Moth, victory point and ruby.";
            GhostBreathText.fontSize = 13;
            GhostsbreathRule.Value = 2;
        }
        if (pickedEntryIndex == 2)
        {
            GhostBreathText.text = "3. Trade 1 ingredient from the pot for a larger version of the same ingredient. 1 = small to medium 2= medium to large 3= small to large ";
            GhostsbreathRule.Value = 3;
        }
        if (pickedEntryIndex == 3)
        {
            GhostBreathText.text = "4. For each ghostsbreath, depending on where it is in the pot you receive the following victory points. Less that 9 droplets = 0, more than 10 = 1, more than 20 = 2, more than 30 = 3.";
            GhostsbreathRule.Value = 4;
        }
    }

    public void SetRules()
    {
      
        _winnerManager.ToadstallRule.Value = ToadstallRule.Value;
        _winnerManager.CrowskullRule.Value = CrowskullRule.Value;
        _winnerManager.SpiderRule.Value = SpiderRule.Value;
        _winnerManager.MandrakeRule.Value = MandrakeRule.Value;
        _winnerManager.MothRule.Value = MothRule.Value;
        _winnerManager.GhostsbreathRule.Value = GhostsbreathRule.Value;
    }
    // Update is called once per frame

}
