using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectileKit;

namespace ProjectileKit
{
    public enum STATE
    {
        CREATE, READY, WAIT, SHOOT, ENDMOVE
    }
}

// 타겟팅형 발사체 스크립트
public class ProjectileController : MonoBehaviour
{
    protected STATE _bulletState = STATE.CREATE;
    protected float _bulletDmg;
    protected float _bulletSpeed = 0.0f;
    [SerializeField] float DestroyDelay; 

    protected GameObject _owner = null;
    protected GameObject _target = null;
    protected Transform _hitPoint = null;
    public UnityAction moveDel = null;
    public UnityAction HitDel = null;

    int layerMask;

    // Targeting?
    protected bool isTargeting = true;

    // Trigger
    public bool fireTrigger = false;

    // property
    public float bulletDmg
    {
        get
        {
            if (_bulletDmg < Mathf.Epsilon)
                _bulletDmg = 1;

            return _bulletDmg;
        }
        set
        {
            if (value < Mathf.Epsilon)
                value = 1;

            _bulletDmg = value;
        }
    }
    
    public float bulletSpeed
    {
        get
        {
            if (_bulletSpeed < Mathf.Epsilon)
                _bulletSpeed = 1;

            return _bulletSpeed;
        }
        set
        {
            if (value < Mathf.Epsilon)
                value = 1;

            _bulletSpeed = value;
        }
    }

    public GameObject owner
    {
        get { return _owner; }
    }

    public GameObject target
    {
        get { return _target; }
    }

    public Transform targetHitPoint
    {
        get { return _hitPoint; }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// public Func

    /// <summary>
    /// 이 함수를 총알 생성 직후에 바로 call 해주면 됩니다.
    /// </summary>
    /// <param name="target">공격 대상</param>
    /// <param name="bulletDmg">공격력</param>
    /// <param name="bulletSpeed">총알 속도(Time.deltaTime * bulletSpeed)</param>
    public void InitBullet(GameObject target, float bulletDmg, float bulletSpeed)
    {
        Debug.Log("InitBullet");
        if (!(_bulletState == STATE.CREATE || _bulletState == STATE.WAIT))
            return;

        if (target == null)
            Debug.Log("Target is Null!");

        _target = target;
        this.bulletDmg = bulletDmg;
        this.bulletSpeed = bulletSpeed;
        this.layerMask = target.layer;
        ChangeState(STATE.READY);
    }

    public void InitBullet(GameObject owner, GameObject target, float bulletDmg, float bulletSpeed)
    {
        _owner = owner;
        InitBullet(target, bulletDmg, bulletSpeed);
    }

    public void InitBullet(GameObject owner, GameObject target, float bulletDmg, float bulletSpeed, bool isTargeting = true)
    {
        _owner = owner;
        this.isTargeting = isTargeting;
        InitBullet(target, bulletDmg, bulletSpeed);
    }

    /// <summary>
    /// bullet의 state를 강제로 HIT로 바꿉니다.
    /// </summary>
    public void SetHit()
    {
        if (_bulletState != STATE.SHOOT)
            return;

        Debug.Log("SetHIT");
        ChangeState(STATE.ENDMOVE);
    }



    // Private Func
    void Awake()
    {
        _bulletState = STATE.CREATE;
    }

    void Update()
    {
        StateProcess();
    }

    // Colision Set
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌");

        if (isTargeting)
        {
            if (other.gameObject == target)
            {
                HitDel?.Invoke();
                ChangeState(STATE.ENDMOVE);
            }    
        }
        else
        {
            if (other.gameObject.layer == layerMask)
            {
                _target = other.gameObject;
                HitDel?.Invoke();
            }
        }
        
    }

    // STATE MACHINE
    void ChangeState(STATE state)
    {
        if (_bulletState == state)
            return;

        _bulletState = state;

        switch (_bulletState)
        {
            case STATE.CREATE:
                break;
            case STATE.READY:
                break;
            case STATE.SHOOT:
                _hitPoint = target.transform;
                if (target.GetComponent<CharacterKit.Damageable>().HitPoint != null)
                {
                    Debug.Log("HitPoint = " + target.name + " : " + target.GetComponent<CharacterKit.Damageable>().HitPoint.name);
                    _hitPoint = target.GetComponent<CharacterKit.Damageable>().HitPoint;
                }
                moveDel?.Invoke();
                break;
            case STATE.ENDMOVE:
                Destroy(this.gameObject, DestroyDelay);
                break;
        }
    }

    void StateProcess()
    {
        switch (_bulletState)
        {
            case STATE.CREATE:
                break;
            case STATE.READY:
                if (fireTrigger)
                    ChangeState(STATE.SHOOT);
                break;
            case STATE.SHOOT:
                break;
            case STATE.ENDMOVE:
                break;
        }
    }
}
