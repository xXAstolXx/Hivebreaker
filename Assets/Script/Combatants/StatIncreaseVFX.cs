using UnityEngine;
using UnityEngine.VFX;

public class StatIncreaseVFX : MonoBehaviour
{

    [SerializeField]
    private VisualEffect health;

    [SerializeField]
    private VisualEffect mAtk;

    [SerializeField]
    private VisualEffect mDef;

    [SerializeField]
    private VisualEffect mov;

    [SerializeField] 
    private VisualEffect mSpd;

    [SerializeField] 
    private VisualEffect pSpd;
    
    [SerializeField]
    private VisualEffect rAtk;

    [SerializeField]
    private VisualEffect rDef;

    [SerializeField]
    private VisualEffect rSpd;

    public void PlayStatVFX(string statName)
    {
        switch (statName) 
        {
            case ("Max_Hp"):
                health.Play();
                break;
            case ("Attack"):
                mAtk.Play();
                break;
            case ("Fire_Power"):
                rAtk.Play();
                break;
            case ("Defense_Melee"):
                mDef.Play();
                break;
            case ("Defense_Ranged"):
                rDef.Play();
                break;
            case ("Move_Speed"):
                mov.Play();
                break;
            case ("Attack_Speed"):
                mSpd.Play();
                break;
            case ("Fire_Rate"):
                rSpd.Play();
                break;
            case ("Bullet_Speed"):
                pSpd.Play();
                break;
        }
    }
}
