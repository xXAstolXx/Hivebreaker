using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadHandler : MonoBehaviour
{
    private static GamePadHandler instance;
    public static GamePadHandler Instance
    { get; private set; }

    private List<Gamepad> gamepads;

    private PlayerGamepadBindings playerGamepadBindings;
    private KeyboardBindings keyboardBindings;

    private Gamepad pairedWithPlayer;
    public Gamepad PairedWithPlayer
    { get; private set; }

    GamepadInUI gamepadInUI;

    private bool isAnyGamepadConnected = false;
    public bool IsAnyGamepadConnected
    { get { return isAnyGamepadConnected; } private set { } }

    private void Awake()
    {
        SetGamePadHandlerAsSingleton();
        Setup();
    }

    private void Setup()
    {
        gamepads = Gamepad.all.ToList<Gamepad>();

        if (gamepads.Count > 0 )
        {
            isAnyGamepadConnected = true;

            if (gamepadInUI != null)
            {
                gamepadInUI.SetSelectedGameObjectInEventSystem();
            }
        }

        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    public void OnContinueFromMainMenu()
    {
        if (isAnyGamepadConnected)
        {
            if (gamepads[0] == null) return;
            PairedWithPlayer = gamepads[0];
        }
    }

    private void SetGamePadHandlerAsSingleton()
    {
        DontDestroyOnLoad(this);
        if (GamePadHandler.Instance != null && GamePadHandler.Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        gamepadInUI = FindObjectOfType<GamepadInUI>();

        Gamepad thisGamepad;
        if (device is Gamepad)
        {
            thisGamepad = device as Gamepad;

            switch (change)
            {
                case InputDeviceChange.Added:
                    gamepads.Add(thisGamepad);
                    gamepadInUI.SetSelectedGameObjectInEventSystem();
                    break;
                case InputDeviceChange.Disconnected:
                    DisconnectGamepad(thisGamepad);
                    break;
                case InputDeviceChange.Reconnected:
                    gamepads.Add(thisGamepad);
                    gamepadInUI.SetSelectedGameObjectInEventSystem();
                    break;
                case InputDeviceChange.Removed:
                    DisconnectGamepad(thisGamepad);
                    break;
                default:
                    break;
            }

            if (Game.Instance == null) return;
            Game.Instance.uiHudRef.ShowPlayerJoinUI();
        }
    }

    private void Update()
    {
        foreach (var gamepad in gamepads) 
        {
            if (gamepad.selectButton.wasPressedThisFrame)
            {
                playerGamepadBindings = FindObjectOfType<PlayerGamepadBindings>();
                keyboardBindings = FindObjectOfType<KeyboardBindings>();

                if (playerGamepadBindings == null) return;
                if (keyboardBindings == null) return;

                PairGamepadWithPlayer(gamepad);
            }

            if (gamepad.startButton.wasPressedThisFrame)
            {
                SetGamepadInCaptureManager(gamepad);
            }

            if (gamepad.leftStick.up.wasPressedThisFrame || gamepad.dpad.up.wasPressedThisFrame)
            {
                GamepadInUISelect();
            }

            if (gamepad.leftStick.down.wasPressedThisFrame || gamepad.dpad.down.wasPressedThisFrame)
            {
                GamepadInUISelect();
            }
        }
    }

    private void GamepadInUISelect()
    {
        gamepadInUI = FindObjectOfType<GamepadInUI>();
        if (gamepadInUI == null) return;
        gamepadInUI.SetSelectedGameObjectInEventSystem();
    }

    private void DisconnectGamepad(Gamepad pad)
    {
        gamepads.Remove(pad);
        UnpairGamepadFromPlayer(pad);
        UnpairGamepadFromCaptureManager(pad);
        gamepadInUI.DeselectGameObjectInEventSystem();
        if (gamepads.Count == 0)
        {
            isAnyGamepadConnected = false;
        }
        if (Game.Instance == null) return;
        Game.Instance.uiHudRef.ShowPlayerJoinUI();
    }

    private void PairGamepadWithPlayer(Gamepad pad)
    {
        if (IsPadAlreadyPaired(pad)) return;

        playerGamepadBindings.PairedGamepad = pad;
        PairedWithPlayer = pad;
        keyboardBindings.isKeyboardControlEnabled = false;

        if (Game.Instance == null) return;
        Game.Instance.uiHudRef.ShowPlayerJoinUI();
    }

    private void UnpairGamepadFromPlayer(Gamepad pad)
    {
        if (playerGamepadBindings.PairedGamepad == pad)
        {
            playerGamepadBindings.PairedGamepad = null;
            PairedWithPlayer = null;
            keyboardBindings.isKeyboardControlEnabled = true;

            if (Game.Instance == null) return;
            Game.Instance.uiHudRef.ShowPlayerJoinUI();
        }
    }

    private void SetGamepadInCaptureManager(Gamepad pad)
    {
        if (IsPadAlreadyPaired(pad)) return;

        if (Game.Instance.captureManagerRef == null) return;
        Game.Instance.captureManagerRef.PairedGamepad = pad;

        if (Game.Instance == null) return;
        Game.Instance.uiHudRef.ShowPlayerJoinUI();
    }

    private void UnpairGamepadFromCaptureManager(Gamepad pad)
    {
        if (Game.Instance.captureManagerRef.PairedGamepad == pad)
        {
            Game.Instance.captureManagerRef.PairedGamepad = null;

            if (Game.Instance == null) return;
            Game.Instance.uiHudRef.ShowPlayerJoinUI();
        }
    }

    private bool IsPadAlreadyPaired(Gamepad pad)
    {
        playerGamepadBindings = FindObjectOfType<PlayerGamepadBindings>();

        if (Game.Instance.captureManagerRef.PairedGamepad == pad) return true;
        if (playerGamepadBindings.PairedGamepad == pad) return true;

        return false;
    }

    public bool IsGamePadLeftToPair()
    {
        int unpairedGamepads = 0;

        if (gamepads == null) return false;

        foreach (Gamepad pad in gamepads) 
        {
            if (pad == null) return false;
            
            if (!IsPadAlreadyPaired(pad))
            {
                unpairedGamepads++;
            }
        }

        if (unpairedGamepads > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
