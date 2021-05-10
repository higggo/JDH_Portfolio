using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileKit;

public class NonTargetingMovement : ProjectileMovement
{
    float currentDist = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        base.InitController();
    }

    protected override void SetMovement()
    {
        base.SetMovement();
        Vector3 direction = target.position;
        direction.y = transform.position.y;
        direction -= transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(direction);

        StartCoroutine(Move(direction));
    }

    IEnumerator Move(Vector3 dir)
    {
        float delta = bulletSpeed * Time.deltaTime;
        while (currentDist > Mathf.Epsilon)
        {
            transform.Translate(dir * delta, Space.World);
            currentDist -= delta;
            yield return null;
        }
        controller.SetHit();
    }
}
