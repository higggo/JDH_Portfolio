using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public SkillController skillcontroller;
    private void Start()
    {
        slider = GetComponent<Slider>();
        skillcontroller = this.transform.parent.GetComponentInParent<SkillController>();
        Initialize(skillcontroller.coolTime);
        skillcontroller.OnCoolTimeChangeDel += () =>
        {
            SetMax(skillcontroller.coolTime);
            SetValue(skillcontroller.coolTime - skillcontroller.remainCoolTime);
        };
    }


    public void Initialize(float coolTime)
    {
        slider.maxValue = coolTime;
        slider.value = 0;
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
