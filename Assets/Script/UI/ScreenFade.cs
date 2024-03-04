using UnityEngine;
using UnityEngine.UI;

public enum Fade
{
    IN,
    OUT
}

public class ScreenFade : MonoBehaviour
{
    private string calledBy;

    private Image blackScreen;

    private float currentTimer;

    private float fadeTime = 2.0f;

    private void Awake()
    {
        blackScreen = GetComponent<Image>();
        
    }

    public void ScreenFadeEffect(Fade fade)
    {
        switch (fade)
        {
            case Fade.IN:
                blackScreen.color = Color.black;
                blackScreen.CrossFadeAlpha(0.0f, fadeTime, true);
                break;
            case Fade.OUT:
                blackScreen.color = Color.clear;
                blackScreen.CrossFadeAlpha(1.0f, fadeTime, true);
                break;
        }
    }

    public void ScreenFadeEffect(Fade fade, string name)
    {
        calledBy = name;
        currentTimer = Time.time;

        switch (fade)
        {
            case Fade.IN:
                blackScreen.color = Color.black;
                blackScreen.CrossFadeAlpha(0.0f, fadeTime, true);
                break;
            case Fade.OUT:
                blackScreen.color = Color.clear;
                blackScreen.CrossFadeAlpha(1.0f, fadeTime, true);
                break;
        }
    }

    private void Update()
    {
        if (calledBy == null) return;

        if (Time.time >= currentTimer + fadeTime) 
        { 
            currentTimer = 0.0f;
            OnFadeComplete();
        }
    }

    public void OnFadeComplete()
    {
        if (calledBy == null) return;

        if (calledBy == "PLAYER")
        {
            Game.Instance.BackToMainMenu();
        }
        if (calledBy == "TELEPORTER")
        {
            Game.Instance.levelLoaderRef.OnLevelCompleted();
        }
        calledBy = null;
    }

    public void DimScreen(float dimValue)
    {
        Color col = new Color(0.0f,0.0f, 0.0f, dimValue);

        blackScreen.color = col;
    }
}
