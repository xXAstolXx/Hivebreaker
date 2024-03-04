using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HUD : MonoBehaviour, ILevelLoadEvent
{

    [SerializeField]
    private HealthBar playerHealthbar;

    [SerializeField]
    private StatOMatWindow oneWayOMatWindow;

    [SerializeField]
    private StatOMatWindow twoWayOMatWindow;

    [SerializeField]
    private StatCanvas extendedStatCanvas;

    [SerializeField]
    private SmallStatCanvas notExtendedCanvas;

    [SerializeField]
    private CompanionPortrait companionPortrait;

    [SerializeField]
    private HealthBar companionHealthbar;

    [SerializeField]
    private PlayerPortrait playerPortrait;

    [SerializeField]
    private DialogWindow dialogWindow;

    [SerializeField]
    private ScreenFade screenFade;

    [SerializeField]
    private ResultScreen winMenu;

    [SerializeField]
    private ResultScreen deathMenu;

    [SerializeField]
    private EndRunPopUp endRunPopUp;

    [SerializeField]
    private FloorCounter floorCount;

    [SerializeField]
    private SetCustomCursor customCursor;

    [SerializeField]
    private PlayerJoinUI p1;

    [SerializeField]
    private PlayerJoinUI p2;

    private void Awake()
    {
        extendedStatCanvas = GetComponentInChildren<StatCanvas>();
        notExtendedCanvas = GetComponentInChildren<SmallStatCanvas>();

        SetOneWayWindowActive(false);
        SetTwoWayWindowActive(false);

        winMenu.ResetCanvas();
        deathMenu.ResetCanvas();

        companionHealthbar.gameObject.SetActive(false);
    }

    private void Start()
    {
        p2.gameObject.SetActive(false);
        p1.gameObject.SetActive(false);
        ShowPlayerJoinUI();
    }

    public void SetLevelNameInUI(string name)
    {
        var levelName = GetComponentInChildren<LevelName>();
        if (levelName == null) return;

        GetComponentInChildren<LevelName>().SetNameInTMP(name);
    }

    public void SetCounter(Stats stats)
    {
        notExtendedCanvas.SetSmallStatCounterValues(stats.m_maxHP,
            stats.m_meleeAttack, stats.m_meleeDefense,
            stats.m_rangedAttack, stats.m_rangedDefense);

        extendedStatCanvas.SetStatCounterValuesForFirstRow(stats.m_maxHP, stats.m_meleeAttack,
            stats.m_meleeDefense, stats.m_meleeAttackSpeed, stats.m_moveSpeed);

        extendedStatCanvas.SetStatCounterValuesForSecondRow(stats.m_rangedAttack, stats.m_rangedDefense,
            stats.m_rangedAttackSpeed, stats.m_bulletSpeed);
    }

    public void UpdateKillCounter(int kills)
    {
        notExtendedCanvas.UpdateKillCounter(kills);
        extendedStatCanvas.UpdateKillCounter(kills);
    }

    public void UpdateMaxHPCounter(int maxHp)
    {
        notExtendedCanvas.UpdateSmallMaxHP(maxHp);
        extendedStatCanvas.UpdateBigMaxHp(maxHp);
    }

    public void SetPlayerMinMaxHealthBar(int maxHealth, int minHealth)
    {
        playerHealthbar.SetMaxHealthAndMinHealth(maxHealth, minHealth);
    }

    public void SetCompanionMinMaxHealthBar(int maxHealth, int minHealth)
    {
        companionHealthbar.SetMaxHealthAndMinHealth(maxHealth, minHealth);
    }

    public void UpdateCompanionHealth(int health)
    {
        companionHealthbar.UpdateHealth(health);
    }

    public void SetCompanionHealthBarActiv(bool value)
    {
        companionHealthbar.gameObject.SetActive(value);
    }
    public void UpdatePlayerHealthBar(int health)
    {
        playerHealthbar.UpdateHealth(health);
    }

    public void ToggleExtendedStats()
    {
        if (extendedStatCanvas.GetAlphaValue() == true)
        {
            extendedStatCanvas.SetAlphaValue(false);
            notExtendedCanvas.SetAlphaValue(true);
        }
        else
        {
            extendedStatCanvas.SetAlphaValue(true);
            notExtendedCanvas.SetAlphaValue(false);

        }
    }

    public void ShowWinScreen()
    {
        winMenu.ShowResult();
    }

    public void SetOneWayOMatDisplay(StatPack pack, OneWayOMat machine)
    {
        oneWayOMatWindow.SetOneWayOMatRef(machine);
        oneWayOMatWindow.SetOneWayOMatWindowInfos(pack);
    }

    public void SetTwoWayOMatDisplay(StatPack increasePack, StatPack decreasePack, TwoWayOMat machine)
    {
        twoWayOMatWindow.SetTwoWayOMatRef(machine);
        twoWayOMatWindow.SetTwoWayOMatWindowInfos(increasePack, decreasePack);
    }

    public void SetOneWayWindowActive(bool active)
    {
        oneWayOMatWindow.gameObject.SetActive(active);
        customCursor.SetMenuCursor();
    }

    public void SetTwoWayWindowActive(bool active)
    {
        twoWayOMatWindow.gameObject.SetActive(active);
        customCursor.SetMenuCursor();
    }

    public void ShowPlayerDamagedPortrait()
    {
        playerPortrait.ShowPlayerDamagePortrait();
    }

    public void SetCompanionPortrait(EnemyType type)
    {
        companionPortrait.SetCompanionPortrait(type);
    }

    public void ShowCompanionDamagePortrait()
    {
        companionPortrait.ShowDamagePortrait();
    }

    public DialogWindow GetDialogBoxRef()
    {
        return dialogWindow;
    }

    public void ScreenFade(Fade fade, string calledBy)
    {
        screenFade.ScreenFadeEffect(fade, calledBy);
    }

    public void DimScreen(float dimValue)
    {
        screenFade.DimScreen(dimValue);
    }

    public void OnPlayerDeath()
    {
        deathMenu.ShowResult();
        customCursor.SetMenuCursor();
        floorCount.FloorCounterDisplay(false);
    }

    public void ShowEndRun()
    {
        endRunPopUp.ShowEndRunPopUp();
        floorCount.FloorCounterDisplay(false);
        customCursor.SetMenuCursor();
    }

    public void ShowPlayerJoinUI()
    {
        if (GamePadHandler.Instance.IsGamePadLeftToPair())
        {
            if (GamePadHandler.Instance.PairedWithPlayer == null)
            {
                p1.gameObject.SetActive(true);
            }
            else 
            { 
                p1.gameObject.SetActive(false); 
            }

            if (Game.Instance.captureManagerRef.PairedGamepad == null)
            {
                p2.gameObject.SetActive(true);
            }
            else
            {
                p2.gameObject.SetActive(false);
            }
        }
        else
        {
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
        }
    }

    #region ILevelLoadEvent

    public void OnLevelLoadEvent()
    {
        screenFade.ScreenFadeEffect(Fade.IN);
        floorCount.IncreaseFloorNumer();
    }

    public void OnLevelUnloadEvent()
    {
        
    }

    public void OnPlayerWasInstanced()
    {
        
    }

    #endregion ILevelLoadEvent
}
