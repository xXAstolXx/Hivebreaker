using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeMenu : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private float stepsForVolumeSlider = 0.05f;

    [SerializeField]
    private string sliderName;

    ApplySettings optionsMenu;

    private void Awake()
    {
        optionsMenu = FindObjectOfType<ApplySettings>();
    }

    private void OnEnable()
    {
        optionsMenu = FindObjectOfType<ApplySettings>();
        SetCurrentValueInSlider();
    }

    private void SetCurrentValueInSlider()
    {
        switch (sliderName) 
        {
            case "MASTERVOLUME":
                volumeSlider.value = optionsMenu.settingsToApply.masterVolume;
                break;
            case "MUSICVOLUME":
                volumeSlider.value = optionsMenu.settingsToApply.musicVolume;
                break;
            case "SFXVOLUME":
                volumeSlider.value = optionsMenu.settingsToApply.sfxVolume;
                break;
        }
    }

    public void OnLeftButtonPressed()
    {
        volumeSlider.value -= stepsForVolumeSlider;
        SetCurrentValueToApply();
        optionsMenu.ApplyAudioSettings(optionsMenu.settingsToApply.masterVolume, 
            optionsMenu.settingsToApply.musicVolume, optionsMenu.settingsToApply.sfxVolume);
    }

    public void OnRightButtonPressed()
    {
        volumeSlider.value += stepsForVolumeSlider;
        SetCurrentValueToApply();
        optionsMenu.ApplyAudioSettings(optionsMenu.settingsToApply.masterVolume,
            optionsMenu.settingsToApply.musicVolume, optionsMenu.settingsToApply.sfxVolume);
    }

    public void SetCurrentValueToApply()
    {
        switch (sliderName)
        {
            case "MASTERVOLUME":
                    optionsMenu.settingsToApply.masterVolume = volumeSlider.value;
                break;
            case "MUSICVOLUME":
                    optionsMenu.settingsToApply.musicVolume = volumeSlider.value;
                break;
            case "SFXVOLUME":
                    optionsMenu.settingsToApply.sfxVolume = volumeSlider.value;
                break;
        }
    }
}
