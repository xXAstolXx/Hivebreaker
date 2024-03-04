using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VsyncMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text contentText;

    List<string> options = new List<string>();
    private int currentVsyncIndex = 0;

    ApplySettings optionsMenu;

    private void Start()
    {
        optionsMenu = FindObjectOfType<ApplySettings>();
        SetStringsInList();
        currentVsyncIndex = QualitySettings.vSyncCount;
        UpdateShownValue();
    }

    private void SetStringsInList()
    {
        options.Add("Off");
        options.Add("On");
    }

    public void OnLeftButtonPressed()
    {
        currentVsyncIndex--;
        if(currentVsyncIndex < 0 )
        {
            currentVsyncIndex = options.Count - 1;
        }
        UpdateShownValue();
        SetVsync();
    }

    public void OnRightButtonPressed()
    {
        currentVsyncIndex++;
        if(currentVsyncIndex == options.Count)
        {
            currentVsyncIndex = options.Count - 1;
        }
        UpdateShownValue();
        SetVsync();
    }

    private void UpdateShownValue()
    {
        contentText.text = options[currentVsyncIndex];
    }

    public void SetVsync()
    {
        optionsMenu.settingsToApply.vSyncIndex = currentVsyncIndex;
    }
}
