using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIBehavior
{
    NONE = 0,
    AGGRESSIVE = 10,
    DEFENSIVE = 20,
    MIXED = 30,
    FOLLOW = 40
}

public class EnemyAI : MonoBehaviour, ILevelLoadEvent
{
    [SerializeField]
    private float closeRangeHitDistance = 2.0f;

    [SerializeField]
    private float fleeRangeDistance = 3.0f;

    private GameObject target;

    private bool hasMeleeAttack = false;
    private bool hasRangedAttack = false;

    private bool hasSwitchedMixed = false;

    private EnemyMovement enemyMovement;

    private CompanionGamepadBindings companionGamepadBindings;

    private ICombatant combatant;

    private NavMeshAgent agent;

    private List<GameObject> players = new List<GameObject>();
    private List<GameObject> enemies;

    [SerializeField]
    private AIBehavior behaviour;
    public AIBehavior Behavior { get { return behaviour; } set { } }

    private void Start()
    {
        combatant = GetComponent<ICombatant>();
        enemyMovement = GetComponent<EnemyMovement>();
        agent = GetComponent<NavMeshAgent>();
        companionGamepadBindings = GetComponent<CompanionGamepadBindings>();

        EvaluateHasAttackPattern();
    }

    public void SetupEnemyAIByRoomCombatState(List<GameObject> enemyList, bool isRoomCleared)
    {
        enemies = enemyList;
        GetAllPlayersToList();

        if (gameObject.CompareTag("Player"))
        {
            if (isRoomCleared)
            {
                behaviour = AIBehavior.FOLLOW;
                target = FindObjectOfType<Player>().gameObject;
            }
            else
            {
                behaviour = Game.Instance.captureManagerRef.GetEnemyInCaptureSlot().AIBehavior;
                PickEnemyAsTarget();
            }
        }

        if (gameObject.CompareTag("Enemy"))
        {
            PickPlayerAsTarget();
        }
    }

    private void GetAllPlayersToList()
    {
        players.Clear();
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (thing.GetComponent<ICombatant>() != null)
            {
                players.Add(thing);
            }
        }
    }

    private void PickPlayerAsTarget()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, players.Count);
        if (players[randomValue] == null) return;

        target = players[randomValue];
    }

    private void PickEnemyAsTarget()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, enemies.Count);
        if (enemies[randomValue] == null) return;

        target = enemies[randomValue];
    }

    private void EvaluateHasAttackPattern()
    {
        if (GetComponent<MeleeAttack>() != null)
        {
            hasMeleeAttack = true;
        }

        if (GetComponent<RangedAttack>() != null)
        { 
            hasRangedAttack = true;
        }
    }

    public void EvaluateNextMove()
    {
        if (companionGamepadBindings.PairedGamepad != null) return;

        switch (behaviour)
        {
            case AIBehavior.NONE:
                break;
            case AIBehavior.AGGRESSIVE:
                MoveTowardsTarget();
                AggressiveAction();
                break;
            case AIBehavior.DEFENSIVE:
                DefensiveAction();
                AggressiveAction();
                break;
            case AIBehavior.MIXED:
                if (hasSwitchedMixed)
                {
                    MoveTowardsTarget();
                    hasSwitchedMixed = !hasSwitchedMixed;
                }
                else
                {
                    DefensiveAction();
                    hasSwitchedMixed = !hasSwitchedMixed;
                }
                AggressiveAction();
                break;
            case AIBehavior.FOLLOW:
                MoveTowardsTarget();
                break;
        }

        if (target == null)
        {
            FindObjectOfType<Room>(false).SetRoomCombatStateInEnemies();
        }
    }

    private void AggressiveAction()
    {
        if (hasMeleeAttack)
        {
            MeleeAttackAction();
        }
        if (hasRangedAttack)
        {
            RangedAttackAction();
        }
    }

    private void MeleeAttackAction()
    {
        if (agent.remainingDistance <= closeRangeHitDistance)
        {
            if (target == null) return;
            
            combatant.MeleeAttack(target.transform.position);
        }
    }

    private void RangedAttackAction()
    {
        if (target == null) return;

        int random = Random.Range(0, 3);
        if (random == 1) return;

        combatant.RangedAttack(target.transform.position);
    }

    private void DefensiveAction()
    {
        if (target == null) return;

        MoveTowardsTarget();

        if (agent.remainingDistance <= fleeRangeDistance)
        {
            FleeFromTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (companionGamepadBindings.PairedGamepad != null) return;
        if (target == null) return;

        transform.LookAt(target.transform.position);
        enemyMovement.MovementAction(target.transform.position);
    }

    private void FleeFromTarget()
    {
        if (target == null) return;

        Vector3 flee = Vector3.MoveTowards(transform.position, target.transform.position, -5.0f);
        enemyMovement.MovementAction(flee);
    }

    public void SetTargetToNull()
    {
        target = null;
    }

    #region ILevelLoadEvent

    void ILevelLoadEvent.OnLevelLoadEvent()
    {
        
    }

    void ILevelLoadEvent.OnLevelUnloadEvent()
    {
        
    }

    void ILevelLoadEvent.OnPlayerWasInstanced()
    {

    }

    #endregion ILevelLoadEvent

}
