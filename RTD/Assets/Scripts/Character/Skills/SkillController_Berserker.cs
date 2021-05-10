using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_Berserker : SkillController
{
    CharController controller;
    GameObject target;

    [Space(40)]
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

    protected override void SkillLogic()
    {
        BuffSkill Buff = new BuffSkill(BUFFCATEGORY.RATIO, id, buffAtkDamageRatio, buffAtkSpeedRatio, 0.0f, buffDuration);
        controller.statInfo.AddBuff(Buff);

        if (skillSound != null)
            SoundManager.I.PlayEffectSound(gameObject, skillSound, 0.8f, 0.5f);
    }

    public override bool PrepareSkill()
    {
        _readyToShot = true;
        return base.PrepareSkill();
    }
}
