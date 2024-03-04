using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button newGameButton;
    
    private SetCustomCursor customCursor;

    [SerializeField]
    private SFXPlayer[] playersSFX;

    private void Start()
    {
        customCursor = GetComponent<SetCustomCursor>();
        customCursor.SetMenuCursor();
        if (!SaveFileManager.Instance.SaveDataExists)
        {
            continueButton.interactable = false;
        }


        if (SaveFileManager.Instance.SettingsDataExists)
        {
            ApplySettingsFromData(SaveFileManager.Instance.SettingsSaveFile);
        }
    }

    private void ApplySettingsFromData(SettingsSaveFile file)
    {
        ApplyVideoSettings(file.screenMode, file.resolutionWidth, file.resolutionHeight, file.vSyncIndex);
        ApplyAudioSettings(file.masterVolume, file.musicVolume, file.sfxVolume);
    }

    private void ApplyVideoSettings(FullScreenMode screenMode, int width, int height, int vSyncIndex)
    {
        Screen.SetResolution(width, height, screenMode);
        QualitySettings.vSyncCount = vSyncIndex;
    }

    private void ApplyAudioSettings(float masterVolume, float musicVolume, float sfxVolume)
    {
        mixer.SetFloat("MASTERVOLUME", Mathf.Log10(masterVolume) * 20.0f);
        mixer.SetFloat("MUSICVOLUME", Mathf.Log10(musicVolume) * 20.0f);
        mixer.SetFloat("SFXVOLUME", Mathf.Log10(sfxVolume) * 20.0f);
    }

    public void EnableButton()
    {
    }

    public void OnButtonContinueClicked(int sceneIndex)
    {
        PlaySFXByTriggerKey("Click");
        SaveFileManager.Instance.isContinue = true;
        GetComponent<LoadScenebyIndex>().LoadSceneByIndex(sceneIndex);
        GamePadHandler.Instance.OnContinueFromMainMenu();
        Destroy(gameObject);
    }

    public void OnButtonNewGameClicked(int sceneIndex)
    {
        PlaySFXByTriggerKey("Click");
        SaveFileManager.Instance.DeleteSaveFile("saveData");
        GamePadHandler.Instance.OnContinueFromMainMenu();
        GetComponent<LoadScenebyIndex>().LoadSceneByIndex(sceneIndex);
        Destroy(gameObject);
    }

    public void OnTutorialClicked(int sceneIndex)
    {
        PlaySFXByTriggerKey("Click");
        SaveFileManager.Instance.playingTutorial = true;
        SaveFileManager.Instance.isContinue = true;
        GetComponent<LoadScenebyIndex>().LoadSceneByIndex(sceneIndex);
        GamePadHandler.Instance.OnContinueFromMainMenu();
        Destroy(gameObject);
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }
}
