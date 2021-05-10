using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamageOnce : EffectDamage
{
    //[SerializeField] float[] DamageableTime = new float[2];
    //float deltaTime;

    //private void FixedUpdate()
    //{
    //    deltaTime += Time.fixedDeltaTime;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (enemyLayer == other.gameObject.layer)
        {
            CharacterKit.FDamageMessage msg;
            msg.Causer = this.gameObject;
            msg.amount = damage;
            other.gameObject.GetComponent<CharacterKit.Damageable>().GetDamage(msg);
        }
    }
}
