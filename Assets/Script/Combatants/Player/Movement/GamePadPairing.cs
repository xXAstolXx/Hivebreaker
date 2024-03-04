using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadPairing : MonoBehaviour
{
    protected Gamepad pairedGamepad;
    public Gamepad PairedGamepad
    { 
        get { return pairedGamepad; } 
        set 
        {
            pairedGamepad = value; 
            OnGamepadPaired();
        }
    }

    protected float stickDeadZone = 0.1f;

    protected Vector3 directionVector;

    private void Awake()
    {
        directionVector = new Vector3();
    }

    public void PairWithGamepad(Gamepad pad)
    {
        pairedGamepad = pad;
    }

    protected virtual void OnGamepadPaired()
    {

    }

}
