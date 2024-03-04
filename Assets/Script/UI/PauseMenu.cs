using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool gameIsPaused = false;

    public bool canBePaused = true;

    [Header("PauseMenu")]
    [SerializeField]
    private GameObject pauseMenuCanvas;

    [SerializeField]
    private FloorCounter floorCounter;

    [SerializeField]
    private CreatureCaptureItemDisplay creatureCaptureItemDisplay;

    private SetCustomCursor customCursor;
    public bool GameIsPaused 
    {
        get
        {
            return gameIsPaused;
        }
    }

    private void Awake()
    {
        customCursor = GetComponent<SetCustomCursor>();
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseFunction();
            creatureCaptureItemDisplay.ToogleActiveinPauseMenu(false);
        }

        if (FindObjectOfType<GamePadHandler>().PairedWithPlayer == null) return;

        if(FindObjectOfType<GamePadHandler>().PairedWithPlayer.startButton.wasPressedThisFrame)
        { 
            PauseFunction();
            creatureCaptureItemDisplay.ToogleActiveinPauseMenu(false);
        }
    }

    private void PauseFunction()
    {
        if (gameIsPaused == false)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void OnContinuePressed()
    {
        Resume();
        creatureCaptureItemDisplay.ToogleActiveinPauseMenu(true);
    }

    public void OnQuitBtnClicked(int index)
    {
        Resume();
        SceneManager.LoadScene(index);
    }

    private void Resume()
    {
        customCursor.SetGameplayCursor();
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        Game.Instance.IsGamePaused = gameIsPaused;
        floorCounter.FloorCounterDisplay(true);
    }

    private void Pause()
    {
        if (!canBePaused) return; 

        customCursor.SetMenuCursor();
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Game.Instance.IsGamePaused = gameIsPaused;
        floorCounter.FloorCounterDisplay(false);
    }
}
