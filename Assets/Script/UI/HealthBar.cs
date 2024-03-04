using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;
    [SerializeField]
    private Image fill;


    public void SetMaxHealthAndMinHealth(int maxHealth, int minHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.minValue = minHealth;
    }

    public void UpdateHealth(int health)
    {
        healthBarSlider.value = health;
    }
}
