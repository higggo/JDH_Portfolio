using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;
using BossKit;

namespace BossKit
{
    public enum BossSTATE
    {
        NONE, CREATE, POSTCREATE,
        WAIT, MOVE,
        DETECT, ATTACK,
        READYSKILL, USESKILL,
        DEAD
    }
}


public class BossController : MonoBehaviour
{
    protected Rigidbody bossRigidbody;
    protected Animator bossAnimator;

    // Script Components
    protected AnimEvent bossAnimEvent;
    protected CharacterStat _statInfo;
    protected SkillController _skillController;

    // LayerMask
    [SerializeField] protected LayerMask enemyLayer;

    // Object
    protected List<GameObject> Targets = new List<GameObject>();
    protected GameObject _target = null;

    // Delay
    protected float _attackDelay = 0.0f;
    [SerializeField] protected float destroyDelay = 3.0f;

    // flag
    protected bool _isDead = false;
    protected bool _canAction = false;

    // StateMachine을 추가 하세요.


    // Property
    public CharacterStat statInfo
    {
        get { return _statInfo; }
    }

    public SkillController skillController
    {
        get
        {
            if (_skillController == null)
                return null;

            return _skillController;
        }
    }


    public LayerMask EnemyLayer
    {
        get { return enemyLayer; }
    }

    public bool isDead
    {
        get { return _isDead; }
    }

    public bool canAction
    {
        get { return _canAction; }
        set { _canAction = value; }
    }

    public GameObject target
    {
        get { return _target; }
    }

    public void SetCanAction(bool val)
    {
        _canAction = val;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitComponents();
    }


    protected virtual void InitComponents()
    {
        bossRigidbody = GetComponent<Rigidbody>();

        // GetStat Script
        _statInfo = GetComponent<CharacterStat>();
        _skillController = GetComponent<SkillController>();

        // Get Animator
        bossAnimator = GetComponentInChildren<Animator>();

        // Get AnimEvent
        bossAnimEvent = GetComponentInChildren<AnimEvent>();
        // Downcast 형식으로 접근하세요.
        //if (bossAnimEvent is AnimEvent_Boss)
        //{
        //    AnimEvent_Boss evt = bossAnimEvent as AnimEvent_Boss;
        //}
        
        // Get Damageable Script
        // 자식에 추가

    }
}

