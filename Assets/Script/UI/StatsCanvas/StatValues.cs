
using Unity.VisualScripting;

using UnityEngine;

public class StatValues : MonoBehaviour
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

    public void SetStatCounterOneRow(int hp, float m_Atk, float m_Def, int m_Atk_Spd, float m_Spd)
    {
        statNumber1.WriteStatValue(hp.ToString());
        statNumber2.WriteStatValue(m_Atk.ToShortString());
        statNumber3.WriteStatValue(m_Def.ToShortString());
        statNumber4.WriteStatValue(m_Atk_Spd.ToShortString());
        statNumber5.WriteStatValue(m_Spd.ToShortString());

    }

    public void SetStatCounterTwoRow(float r_Atk, float r_Def, int r_Atk_Spd, float b_Spd)
    {
        statNumber2.WriteStatValue(r_Atk.ToShortString());
        statNumber3.WriteStatValue(r_Def.ToShortString());
        statNumber4.WriteStatValue(r_Atk_Spd.ToString());
        statNumber5.WriteStatValue(b_Spd.ToShortString());
    }

    public void SetKillCounter(int kills)
    {
        statNumber1.WriteStatValue(kills.ToString());
    }

    public void UpdateExtendedHP(int hp)
    {
        statNumber1.WriteStatValue(hp.ToString());
    }
    
}
