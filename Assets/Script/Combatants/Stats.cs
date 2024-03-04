using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[Serializable]
public class Stats
{
    public int m_maxHP;
    public int m_hp;

    public int m_meleeAttack;
    public int m_rangedAttack;

    public int m_meleeDefense;
    public int m_rangedDefense;

    public float m_moveSpeed;

    public int m_meleeAttackSpeed;
    public int m_rangedAttackSpeed;

    public float m_bulletSpeed;


    public Stats(int maxHp, int hp, int meleeAttack, int rangedAttack,
        int meleeDefense, int  rangedDefense,
        float moveSpeed, int meleeAttackSpeed, int rangedAttackSpeed, float bulletSpeed) 
    { 
        m_maxHP = maxHp;
        m_hp = hp;
        m_meleeAttack = meleeAttack;
        m_rangedAttack = rangedAttack;
        m_meleeDefense = meleeDefense;
        m_rangedDefense = rangedDefense;
        m_moveSpeed = moveSpeed;
        m_meleeAttackSpeed = meleeAttackSpeed;
        m_rangedAttackSpeed = rangedAttackSpeed;
        m_bulletSpeed = bulletSpeed;
    }

    public Stats()
    {
        m_maxHP = 0;
        m_hp = 0;
        m_meleeAttack = 0;
        m_rangedAttack = 0;
        m_meleeDefense = 0;
        m_rangedDefense = 0;
        m_moveSpeed = 0f;
        m_meleeAttackSpeed = 0;
        m_rangedAttackSpeed = 0;
        m_bulletSpeed = 0;
    }

    public Stats(Stats other)
    {
        m_maxHP = other.m_maxHP;
        m_hp = other.m_hp;
        m_meleeAttack = other.m_meleeAttack;
        m_rangedAttack = other.m_rangedAttack;
        m_meleeDefense = other.m_meleeDefense;
        m_rangedDefense = other.m_rangedDefense;
        m_moveSpeed = other.m_moveSpeed;
        m_meleeAttackSpeed = other.m_meleeAttackSpeed;
        m_rangedAttackSpeed = other.m_rangedAttackSpeed;
        m_bulletSpeed = other.m_bulletSpeed;
    }

    public static Stats operator +(Stats a, Stats b)
    {
        Stats result = new Stats();

        result.m_hp = a.m_hp + b.m_hp;
        result.m_maxHP = a.m_maxHP + b.m_maxHP;
        result.m_meleeAttack = a.m_meleeAttack + b.m_meleeAttack;
        result.m_rangedAttack = a.m_rangedAttack + b.m_rangedAttack;
        result.m_meleeDefense = a.m_meleeDefense + b.m_meleeDefense;
        result.m_rangedDefense = a.m_rangedDefense + b.m_rangedDefense;
        result.m_moveSpeed = a.m_moveSpeed + b.m_moveSpeed;
        result.m_meleeAttackSpeed = a.m_meleeAttackSpeed + b.m_meleeAttackSpeed;
        result.m_rangedAttackSpeed = a.m_rangedAttackSpeed + b.m_rangedAttackSpeed;
        result.m_bulletSpeed = a.m_bulletSpeed + b.m_bulletSpeed;
        
        return result;
    }

    public static Stats operator -(Stats a, Stats b)
    {
        Stats result = new Stats();

        result.m_hp = a.m_hp - b.m_hp;
        result.m_maxHP = a.m_maxHP - b.m_maxHP;
        result.m_meleeAttack = a.m_meleeAttack - b.m_meleeAttack;
        result.m_rangedAttack = a.m_rangedAttack - b.m_rangedAttack;
        result.m_meleeDefense = a.m_meleeDefense - b.m_meleeDefense;
        result.m_rangedDefense = a.m_rangedDefense - b.m_rangedDefense;
        result.m_moveSpeed = a.m_moveSpeed - b.m_moveSpeed;
        result.m_meleeAttackSpeed = a.m_meleeAttackSpeed - b.m_meleeAttackSpeed;
        result.m_rangedAttackSpeed = a.m_rangedAttackSpeed - b.m_rangedAttackSpeed;
        result.m_bulletSpeed = a.m_bulletSpeed - b.m_bulletSpeed;

        return result;
    }

    public static Stats operator *(Stats a, Stats b)
    {
        Stats result = new Stats();

        result.m_hp = a.m_hp * b.m_hp;
        result.m_maxHP = a.m_maxHP * b.m_maxHP;
        result.m_meleeAttack = a.m_meleeAttack * b.m_meleeAttack;
        result.m_rangedAttack = a.m_rangedAttack * b.m_rangedAttack;
        result.m_meleeDefense = a.m_meleeDefense * b.m_meleeDefense;
        result.m_rangedDefense = a.m_rangedDefense * b.m_rangedDefense;
        result.m_moveSpeed = a.m_moveSpeed * b.m_moveSpeed;
        result.m_meleeAttackSpeed = a.m_meleeAttackSpeed * b.m_meleeAttackSpeed;
        result.m_rangedAttackSpeed = a.m_rangedAttackSpeed * b.m_rangedAttackSpeed;
        result.m_bulletSpeed = a.m_bulletSpeed * b.m_bulletSpeed;

        return result;
    }

    public static Stats operator /(Stats a, Stats b)
    {
        Stats result = new Stats();

        result.m_hp = a.m_hp / b.m_hp;
        result.m_maxHP = a.m_maxHP / b.m_maxHP;
        result.m_meleeAttack = a.m_meleeAttack / b.m_meleeAttack;
        result.m_rangedAttack = a.m_rangedAttack / b.m_rangedAttack;
        result.m_meleeDefense = a.m_meleeDefense / b.m_meleeDefense;
        result.m_rangedDefense = a.m_rangedDefense / b.m_rangedDefense;
        result.m_moveSpeed = a.m_moveSpeed / b.m_moveSpeed;
        result.m_meleeAttackSpeed = a.m_meleeAttackSpeed / b.m_meleeAttackSpeed;
        result.m_rangedAttackSpeed = a.m_rangedAttackSpeed / b.m_rangedAttackSpeed;
        result.m_bulletSpeed = a.m_bulletSpeed / b.m_bulletSpeed;

        return result;
    }

    public void IncreaseStat(string statName, float value)
    {
        if (statName == "Max_Hp")
        {
            IncreaseMaxHP((int) value);
            IncreaseHP(m_maxHP - m_hp);
        }

        if (statName == "HP")
        {
            IncreaseHP((int) value);

            m_hp = Mathf.Clamp(m_hp, 0, m_maxHP);
        }

        if (statName == "Attack")
        {
            m_meleeAttack += (int)value;
        }

        if (statName == "Fire_Power")
        {
            m_rangedAttack += (int)value;
        }

        if (statName == "Defense_Melee")
        {
            m_meleeDefense += (int)value;
        }

        if (statName == "Defense_Ranged")
        {
            m_rangedDefense += (int)value;
        }

        if (statName == "Attack_Speed")
        {
            m_meleeAttackSpeed += (int)value;
        }

        if (statName == "Fire_Rate")
        {
            m_rangedAttackSpeed += (int)value;
        }

        if (statName == "Move_Speed")
        {
            m_moveSpeed += value;
        }

        if (statName == "Bullet_Speed")
        {
            m_bulletSpeed += value;
        }
    }

    public void DecreaseStat(string statName, float value)
    {
        if (statName == "Max_Hp")
        {
            DecreaseMaxHP((int)value);

            m_maxHP = Mathf.Clamp(m_maxHP, 0, m_maxHP);
            m_hp = Mathf.Clamp(m_hp, 0, m_maxHP);
        }

        if (statName == "HP")
        {
            DecreaseHP((int)value);

            m_hp = Mathf.Clamp(m_hp, 0, m_maxHP);
        }

        if (statName == "Attack")
        {
            m_meleeAttack -= (int)value;

            m_meleeAttack = Mathf.Clamp(m_meleeAttack, 0, m_meleeAttack);
        }

        if (statName == "Fire_Power")
        {
            m_rangedAttack -= (int)value;

            m_rangedAttack = Mathf.Clamp(m_rangedAttack, 0, m_rangedAttack);
        }

        if (statName == "Defense_Melee")
        {
            m_meleeDefense -= (int)value;

            m_meleeDefense = Mathf.Clamp(m_meleeDefense, 0, m_meleeDefense);
        }

        if (statName == "Defense_Ranged")
        {
            m_rangedDefense -= (int)value;

            m_rangedDefense = Mathf.Clamp(m_rangedDefense, 0, m_rangedDefense);
        }

        if (statName == "Attack_Speed")
        {
            m_meleeAttackSpeed -= (int)value;

            m_meleeAttackSpeed = Mathf.Clamp(m_meleeAttackSpeed, 0, m_meleeAttackSpeed);
        }

        if (statName == "Fire_Rate")
        {
            m_rangedAttackSpeed -= (int)value;

            m_rangedAttackSpeed = Mathf.Clamp(m_rangedAttackSpeed, 0, m_rangedAttackSpeed);
        }

        if (statName == "Move_Speed")
        {
            m_moveSpeed -= value;

            m_moveSpeed = Mathf.Clamp(m_moveSpeed, 0, m_moveSpeed);

        }

        if (statName == "Bullet_Speed")
        {
            m_bulletSpeed += value;

            m_bulletSpeed = Mathf.Clamp(m_bulletSpeed, 0, m_bulletSpeed);
        }
    }

    private void IncreaseMaxHP(int value)
    { m_maxHP += value; }

    private void IncreaseHP(int value)
    { m_hp += value; }

    private void DecreaseMaxHP(int value)
    { m_maxHP -= value; }

    private void DecreaseHP(int value)
    { m_hp -= value; }

    public void SetStatsToZero()
    {
        m_hp = 0;
        m_maxHP = 0;
        m_meleeAttack = 0;
        m_rangedAttack = 0;
        m_moveSpeed = 0;
        m_bulletSpeed = 0;
        m_meleeDefense = 0;
        m_rangedDefense = 0;
        m_meleeAttackSpeed = 0;
        m_rangedAttackSpeed = 0;
    }

    public void PrintStatsToConsole()
    {
        Debug.Log("--- " + this.ToString() + " ---");
        Debug.Log("HP " + m_hp);
        Debug.Log("MAX_HP " + m_maxHP);
        Debug.Log("MELEE_ATK " + m_meleeAttack);
        Debug.Log("RANGED_ATK " + m_rangedAttack);
        Debug.Log("MELEE_DEF " + m_meleeDefense);
        Debug.Log("RANGED_DEF " + m_rangedDefense);
        Debug.Log("MELEE_SPD " + m_meleeAttackSpeed);
        Debug.Log("RANGED_SPD " + m_rangedAttackSpeed);
        Debug.Log("MOVESPEED " + m_moveSpeed);
        Debug.Log("BULLET_SPD " + m_bulletSpeed);
        Debug.Log("-----------------");
    }
}
