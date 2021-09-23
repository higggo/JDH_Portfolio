using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileMovement : MonoBehaviour
{
    Vector3 attackerPos;
    Vector3 targetPos;
    float damage;
    public UnityAction hit;
    public void Shoot(GameObject attacker, GameObject target, float damage)
    {
        attackerPos = attacker.transform.position;
        targetPos = target.transform.position;
        this.damage = damage;
        SetHit(attacker, target, damage);
        Vector3 lookPos = targetPos - attackerPos;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1.0f);
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        float speed = 50.0f;
        Vector3 dir = (targetPos - attackerPos).normalized;
        float distance = (targetPos - attackerPos).magnitude;
        float deltaDist;
        Vector3 pos;

        while (distance > 0.0f)
        {
            pos = transform.position;
            deltaDist = speed * Time.deltaTime;
            if (distance - deltaDist < 0.0f)
            {
                deltaDist = distance;
            }
            distance -= deltaDist;
            pos += dir * deltaDist;
            transform.position = pos;
            yield return null;
        }
        hit?.Invoke();

        Destroy(gameObject);
    }

    public virtual void SetHit(GameObject attacker, GameObject target, float damage)
    {
        hit = () => { attacker.GetComponent<Character>().ProjectileHit(target, this.damage); };
    }
}
