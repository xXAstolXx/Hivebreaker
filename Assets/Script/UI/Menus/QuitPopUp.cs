using UnityEngine;
using UnityEngine.EventSystems;

public class QuitPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject preselectedButton;

    private void OnEnable()
    {
        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null) return;
        if (!GamePadHandler.Instance.IsAnyGamepadConnected) return;

        eventSystem.SetSelectedGameObject(preselectedButton);
    }
}
