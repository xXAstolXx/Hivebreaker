using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text contentText;


    Resolution[] resolutions;
    List<string> options = new List<string>();
    private int currentResolutionIndex = 0;

    ApplySettings optionsMenu;

    private void Start()
    {
        optionsMenu = FindObjectOfType<ApplySettings>();

        resolutions = Screen.resolutions;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        UpdateShownValue();
    }

    public void OnLeftButtonPressed()
    {
        currentResolutionIndex--;
        if(currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutions.Length -1;
        }
        UpdateShownValue();
        SetResolution();
    }

    public void OnRightButtonPressed()
    {
        currentResolutionIndex++;
        if(currentResolutionIndex == resolutions.Length)
        {
            currentResolutionIndex = resolutions.Length - 1;
        }
        UpdateShownValue();
        SetResolution();
    }

    private void UpdateShownValue()
    {
        contentText.text = options[currentResolutionIndex];
    }

    public void SetResolution()
    {
        optionsMenu.settingsToApply.resolutionWidth = resolutions[currentResolutionIndex].width;
        optionsMenu.settingsToApply.resolutionHeight = resolutions[currentResolutionIndex].height;
    }
}
