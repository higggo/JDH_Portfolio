using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_DarkWizard : SkillController
{
    CharController controller;
    GameObject target;

    [Space(40)]
    [SerializeField] GameObject PoisonEffect;
    [SerializeField] float PoisonLifeTime;
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
        var obj = Instantiate(PoisonEffect, target.transform.position, controller.transform.rotation);
        obj.GetComponent<EffectDamage>().Init(target.layer, Damage, PoisonLifeTime);

        if (skillSound != null)
            SoundManager.I.PlayEffectSound(obj, skillSound);
    }

    public override bool PrepareSkill()
    {
        if (CharUtils.FindTarget(controller.transform, controller.enemyLayer, controller.statInfo.attackRange, out target))
        {
            _readyToShot = true;
        }

        return base.PrepareSkill();
    }
}
