using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textBox;

    [SerializeField]
    private TextMeshProUGUI nameBox;

    [SerializeField]
    private Image portraitImageSlot;

    private Image[] images;

    [SerializeField]
    private Sprite[] portraits;

    private float fadeSpeed = 1.0f;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
        foreach (Image img in images) 
        {
            img.CrossFadeAlpha(0.0f, 0.0f, true);
        }
        textBox.CrossFadeAlpha(0.0f, 0.0f, true);
        nameBox.CrossFadeAlpha(0.0f, 0.0f, true);
    }

    public void WriteTextToDialogBox(string text)
    {
        if (ApplyDialogSetup(text))
        {
            int removeCount = GetDialogName(text).Length + 8;
            text = text.Substring(removeCount);
        }
        
        textBox.text = text;
    }

    private void WriteTextToNameBox(string name) 
    {
        nameBox.text = name;
    }

    private void SetImageInPortrait(string name)
    {
        switch (name)
        {
            case "Frankie":
                portraitImageSlot.sprite = portraits[0];
                break;
            case "???":
                portraitImageSlot.sprite = portraits[1];
                break;
            case "Sam":
                portraitImageSlot.sprite = portraits[2];
                break;
            case "Pippin":
                portraitImageSlot.sprite = portraits[3];
                break;
        }
    }

    private bool ApplyDialogSetup(string text)
    {
        string name = GetDialogName(text);

        if (name == "")
        { return false; }

        WriteTextToNameBox(name);
        SetImageInPortrait(name);

        return true;
    }

    private string GetDialogName(string text)
    {
        if (text.StartsWith("<setup="))
        {
            return text.Split('=', '>')[1];

        }

        return "";
    }

    public void FadeDialogBox(Fade fade)
    { 
        switch (fade) 
        {
            case Fade.IN:
                foreach (Image img in images)
                {
                    img.CrossFadeAlpha(1.0f, fadeSpeed, true);
                    textBox.CrossFadeAlpha(1.0f, fadeSpeed, true);
                    nameBox.CrossFadeAlpha(1.0f, fadeSpeed, true);
                    Game.Instance.uiHudRef.DimScreen(0.4f);
                }
                break;
            case Fade.OUT:
                foreach (Image img in images)
                {
                    img.CrossFadeAlpha(0.0f, fadeSpeed, true);
                    textBox.CrossFadeAlpha(0.0f, fadeSpeed, true);
                    nameBox.CrossFadeAlpha(0.0f, fadeSpeed, true);
                    Game.Instance.uiHudRef.DimScreen(0.0f);
                    ResetDialogBox();
                }
                break;
        }
    }


    private void ResetDialogBox()
    {
        textBox.text = "";
        nameBox.text = "";
        portraitImageSlot.sprite = portraits[1];
    }
}
