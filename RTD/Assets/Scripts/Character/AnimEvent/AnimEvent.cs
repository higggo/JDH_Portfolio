using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityAction AttackDel;
    public UnityAction AttackEndDel;
    public UnityAction BasicAttackEffectDel;
    public UnityAction DeadDel;

    // Skills
    public UnityAction SkillReadyDel;
    public UnityAction SkillStartDel;
    public UnityAction SkillEndDel;

    public virtual void OnAttack()
    {
        AttackDel?.Invoke();
        BasicAttackEffectDel?.Invoke();
    }

    public virtual void OnAttackEnd()
    {
        AttackEndDel?.Invoke();
    }

    public virtual void OnDead()
    {
        DeadDel?.Invoke();
    }

    public virtual void OnSkillReady()
    {
        SkillReadyDel?.Invoke();
    }

    public virtual void OnSkillFire()
    {
        SkillStartDel?.Invoke();
    }

    public virtual void OnSkillEnd()
    {
        SkillEndDel?.Invoke();
    }
}
