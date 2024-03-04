using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField]
    private Transform rangedAttackLauncher;

    [SerializeField]
    private Animator animator;

    private float nextDamageEvent = 0.0f;
    private float attackDelay = 1.0f;

    private const float baseAttackDelay = 1.0f;

    public bool FireBullet(int attackSpeedInPercent, int damage, float bulletSpeed, DamageBullet bullet)
    {
        Transform levelAsParent = FindObjectOfType<RoomManager>().transform;

        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time;
            nextDamageEvent += attackDelay;
            AttackCoolDown(attackSpeedInPercent);

            SetupBullet(bulletSpeed, damage, bullet);

            DamageBullet firedBullet = Instantiate(bullet, rangedAttackLauncher.position, rangedAttackLauncher.rotation, levelAsParent);
            firedBullet.tag = gameObject.tag;

            return true;
        }
        return false;
    }

    public bool FireBullet(int attackSpeedInPercent, float bulletSpeed, CaptureBullet captureBullet)
    {
        Transform levelAsParent = FindObjectOfType<RoomManager>().transform;

        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time;
            nextDamageEvent += attackDelay;
            AttackCoolDown(attackSpeedInPercent);

            SetupBullet(bulletSpeed, captureBullet);

            CaptureBullet firedBullet = Instantiate(captureBullet, rangedAttackLauncher.position, rangedAttackLauncher.rotation, levelAsParent);
            firedBullet.tag = gameObject.tag;

            return true;
        }
        return false;
    }

    private void SetupBullet(float bulletSpeed, int damage, DamageBullet bullet)
    {
        bullet.tag = gameObject.tag;
        bullet.bulletSpeed = bulletSpeed;
        bullet.damage = damage;
        Vector3 directionVector = Vector3.zero;
        directionVector.x = rangedAttackLauncher.transform.forward.x;
        directionVector.z = rangedAttackLauncher.transform.forward.z;
        bullet.direction = directionVector;
    }

    private void SetupBullet(float bulletSpeed, CaptureBullet captureBullet)
    {
        captureBullet.tag = gameObject.tag;
        captureBullet.bulletSpeed = bulletSpeed;
        Vector3 directionVector = Vector3.zero;
        directionVector.x = rangedAttackLauncher.transform.forward.x;
        directionVector.z = rangedAttackLauncher.transform.forward.z;
        captureBullet.direction = directionVector;
    }

    private void AttackCoolDown(int attackSpeedInPercent)
    {
        float temp;
        temp = (10.0f * attackSpeedInPercent) / 1000.0f;
        attackDelay = baseAttackDelay;

        if (temp < 1.0f)
        {
            attackDelay += (1.0f - temp);
        }
        else
        {
            attackDelay -= (temp - 1.0f);
        }
    }
}