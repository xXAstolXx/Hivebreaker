using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class RadialHealthSlider : MonoBehaviour
{
    private Slider slider;

    private ParentConstraint constraint;

    private void Awake()
    {
        constraint = GetComponent<ParentConstraint>();
        slider = GetComponentInChildren<Slider>(); 
    }

    private void Start()
    {
        SetupConstraint();
    }

    private void SetupConstraint()
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = Game.Instance.transform;
        source.weight = 1.0f;
        constraint.AddSource(source);
        constraint.SetRotationOffset(0,new Vector3(90,0,-45));
    }

    public void SetupHealthSlider(int maxHP, int minValue)
    {

        slider.minValue = minValue;
        slider.maxValue = maxHP;
    }

    public void SetHealthToSlider(int hp)
    {
        slider.value = hp;
    }
}
