using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResultScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text floorStatTxt;

    [SerializeField]
    private TMP_Text killStatTxt;

    [SerializeField]
    private TMP_Text timeStatTxt;

    [SerializeField]
    private TMP_Text highscoreTxt;

    [SerializeField]
    private GameObject preselectedButton;

    private void OnEnable()
    {
        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null) return;
        if (!GamePadHandler.Instance.IsAnyGamepadConnected) return;

        eventSystem.SetSelectedGameObject(preselectedButton);
    }

    public void ShowResult()
    {
        gameObject.SetActive(true);
        SetStatsInScreen();
        PauseGame();
    }

    public void ResetCanvas()
    {
        gameObject.SetActive(false);
    }
    private void SetStatsInScreen()
    {
        floorStatTxt.text = Game.Instance.highscoreSystemRef.ClearedFloors.ToString();
        timeStatTxt.text = Game.Instance.highscoreSystemRef.CalculateRunTime().ToString(@"mm\:ss\:ff");
        killStatTxt.text = Game.Instance.highscoreSystemRef.KillCounter.ToString();
        if (highscoreTxt != null) 
        { 
            highscoreTxt.text = Game.Instance.highscoreSystemRef.CalculateHighScore().ToString();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1.0f;
    }

    public void OnMainMenuClicked(int index)
    {
        UnPauseGame();
        SceneManager.LoadScene(index);
    }

    public void OnNewRunClicked(int index)
    {
        UnPauseGame();
        SceneManager.LoadScene(index);
    }
}
