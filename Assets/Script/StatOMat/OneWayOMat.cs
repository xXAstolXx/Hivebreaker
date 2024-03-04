using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OneWayOMat : Interactable
{
    [SerializeField]
    private string oneWayOMatDataPath;

    private List<OneWayOMatData> oneWayData = new List<OneWayOMatData>();

    private Stats tempStats;

    Dictionary<string, float> packsDictionary = new Dictionary<string, float>();

    private List<SFXPlayer> playerSFX = new List<SFXPlayer>();

    StatPack packForUI;


    private void Awake()
    {
        tempStats = new Stats(0,0,0,0,0,0,0f,0,0,0f);

        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            playerSFX.Add(audioPlayer);
        }
    }

    private void GetOneWayDataFromFolder()
    {
        foreach (var data in Resources.LoadAll(oneWayOMatDataPath)) 
        {
            oneWayData.Add((OneWayOMatData)data);
        }

    }

    protected override void Start()
    {
        base.Start();
        interactName = "STATOMAT";

        GetOneWayDataFromFolder();
        IncreaseOption();
        ClearEntrysFromDictionary(packsDictionary);
    }

    private void AddEntrysToDictionary(OneWayOMatData data)
    {
        packsDictionary.Add("Max_Hp", data.maxHP);
        packsDictionary.Add("Attack", data.meleeAttack);
        packsDictionary.Add("Fire_Power", data.rangedAttack);
        packsDictionary.Add("Defense_Melee", data.meleeDefense);
        packsDictionary.Add("Defense_Ranged", data.rangedDefense);
        packsDictionary.Add("Attack_Speed", data.meleeAttackSpeed);
        packsDictionary.Add("Fire_Rate", data.rangedAttackSpeed);
        packsDictionary.Add("Move_Speed", data.moveSpeed);
        packsDictionary.Add("Bullet_Speed", data.bulletSpeed);
    }

    private void ClearEntrysFromDictionary(Dictionary<string, float> dict)
    {
        dict.Clear();
    }

    public StatPack CreateStat(string namefromStat, float value)
    {
        return new StatPack(namefromStat, value);
    }

    private void IncreaseOption()
    {
        int floorPackToIndex = (int)Game.Instance.levelLoaderRef.GetCurrentFloorPackEntry();
        AddEntrysToDictionary(oneWayData[floorPackToIndex]);
        string key = PickRandomStat();
        StatPack addPack = CreateStat(key ,packsDictionary[key]);
        packForUI = addPack;

        tempStats.IncreaseStat(addPack.statName, addPack.statValue);
    }

    private string PickRandomStat()
    {
        int randomIndex = UnityEngine.Random.Range(0, packsDictionary.Count);

        return packsDictionary.Keys.ElementAt<string>(randomIndex);
    }

    #region UI 

    private void PickedStatToUI(StatPack pack)
    {
        ui.SetOneWayOMatDisplay(pack, this);
    }

    protected override void DisplayWindow(bool showWindow)
    {
        ui.SetOneWayWindowActive(showWindow);
        PickedStatToUI(packForUI);
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playerSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    #endregion

    public void OnStatsAccept()
    {
        ApplyStats();
    }

    public void OnStatsDeclined() 
    {
        otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(true);
        otherObject.GetComponent<Player>().ToggleStatMachineAnimation(false);
        DisplayWindow(false);
    }

    private void ApplyStats()
    {
        Player player = otherObject.GetComponent<Player>();
        player.AddStats(tempStats);
        PlaySFXByTriggerKey("SUCCESS");
        DisplayWindow(false);
        otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(true);
        player.ClearInteractItem();
        isInteractable = false;
        ShowInteractKey(false);
    }

    protected override void ShowInteractKey(bool value)
    {
        if (FindObjectOfType<Room>(false).gameState == GameState.COMBAT) return;

        base.ShowInteractKey(value);
    }

    public override void InteractWith()
    {
        if (FindObjectOfType<Room>(false).gameState == GameState.COMBAT) return;

        base.InteractWith();

        if (isInteractable == true)
        {
            otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(false);
            DisplayWindow(true);
        }
    }
}
