using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    public UI_HUD uiHudRef;


    private SetCustomCursor customCursor;

    [HideInInspector]
    public RoomManager roomManagerRef;

    [SerializeField]
    public MusicPlayer musicPlayerRef;

    [SerializeField]
    public LevelLoader levelLoaderRef;

    [SerializeField]
    public CaptureManager captureManagerRef;

    [SerializeField]
    public HighscoreSystem highscoreSystemRef;

    private GameObject playerGameObjectRef;
    public GameObject PlayerGameObjectRef { get { return playerGameObjectRef; } private set { } }

    private static Game instance;

    public static Game Instance { get; private set; }

    private bool isGamePaused;
    public bool IsGamePaused 
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }
    private void Awake()
    {
        SetGameAsSingleton();
        customCursor = GetComponent<SetCustomCursor>();
    }

    private void Start()
    {
        levelLoaderRef.OnGameStart();
        customCursor.SetGameplayCursor();
    }

    private void SetGameAsSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnLevelPrefabLoaded()
    {
        foreach (ILevelLoadEvent levelLoadEvent in GetComponentsInChildren<ILevelLoadEvent>()) 
        {
            levelLoadEvent.OnLevelLoadEvent();
        }
    }

    public void OnLevelUnloaded()
    {
        foreach (ILevelLoadEvent levelLoadEvent in GetComponentsInChildren<ILevelLoadEvent>())
        {
            levelLoadEvent.OnLevelUnloadEvent();
        }
    }

    public void OnPlayerLoaded()
    {
        foreach (ILevelLoadEvent levelLoadEvent in GetComponentsInChildren<ILevelLoadEvent>())
        {
            levelLoadEvent.OnPlayerWasInstanced();
        }
        playerGameObjectRef = FindObjectOfType<Player>().gameObject;
    }


}

