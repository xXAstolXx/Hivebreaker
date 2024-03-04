using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour, ICombatant
{
    [SerializeField]
    DamageBullet bullet;

    [SerializeField]
    private string enemyDataPath;

    [SerializeField]
    private VisualEffect impact;

    [SerializeField]
    private VisualEffect dust;

    [SerializeField]
    private StatIncreaseVFX vfxController;

    public Animator animator;

    private EnemyMovement enemyMovement;

    private NavMeshAgent agent;
    private Stats stats;
    public Stats Stats { get { return stats; } set { } }

    private MeleeAttack meleeAttack;
    private RangedAttack rangedAttack;

    [SerializeField]
    private EnemyType enemyAttackingType;
    public EnemyType EnemyAttackingType { get { return enemyAttackingType; } set { } }

    private List<SFXPlayer> playerSFX = new List<SFXPlayer>();

    void Awake()
    {
        stats = new Stats();

        agent = GetComponent<NavMeshAgent>();
        meleeAttack = GetComponent<MeleeAttack>();
        rangedAttack = GetComponent<RangedAttack>();
        enemyMovement = GetComponent<EnemyMovement>();

        animator = GetComponentInChildren<Animator>();

        if (gameObject.tag != "Player")
        {
            SetStartStats(GetEnemyDataFromFolder());
        }

        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            playerSFX.Add(audioPlayer);
        }
    }

    private void Start()
    {
        if (gameObject.tag != "Player")
        {
            agent.angularSpeed = 0;
        }
    }

    private EnemyData GetEnemyDataFromFolder()
    {
        List<EnemyData> enemyData = new List<EnemyData>();

        foreach (var data in Resources.LoadAll(enemyDataPath))
        {
            Debug.Log(data.name);
            enemyData.Add((EnemyData)data);
        }

        int everyTenFloor = Game.Instance.levelLoaderRef.EveryTenFloor;

        if (everyTenFloor > enemyData.Count - 1)
        {

            return enemyData[enemyData.Count - 1];
        }

        EnemyData pickedData = enemyData[everyTenFloor];
        return pickedData;
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playerSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    public void IncreaseStatsByCapsule()
    {
        stats.PrintStatsToConsole();

        EnemyData enemyData = GetEnemyDataFromFolder();

        stats.m_maxHP += enemyData.capsuleMaxHP;
        if (enemyData.capsuleMaxHP > 0)
        {
            vfxController.PlayStatVFX("Max_Hp");
        }

        stats.m_meleeAttack += enemyData.capsuleMeleeAttack;
        if (enemyData.capsuleMeleeAttack > 0)
        {
            vfxController.PlayStatVFX("Attack");
        }

        stats.m_rangedAttack += enemyData.capsuleRangedAttack;
        if (enemyData.capsuleRangedAttack > 0)
        {
            vfxController.PlayStatVFX("Fire_Power");
        }

        stats.m_meleeDefense += enemyData.capsuleMeleeDefense;
        if (enemyData.capsuleMeleeDefense > 0)
        {
            vfxController.PlayStatVFX("Defense_Melee");
        }

        stats.m_rangedDefense += enemyData.capsuleRangedDefense;
        if (enemyData.capsuleRangedDefense > 0)
        {
            vfxController.PlayStatVFX("Defense_Ranged");
        }

        stats.m_moveSpeed += enemyData.capsuleMoveSpeed;
        if (enemyData.capsuleMoveSpeed > 0)
        {
            vfxController.PlayStatVFX("Move_Speed");
        }

        stats.m_meleeAttackSpeed += enemyData.capsuleMeleeAttackSpeed;
        if (enemyData.capsuleMeleeAttackSpeed > 0)
        {
            vfxController.PlayStatVFX("Attack_Speed");
        }

        stats.m_rangedAttackSpeed += enemyData.capsuleRangedAttackSpeed;
        if (enemyData.capsuleRangedAttackSpeed > 0)
        {
            vfxController.PlayStatVFX("Fire_Rate");
        }

        stats.m_bulletSpeed += enemyData.capsuleBulletSpeed;
        if (enemyData.capsuleBulletSpeed > 0)
        {
            vfxController.PlayStatVFX("Bullet_Speed");
        }

        stats.PrintStatsToConsole();
    }

    #region ICombatant

    public void HitByAttack(int damage, GameObject hitBy, DamageType damageType)
    {
        TakeDamage(damage, damageType);
        PlaySFXByTriggerKey("GOT_HIT");
        impact.Play();
        Knockback(hitBy.transform);
        GetComponent<EnemyCapture>().OnCompanionDamaged();
    }

    public void TakeDamage(int damage, DamageType damageType) 
    {
        int calculatedDamage = 0;

        switch (damageType)
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

    }

    public void MeleeAttack(Vector3 position) 
    {
        stats.PrintStatsToConsole();
        if (meleeAttack == null) return;

        transform.LookAt(position);
        if (meleeAttack.Attack(stats.m_meleeAttackSpeed, stats.m_meleeAttack))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void RangedAttack(Vector3 position) 
    {
        if (rangedAttack == null) return;

        transform.LookAt(position);
        if (rangedAttack.FireBullet(stats.m_rangedAttackSpeed, stats.m_rangedAttack, stats.m_bulletSpeed, bullet))
        {
            PlaySFXByTriggerKey("RANGE_ATTACK");
            animator.SetTrigger("Fire");
        }
    }

    public void HealHP(int amount)
    {
        stats.IncreaseStat("HP", amount);
        if (gameObject.tag == "Player")
        {
            Game.Instance.uiHudRef.UpdateCompanionHealth(stats.m_hp);
        }
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

        agent.speed = stats.m_moveSpeed;
        stats.PrintStatsToConsole();

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

    public void CombatantDeath()
    {
        dust.Play();
        PlaySFXByTriggerKey("DESTROYED");
        if (gameObject.tag == "Enemy")
        {
            FindObjectOfType<Room>(false).RemoveEnemy(gameObject);
            Game.Instance.highscoreSystemRef.KillCounter++;
        }
        if (gameObject.tag == "Player")
        {
            Game.Instance.captureManagerRef.ClearCaptureSlot();
            Game.Instance.uiHudRef.SetCompanionPortrait(EnemyType.NONE);
            Game.Instance.uiHudRef.SetCompanionHealthBarActiv(false);
        }
        foreach (Component component in GetComponentsInChildren<Component>())
        {
            if (component == component.GetComponent<Transform>()) continue;
            if (component == component.GetComponent<SFXPlayer>()) continue;
            if (component == component.GetComponent<AudioSource>()) continue;
            if (component == component.GetComponent<VisualEffect>()) continue;
            if (component == component.GetComponent<Renderer>())
            {
                if (component != component.GetComponent<SkinnedMeshRenderer>()) continue;
            }
            if (component == component.GetComponent<CanvasRenderer>()) continue;

            PlaySFXByTriggerKey("DEATH");
            Destroy(component);
            Destroy(gameObject, 5.0f);
        }
    }

    public void Knockback(Transform hitBy)
    {
        enemyMovement.KnockbackAction(hitBy);
    }

    #endregion ICombatant

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

        agent.speed = stats.m_moveSpeed;
        stats.m_hp = stats.m_maxHP;
    }

    public void SetStartStats(EnemyData data, int amountBonusBoost)
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

        agent.speed = stats.m_moveSpeed;
        stats.m_hp = stats.m_maxHP;

        for (int i = 0;  i < amountBonusBoost; i++) 
        {
            IncreaseStatsByCapsule();
        }
    }
}
