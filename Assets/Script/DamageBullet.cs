using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageBullet : BulletBase
{
    [HideInInspector]
    public int damage;

    public DamageType damageType = DamageType.RANGED;

    protected override bool HasCollided(Collider other)
    {
        return base.HasCollided(other);
    }

    protected override void BulletHit(Collider other)
    {
        var combatant = other.GetComponent<ICombatant>();

        if (combatant == null) return;
        if (other.tag == gameObject.tag) return;

        combatant.HitByAttack(damage, gameObject, damageType);
        DestroyBullet();
    }
}
        
