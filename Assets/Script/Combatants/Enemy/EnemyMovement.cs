using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private bool isKnockedBack = false;

    private Enemy enemy;

    private EnemyAI enemyAI;

    private Vector3 hitBy;

    private CapsuleCollider capsuleCollider;

    private NavMeshAgent agent;

    private float knockbackSpeed = 25.0f;

    private float knockbackTimer = 0.0f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float knockbackCooldown = 1.5f;

    private bool isMovementEnabled = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Enemy>().animator;
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void MovementAction(Vector3 target)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetPet")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fire")) return;

        if (!isMovementEnabled) return;

        animator.SetBool("isWalking", true);
        agent.SetDestination(target);
    }

    public void MovementAction(Vector3 target, bool isAnimationPlaying)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetPet")) return;

        if (!isMovementEnabled) return;

        animator.SetBool("isWalking", isAnimationPlaying);
        agent.SetDestination(target += gameObject.transform.position);

    }

    public void AttackAction(Vector3 aim)
    {
        if (!isMovementEnabled) return;

        enemy = GetComponent<Enemy>();
        enemy.MeleeAttack(aim);
    }

    public void RangedAttackAction(Vector3 aim)
    {
        if (!isMovementEnabled) return;

        enemy = GetComponent<Enemy>();
        enemy.RangedAttack(aim);
    }

   

    public void KnockbackAction(Transform hitByTransform)
    {
        animator.SetTrigger("Hit1");
        isKnockedBack = true;
        capsuleCollider.enabled = false;
        hitBy = hitByTransform.position;
        transform.LookAt(hitBy);
        knockbackTimer = Time.time + knockbackCooldown;
    }

    public void OnKnockbackEnd()
    {
        isKnockedBack = false;
        capsuleCollider.enabled = true;
    }

    private void Update()
    {
        if (isKnockedBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, hitBy, -knockbackSpeed * Time.deltaTime);

            if (Time.time >= knockbackTimer) 
            {
                OnKnockbackEnd();
            }
        }

        if (agent.remainingDistance < 2.0f)
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        isMovementEnabled = enabled;
        if (!isMovementEnabled) 
        {
            agent.SetDestination(transform.position);

        }
        enemyAI.EvaluateNextMove();
    }
}
