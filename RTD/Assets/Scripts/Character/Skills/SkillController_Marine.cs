using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

[RequireComponent(typeof(CharController))]
public class SkillController_Marine : SkillController
{
    [SerializeField] float skillDamage;
    [SerializeField] float skillRange;
    [SerializeField] float collisionRadius;
    [SerializeField] LayerMask mask;
    
    CharController controller;
    GameObject skillTarget = null;


    // Start is called before the first frame update
    void Start()
    {
        InitComponents();
        controller = GetComponent<CharController>();
        mask = controller.enemyLayer;
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    protected sealed override void SkillOn()
    {
        StopAllCoroutines();
        base.SkillOn();
    }

    protected sealed override void SkillLogic()
    {
        StartCoroutine(CheckCollision(1.0f, 0.1f, 100.0f));
    }

    protected sealed override void SkillReady()
    {
        // Play Particle ReadyParticle

        base.SkillReady();
    }

    public sealed override bool PrepareSkill()
    {
        if (CharUtils.FindTarget(transform, mask, skillRange, out skillTarget))
        {
            _readyToShot = true;
            StartCoroutine(StartRotateToTarget(skillTarget));
        }
        else
        {
            if (controller.isInBossRoom)
            {
                if (CharUtils.FindTarget(transform, mask, controller.detectRange, out skillTarget))
                {
                    Vector3 movePos = skillTarget.transform.position;
                    movePos.y = transform.position.y;
                    Vector3 dir = movePos - transform.position;
                    dir.Normalize();
                    dir *= -1.0f;
                    movePos += dir * skillRange;
                    controller.CharacterMove(movePos);
                }
                    
            }
        }
        
        return base.PrepareSkill();
    }

    /// <summary>
    ///     - Time동안 CheckTime의 사이로 skillDamage를 입힙니다.
    /// </summary>
    /// <param name="time">
    ///         - 코루틴 지속 시간
    /// </param>
    /// <param name="checkTime">
    ///         - 코루틴 검사 시간
    /// </param>
    /// <param name="maxDist">
    ///         - 마지막 도달 거리
    /// </param>
    /// <returns></returns>
    IEnumerator CheckCollision(float time, float checkTime, float maxDist)
    {
        if (time < Mathf.Epsilon)
            time = 1.0f;

        if (checkTime > time)
            checkTime = 0.1f;

        float curdist;
        Vector3 CapsuleEndPos = transform.position + Vector3.up * GetComponent<CapsuleCollider>().height * transform.lossyScale.y;
        FDamageMessage msg = new FDamageMessage();
        msg.Causer = gameObject;
        msg.amount = skillDamage;

        while (time > Mathf.Epsilon)
        {
            time -= checkTime;
            curdist = maxDist - time * maxDist;
            RaycastHit[] hitInfo = Physics.CapsuleCastAll(transform.position, CapsuleEndPos, collisionRadius, SkillParticleStartPos.forward, curdist, mask);
            foreach (RaycastHit hit in hitInfo)
            {
                hit.transform.gameObject.GetComponent<Damageable>().GetDamage(msg);
            }
            yield return new WaitForSeconds(checkTime);
        }
    }

    IEnumerator StartRotateToTarget(GameObject target)
    {
        Vector3 ClonePos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        while (canUseSkill)
        {
            if (target == null)
                CharUtils.FindTarget(controller, mask, skillRange, out target);
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

    private void OnDrawGizmosSelected()
    {
        DebugExtension.DrawCylinder(transform.position, transform.position + Vector3.up * GetComponent<CapsuleCollider>().height * transform.lossyScale.y, Color.blue, collisionRadius);
    }
}
