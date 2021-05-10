using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBasicAttack : BasicAttack
{
    [SerializeField] Transform bulletStartPos;
    // Update is called once per frame
    void Update()
    {

    }

    public override void OnAttack(GameObject target)
    {
        if (target == null)
            return;

        ProjectileManager manager = GetComponent<ProjectileManager>();
        manager.FireProjectile(bulletStartPos.position, gameObject, target, statInfo.attackDamage);

        if (attackClip != null)
            SoundManager.I.PlayEffectSound(gameObject, attackClip);
    }
}
