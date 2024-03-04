using UnityEngine;

public class EndRunPopUp : MonoBehaviour
{

    [SerializeField]
    private FloorCounter floorCounterDisplay;

    public void ShowEndRunPopUp()
    {
        PauseGame();
        gameObject.SetActive(true);
    }

    public void OnEscapeClicked()
    {
        gameObject.SetActive(false);
        Game.Instance.uiHudRef.ShowWinScreen();
    }

    public void OnGoOnClicked()
    {
        UnPauseGame();
        gameObject.SetActive(false);
        FindObjectOfType<Teleporter>().ExitLevel();
        floorCounterDisplay.FloorCounterDisplay(true);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1f;
    }
}
