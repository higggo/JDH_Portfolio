using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamage : MonoBehaviour
{
    public LayerMask enemyLayer;
    [SerializeField] protected float damage;
    [SerializeField] protected float EffectTime;

    public virtual void Init(LayerMask mask, float damage, float effectTime = 1.0f)
    {
        enemyLayer = mask;
        this.damage = damage;
        this.EffectTime = effectTime;
        GetComponent<Collider>().isTrigger = true;
    }

}
