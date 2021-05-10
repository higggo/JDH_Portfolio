using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_SwordMaster : SkillController
{
    CharController controller;
    GameObject target;

    [Space(40)]
    [SerializeField] float skillDamage;
    [SerializeField] float buffDuration;
    [SerializeField] float buffAtkDamageRatio;
    [SerializeField] float buffAtkSpeedRatio;

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
        id = (int)controller.statInfo.id;
        ChangeState(SKILLSTATE.WAIT);
    }

    protected override void SkillOn()
    {
        // 애니메이션 델리게이트에 붙일 함수
        // 여기서 파티클 Instantiate및 Trasnform 조정하고 해당 로직 작성하시면 됩니다.
        if (SkillParticle != null)
        {
            if (target == null)
                CharUtils.FindTarget(controller.transform, controller.enemyLayer, controller.statInfo.attackRange, out target);

            GameObject obj = null;
            obj = Instantiate(SkillParticle, SkillParticleStartPos);
            obj.GetComponent<EffectDamageTick>()?.Init(target.layer, skillDamage, buffDuration);
        }
        
        SkillLogic();
        _readyToShot = false;
    }

    protected override void SkillLogic()
    {
        BuffSkill Buff = new BuffSkill(BUFFCATEGORY.RATIO, id, buffAtkDamageRatio, buffAtkSpeedRatio, 0.0f, buffDuration);
        controller.statInfo.AddBuff(Buff);
        if (skillSound != null)
            SoundManager.I.PlayEffectSound(gameObject, skillSound);
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
