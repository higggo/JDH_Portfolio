using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_BowKnight : SkillController
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
        if (target == null)
            return;

        CharUtils.RotateToTarget(controller.transform, target.transform);
        GetComponent<ProjectileManager>().FireProjectile(SkillParticleStartPos.position, controller.gameObject, target, Damage, false);
        if (skillSound != null)
            SoundManager.I.PlayEffectSound(gameObject, skillSound, 0.8f, 0.75f);
    }

    public override bool PrepareSkill()
    {
        if (CharUtils.FindTarget(controller.transform, controller.enemyLayer, controller.statInfo.attackRange, out target))
        {
            _readyToShot = true;
            StartCoroutine(StartRotateToTarget());
        }

        return base.PrepareSkill();
    }

    IEnumerator StartRotateToTarget()
    {
        while (canUseSkill)
        {
            if (target == null)
            {
                if (!CharUtils.FindTarget(controller.transform, controller.enemyLayer, controller.statInfo.attackRange, out target))
                {
                    yield return null;
                    continue;
                }
            }
            else
            {
                Quaternion targetRot = CharUtils.GetRotationToTarget(controller.transform, target.transform);
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRot, Time.deltaTime * 10.0f);
            }
            
            yield return null;
        }
    }
}
