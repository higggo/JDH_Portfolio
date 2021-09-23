using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Character : Character_Core
{

    public override void ApplyDamage(GameObject target, float damage)
    {
    }
    public override void StartChasing()
    {
    }
    public override void StopChasing()
    {
    }
    public override void SetMyCharacter(string uid)
    {
        if (uid == FAuth.CurrentUser.UserId)
        {
            Units.MyCharacter = gameObject;
            transform.Find("SpringArm").Find("PlayerCamera").gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        }
    }
    public override void ApplyHP()
    {
    }
}
