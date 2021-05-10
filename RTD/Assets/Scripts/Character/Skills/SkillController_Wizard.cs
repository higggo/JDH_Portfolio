using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class SkillController_Wizard : SkillController
{
    CharController controller;

    [Space(40)]
    [SerializeField] int skillCount;
    [SerializeField] float skillRange;
    [SerializeField] float damage;


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
        List<GameObject> Enemies = new List<GameObject>();
        if (CharUtils.FindTargetAll(controller.transform, controller.enemyLayer, ref Enemies, skillRange))
        {
            if (Enemies.Count <= skillCount)
            {
                foreach (GameObject enemy in Enemies)
                {
                    if (SkillParticle != null)
                    {
                        Vector3 EffectPos = enemy.transform.position;
                        EffectPos.y += 0.5f;
                        GameObject obj = Instantiate(SkillParticle, EffectPos, Quaternion.identity);
                        obj.GetComponent<EffectDamageOnce>()?.Init(enemy.layer, damage);
                        if (skillSound != null)
                            SoundManager.I.PlayEffectSound(obj, skillSound);
                    }
                }
                return;
            }
            else
            {
                List<int> idxes = new List<int>();
                while (idxes.Count < skillCount)
                {
                    int added = Random.Range(0, Enemies.Count);
                    if (!idxes.Contains(added))
                        idxes.Add(added);
                }

                foreach (int index in idxes)
                {
                    if (SkillParticle != null)
                    {
                        Vector3 EffectPos = Enemies[index].transform.position;
                        EffectPos.y += 0.5f;
                        GameObject obj = Instantiate(SkillParticle, EffectPos, Quaternion.identity);
                        obj.GetComponent<EffectDamageOnce>()?.Init(Enemies[index].layer, damage);
                        if (skillSound != null)
                            SoundManager.I.PlayEffectSound(obj, skillSound);
                    }
                }
            }
        }
    }

    public override bool PrepareSkill()
    {
        GameObject obj = null;
        if (CharUtils.FindTarget(controller.transform, controller.enemyLayer, skillRange, out obj))
        {
            CharUtils.RotateToTarget(controller.transform, obj.transform);
            _readyToShot = true;
        }
        
        return base.PrepareSkill();
    }
}
