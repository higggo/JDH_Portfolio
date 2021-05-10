using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class DragonController : BossController
{
    // BossController.SetCanAction(bool)으로 행동 제어 가능
    public enum STATE
    {
        NONE, CREATE, POSTCREATE,
        WAIT, MOVE, CHANGEPHASE,
        DETECT, ATTACK,
        READYSKILL, USESKILL,
        DEAD
    }

    public enum PHASE
    {
        PHASE01, PHASE02
    }

    // 추가 정보
    [SerializeField] int Level;
    [SerializeField] float _fireBallDamage;
    [SerializeField] float detectRange;
    [SerializeField] float flyAttackSpeed;

    AnimatorStateInfo info;
    
    // STATE MACHINE
    STATE dragonState = STATE.NONE;
    PHASE dragonPhase = PHASE.PHASE01;

    // flags
    bool isInAir = false;
    bool isAttackNow = false;

    // Coroutine
    Coroutine MoveSet = null;

    // Instantiate될 때 position
    Vector3 StartPosition;
    Transform HealthBar;

    // property
    public float fireBallDamage
    {
        get 
        { 
            if (_fireBallDamage <= 0.0f)
                _fireBallDamage = 1.0f;
            
            return _fireBallDamage;
        }
    }

    public float DetectRange
    {
        get { return detectRange; }
    }


    // 타겟을 발견하면 1/4 확률로 타겟을 변경?
    // 타겟을 발견하면 기본공격이 우선
    // 스킬 쿨타임이 다 차면, 스킬 사용. (Ground일때, Sky일때 다름)

    [ContextMenu("_canAction true")]
    void SetAction()
    {
        _canAction = true;
    }

    [ContextMenu("_canAction false")]
    void SetActionfalse()
    {
        _canAction = false;
    }

    public void ChangeDetectState()
    {
        ChangeState(STATE.DETECT);
    }


    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.CREATE);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }


    protected override void InitComponents()
    {
        base.InitComponents();
        // 추가
        StartPosition = transform.position;
        HealthBar = GetComponentInChildren<HPBar>()?.transform;
        GetComponent<Damageable>().onDeadDel += () => { ChangeState(STATE.DEAD); };
        AnimEvent_Dragon tmpEvent;
        if (bossAnimEvent is AnimEvent_Dragon)
        {
            tmpEvent = bossAnimEvent as AnimEvent_Dragon;
            tmpEvent.AttackDel += OnAttack;
            tmpEvent.AttackInAirDel += OnAttack;
            tmpEvent.DeadDel += OnDead;
        }
    }

    void ChangeState(STATE state)
    {
        if (dragonState == state)
            return;

        dragonState = state;
        switch (dragonState)
        {
            case STATE.CREATE:
                InitComponents();
                ChangeState(STATE.POSTCREATE);
                break;
            case STATE.POSTCREATE:
                statInfo.UpdateStat(Level);
                ChangeState(STATE.WAIT);
                break;
            case STATE.WAIT:
                ResetDragon();
                break;
            case STATE.CHANGEPHASE:
                StartCoroutine(ChangePhase());
                break;
            case STATE.MOVE:
                bossAnimator.SetBool("B_Move", true);
                if (MoveSet != null)
                    StopCoroutine(MoveSet);

                if (dragonPhase == PHASE.PHASE01)
                {
                    Vector3 movepos = _target.transform.position;
                    movepos.y = transform.position.y;
                    MoveSet = StartCoroutine(Move(movepos));
                }
                else if (dragonPhase == PHASE.PHASE02)
                {
                    MoveSet = StartCoroutine(Move(StartPosition));
                }
                
                break;
            case STATE.DETECT:
                break;
            case STATE.ATTACK:
                bossAnimator.SetTrigger("T_Basic Attack");
                break;
            case STATE.READYSKILL:
                break;
            case STATE.USESKILL:
                bossAnimator.SetTrigger("T_Skill");
                break;
            case STATE.DEAD:
                bossAnimator.SetTrigger("T_Dead");
                StopAllCoroutines();
                _canAction = false;
                break;
        }
    }

    void StateProcess()
    {
        // phase one

        if (dragonPhase == PHASE.PHASE01)
        {
            switch (dragonState)
            {
                case STATE.CREATE:
                    break;
                case STATE.POSTCREATE:
                    break;
                case STATE.WAIT:
                    if (canAction)
                        ChangeState(STATE.DETECT);

                    // TEST ChangePhase02
                    if (statInfo.HP < statInfo.MaxHP * 0.5f
                        && Level >= 3)
                    {
                        ChangeState(STATE.CHANGEPHASE);
                        break;
                    }
                    break;
                case STATE.DETECT:
                    if (!canAction)
                    {
                        ChangeState(STATE.WAIT);
                        break;
                    }

                    // ChangePhase02 ( Flag : HP, Level )
                    if (statInfo.HP < statInfo.MaxHP * 0.5f
                        && Level >= 3)
                    {
                        ChangeState(STATE.CHANGEPHASE);
                        break;
                    }

                    // use to skill
                    if (_skillController != null
                        && Level >= 2)
                    {
                        if (_skillController.canUseSkill)
                        {
                            ChangeState(STATE.READYSKILL);
                            break;
                        }
                    }

                    // Find AttackTarget
                    if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                    {
                        if (!CharUtils.IsInRange(this.transform, _target.transform, detectRange))
                            _target = null;
                    }
                    else
                    {
                        _target = null;
                        if (!CharUtils.FindTarget(this.transform, enemyLayer, detectRange, out _target))
                        {
                            ChangeState(STATE.WAIT);
                        }
                    }

                    // Check Target In AttackRange
                    if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                    {
                        if (!CharUtils.IsInRange(this.transform, _target.transform, statInfo.attackRange, enemyLayer))
                            ChangeState(STATE.MOVE);
                        else
                            ChangeState(STATE.ATTACK);
                    }
                    break;
                case STATE.MOVE:
                    if (CharUtils.IsInRange(transform, _target.transform, statInfo.attackRange, enemyLayer)
                        || _target == null
                        || _target.GetComponent<Damageable>().IsDead)
                    {
                        bossAnimator.SetBool("B_Move", false);
                        StopCoroutine(MoveSet);
                        ChangeState(STATE.DETECT);
                    }
                    break;
                case STATE.ATTACK:
                    if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                        RotateToTarget(_target.transform.position);
                    break;
                case STATE.READYSKILL:
                    if (_skillController.PrepareSkill())
                        ChangeState(STATE.USESKILL);
                    break;
                case STATE.USESKILL:
                    if (!_skillController.canUseSkill)
                        ChangeState(STATE.DETECT);
                    break;
                case STATE.DEAD:
                    break;
            }
        }
        else
        {
            switch(dragonState)
            {
                case STATE.WAIT:
                    if (canAction)
                        ChangeState(STATE.DETECT);
                    break;
                case STATE.DETECT:
                    if (!canAction)
                    {
                        ChangeState(STATE.WAIT);
                        break;
                    }

                    // use to skill
                    if (_skillController != null)
                    {
                        if (_skillController.canUseSkill)
                        {
                            ChangeState(STATE.READYSKILL);
                            break;
                        }
                    }

                    // Find AttackTarget
                    if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                    {
                        if (!CharUtils.IsInRange(this.transform, _target.transform, detectRange))
                            _target = null;
                    }
                    else
                    {
                        _target = null;
                        if (!CharUtils.FindTarget(this.transform, enemyLayer, detectRange, out _target))
                        {
                            ChangeState(STATE.WAIT);
                        }
                    }

                    // Check Target In AttackRange
                    if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                    {
                        ChangeState(STATE.ATTACK);
                    }
                    break;
                case STATE.MOVE:
                    if (Vector3.Distance(transform.position, StartPosition) < 0.1f)
                        ChangeState(STATE.DETECT);
                    break;
                case STATE.ATTACK:
                    if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                        RotateToTarget(_target.transform.position);
                    break;
                case STATE.READYSKILL:
                    if (_skillController.PrepareSkill())
                        ChangeState(STATE.USESKILL);
                    break;
                case STATE.USESKILL:
                    if (!_skillController.canUseSkill)
                        ChangeState(STATE.DETECT);
                    break;
                case STATE.DEAD:
                    break;
            }
          
        }
    }

    void RotateToTarget(Vector3 targetPos)
    {
        targetPos.y = transform.position.y;
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();
        Quaternion targetRot = Quaternion.LookRotation(dir);
        if (targetRot == transform.rotation)
            return;
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
    }


    void ResetDragon()
    {
        skillController.ResetAll();
        _attackDelay = 0.0f;
        _target = null;
        StopAllCoroutines();
    }

    void UpdateAnimInfo()
    {
        info = bossAnimator.GetCurrentAnimatorStateInfo(0);
    }


    void OnDead()
    {
        Destroy(gameObject, destroyDelay);
    }


    void OnAttack()
    {
        
        if (_target == null)
            return;

        Debug.LogWarning("OnAttack : Dragon");
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        if (!isInAir)
            _attackDelay = 1.0f / statInfo.attackSpeed;
        else
            _attackDelay = 1.0f / flyAttackSpeed;

        while (_attackDelay > Mathf.Epsilon)
        {
            _attackDelay -= Time.deltaTime;
            yield return null;
        }
        _attackDelay = 0.0f;
        ChangeState(STATE.DETECT);
    }

    IEnumerator ChangePhase()
    {
        bossAnimator.SetBool("B_IsInAir", true);
        while (!info.IsName("Dragon Fly Idle(W)") && !isDead)
        {
            UpdateAnimInfo();
            yield return null;
        }

        if (!isDead)
        {
            isInAir = true;
            ResetDragon();
            dragonPhase = PHASE.PHASE02;
            ChangeState(STATE.MOVE);
        }
    }

    IEnumerator Move(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.Normalize();
        float deltaSpeed = statInfo.moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(position, transform.position);
        while (distance > Mathf.Epsilon)
        {
            RotateToTarget(position);
            if (distance < deltaSpeed)            
                deltaSpeed = distance;

            transform.position += deltaSpeed * dir;
            distance -= deltaSpeed;
            yield return null;
        }
    }
}
