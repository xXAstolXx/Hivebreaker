using UnityEngine;

public class CompanionGamepadBindings : GamePadPairing
{
    private EnemyMovement enemyMovement;

    private EnemyAI enemyAI;

    [SerializeField]
    private Indicator indicator;

    private Vector3 aimVector = Vector3.zero;
    private float aimPower = 2.0f;

    private void Start()
    {
        if (gameObject.tag != "Player") return;

        enemyMovement = GetComponent<EnemyMovement>();
        enemyAI = GetComponent<EnemyAI>();

        GetGamepadFromCaptureManager();
        indicator.gameObject.SetActive(true);
    }

    private void GetGamepadFromCaptureManager()
    {
        if (Game.Instance.captureManagerRef.PairedGamepad == null) return;
        PairedGamepad = Game.Instance.captureManagerRef.PairedGamepad;
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
            directionVector *= 3.0f;

            isAnimationPlaying = true;
        }

        enemyMovement.MovementAction(directionVector, isAnimationPlaying);
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
            enemyMovement.AttackAction(aimVector);
        }

        if (pairedGamepad.rightShoulder.wasPressedThisFrame)
        {
            enemyMovement.RangedAttackAction(aimVector);
        }

        if (pairedGamepad.yButton.wasPressedThisFrame)
        {
            //
        }

        if (pairedGamepad.leftShoulder.wasPressedThisFrame)
        {
            //
        }

        if (pairedGamepad.xButton.wasPressedThisFrame)
        {
            //
        }

        if (pairedGamepad.rightStickButton.wasPressedThisFrame)
        {
            //
        }
    }

    private void AimIndicatorBinding()
    {
        if (indicator == null) return;
        indicator.RotateTowards(aimVector);
    }

    protected override void OnGamepadPaired()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetTargetToNull();
    }

    void Update()
    {
        if (pairedGamepad == null) return;
        if (Game.Instance.IsGamePaused) return;

        aimVector = CalculateCurrentAim();

        AimIndicatorBinding();
        ActionKeyBindings();
        MovementKeyBindings();
    }
}
