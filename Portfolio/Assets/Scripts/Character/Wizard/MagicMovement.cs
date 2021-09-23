using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicMovement : ProjectileMovement
{
    public GameObject CollideEffect;
    public GameObject Flash;
    void Start()
    {
        if (Flash != null)
        {
            var flashInstance = Instantiate(Flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }
    public override void SetHit(GameObject attacker, GameObject target, float damage)
    {
        Vector3 normal = target.transform.position.normalized;
        Vector3 point = target.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal);
        Vector3 pos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) + normal * 0f;

        hit = () => {
            attacker.GetComponent<Character>().ProjectileHit(target, damage);
            if (CollideEffect != null)
            {
                var hitInstance = Instantiate(CollideEffect, pos, rot);
                hitInstance.transform.LookAt(point + normal);

                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }
        };
    }
}
