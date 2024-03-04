using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCustomCursor : MonoBehaviour
{
    [Header("Custom Cursor")]
    [SerializeField]
    private Texture2D menuCursor;
    [SerializeField]
    private Texture2D gameplayCursor;

    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    public void SetGameplayCursor()
    {
        if(gameplayCursor != null)
        {
            Cursor.SetCursor(gameplayCursor, hotSpot, cursorMode);
        }
    }

    public void SetMenuCursor()
    {
        if(menuCursor != null)
        {
            Cursor.SetCursor (menuCursor, hotSpot, cursorMode);
        }
    }

}
