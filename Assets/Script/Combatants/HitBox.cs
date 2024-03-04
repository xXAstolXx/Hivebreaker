using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HitBox : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private SphereCollider sphereCollider;

    private int attackDamage = 0;

    private List<SFXPlayer> playerSFX = new List<SFXPlayer>();

    public DamageType damageType = DamageType.MELEE;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        sphereCollider = GetComponent<SphereCollider>();

        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            playerSFX.Add(audioPlayer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != gameObject.tag)
        {
            var combatant = other.GetComponent<ICombatant>();
            var destructible = other.GetComponent<DestructibleItem>();
            if (combatant != null)
            {
                combatant.HitByAttack(attackDamage, gameObject, damageType);
                PlaySFXByTriggerKey("ATTACK_HIT");
            }
            else
            {
                PlaySFXByTriggerKey("ATTACK_MISSED");
            }
            if (destructible != null)
            {
                destructible.OnDestructibleItemHit();
            }
        }
        
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playerSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    public void Attack(int damage)
    {
        SetAttackDamage(damage);
        playableDirector.Play();
    }

    private void SetAttackDamage(int value)
    {
        attackDamage = value;
    }

    public void OnAttackStart()
    {
        sphereCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        sphereCollider.enabled = false;
    }
}
