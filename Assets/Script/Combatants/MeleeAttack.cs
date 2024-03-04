using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class MeleeAttack : MonoBehaviour
{
    GameObject meleeAttackHitBox;

    [SerializeField]
    private Animator animator;

    private float nextDamageEvent = 0.0f;
    private float attackDelay = 1.0f;


    private const float baseAttackDelay = 1.0f;

    private void Awake()
    {
        meleeAttackHitBox = GetComponentInChildren<HitBox>().gameObject;
    }

    public bool Attack(int attackSpeedInPercent, int attackDamage)
    {
        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time;
            nextDamageEvent += attackDelay;
            AttackCoolDown(attackSpeedInPercent);

            animator.SetFloat("attackSpeed", attackSpeedInPercent / 100.0f);

            meleeAttackHitBox.GetComponent<HitBox>().Attack(attackDamage);

            return true;
        }
        return false;
    }

    private void AttackCoolDown(int attackSpeedInPercent)
    {
        float temp;
        temp = (10.0f * attackSpeedInPercent) / 1000.0f;
        attackDelay = baseAttackDelay;

        if(temp < 1.0f)
        {
            attackDelay += (1.0f - temp);

        }
        else
        {
            attackDelay -= (temp - 1.0f);
        }
    }
}
