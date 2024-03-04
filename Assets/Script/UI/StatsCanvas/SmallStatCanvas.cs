using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SmallStatCanvas : MonoBehaviour
{
    [SerializeField]
    private SmallStatValues smallValues;

    private float windowOpacity = 1.0f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetAlphaValue(bool visible)
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

    public void SetSmallStatCounterValues(int hp, float m_Atk, float m_Def, float r_Atk, float r_Def)
    {
        smallValues.SetSmallStatCounter(hp, m_Atk, m_Def, r_Atk, r_Def);
    }

    public void UpdateKillCounter(int kills)
    {
        smallValues.SetKillCounter(kills);
    }

    public void UpdateSmallMaxHP(int hp)
    {
        smallValues.UpdateHp(hp);

    }
}
