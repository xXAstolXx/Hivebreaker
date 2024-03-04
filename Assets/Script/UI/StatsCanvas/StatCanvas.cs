
using UnityEngine;

public class StatCanvas : MonoBehaviour
{


    [SerializeField]
    private StatValues values1;
    [SerializeField]
    private StatValues values2;


    private float windowOpacity = 1.0f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public  void SetAlphaValue(bool visible)
    {
        if (visible)
        {
            canvasGroup.alpha = windowOpacity;
        }
        else
        {
            canvasGroup.alpha = 0.0f;

        }

    }

    public bool GetAlphaValue()
    {
        return canvasGroup.alpha > 0.0f;
    }

    public void SetStatCounterValuesForFirstRow(int hp, float m_Atk, float m_Def, int m_Atk_Spd, float m_Spd)
    {
        values1.SetStatCounterOneRow(hp,m_Atk, m_Def, m_Atk_Spd, m_Spd);
    }

    public void SetStatCounterValuesForSecondRow(float r_Atk, float r_Def, int r_Atk_Spd, float b_Spd)
    {
        values2.SetStatCounterTwoRow(r_Atk, r_Def, r_Atk_Spd, b_Spd);
    }

    public void UpdateKillCounter(int kills)
    {
        values2.SetKillCounter(kills);
    }

    public void UpdateBigMaxHp(int hp)
    {
        values1.UpdateExtendedHP(hp);
    }

}
