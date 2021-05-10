using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage_Lich : BulletDamage
{
    [SerializeField] float DebuffFieldDuration;

    protected override void PlayHitEffect()
    {
        if (hitEffect == null)
            return;

        GameObject effectObj;
        if (controller.target != null)
            HitEffectPos = controller.target.transform;

        if (HitEffectPos == null)
            effectObj = Instantiate(hitEffect, transform.position, transform.rotation);
        else
            effectObj = Instantiate(hitEffect, HitEffectPos.position, HitEffectPos.rotation);

        if (effectObj.GetComponent<EffectDamage_Lich>() != null)
            effectObj.GetComponent<EffectDamage_Lich>().Init(controller.target.layer, controller.bulletDmg, DebuffFieldDuration);
    }
}
