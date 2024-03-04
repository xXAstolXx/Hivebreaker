using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombatant
{
    [SerializeField]
    private DamageBullet bullet;

    [SerializeField]
    private CaptureBullet captureBullet;

    private VoiceLineTrigger voiceLineTrigger;

    [SerializeField]
    private string[] voiceLines;

    private bool hasSpokenVoiceLine = false;

    private bool isCaptureBulletReady = false;
    public bool IsCaptureBulletReady 
    { 
        get { return isCaptureBulletReady; }
        set { isCaptureBulletReady = value; }
    }

    private bool hasCaptureBullet = false;
    public bool HasCaptureBullet
    {
        get { return hasCaptureBullet; }
        set 
        { 
            hasCaptureBullet = value; 
            isCaptureBulletReady = value; 
            Camera.main.GetComponentInChildren<CreatureCaptureItemDisplay>().ShowItem(value);
        }
    }

    private Stats stats;
    public Stats Stats 
    {
        get { return stats; }
        set 
        {
            stats = value;
            UpdateUI();
            SetMoveSpeed(stats.m_moveSpeed);
        }
    }

    private UI_HUD ui;

    private MeleeAttack meleeAttack;
    private RangedAttack rangedAttack;

    private Animator animator;

    private List<SFXPlayer> playerSFX = new List<SFXPlayer>();

    private PlayerMovement playerMovement;

    private CompanionController companionController;

    private RadialHealthSlider radialHealthSlider;

    private Interactable interactableItem;
    private string interactName;

    private DialogTrigger trigger;

    [SerializeField]
    private EnemyData enemyData; //stats for player

    private float currentTime = 0.0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        meleeAttack = GetComponent<MeleeAttack>();
        rangedAttack = GetComponent<RangedAttack>();
        stats = new Stats();
        animator = GetComponent<Animator>();
        companionController = GetComponent<CompanionController>();
        radialHealthSlider = GetComponentInChildren<RadialHealthSlider>();
        voiceLineTrigger = GetComponent<VoiceLineTrigger>();

        ui = Game.Instance.uiHudRef;

        GetSFXPlayers();
    }

    private void Start()
    {
        Game.Instance.OnPlayerLoaded();

        if (Game.Instance.levelLoaderRef.GetPlayerStats() == null)
        {
            SetStartStats(enemyData);
        }
        else
        {
            SetStartStats(Game.Instance.levelLoaderRef.GetPlayerStats());
            HasCaptureBullet = Game.Instance.levelLoaderRef.HasPlayerCaptureBullet;
        }
        SetMoveSpeed(stats.m_moveSpeed);

        SetupUI();
        UpdateUI();

        currentTime = Time.time;
        playerMovement.SetMovementEnabled(false);
    }

    private void Update()
    {
        if (currentTime > 0.0f)
        {
            if (Time.time > currentTime + 2.0f)
            {
                currentTime = 0.0f;
                if (trigger != null) return;
                playerMovement.SetMovementEnabled(true);
            }
        }
    }

    private void GetSFXPlayers()
    {
        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            playerSFX.Add(audioPlayer);
        }
    }

    private void SetupUI()
    {
        ui.SetPlayerMinMaxHealthBar(stats.m_maxHP, 0);
        radialHealthSlider.SetupHealthSlider(stats.m_maxHP, 0);
        ui.SetCounter(stats);
    }

    private void UpdateUI()
    {
        Debug.Log("PlayerHP: " + stats.m_hp);
        ui.UpdatePlayerHealthBar(stats.m_hp);
        radialHealthSlider.SetHealthToSlider(stats.m_hp);
        ui.UpdateMaxHPCounter(stats.m_hp);
        ui.SetCounter(stats);
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playerSFX) 
        { 
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    public void SetDialogTrigger(DialogTrigger trigger)
    {
        if (trigger == null) return;

        this.trigger = trigger;
    }

    public void DialogAdvanceAction()
    {
        if (this.trigger == null) return;
        trigger.AdvanceAction();
    }

    public void SetInteractItem(GameObject other, string name)
    {
        interactableItem = other.GetComponent<Interactable>();
        interactName = name;
    }

    public void ClearInteractItem()
    {
        interactableItem = null;
        interactName = "";
    }

    public void InteractAction()
    {
        if (interactableItem == null) return;

        if (interactName == "STATOMAT")
        {
            playerMovement.StopPlayerMovement();
            ToggleStatMachineAnimation(true);
            transform.LookAt(interactableItem.transform.position);
        }

        if (interactName == "DIALOG_INTERACT")
        {
            playerMovement.StopPlayerMovement();
            playerMovement.SetPlayerAnimationIdle();
        }

        if (interactName == "TELEPORTER")
        {
            //play teleport vfx
        }

        interactableItem.InteractWith();
    }

    public void EnterDoor(Transform spawnpoint)
    {
        GetComponent<PlayerAgent>().TeleportPlayer(spawnpoint);
        companionController.TeleportCompanionAction();
    }

    #region ICombatant

    public void HitByAttack(int damage, GameObject hitBy, DamageType damageType)
    {
        if (playerMovement.canTakeDamage)
        {
            transform.LookAt(hitBy.transform);
            TakeDamage(damage, damageType);
            PlaySFXByTriggerKey("GOT_HIT");
            Knockback(hitBy.transform);
            ui.ShowPlayerDamagedPortrait();
        }
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        int calculatedDamage = 0;

        switch(damageType)
        {
            case DamageType.MELEE:
                calculatedDamage = damage - stats.m_meleeDefense;
                break;
            case DamageType.RANGED:
                calculatedDamage = damage - stats.m_rangedDefense;
                break;
            case DamageType.SPECIAL:
                calculatedDamage = damage - ((stats.m_rangedDefense + stats.m_meleeDefense) / 2);
                break;
        }

        calculatedDamage = Mathf.Clamp(calculatedDamage, 1, calculatedDamage);

        stats.DecreaseStat("HP", calculatedDamage);
        if (stats.m_hp <= 0) 
        { 
            CombatantDeath();
        }
        UpdateUI();
    }

    public void HealHP(int amount)
    {
        if (stats.m_hp <= 0) return;

        stats.IncreaseStat("HP", amount);
        companionController.HealCreatureAction(amount);

        UpdateUI();
    }

    public void SetStartStats(Stats data)
    {
        stats.m_maxHP = data.m_maxHP;
        stats.m_hp = data.m_hp;
        stats.m_meleeAttack = data.m_meleeAttack;
        stats.m_rangedAttack = data.m_rangedAttack;
        stats.m_meleeDefense = data.m_meleeDefense;
        stats.m_rangedDefense = data.m_rangedDefense;
        stats.m_moveSpeed = data.m_moveSpeed;
        stats.m_meleeAttackSpeed = data.m_meleeAttackSpeed;
        stats.m_rangedAttackSpeed = data.m_rangedAttackSpeed;
        stats.m_bulletSpeed = data.m_bulletSpeed;
    }

    public void SetStartStats(int maxHP, int hp, int meleeAttack,
            int rangedAttack, int meleeDefense, int rangedDefense, float moveSpeed,
            int meleeAttackSpeed, int rangedAttackSpeed, float bulletSpeed)
    {
        stats.m_maxHP = maxHP;
        stats.m_hp = hp;
        stats.m_meleeAttack = meleeAttack;
        stats.m_rangedAttack = rangedAttack;
        stats.m_meleeDefense = meleeDefense;
        stats.m_rangedDefense = rangedDefense;
        stats.m_moveSpeed = moveSpeed;
        stats.m_meleeAttackSpeed = meleeAttackSpeed;
        stats.m_rangedAttackSpeed = rangedAttackSpeed;
        stats.m_bulletSpeed = bulletSpeed;
    }

    public void RandomVoiceLine()
    {
        if (hasSpokenVoiceLine) return;

        int randomChance = Random.Range(0, 3);

        if (randomChance == 1) return;

        int randomRange = Random.Range(0, voiceLines.Length);

        voiceLineTrigger.ShowBubble(true, voiceLines[randomRange]);
        hasSpokenVoiceLine = true;
    }

    public void MeleeAttack(Vector3 position)
    {
        if (meleeAttack == null) return;


        if (meleeAttack.Attack(stats.m_meleeAttackSpeed, stats.m_meleeAttack))
        {
            transform.LookAt(position);

            animator.SetTrigger("Attack");
            animator.SetBool("isPunchMirrored", !animator.GetBool("isPunchMirrored"));
            PlaySFXByTriggerKey("PUNCH");
        }
    }

    public void RangedAttack(Vector3 position)
    {
        if (rangedAttack == null) return;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        { 
            transform.LookAt(position);
        }

        if (rangedAttack.FireBullet(stats.m_rangedAttackSpeed, stats.m_rangedAttack, stats.m_bulletSpeed, bullet))
        {
            animator.SetTrigger("Shoot");
        }
    }

    public void CombatantDeath()
    {
        animator.SetTrigger("Death");
        playerMovement.StopPlayerMovement();
        playerMovement.SetMovementEnabled(false);
        playerMovement.canTakeDamage = false;
        Game.Instance.uiHudRef.OnPlayerDeath();
        SaveFileManager.Instance.DeleteSaveFile("saveData");
        PlaySFXByTriggerKey("DEATH");
    }

    public void Knockback(Transform hitBy)
    {
        playerMovement.KnockbackAction(hitBy);
    }

    #endregion

    public void SetStartStats(EnemyData data)
    {
        stats.m_maxHP = data.maxHP;
        stats.m_meleeAttack = data.meleeAttack;
        stats.m_rangedAttack = data.rangedAttack;
        stats.m_meleeDefense = data.meleeDefense;
        stats.m_rangedDefense = data.rangedDefense;
        stats.m_moveSpeed = data.moveSpeed;
        stats.m_meleeAttackSpeed = data.meleeAttackSpeed;
        stats.m_rangedAttackSpeed = data.rangedAttackSpeed;
        stats.m_bulletSpeed = data.bulletSpeed;

        stats.m_hp = stats.m_maxHP;
    }

    public void CaptureAttack(Vector3 position)
    {
        if (rangedAttack == null) return;
        if (captureBullet == null) return;
        if (!hasCaptureBullet) return;
        if (!isCaptureBulletReady) return;

        transform.LookAt(position);

        if (rangedAttack.FireBullet(stats.m_rangedAttackSpeed, 30.0f, captureBullet))
        {
            transform.LookAt(position);
            PlaySFXByTriggerKey("SHOT");
            isCaptureBulletReady = false;
        }
    }

    public void AddStats(Stats statsToAdd)
    {
        stats += statsToAdd;
        UpdateUI();
        SetMoveSpeed(stats.m_moveSpeed);
        ToggleStatMachineAnimation(false);
    }

    public void DecreaseStats(Stats statsToDecrease)
    {
        stats -= statsToDecrease;
        UpdateUI();
        SetMoveSpeed(stats.m_moveSpeed);
        ToggleStatMachineAnimation(false);
    }

    private void SetMoveSpeed(float speed)
    {
        GetComponent<PlayerAgent>().SetAgentSpeed(speed);
    }

    public void ToggleStatMachineAnimation(bool value)
    {
        animator.SetBool("isUsingStatMachine", value);
    }
}
