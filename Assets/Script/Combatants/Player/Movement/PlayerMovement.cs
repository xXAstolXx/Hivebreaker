using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float dashSpeed = 40.0f;

    private bool canDash = true;
    private bool isDashing = false;
    private bool isMovementEnabled = true;
    private bool isAttacking = false;

    private bool isKnockedBack = false;

    [HideInInspector]
    public bool canTakeDamage = true;

    private Player player;

    private NavMeshAgent agent;

    private Animator animator;

    private PlayableDirector playableDirector;

    private BoxCollider boxCollider;

    private List<SFXPlayer> sfx = new List<SFXPlayer>();

    private UI_HUD ui;

    [SerializeField]
    private Transform agentDestination;

    [SerializeField]
    private PlayableAsset dashTimeline;

    [SerializeField]
    private PlayableAsset knockbackTimeline;

    private Vector3 hitBy;

    [SerializeField]
    private VisualEffect[] dashCooldownVFX;

    [SerializeField]
    private TrailRenderer[] dashTrails;

    private void Awake()
    {
        player = GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playableDirector = GetComponent<PlayableDirector>();
        boxCollider = GetComponent<BoxCollider>();
        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            sfx.Add(audioPlayer);
        }
        agent.updateRotation = false;
    }


    private void Start()
    {
        ui = Game.Instance.uiHudRef;
    }

    private void Update()
    {
        if (isDashing) 
        {
            transform.position = Vector3.MoveTowards(transform.position, agentDestination.position, dashSpeed * Time.deltaTime);
        }

        if (isKnockedBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, hitBy, -dashSpeed * Time.deltaTime);
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            isAttacking = false;
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        isMovementEnabled = enabled;
    }

    public void SetPlayerAnimationIdle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isUsingStatMachine", false);
    }

    public void StopPlayerMovement()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        agent.isStopped = false;
    }

    public void UIToggleAction()
    {
        ui.ToggleExtendedStats();
    }

    public void AttackAction(Vector3 position)
    {
        isAttacking = true;
        if (isMovementEnabled == true && isDashing == false)
        {
            player.MeleeAttack(position);
            agent.SetDestination(transform.position + transform.forward);
        }
    }

    public void MovementAction(Vector3 directionVector, bool isAnimationPlaying)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Pet")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("isUsingStatMachine")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

        if (isMovementEnabled == true && isDashing == false && isAttacking == false)
        {
            animator.SetBool("isWalking", isAnimationPlaying);
            float calculatedInGameSpeed = directionVector.magnitude;
            if (animator.GetFloat("walkingSpeed") != calculatedInGameSpeed) 
            { 
                 animator.SetFloat("walkingSpeed", calculatedInGameSpeed);
            }

            transform.LookAt(transform.position + directionVector);
            agent.destination = transform.position + directionVector;
        }
    }

    public void RangedAttackAction(Vector3 position)
    {
        if (isMovementEnabled == true && isDashing == false)
        {
            player.RangedAttack(position);
        }
    }

    public void CaptureAttackAction(Vector3 position)
    {
        if (isMovementEnabled == true && isDashing == false)
        {
            player.CaptureAttack(position);
        }
    }

    public void KnockbackAction(Transform hitByTransform)
    {
        canDash = false;
        canTakeDamage = false;
        isKnockedBack = true;
        boxCollider.enabled = false;
        hitBy = hitByTransform.position;
        transform.LookAt(hitBy);
        playableDirector.playableAsset = knockbackTimeline;
        playableDirector.Play();
    }

    public void OnKnockbackEnd()
    {
        canDash = true;
        canTakeDamage = true;
        isKnockedBack = false;
        boxCollider.enabled = true;
    }

    public void DashAction()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Pet")) return;

        if (isMovementEnabled == true && canDash == true)
        {
            isDashing = true;
            canDash = false;
            canTakeDamage = false;
            animator.SetBool("isDashing", isDashing);
            playableDirector.playableAsset = dashTimeline;
            playableDirector.Play();
            PlaySFXByTriggerKey("DASH");
            ShowDashTrails(true);
        }
    }

    public void OnDashEnd()
    {
        canTakeDamage = true;
        isDashing = false;
        animator.SetBool("isDashing", isDashing);
        ShowDashCooldownVFX(true);
        ShowDashTrails(false);
    }

    private void ShowDashCooldownVFX(bool value)
    {
        foreach (VisualEffect effect in dashCooldownVFX)
        {
            if (value)
            {
                effect.Play();
            }
            else
            {
                effect.Stop();
            }
        }
    }

    private void ShowDashTrails(bool value)
    {
        foreach (TrailRenderer trail in dashTrails)
        {
                trail.emitting = value;
        }
    }

    public void OnDashCooldownEnd()
    {
        ShowDashCooldownVFX(false);
        canDash = true;
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in sfx)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }
}
