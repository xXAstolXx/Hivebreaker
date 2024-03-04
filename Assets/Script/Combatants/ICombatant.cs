using UnityEngine;



    public interface ICombatant
    {
        public void HitByAttack(int damage, GameObject hitBy, DamageType damageType);

        public void TakeDamage(int damage, DamageType damageType);

        public void MeleeAttack(Vector3 position);

        public void RangedAttack(Vector3 position);

        public void HealHP(int amount);

        public void SetStartStats(Stats stats);

        public void SetStartStats(int maxHP, int hp, int meleeAttack,
            int rangedAttack, int meleeDefense, int rangedDefense, float moveSpeed,
            int meleeAttackSpeed, int rangedAttackSpeed, float bulletSpeed);

        public void CombatantDeath();

        public void Knockback(Transform hitBy);
    }

