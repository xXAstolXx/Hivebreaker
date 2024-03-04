using UnityEngine;
using UnityEngine.EventSystems;

public class GamepadInUI : MonoBehaviour
{
    EventSystem eventSystem;

    [SerializeField]
    private GameObject select;

    public void SetSelectedGameObjectInEventSystem()
    {
        eventSystem = EventSystem.current;
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(select);
        }
    }

    public void DeselectGameObjectInEventSystem()
    {
        eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null);
    }
}
