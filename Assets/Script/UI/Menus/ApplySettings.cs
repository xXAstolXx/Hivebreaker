using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ApplySettings : MonoBehaviour
{
    [SerializeField]
    private KeyCode ApplyBtn;

    [SerializeField]
    private AudioMixer mixer;

    public SettingsSaveFile settingsToApply = new SettingsSaveFile();

    [SerializeField]
    private GameObject preselectedButton;

    [SerializeField]
    private SFXPlayer[] playersSFX;

    private void Awake()
    {
        if (SaveFileManager.Instance.SettingsDataExists)
        {
            LoadSettings();
        }
    }

    private void OnEnable()
    {
        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null) return;
        if (!GamePadHandler.Instance.IsAnyGamepadConnected) return;

        eventSystem.SetSelectedGameObject(preselectedButton);
    }

    private SettingsSaveFile GetCurrentSettings()
    {
        FullScreenMode mode = Screen.fullScreenMode;
        Resolution resolution = Screen.currentResolution;
        int vSyncIndex = QualitySettings.vSyncCount;

        mixer.GetFloat("MASTERVOLUME", out float master);
        mixer.GetFloat("MUSICVOLUME", out float music);
        mixer.GetFloat("SFXVOLUME", out float sfx);

        return new SettingsSaveFile(mode, resolution, vSyncIndex, master, music, sfx);
    }

    private void Update()
    {
        if(Input.GetKeyUp(ApplyBtn))
        {
            OnApplyButton();
        }
    }

    public void OnApplyButton()
    {
        PlaySFXByTriggerKey("CHANGE_CLICK");

        ApplyVideoSettings(settingsToApply.screenMode, settingsToApply.resolutionWidth,
            settingsToApply.resolutionHeight, settingsToApply.vSyncIndex);

        SaveFileManager.Instance.SaveClassToJson<SettingsSaveFile>(settingsToApply, "settings");
    }

    private void ApplyVideoSettings(FullScreenMode screenMode, int width, int height, int vSyncIndex)
    {
        Screen.SetResolution(width, height, screenMode);
        QualitySettings.vSyncCount = vSyncIndex;
    }

    public void ApplyAudioSettings(float masterVolume, float musicVolume, float sfxVolume)
    {
        mixer.SetFloat("MASTERVOLUME", Mathf.Log10( masterVolume) * 20.0f);
        mixer.SetFloat("MUSICVOLUME", Mathf.Log10(musicVolume) * 20.0f);
        mixer.SetFloat("SFXVOLUME", Mathf.Log10(sfxVolume) * 20.0f);
        SaveFileManager.Instance.SaveClassToJson<SettingsSaveFile>(settingsToApply, "settings");
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    private void LoadSettings()
    {
        settingsToApply.resolutionWidth = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").resolutionWidth;
        settingsToApply.resolutionHeight = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").resolutionHeight;
        settingsToApply.vSyncIndex = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").vSyncIndex;
        settingsToApply.screenMode = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").screenMode;

        settingsToApply.masterVolume = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").masterVolume;
        settingsToApply.musicVolume = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").musicVolume;
        settingsToApply.sfxVolume = SaveFileManager.Instance.LoadClassFromJsonFile<SettingsSaveFile>("settings").sfxVolume;
    }
}
