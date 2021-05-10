using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class DragonBasicAttack : MonoBehaviour
{
    Animator animator;
    AnimEvent_Dragon animEvent;
    CharacterStat stat;
    DragonController controller;

    [SerializeField] Transform bulletStartPos;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animEvent = GetComponentInChildren<AnimEvent_Dragon>();
        stat = GetComponent<CharacterStat>();
        controller = GetComponent<DragonController>();

        animEvent.AttackInAirDel += OnAttackByFireBall;
        animEvent.AttackDel += OnAttackByBasicAttack;
    }

    void OnAttackByFireBall()
    {
        if (controller == null || controller.target == null)
        {
            Debug.LogWarning("controller is null ref");
            return;
        }
            

        ProjectileManager manager = GetComponent<ProjectileManager>();
        manager.FireProjectile(bulletStartPos.position, controller.gameObject, controller.target, controller.fireBallDamage);
    }

    void OnAttackByBasicAttack()
    {
        if (controller == null || controller.target == null)
        {
            Debug.LogWarning("controller is null ref");
            return;
        }

        var targetDamageComp = controller.target.GetComponent<Damageable>();
        if (targetDamageComp == null)
            return;

        FDamageMessage msg;
        msg.Causer = gameObject;
        msg.amount = stat.attackDamage;

        targetDamageComp.GetDamage(msg);
    }
}
