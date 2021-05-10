using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 해당 클래스를 상속받아 스킬 로직 스크립트를 작성하시면 됩니다.
/// </summary>
public class SkillController : MonoBehaviour
{
    public enum SKILLCLASS
    {
        UNKNOWN = -1, ATTACK, BUFF, DEBUFF
    }

    public enum SKILLSTATE
    {
        CREATE, WAIT, STARTCOOLDOWN, STOPCOOLDOWN, PREPARE, USESKILL
    }

    // Enum
    public SKILLCLASS skillClass = SKILLCLASS.UNKNOWN;
    public SKILLSTATE skillState = SKILLSTATE.CREATE;

    // Delegate
    public UnityAction OnCoolTimeChangeDel;

    // Components
    [SerializeField] protected AnimEvent animEvent;
    [SerializeField] protected GameObject SkillParticle;
    [SerializeField] protected GameObject SkillReadyParticle;
    
    // Particle Flag
    [SerializeField] protected bool _attachToParent = true;

    [Space(10)]

    // Transform
    [SerializeField] protected Transform SkillParticleStartPos;
    [SerializeField] protected Transform SkillReadyParticleStartPos;

    // cool Time
    [SerializeField] protected float _coolTime;
    protected float _remainCoolTime;

    // Flags
    protected bool _startCoolDown = false;
    protected bool _canUseSkill = false;
    protected bool _readyToShot = false;

    // Id
    protected int id;

    // Sound
    public AudioClip skillSound;

    // property
    public float coolTime
    {
        get { return _coolTime; }
    }

    public float remainCoolTime
    {
        get { return _remainCoolTime; }
    }

    public bool startCoolDown
    {
        get { return _startCoolDown; }
        set { _startCoolDown = value; }
    }

    public bool canUseSkill
    {
        get { return _canUseSkill; }
    }

    public bool readyToShot
    {
        get { return _readyToShot; }
    }

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

    protected virtual void InitComponents()
    {
        animEvent = GetComponentInChildren<AnimEvent>();
        animEvent.SkillStartDel += SkillOn;
        animEvent.SkillReadyDel += SkillReady;
        animEvent.SkillEndDel += EndUseSkill;
        ChangeState(SKILLSTATE.WAIT);
    }

    public void ResetRemainCoolTime()
    {
        _remainCoolTime = _coolTime;
    }

    public void ResetAll()
    {
        ResetRemainCoolTime();
        _canUseSkill = false;
        _startCoolDown = false;
        _readyToShot = false;
    }

    public virtual void EndUseSkill()
    {
        // 스킬 애님 끝에 꼭 붙여주세요.
        if (!_canUseSkill)
            return;

        _canUseSkill = false;
    }

    public virtual bool PrepareSkill()
    {
        return _readyToShot;
    }

    protected virtual void SkillOn()
    {
        // 애니메이션 델리게이트에 붙일 함수
        // 여기서 파티클 Instantiate및 Trasnform 조정하고 해당 로직 작성하시면 됩니다.
        if (SkillParticle != null)
        {
            if (_attachToParent)
                Instantiate(SkillParticle, SkillParticleStartPos);
            else
                Instantiate(SkillParticle, SkillParticleStartPos.position, SkillParticleStartPos.rotation);
        }
        SkillLogic();
        _readyToShot = false;
    }

    protected virtual void SkillLogic()
    {
        // SkillOn()에서 사용될 로직입니다.
        // 데미지를 주거나 버프를 주는 함수를 여기서 작성
        return;
    }


    protected virtual void SkillReady()
    {
        // 애니메이션 델리게이트에 붙일 함수
        // 스킬 준비과정 파티클이 필요할때 호출.
        if (SkillReadyParticle != null)
        {
            Instantiate(SkillReadyParticle, SkillReadyParticleStartPos);
        }

    }

    protected virtual void ChangeState(SKILLSTATE state)
    {
        if (skillState == state)
            return;

        skillState = state;

        switch (skillState)
        {
            case SKILLSTATE.WAIT:
                ResetRemainCoolTime();
                OnCoolTimeChangeDel?.Invoke();
                break;
            case SKILLSTATE.STARTCOOLDOWN:
                ResetRemainCoolTime();
                OnCoolTimeChangeDel?.Invoke();
                break;
            case SKILLSTATE.STOPCOOLDOWN:
                // To Do SomeThing...
                break;
            case SKILLSTATE.USESKILL:
                _canUseSkill = true;
                break;
        }
    }

    protected virtual void StateProcess()
    {
        switch (skillState)
        {
            case SKILLSTATE.WAIT:
                if (_startCoolDown)
                    ChangeState(SKILLSTATE.STARTCOOLDOWN);
                break;
            case SKILLSTATE.STARTCOOLDOWN:
                if (!_startCoolDown)
                    ChangeState(SKILLSTATE.STOPCOOLDOWN);

                _remainCoolTime -= Time.deltaTime;
                OnCoolTimeChangeDel?.Invoke();

                if (_remainCoolTime < Mathf.Epsilon)
                    ChangeState(SKILLSTATE.USESKILL);
                break;
            case SKILLSTATE.STOPCOOLDOWN:
                ChangeState(SKILLSTATE.WAIT);
                break;
            case SKILLSTATE.USESKILL:
                if (!_canUseSkill)
                    ChangeState(SKILLSTATE.STARTCOOLDOWN);
                break;
        }
    }



}

// Skill Manager에 있어야 하는것.
/*
 * 1) 파티클?
 * 2) 시작 transform
 * 3) 델리게이트에 붙여줄 함수
 * 4) 
 */
