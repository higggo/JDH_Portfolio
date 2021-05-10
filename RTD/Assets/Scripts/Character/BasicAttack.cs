using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class BasicAttack : MonoBehaviour
{
    protected CharacterStat statInfo;
    public Transform EffectStartPos;
    [SerializeField] protected GameObject AttackEffect;
    [SerializeField] protected AudioClip attackClip;
    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        statInfo = GetComponent<CharacterStat>();

        if (AttackEffect != null)
            GetComponentInChildren<AnimEvent>().BasicAttackEffectDel += OnEffectStart;
    }

    // 이 함수를 override해서 다른 캐릭터의 기본공격 로직을 바꾸세요.
    public virtual void OnAttack(GameObject Target)
    {
        if (Target == null)
            return;

        // 데미지 전달.
        var targetDamageComp = Target.GetComponent<Damageable>();
        if (targetDamageComp == null)
            return;

        FDamageMessage msg = new FDamageMessage();
        msg.Causer = this.gameObject;
        msg.amount = this.statInfo.attackDamage;
        targetDamageComp.GetDamage(msg);
    }

    public virtual void OnEffectStart()
    {
        // Modify Effect Rotation.        
        var obj = Instantiate<GameObject>(AttackEffect, EffectStartPos);
        obj.transform.Rotate(AttackEffect.transform.rotation.eulerAngles);

        // Set Destroy Time
        float DestroyDelay = obj.GetComponentInChildren<ParticleSystem>().main.duration;
        Destroy(obj, DestroyDelay);
    }
}
