using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Monster_Core
{
    new void Awake()
    {
        base.Awake();

    }
    new void Start()
    {
        base.Start();
    }
    public override void ApplyDamage(float damage)
    {
    }

    public override void ApplyHit(GameObject target, float damage)
    {
    }

    public override void ApplyHP()
    {
    }
    public override void StartChasing()
    {
    }
    public override void StopChasing()
    {
    }

    public override void ChaseTarget(GameObject target)
    {
    }
}

