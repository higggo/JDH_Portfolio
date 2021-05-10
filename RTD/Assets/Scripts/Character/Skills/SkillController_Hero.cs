using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_Hero : SkillController
{
    CharController controller;

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

    protected override void SkillReady()
    {
        // 애니메이션 델리게이트에 붙일 함수
        // 스킬 준비과정 파티클이 필요할때 호출.
        if (SkillReadyParticle != null)
        {
            Instantiate(SkillReadyParticle, SkillReadyParticleStartPos.position, Quaternion.identity);
        }

    }


    protected override void SkillLogic()
    {
        if (skillSound != null)
            SoundManager.I.PlayEffectSound(gameObject, skillSound, 0.8f, 0.5f);

        List<GameObject> Alies = new List<GameObject>();
        Alies = CharUtils.GetInFieldAllCharacters(controller.gameObject);

        foreach (GameObject character in Alies)
        {
            if (character.GetComponent<CharacterStat>().union != UNION.WARRIOR)
                continue;

            BuffSkill Buff = new BuffSkill(BUFFCATEGORY.RATIO, id, buffAtkDamageRatio, buffAtkSpeedRatio, 0.0f, buffDuration);
            character.GetComponent<CharacterStat>().AddBuff(Buff);
        }

    }

    public override bool PrepareSkill()
    {
        _readyToShot = true;

        return base.PrepareSkill();
    }

}
