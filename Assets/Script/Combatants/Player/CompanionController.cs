using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    private GameObject currentSpawnedCompanion;
    public GameObject CurrentSpawnedCompanion
    {
        get { return currentSpawnedCompanion; }
        set { currentSpawnedCompanion = value; }
    }

    private CaptureManager captureManager;

    private Animator animator;

    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        captureManager = Game.Instance.captureManagerRef;
    }

    private void Start()
    {
        if (captureManager.WasSpawnedInLastLevel)
        {
            SummonCreatureAction();
        }
    }

    public void OnEnemyCaptured()
    {
        player.HasCaptureBullet = false;
        KillCompanion();
    }

    public void SummonCreatureAction()
    {
        if (captureManager.GetEnemyInCaptureSlot() == null) return;
        if (currentSpawnedCompanion != null) return;

        CapturedEnemy capturedEnemyData = captureManager.GetEnemyInCaptureSlot();
        GameObject enemyPrefab = captureManager.GetEnemyPrefab(capturedEnemyData.EnemyType);

        CurrentSpawnedCompanion = Instantiate(enemyPrefab, transform.position, Quaternion.identity, Game.Instance.transform);
        currentSpawnedCompanion.GetComponent<EnemyCapture>().SummonAction(capturedEnemyData, gameObject);
    }

    public void WithdrawCreatureAction()
    {
        if (currentSpawnedCompanion == null) return;

        currentSpawnedCompanion.GetComponent<EnemyCapture>().WithDrawAction();
        currentSpawnedCompanion = null;
    }

    public void TeleportCompanionAction()
    {
        if (currentSpawnedCompanion == null) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Pet")) return;

        currentSpawnedCompanion.GetComponent<EnemyCapture>().TeleportToPlayer();
    }

    public void PetCreatureAction()
    {
        if (currentSpawnedCompanion == null) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Pet")) return;

        if (currentSpawnedCompanion.GetComponent<Enemy>().EnemyAttackingType == EnemyType.RANGED)
        {
            currentSpawnedCompanion.GetComponent<EnemyCapture>().PetAction();
            transform.LookAt(currentSpawnedCompanion.transform.position);
            animator.SetTrigger("Pet");
        }
    }

    public void HealCreatureAction(int amount)
    {
        if (currentSpawnedCompanion == null) return;

        currentSpawnedCompanion.GetComponent<Enemy>().HealHP(amount);
    }

    public void OnLevelExit()
    {
        if (currentSpawnedCompanion != null)
        {
            captureManager.WasSpawnedInLastLevel = true;
        }
        else
        {
            captureManager.WasSpawnedInLastLevel = false;
        }
        WithdrawCreatureAction();
    }

    public void KillCompanion()
    {
        if (currentSpawnedCompanion == null) return;
        currentSpawnedCompanion.GetComponent<EnemyCapture>().KillCompanion();
    }

    public void OnCapsuleItemCollected()
    {
        if (currentSpawnedCompanion == null) return;

        currentSpawnedCompanion.GetComponent<EnemyCapture>().CapsuleStatIncrease();
    }
}
