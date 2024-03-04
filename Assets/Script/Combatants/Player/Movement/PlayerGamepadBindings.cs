using UnityEngine;

public class PlayerGamepadBindings : GamePadPairing
{
    private PlayerMovement playerMovement;

    private Player player;

    private CompanionController companionController;

    private Indicator indicator;

    private Vector3 aimVector = Vector3.zero;
    private float aimPower = 2.0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        companionController = GetComponent<CompanionController>();
        indicator = GetComponentInChildren<Indicator>();

        Game.Instance.captureManagerRef.SetGamepadInCompanion();
    }

    private void Start()
    {
        indicator.gameObject.SetActive(true);
        TryPairWithGamepadOnStart();
    }

    private void TryPairWithGamepadOnStart()
    {
        GamePadHandler gamePadHandler = FindObjectOfType<GamePadHandler>();
        if (gamePadHandler == null) return;
        if (gamePadHandler.PairedWithPlayer == null) return;

        pairedGamepad = gamePadHandler.PairedWithPlayer;
    }

    private Vector3 CalculateRotatedVector(Vector2 stickInput)
    {
        int rotationAngle = -45;
        
        Vector3 vector3 = Vector3.zero;

        vector3.x = (Mathf.Cos(rotationAngle) * stickInput.x) - (Mathf.Sin(rotationAngle) * stickInput.y);
        vector3.z = (Mathf.Sin(rotationAngle) * stickInput.x) + (Mathf.Cos(rotationAngle) * stickInput.y);

        return vector3;
    }

    private void MovementKeyBindings()
    {
        bool isAnimationPlaying = false;

        directionVector = Vector3.zero;

        if (pairedGamepad.leftStick.magnitude > stickDeadZone) 
        {
            Vector2 lStickInput = pairedGamepad.leftStick.ReadValue();
            directionVector = CalculateRotatedVector(lStickInput);

            isAnimationPlaying = true;
        }

        playerMovement.MovementAction(directionVector, isAnimationPlaying);
    }

    private Vector3 CalculateCurrentAim()
    {
        Vector3 newAim = transform.position;

        if (pairedGamepad.rightStick.magnitude > stickDeadZone)
        {
            Vector2 rStickInput = pairedGamepad.rightStick.ReadValue();
            
            newAim += CalculateRotatedVector(rStickInput) * aimPower;
            return newAim;
        }
        return newAim += transform.forward;
    }

    private void ActionKeyBindings()
    {
        if (pairedGamepad.rightTrigger.wasPressedThisFrame)
        {
            playerMovement.AttackAction(aimVector);
        }

        if (pairedGamepad.rightShoulder.wasPressedThisFrame)
        {
            playerMovement.RangedAttackAction(aimVector);
        }

        if (pairedGamepad.yButton.wasPressedThisFrame)
        {
            playerMovement.CaptureAttackAction(aimVector);
        }

        if (pairedGamepad.leftShoulder.wasPressedThisFrame)
        {
            playerMovement.DashAction();
        }

        if (pairedGamepad.xButton.wasPressedThisFrame)
        {
            player.InteractAction();
            player.DialogAdvanceAction();
        }

        if (pairedGamepad.rightStickButton.wasPressedThisFrame)
        {
            playerMovement.UIToggleAction();
        }
    }

    private void CreatureKeyBindings()
    {
        if (pairedGamepad.dpad.up.wasPressedThisFrame)
        {
            companionController.SummonCreatureAction();
        }

        if (pairedGamepad.dpad.down.wasPressedThisFrame)
        {
            companionController.WithdrawCreatureAction();
        }

        if (pairedGamepad.dpad.left.wasPressedThisFrame)
        {
            companionController.PetCreatureAction();
        }

        if (pairedGamepad.dpad.right.wasPressedThisFrame)
        {
            companionController.TeleportCompanionAction();
        }
    }

    private void AimIndicatorBinding()
    {
        if (indicator == null) return;
        indicator.RotateTowards(aimVector);
    }

    private void Update()
    {
        if (pairedGamepad == null) return;
        if (Game.Instance.IsGamePaused) return;

        aimVector = CalculateCurrentAim();

        AimIndicatorBinding();
        ActionKeyBindings();
        MovementKeyBindings();
        CreatureKeyBindings();
    }
}
