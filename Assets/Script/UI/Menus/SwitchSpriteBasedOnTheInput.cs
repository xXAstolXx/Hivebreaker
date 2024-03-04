using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSpriteBasedOnTheInput : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite keyboardIcon;

    [SerializeField]
    private Sprite controllereIcon;

    [SerializeField]
    private bool UsingKeyboard;

    private void Update()
    {
        SwitchIcon();
    }
    private void SwitchIcon()
    {
        if(UsingKeyboard)
        {
            image.sprite = keyboardIcon;
        }
        if(!UsingKeyboard)
        {
            image.sprite = controllereIcon;
        }
    }
}
