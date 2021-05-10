using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
public class BasicAttack_MageKnight : BasicAttack
{
    GameObject Lightning;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnAttack(GameObject Target)
    {
        base.OnAttack(Target);

        if (Target == null)
            return;

        AttackEffect.GetComponent<LightningBoltScript>().StartObject = EffectStartPos.gameObject;
        AttackEffect.GetComponent<LightningBoltScript>().EndObject = Target.GetComponent<CharacterKit.Damageable>().HitPoint.gameObject;
        var obj = Instantiate(AttackEffect);
        SoundManager.I.PlayEffectSound(obj, attackClip);
        if (Lightning != null)
            Destroy(Lightning);
        Lightning = obj;
    }

    public override void Init()
    {
        statInfo = GetComponent<CharacterStat>();

        if (AttackEffect != null)
            GetComponentInChildren<AnimEvent>().AttackEndDel += () => { 
                if (Lightning == null) return;
                Destroy(Lightning);
            };

        GetComponent<CharacterKit.Damageable>().onDeadDel += () => { if (Lightning != null) Destroy(Lightning); };
    }


    IEnumerator EffectDeleteTimer(GameObject EffectObj)
    {
        Animator animator = GetComponentInChildren<Animator>();

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            yield return null;
        }
        Destroy(EffectObj);

    }

}
