
using Unity.VisualScripting;

using UnityEngine;

public class SmallStatValues : MonoBehaviour
{
    [SerializeField]
    private StatNumber statNumber1;
    [SerializeField]
    private StatNumber statNumber2;
    [SerializeField]
    private StatNumber statNumber3;
    [SerializeField]
    private StatNumber statNumber4;
    [SerializeField]
    private StatNumber statNumber5;
    [SerializeField]
    private StatNumber statNumber6;

    public void SetSmallStatCounter(int hp, float m_AtK, float m_Def, float r_Atk, float r_Def)
    {
        statNumber1.WriteStatValue(hp.ToString());
        statNumber2.WriteStatValue(m_AtK.ToShortString());
        statNumber3.WriteStatValue(m_Def.ToShortString());
        statNumber5.WriteStatValue(r_Atk.ToShortString());
        statNumber6.WriteStatValue(r_Def.ToShortString());
    }

    public void SetKillCounter(int kills)
    {
        statNumber4.WriteStatValue(kills.ToString());
    }

    public void UpdateHp(int hp)
    {
        statNumber1.WriteStatValue(hp.ToString());
    }
}
