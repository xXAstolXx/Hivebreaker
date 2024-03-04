using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedEnemy
{
    private EnemyType m_type;
    public EnemyType EnemyType { get { return m_type; } }

    private AIBehavior m_behavior;
    public AIBehavior AIBehavior { get { return m_behavior; } }

    private Stats m_stats;
    public Stats Stats { get { return m_stats; } }

    public CapturedEnemy(EnemyType type, AIBehavior behavior, Stats stats) 
    { 
        m_type = type;
        m_behavior = behavior;
        m_stats = stats;
    }

    public void OverwriteStats(Stats stats)
    {
        m_stats = stats;
    }
}
