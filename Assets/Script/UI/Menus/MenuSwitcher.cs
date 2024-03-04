using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject videoContent;

    [SerializeField]
    private GameObject audioContent;

    [SerializeField]
    private GameObject controlsContent;


    public void SwitchContent(string contentName)
    {
        switch (contentName)
        {
            case "VideoContent":
                DisableContentMenus();
                videoContent.SetActive(true);
                break;
            case "AudioContent":
                DisableContentMenus();
                audioContent.SetActive(true);
                break;
            case "ControlsContent":
                DisableContentMenus();
                controlsContent.SetActive(true);
                break;
            case "CreditsContent":
                Application.OpenURL("https://s4g.itch.io/hivebreaker");
                break;

        }


    }

    private void DisableContentMenus()
    {
        videoContent.SetActive(false);
        audioContent.SetActive(false);
        controlsContent.SetActive(false);
    }
}
