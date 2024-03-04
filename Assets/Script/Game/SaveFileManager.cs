using System.IO;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    private static SaveFileManager instance;
    public static SaveFileManager Instance
    { get; private set; }

    private string publicOSFolder;
    private string fileExtension = ".json";

    private string saveDataFileName = "saveData";
    private string settingsDataFileName = "settings";

    private bool saveDataExists;
    public bool SaveDataExists 
    { 
        get { return saveDataExists; }
        set 
        {
            saveDataExists = value;
            if (value)
            gameSaveFile = LoadClassFromJsonFile<GameSaveFile>(saveDataFileName);
        } 
    }

    private bool settingsDataExists;
    public bool SettingsDataExists 
    {
        get { return settingsDataExists; }
        private set 
        {
            settingsDataExists = value;
            if (value)
            settingsSaveFile = LoadClassFromJsonFile<SettingsSaveFile>(settingsDataFileName);
        } 
    }

    public bool playingTutorial = false;

    public bool isContinue = false;

    private GameSaveFile gameSaveFile;
    public GameSaveFile LoadedGameSaveFile { get { return gameSaveFile; } private set { gameSaveFile = value; } }

    private SettingsSaveFile settingsSaveFile;
    public SettingsSaveFile SettingsSaveFile { get { return settingsSaveFile; } private set { settingsSaveFile = value; } }

    private void Awake()
    {
        SetSaveFileManagerAsSingleton();
        publicOSFolder = Application.persistentDataPath + Path.DirectorySeparatorChar;

        SaveDataExists = SaveFileExists(saveDataFileName);
        SettingsDataExists = SaveFileExists(settingsDataFileName);
    }

    public bool SaveClassToJson<T>(T classToFile, string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName);
        string classAsJson = JsonUtility.ToJson(classToFile, true);
        File.WriteAllText(pathToFile, classAsJson);
        return File.Exists(pathToFile);
    }

    public void LoadClassFromJsonFile<T>(T classToOverwrite, string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName);
        string classAsJson = File.ReadAllText(pathToFile);
        JsonUtility.FromJsonOverwrite(pathToFile, classToOverwrite);
    }

    public T LoadClassFromJsonFile<T>(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName);
        string classAsJson = File.ReadAllText(pathToFile);
        return JsonUtility.FromJson<T>(classAsJson);
    }

    private void SetSaveFileManagerAsSingleton()
    {
        DontDestroyOnLoad(this);
        if (SaveFileManager.Instance != null && SaveFileManager.Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private bool SaveFileExists(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName);
        return File.Exists(pathToFile);
    }

    private string GenerateFilePathByName(string fileName)
    {
        return publicOSFolder + fileName + fileExtension;
    }

    public void DeleteSaveFile(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName);
        if (SaveFileExists(fileName))
        {
            File.Delete(pathToFile);
        }
        LoadedGameSaveFile = null;
        saveDataExists = false;
    }
}
