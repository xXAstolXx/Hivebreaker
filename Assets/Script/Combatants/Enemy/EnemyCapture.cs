using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCapture : MonoBehaviour
{
    private UI_HUD ui;
    private Enemy enemy;
    private EnemyAI enemyAI;

    private GameObject player;

    [SerializeField]
    private Material capturedMaterial;

    [SerializeField]
    private ParticleSystem capturedEffect;

    private float currentTimer = 0.0f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAI = GetComponent<EnemyAI>();
        ui = Game.Instance.uiHudRef;
    }

    public void OnCaptureItemHit()
    {
        capturedEffect.Play();
        GetComponent<EnemyMovement>().SetMovementEnabled(false);

        currentTimer = Time.time;
    }

    private void Update()
    {
        if (currentTimer == 0.0f) return;

        if (Time.time >= currentTimer + 2.0f)
        {
            currentTimer = 0.0f;
            CaptureAction();
        }
    }

    public void CaptureAction()
    {
        EnemyType enemyType = enemy.EnemyAttackingType;
        AIBehavior behavior = enemyAI.Behavior;
        Stats enemyStats = IncreaseStatOnCapture(enemy.Stats);
        CapturedEnemy newCapturedEnemy = new CapturedEnemy(enemyType, behavior, enemyStats);

        enemyAI.Behavior = AIBehavior.NONE;
        Game.Instance.captureManagerRef.SetEnemyInCaptureSlot(newCapturedEnemy);
        
        if (enemy.EnemyAttackingType == EnemyType.RANGED || enemy.EnemyAttackingType == EnemyType.RANGED_CRATE)
        {
            Game.Instance.uiHudRef.SetCompanionPortrait(EnemyType.RANGED);
        }
        else
        {
            Game.Instance.uiHudRef.SetCompanionPortrait(EnemyType.MELEE);
        }
       
        enemy.CombatantDeath();
    }

    public void SummonAction(CapturedEnemy enemyData, GameObject summonedBy)
    {
        ChangeTag("Player");

        GetComponentInChildren<SkinnedMeshRenderer>().material = capturedMaterial;

        player = summonedBy;

        enemy.EnemyAttackingType = enemyData.EnemyType;
        enemy.SetStartStats(enemyData.Stats);
        enemyAI.Behavior = enemyData.AIBehavior;

        FindObjectOfType<Room>(false).SetRoomCombatStateInEnemies();

        ui.SetCompanionHealthBarActiv(true);
        ui.SetCompanionMinMaxHealthBar(enemy.Stats.m_maxHP, 0);
        CompanionHealthToUi(enemy.Stats.m_hp);
    }

    public void WithDrawAction()
    {
        EnemyType enemyType = enemy.EnemyAttackingType;
        AIBehavior behavior;
        
        if (enemyAI.Behavior == AIBehavior.FOLLOW)
        {
            behavior = Game.Instance.captureManagerRef.GetEnemyInCaptureSlot().AIBehavior;
        }
        else
        { 
            behavior = enemyAI.Behavior;
        }
        Stats enemyStats = enemy.Stats;
        CapturedEnemy withdrawnCapturedEnemy = new CapturedEnemy(enemyType, behavior, enemyStats);
        Game.Instance.captureManagerRef.WithDrawToCaptureSlot(withdrawnCapturedEnemy);
        ui.SetCompanionHealthBarActiv(false);
        Destroy(gameObject);
    }

    private void ChangeTag(string tag)
    {
        foreach (Transform obj in GetComponentsInChildren<Transform>()) 
        { 
            obj.gameObject.tag = tag;
        }
    }

    public void PetAction()
    {
        if (enemy.EnemyAttackingType == EnemyType.RANGED)
        {
            TeleportToPlayer();
            transform.LookAt(player.transform.position);
            enemy.animator.SetTrigger("GetPet");
        }
    }

    public void OnCompanionDamaged()
    {
        if (player == null) return;
        if (gameObject.tag != "Player") return;

        Game.Instance.uiHudRef.ShowCompanionDamagePortrait();

        CompanionHealthToUi(enemy.Stats.m_hp);
    }

    public void KillCompanion()
    {
        enemy.CombatantDeath();
        ui.SetCompanionHealthBarActiv(false);
    }

    public void TeleportToPlayer()
    {
        Vector3 teleportPos = player.transform.position;
        teleportPos += player.transform.forward;
        GetComponent<NavMeshAgent>().Warp(teleportPos);
    }

    public void CapsuleStatIncrease()
    {
        enemy.IncreaseStatsByCapsule();
        ui.SetCompanionMinMaxHealthBar(enemy.Stats.m_maxHP, 0);
        CompanionHealthToUi(enemy.Stats.m_hp);
    }

    private Stats IncreaseStatOnCapture(Stats stats)
    {
        float value = (stats.m_maxHP / 100) * 25;

        stats.IncreaseStat("Max_Hp", (int)value);
        CapsuleStatIncrease();

        return stats;
    }

    private void CompanionHealthToUi(int health)
    {
       if(gameObject.tag != "Player") { return; }

       ui.UpdateCompanionHealth(health);
    }
}
