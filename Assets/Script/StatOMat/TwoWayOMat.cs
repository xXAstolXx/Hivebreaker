using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class TwoWayOMat : Interactable
{
    [SerializeField]
    private string twoWayOMatDataPath;

    private List<TwoWayOMatData> twoWayData = new List<TwoWayOMatData>();

    private Stats statsPack1;
    private Stats statsPack2;

    Dictionary<string, float> packsDictionary = new Dictionary<string, float>();

    private List<SFXPlayer> playerSFX = new List<SFXPlayer>();

    StatPack packToUIIncrease;
    StatPack packToUIDecrease;

    private void Awake()
    {
        statsPack1 = new Stats();
        statsPack2 = new Stats();

        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            playerSFX.Add(audioPlayer);
        }
    }

    protected override void Start()
    {
        base.Start();
        interactName = "STATOMAT";

        GetTwoWayDataFromFolder();
        IncreaseDecreaseOption();
        ClearEntrysFromDictionary(packsDictionary);
    }

    private void GetTwoWayDataFromFolder()
    {
        foreach (var data in Resources.LoadAll(twoWayOMatDataPath))
        {
            twoWayData.Add((TwoWayOMatData)data);
        }

    }

    private void AddPositiveEntrysToDictionary(TwoWayOMatData data)
    {
        packsDictionary.Add("Max_Hp", data.maxHPPlus);
        packsDictionary.Add("Attack", data.meleeAttackPlus);
        packsDictionary.Add("Fire_Power", data.rangedAttackPlus);
        packsDictionary.Add("Defense_Melee", data.meleeDefensePlus);
        packsDictionary.Add("Defense_Ranged", data.rangedDefensePlus);
        packsDictionary.Add("Attack_Speed", data.meleeAttackSpeedPlus);
        packsDictionary.Add("Fire_Rate", data.rangedAttackSpeedPlus);
        packsDictionary.Add("Move_Speed", data.moveSpeedPlus);
        packsDictionary.Add("Bullet_Speed", data.bulletSpeedPlus);
    }

    private void AddNegativeEntrysToDictionary(TwoWayOMatData data)
    {
        packsDictionary.Add("Max_Hp", data.maxHPMinus);
        packsDictionary.Add("Attack", data.meleeAttackMinus);
        packsDictionary.Add("Fire_Power", data.rangedAttackMinus);
        packsDictionary.Add("Defense_Melee", data.meleeDefenseMinus);
        packsDictionary.Add("Defense_Ranged", data.rangedDefenseMinus);
        packsDictionary.Add("Attack_Speed", data.meleeAttackSpeedMinus);
        packsDictionary.Add("Fire_Rate", data.rangedAttackSpeedMinus);
        packsDictionary.Add("Move_Speed", data.moveSpeedMinus);
        packsDictionary.Add("Bullet_Speed", data.bulletSpeedMinus);
    }

    private void ClearEntrysFromDictionary(Dictionary<string, float> dict)
    {
        dict.Clear();
    }

    public StatPack CreateStat(string namefromStat, float value)
    {
        return new StatPack(namefromStat, value);
    }

    private void IncreaseDecreaseOption()
    {
        int floorPackToIndex = (int)Game.Instance.levelLoaderRef.GetCurrentFloorPackEntry();
        AddPositiveEntrysToDictionary(twoWayData[floorPackToIndex]);
        string key = PickRandomStat();
        StatPack highRandomPack = CreateStat(key, packsDictionary[key]);
        
        statsPack1.IncreaseStat(highRandomPack.statName, highRandomPack.statValue);
        
        ClearEntrysFromDictionary(packsDictionary);

        AddPositiveEntrysToDictionary(twoWayData[floorPackToIndex]);
        packsDictionary.Remove(key);
        key = PickRandomStat();
        StatPack lowDecreaseRandomPack = CreateStat(key, packsDictionary[key]);
        packToUIIncrease = highRandomPack;
        packToUIDecrease = lowDecreaseRandomPack;

        statsPack2.IncreaseStat(lowDecreaseRandomPack.statName, lowDecreaseRandomPack.statValue);
    }


    private string PickRandomStat()
    {
        int randomIndex = Random.Range(0, packsDictionary.Count);

        return packsDictionary.Keys.ElementAt<string>(randomIndex);
    }

    #region UI 

    private void PickedStatToUI(StatPack increasePack, StatPack decreasePack)
    {
        ui.SetTwoWayOMatDisplay(increasePack, decreasePack, this); 
    }

    protected override void DisplayWindow(bool showWindow)
    {
        ui.SetTwoWayWindowActive(showWindow);
        PickedStatToUI(packToUIIncrease, packToUIDecrease);
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
        player.AddStats(statsPack1);
        player.DecreaseStats(statsPack2);
        PlaySFXByTriggerKey("SUCCESS");
        DisplayWindow(false);
        otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(true);
        player.ClearInteractItem();
        isInteractable = false;
        ShowInteractKey(false);
    }

    public override void InteractWith()
    {
        base.InteractWith();

        if (isInteractable == true)
        {
            DisplayWindow(true);
            otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(false);
        }
    }
}
