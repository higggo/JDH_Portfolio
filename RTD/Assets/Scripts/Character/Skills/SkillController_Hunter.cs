using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_Hunter : SkillController
{
    CharController controller;

    [Space(40)]
    [SerializeField] int buffCount = 1;
    [SerializeField] float buffDuration;
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
        SkillLogic();
        _readyToShot = false;
    }

    protected override void SkillLogic()
    {
        if (skillSound != null)
            SoundManager.I.PlayEffectSound(gameObject, skillSound, 0.8f, 0.35f);

        List<GameObject> Alies = new List<GameObject>();
        Alies = CharUtils.GetInFieldAllCharacters(controller.gameObject);

        if (buffCount >= Alies.Count)
        {
            foreach (GameObject obj in Alies)
            {
                BuffSkill Buff = new BuffSkill(BUFFCATEGORY.RATIO, id, 0.0f, buffAtkSpeedRatio, 0.0f, buffDuration);
                obj.GetComponent<CharacterStat>()?.AddBuff(Buff);
                if (SkillParticle != null)
                {
                    Vector3 EffectPos = obj.transform.position;
                    EffectPos.y += 0.5f;
                    Instantiate(SkillParticle, EffectPos, Quaternion.identity);
                }
            }
            return;
        }

        List<int> idxes = new List<int>();
        while (idxes.Count < buffCount)
        {
            int added = Random.Range(0, Alies.Count);
            if (!idxes.Contains(added))
                idxes.Add(added);
        }

        foreach (int index in idxes)
        {
            BuffSkill Buff = new BuffSkill(BUFFCATEGORY.RATIO, id, 0.0f, buffAtkSpeedRatio, 0.0f, buffDuration);
            Alies[index].GetComponent<CharacterStat>()?.AddBuff(Buff);
            if (SkillParticle != null)
            {
                Vector3 EffectPos = Alies[index].transform.position;
                EffectPos.y += 0.5f;
                Instantiate(SkillParticle, EffectPos, Quaternion.identity);
            }
        }
    }

    public override bool PrepareSkill()
    {
        _readyToShot = true;

        return base.PrepareSkill();
    }
}
