using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Playables;

public class StaticDamageArea : MonoBehaviour
{

    // Später damage over Time in Update verlagern

    [SerializeField]
    private int damage = 2;

    private PlayableDirector playableDirector;

    private List<ICombatant> combatants = new List<ICombatant>();

    public DamageType damageType = DamageType.SPECIAL;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetOtherCombatant(other) != null)
        { 
            combatants.Add(GetOtherCombatant(other));
        }

        if(combatants.Count > 0)
        {
            playableDirector.Play();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (GetOtherCombatant(other) != null)
        {
            combatants.Remove(GetOtherCombatant(other));
        }

        if(combatants.Count == 0)
        {
            playableDirector.Stop();
        }
    }

    public void DealDamageOverTime()
    {
        foreach (var combatant in combatants)
        {
            combatant.TakeDamage(damage, damageType);
        }
    }

    private ICombatant GetOtherCombatant(Collider other)
    {
        return  other.GetComponent<ICombatant>();
    }
}
