using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class BulletDamage : MonoBehaviour
{
    public GameObject hitEffect;
    protected ProjectileController controller;
    [SerializeField] protected Transform HitEffectPos;
    [SerializeField] protected AudioClip HitSound;
    public float hitSoundVolume = 0.0f;

    void Start()
    {
        InitComponent();
    }

    protected virtual void InitComponent()
    {
        controller = GetComponent<ProjectileController>();
        controller.HitDel += SetDamage;
    }

    protected virtual void SetDamage()
    {
        Debug.Log("SetDamage");

        GameObject target = controller.target;
        if (target == null)
            return;

        FDamageMessage msg = new FDamageMessage();
        msg.Causer = (controller.owner != null) ? controller.owner : this.gameObject;
        msg.amount = controller.bulletDmg;

        target.GetComponent<Damageable>()?.GetDamage(msg);
        PlayHitEffect();

        if (HitSound != null)
            SoundManager.I.PlayEffectSound(target, HitSound, 0.5f, hitSoundVolume);
    }

    protected virtual void PlayHitEffect()
    {
        if (hitEffect == null)
            return;

        GameObject effectObj;
        if (HitEffectPos == null)
            effectObj = Instantiate(hitEffect, transform.position, transform.rotation);
        else
            effectObj = Instantiate(hitEffect, HitEffectPos.position, HitEffectPos.rotation);

        if (effectObj.GetComponent<EffectDamage>() != null)
            effectObj.GetComponent<EffectDamage>().Init(controller.target.layer, controller.bulletDmg);
    }
}
