using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardBindings : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private Player player;

    private CompanionController companionController;

    private Indicator indicator;

    private PlayerInputActions playerInputBindings;
    private InputAction wKey;
    private InputAction aKey;
    private InputAction sKey;
    private InputAction dKey;

    private InputAction upArrowKey;
    private InputAction downArrowKey;
    private InputAction leftArrowKey;
    private InputAction rightArrowKey;

    private InputAction meleeAttackKey;
    private InputAction rangedAttackKey;

    private InputAction mousePos;

    private InputAction itemKey;

    private InputAction dashKey;

    private InputAction tabKey;

    private InputAction interactKey;

    private Vector3 directionVector;

    public bool isKeyboardControlEnabled = true;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        player = GetComponent<Player>();

        companionController = GetComponent<CompanionController>();

        indicator = GetComponentInChildren<Indicator>();

        playerInputBindings = new PlayerInputActions();
        wKey = playerInputBindings.KeyboardMouse.W;
        aKey = playerInputBindings.KeyboardMouse.A;
        sKey = playerInputBindings.KeyboardMouse.S;
        dKey = playerInputBindings.KeyboardMouse.D;

        upArrowKey = playerInputBindings.KeyboardMouse.Summon;
        downArrowKey = playerInputBindings.KeyboardMouse.Withdraw;
        leftArrowKey = playerInputBindings.KeyboardMouse.SlotLeft;
        rightArrowKey = playerInputBindings.KeyboardMouse.SlotRight;

        meleeAttackKey = playerInputBindings.KeyboardMouse.MeleeAttack;
        rangedAttackKey = playerInputBindings.KeyboardMouse.RangedAttack;

        mousePos = playerInputBindings.KeyboardMouse.MousePos;

        tabKey = playerInputBindings.KeyboardMouse.Tab;
        itemKey = playerInputBindings.KeyboardMouse.Item;
        dashKey = playerInputBindings.KeyboardMouse.Dash;
        interactKey = playerInputBindings.KeyboardMouse.Interact;

        directionVector = new Vector3();
    }

    private void Start()
    {
        indicator.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        wKey.Enable();
        aKey.Enable();
        sKey.Enable();
        dKey.Enable();
        upArrowKey.Enable();
        downArrowKey.Enable();
        leftArrowKey.Enable();
        rightArrowKey.Enable();
        meleeAttackKey.Enable();
        rangedAttackKey.Enable();
        mousePos.Enable();
        tabKey.Enable();
        itemKey.Enable(); 
        dashKey.Enable();
        interactKey.Enable();
    }

    private void OnDisable()
    {
        wKey.Disable();
        aKey.Disable();
        sKey.Disable();
        dKey.Disable();
        upArrowKey.Disable();
        downArrowKey.Disable();
        leftArrowKey.Disable();
        rightArrowKey.Disable();
        meleeAttackKey.Disable();
        rangedAttackKey.Disable();
        mousePos.Disable();
        tabKey.Disable();
        itemKey.Disable(); 
        dashKey.Disable();
        interactKey.Disable();
    }

    private void MovementKeyBindings()
    {
        bool isAnimationPlaying = false;

        directionVector = Vector3.zero;

        if (wKey.IsPressed() == true)
        {
            directionVector.x += 1.0f;
            directionVector.y += 0.0f;
            directionVector.z += 1.0f;

            isAnimationPlaying = true;
        }

        if (aKey.IsPressed() == true)
        {
            directionVector.x += -1.0f;
            directionVector.y += 0.0f;
            directionVector.z += 1.0f;

            isAnimationPlaying = true;
        }

        if (sKey.IsPressed() == true)
        {
            directionVector.x += -1.0f;
            directionVector.y += 0.0f;
            directionVector.z += -1.0f;

            isAnimationPlaying = true;
        }

        if (dKey.IsPressed() == true)
        {
            directionVector.x += 1.0f;
            directionVector.y += 0.0f;
            directionVector.z += -1.0f;

            isAnimationPlaying = true;
        }

        if (!wKey.IsPressed() && !aKey.IsPressed() && !sKey.IsPressed() && !dKey.IsPressed())
        {
            directionVector = Vector3.zero;

            isAnimationPlaying = false;
        }

        playerMovement.MovementAction(directionVector, isAnimationPlaying);
    }

    private void ActionKeyBindings()
    {
        if (meleeAttackKey.WasPerformedThisFrame())
        {
            playerMovement.AttackAction(MousePosToGamePosition(mousePos.ReadValue<Vector2>()));
        }

        if (rangedAttackKey.WasPerformedThisFrame())
        {
            playerMovement.RangedAttackAction(MousePosToGamePosition(mousePos.ReadValue<Vector2>()));
        }
        
        if (itemKey.WasPerformedThisFrame())
        {
            playerMovement.CaptureAttackAction(MousePosToGamePosition(mousePos.ReadValue<Vector2>()));
        }

        if (dashKey.WasPerformedThisFrame())
        {
            playerMovement.DashAction();
        }

        if (interactKey.WasPerformedThisFrame())
        {
            player.InteractAction();
            player.DialogAdvanceAction();
        }

        if (tabKey.WasPressedThisFrame())
        {
            playerMovement.UIToggleAction();
        }
    }

    private void CreatureKeyBindings()
    {
        if (upArrowKey.WasPerformedThisFrame()) 
        {
            companionController.SummonCreatureAction();
        }

        if (downArrowKey.WasPerformedThisFrame())
        {
            companionController.WithdrawCreatureAction();
        }

        if (leftArrowKey.WasPerformedThisFrame())
        {
            companionController.PetCreatureAction();
        }

        if (rightArrowKey.WasPerformedThisFrame())
        {
            companionController.TeleportCompanionAction();
        }
    }

    private void AimIndicatorBinding()
    {
        if (indicator == null) return;
        Vector3 aimVector = MousePosToGamePosition(mousePos.ReadValue<Vector2>());
        indicator.RotateTowards(aimVector);
    }

    private Vector3 MousePosToGamePosition(Vector2 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("MouseRaycast");

        if (Physics.Raycast(ray, out hit, 200.0f, mask))
        {
            return hit.point;
        }

        return hit.point;
    }

    private void Update()
    {
        if (!isKeyboardControlEnabled) return;
        if (Game.Instance.IsGamePaused) return;

        MovementKeyBindings();
        ActionKeyBindings();
        CreatureKeyBindings();
        AimIndicatorBinding();
    }
}
