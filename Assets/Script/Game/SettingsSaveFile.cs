using UnityEngine;

public class SettingsSaveFile
{
    public FullScreenMode screenMode;
    public int resolutionWidth;
    public int resolutionHeight;
    public int vSyncIndex;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;


    public SettingsSaveFile() 
    {
        this.screenMode = FullScreenMode.FullScreenWindow;
        this.resolutionWidth = 1920;
        this.resolutionHeight = 1080;
        this.vSyncIndex = 0;
        this.masterVolume = 1.0f;
        this.musicVolume = 1.0f;
        this.sfxVolume = 1.0f;
    }

    public SettingsSaveFile(FullScreenMode screenMode, Resolution resolution,
        int vSyncIndex, float masterVolume, float musicVolume, float sfxVolume)
    {
        this.screenMode = screenMode;
        this.resolutionWidth = resolution.width;
        this.resolutionHeight = resolution.height;
        this.vSyncIndex = vSyncIndex;
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
    }
}
