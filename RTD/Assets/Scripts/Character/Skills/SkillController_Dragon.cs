using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;


public class SkillController_Dragon : SkillController
{

    int skillCount = 0;
    [SerializeField] int skillEndCount;
    [SerializeField] float flameColliderRad;
    [SerializeField] float flameDamagePerSeconds;

    GameObject skillTarget;
    DragonController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<DragonController>();
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    protected override void InitComponents()
    {
        base.InitComponents();
        skillEndCount = (skillEndCount <= 0) ? 1 : skillEndCount;
        if (animEvent is AnimEvent_Dragon)
        {
            AnimEvent_Dragon tmpEvent = animEvent as AnimEvent_Dragon;
            tmpEvent.SkillInAirStartDel += SkillOnInAir;
            tmpEvent.SkillInAirEndDel += base.EndUseSkill;
        }
    }

    public override void EndUseSkill()
    {
        if (skillCount < skillEndCount - 1)
        {
            skillCount++;
            controller.ChangeDetectState();
        }
        else
        {
            skillCount = 0;
            _canUseSkill = false;
            ResetRemainCoolTime();
        }
    }


    protected override void SkillOn()
    {
        ProjectileManager manager = GetComponent<ProjectileManager>();
        float fireballDmg = controller.fireBallDamage;
        manager.FireProjectile(SkillParticleStartPos.position, controller.gameObject, skillTarget, fireballDmg);
        _readyToShot = false;
    }

    void SkillOnInAir()
    {
        if (SkillParticle != null)
        {
            GameObject obj = Instantiate(SkillParticle,  SkillParticleStartPos);
            Vector3 scale = obj.transform.localScale;
            scale.x *= transform.localScale.x;
            scale.y *= transform.localScale.y;
            scale.z *= transform.localScale.z;
            obj.transform.localScale = scale;
            float radius = flameColliderRad * scale.x;
            StartCoroutine(FireFlameInAir(flameColliderRad, flameDamagePerSeconds, obj));
            SkillLogic();
        }
    }

    public sealed override bool PrepareSkill()
    {
        if (controller == null)
        {
            Debug.LogWarning("SkillController_Dragon : controller ref is Null!");
            return false;
        }

        if (CharUtils.FindRandomTarget(transform, controller.EnemyLayer, controller.DetectRange, out skillTarget))
        {
            _readyToShot = true;
            StartCoroutine(StartRotateToTarget(skillTarget));
        }

        return base.PrepareSkill();
    }

    IEnumerator FireFlameInAir(float flameRad, float dmgPerSeconds, GameObject flameParticle)
    {
        Vector3 ColliderStartPos = SkillParticleStartPos.position;
        Vector3 dir = SkillParticleStartPos.forward;
        float deltatime = 0.1f;
        while (_canUseSkill)
        {
            if (deltatime >= 0.1f)
            {
                ColliderStartPos = SkillParticleStartPos.position;
                dir = SkillParticleStartPos.forward;
                RaycastHit[] hitInfo = Physics.SphereCastAll(ColliderStartPos, flameRad, dir, 200.0f, controller.EnemyLayer);
                CharacterKit.FDamageMessage msg;
                msg.Causer = this.gameObject;
                msg.amount = dmgPerSeconds * 0.1f;
                Debug.DrawLine(ColliderStartPos, ColliderStartPos + dir * 100.0f, Color.black);
                foreach (RaycastHit hit in hitInfo)
                {
                    hit.collider.gameObject.GetComponent<CharacterKit.Damageable>()?.GetDamage(msg);
                }
                deltatime = 0.0f;
            }
            deltatime += Time.deltaTime;
            yield return null;
        }
        ResetRemainCoolTime();
        Destroy(flameParticle);
    }

    IEnumerator StartRotateToTarget(GameObject target)
    {
        Vector3 ClonePos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        while (canUseSkill)
        {
            if (target == null)
                CharUtils.FindRandomTarget(transform, controller.EnemyLayer, controller.DetectRange, out target);
            if (target != null)
            {
                ClonePos = target.transform.position;
                ClonePos.y = transform.position.y;
                dir = ClonePos - transform.position;
                dir.Normalize();
            }

            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
            yield return null;
        }
    }


    protected override void ChangeState(SKILLSTATE state)
    {
        if (skillState == state)
            return;

        skillState = state;

        switch (skillState)
        {
            case SKILLSTATE.WAIT:
                ResetRemainCoolTime();
                break;
            case SKILLSTATE.STARTCOOLDOWN:
                break;
            case SKILLSTATE.STOPCOOLDOWN:
                // To Do SomeThing...
                break;
            case SKILLSTATE.USESKILL:
                _canUseSkill = true;
                break;
        }
    }

    protected override void StateProcess()
    {
        switch (skillState)
        {
            case SKILLSTATE.WAIT:
                if (controller != null && controller.canAction)
                    _startCoolDown = true;
                break;
            case SKILLSTATE.STARTCOOLDOWN:
                if (controller != null && !controller.canAction)
                    _startCoolDown = false;
                break;
        }
        base.StateProcess();
    }

    private void OnDrawGizmosSelected()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(SkillParticleStartPos.position, SkillParticleStartPos.forward, out hitinfo))
        {
            DebugExtension.DrawCircle(hitinfo.point, Vector3.up, Color.black, flameColliderRad * transform.localScale.x);
        }
    }
}
