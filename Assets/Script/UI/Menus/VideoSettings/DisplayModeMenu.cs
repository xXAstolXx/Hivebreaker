using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayModeMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text contentText;

    List<string> options = new List<string>();
    private int currentScreenModeIndex = 0;

    ApplySettings optionsMenu;

    private void Start()
    {
        optionsMenu = FindObjectOfType<ApplySettings>();
        SetStringsInList();
        SetUIToCurrent();
        UpdateShownValue();
    }

    private void SetUIToCurrent()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                currentScreenModeIndex = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                currentScreenModeIndex = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                currentScreenModeIndex = 2;
                break;
            case FullScreenMode.Windowed:
                currentScreenModeIndex = 2;
                break;
        }
    }

    public void OnLeftButtonPressed()
    {
        currentScreenModeIndex--;
        if (currentScreenModeIndex < 0)
        {
            currentScreenModeIndex = options.Count - 1;
        }
        UpdateShownValue();
        SetDisplayMode();

    }

    public void OnRightButtonPressed()
    {
        currentScreenModeIndex++;
        if(currentScreenModeIndex == options.Count)
        {
            currentScreenModeIndex = 0;
        }
        UpdateShownValue();
        SetDisplayMode();
    }

    private  void SetStringsInList()
    {
        options.Add("Fullscreen"); //ExclusiveFullscreen
        options.Add("Borderless"); //FullScreenWindow
        options.Add("Window");   //Windowed;

    }
    private void UpdateShownValue()
    {
        contentText.text = options[currentScreenModeIndex];
    }

    public void SetDisplayMode()
    {
        if(currentScreenModeIndex == 0)
        {
            optionsMenu.settingsToApply.screenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if(currentScreenModeIndex == 1)
        {
            optionsMenu.settingsToApply.screenMode = FullScreenMode.FullScreenWindow;
        }
        if (currentScreenModeIndex == 2)
        {
            optionsMenu.settingsToApply.screenMode = FullScreenMode.Windowed;
        }
    }
}
