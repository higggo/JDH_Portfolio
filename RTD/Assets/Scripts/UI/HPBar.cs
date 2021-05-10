using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public CharacterStat statInfo;
    private void Start()
    {
        slider = GetComponent<Slider>();
        statInfo = GetComponentInParent<CharacterStat>();
        Initialize(statInfo.MaxHP);
        statInfo.OnHPChangeDel += () => 
        {
            SetMax(statInfo.MaxHP);
            SetValue(statInfo.HP);
            fill.color = gradient.Evaluate(slider.normalizedValue);
        };
    }


    public void Initialize(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetMax(float health)
    {
        slider.maxValue = health;
    }

    public void SetValue(float health)
    {
        slider.value = health;
    }
}
