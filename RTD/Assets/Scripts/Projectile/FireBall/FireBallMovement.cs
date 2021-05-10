using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileKit;

public class FireBallMovement : ProjectileMovement
{
    // Start is called before the first frame update
    void Start()
    {
        InitController();
    }

    protected override void SetMovement()
    {
        base.SetMovement();
        StartCoroutine(Move(target));
    }

    protected override void SetRotate(Vector3 target)
    {
        base.SetRotate(target);
    }

    IEnumerator Move(Transform target)
    {
        Vector3 ClonePos = target.position;
        Vector3 dir = transform.forward;
        dir.Normalize();
        SetRotate(ClonePos);
        float distance = Vector3.Distance(ClonePos, transform.position);
        float delta = Time.deltaTime * bulletSpeed;

        while (distance > 0.1f)
        {
            SetRotate(ClonePos);
            dir = transform.forward;
            if (delta > distance)
                delta = distance;

            distance -= delta;
            transform.Translate(dir * delta, Space.World);

            if (target != null)
            {
                ClonePos = target.position;
                distance = Vector3.Distance(ClonePos, transform.position);
            }
            yield return null;
        }
        endMove = true;
        SetHit();
    }

    void SetHit()
    {
        // Stop Movement or Destroy
        controller.SetHit();
    }
}
