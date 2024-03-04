using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatOMatWindow : MonoBehaviour
{
    [SerializeField]
    private SFXPlayer[] playersSFX;

    [SerializeField]
    private OneWayOMatDisplay oneWayOMatDisplay;

    [SerializeField]
    private TwoWayOMatDisplay twoWayOMatDisplay;

    private OneWayOMat oneWayOMat;

    private TwoWayOMat twoWayOMat;

    [SerializeField]
    private GameObject preselectedButton;

    [SerializeField]
    private SetCustomCursor customCursor;

    [SerializeField]
    private TMP_Text description;

    private void OnEnable()
    {
        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null) return;
        if (!GamePadHandler.Instance.IsAnyGamepadConnected) return;

        eventSystem.SetSelectedGameObject(preselectedButton);
    }

    public void SetOneWayOMatWindowInfos(StatPack pack)
    {
        oneWayOMatDisplay.SetStatInfos(pack);

        string explanation = "If you accept, your " + pack.statName + " will INCREASE by " + pack.statValue.ToString() + ".";
        description.text = explanation;
    }

    public void SetTwoWayOMatWindowInfos(StatPack increasePack, StatPack decreasePack)
    {
        twoWayOMatDisplay.SetStatInfos(increasePack, decreasePack);
        string explanation = "If you accept, your " + increasePack.statName + " will INCREASE by " + increasePack.statValue.ToString()
            + ", but your " + decreasePack.statName + " will DECREASE by " + decreasePack.statValue.ToString() + "!";
        description.text = explanation;
    }

    public void SetOneWayOMatRef(OneWayOMat machine)
    {
        oneWayOMat = machine;

    }

    public void SetTwoWayOMatRef(TwoWayOMat machine)
    {
        twoWayOMat = machine;
    }

    public void OnOneWayAcceptClicked()
    {
        oneWayOMat.OnStatsAccept();
        customCursor.SetGameplayCursor();

        PlaySFXByTriggerKey("Click");
    }

    public void OnOneWayDeclineClicked()
    {
        oneWayOMat.OnStatsDeclined();
        customCursor.SetGameplayCursor();

        PlaySFXByTriggerKey("Click");
    }

    public void OnTwoWayAcceptClicked()
    {
        twoWayOMat.OnStatsAccept();
        customCursor.SetGameplayCursor();

        PlaySFXByTriggerKey("Click");
    }

    public void OnTwoWayDeclineClicked()
    {
        twoWayOMat.OnStatsDeclined();
        customCursor.SetGameplayCursor();

        PlaySFXByTriggerKey("Click");
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }
}
