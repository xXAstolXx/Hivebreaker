using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatIcon : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private Sprite max_HP_Icon;
    [SerializeField]
    private Sprite melee_ATK_Icon; 
    [SerializeField]
    private Sprite ranged_ATK_Icon;
    [SerializeField]
    private Sprite melee_DEF_Icon;
    [SerializeField]
    private Sprite ranged_DEF_Icon;
    [SerializeField]
    private Sprite movement_SPD_Icon;
    [SerializeField]
    private Sprite melee_ATK_SPD_Icon;
    [SerializeField]
    private Sprite ranged_ATK_SPD_Icon;
    [SerializeField]
    private Sprite projectile_SPD;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetIcon(string statName)
    {
        switch (statName)
        {
            case ("Max_Hp"):
                image.sprite = max_HP_Icon;
                break;
            case ("Attack"):
                image.sprite = melee_ATK_Icon;
                break;
            case ("Fire_Power"):
                image.sprite = ranged_ATK_Icon;
                break;
            case ("Defense_Melee"):
                image.sprite = melee_DEF_Icon;
                break;
            case ("Defense_Ranged"):
                image.sprite = ranged_DEF_Icon;
                break;
            case ("Move_Speed"):
                image.sprite = movement_SPD_Icon;
                break;
            case ("Attack_Speed"):
                image.sprite = melee_ATK_SPD_Icon;
                break;
            case ("Fire_Rate"):
                image.sprite = ranged_ATK_SPD_Icon;
                break;
            case ("Bullet_Speed"):
                image.sprite = projectile_SPD;
                break;
        }
    }
}
