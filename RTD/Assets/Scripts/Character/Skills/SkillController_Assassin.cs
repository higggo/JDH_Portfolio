using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_Assassin : SkillController
{
    CharController controller;
    GameObject target;
    [SerializeField] float Damage;

    // Start is called before the first frame update
    void Start()
    {
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    protected override void InitComponents()
    {
        controller = GetComponentInParent<CharController>();

        if (controller != null)
            animEvent = controller.animEvent;

        animEvent.SkillStartDel += SkillOn;
        animEvent.SkillReadyDel += SkillReady;
        animEvent.SkillEndDel += EndUseSkill;
        ChangeState(SKILLSTATE.WAIT);
    }

    protected override void SkillLogic()
    {
        if (target == null || !CharUtils.IsInRange(controller.transform, target.transform, controller.statInfo.attackRange))
        {
            if (!CharUtils.FindTarget(controller.transform, controller.enemyLayer, controller.statInfo.attackRange, out target))
                return;
        }
        CharUtils.RotateToTarget(controller.transform, target.transform);
        GetComponent<ProjectileManager>().FireProjectile(SkillParticleStartPos.position, controller.gameObject, target, Damage, false);
        if (skillSound != null)
            SoundManager.I.PlayEffectSound(gameObject, skillSound);
    }

    public override bool PrepareSkill()
    {
        if (CharUtils.FindTarget(controller.transform, controller.enemyLayer, controller.statInfo.attackRange, out target))
        {
            _readyToShot = true;
            CharUtils.RotateToTarget(controller.transform, target.transform);
        }

        return base.PrepareSkill();
    }
}
