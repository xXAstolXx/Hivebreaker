using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField]
    private SFXPlayer[] playersSFX;

    [SerializeField]
    private int healAmount = 50;

    [SerializeField]
    private bool isPercentage = false;

    private ICombatant player;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if(player != null)
        {
            if (isPercentage)
            {
                healAmount = (player.Stats.m_hp / 100) * healAmount;
            }
            
            player.HealHP(healAmount);
            DestroyGameObject();
        }
    }

    private void DestroyGameObject()
    {
        PlaySFXByTriggerKey("PICK");
        Destroy(gameObject);
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }
}
