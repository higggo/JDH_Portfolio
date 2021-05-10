using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class test1 : MonoBehaviour
{
    float dist = 20.0f;
    bool direction = true;
    float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Damageable>().onDeadDel += Dead;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 360.0f * Time.deltaTime, 0.0f);

        if (direction)
        {
            transform.Translate(Time.deltaTime * speed, 0, 0, Space.World);
        }
        else
            transform.Translate(Time.deltaTime * -speed, 0, 0, Space.World);

        dist -= Time.deltaTime * speed;

        if (dist < Mathf.Epsilon)
        {
            dist = 20.0f;
            direction = !direction;
        }
    }

    void Dead()
    {
        float destroyTime = 3.0f;
        Destroy(gameObject, destroyTime);
        enabled = false;
    }
}
