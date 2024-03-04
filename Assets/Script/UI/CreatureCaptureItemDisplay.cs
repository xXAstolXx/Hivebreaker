using UnityEngine;
using UnityEngine.UI;

public class CreatureCaptureItemDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject item;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Sprite questionMarkBackground;

    [SerializeField]
    private Sprite normalBackground;

    private void Awake()
    {
        background.sprite = questionMarkBackground;
        item.SetActive(false);
    }

    public void ShowItem(bool value)
    {
        if (value ) 
        { 
            background.sprite = normalBackground; 
        }
        else
        {
            background.sprite = questionMarkBackground;
        }

        item.SetActive(value);
    }

    public void ToogleActiveinPauseMenu(bool value)
    {
        gameObject.SetActive(value);
    }
}
