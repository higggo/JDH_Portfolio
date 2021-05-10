using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;
using UnityEngine.AI;

public class CharController : MonoBehaviour
{
    // Basic Components
    Rigidbody CharacterRigidbody;
    Animator CharacterAnimator;
    NavMeshAgent navAgent;

    // Script Components
    AnimEvent CharacterAnimEvent;
    CharacterStat _statInfo;
    SkillController _skillController;

    // LayerMask
    public LayerMask enemyLayer;
    
    // StateMachine
    public BASICSTATE characterState = BASICSTATE.NONE;

    // flags
    [SerializeField, Tooltip("켜주면 공격합니다.")]
    bool _isInField = false;
    [SerializeField, Tooltip("보스방 진입시 true, 아니면 false")]
    bool _isInBossRoom = false;

    // Delay
    float _attackDelay = 0.0f;
    [SerializeField] float destroyDelay = 3.0f;

    // Object
    List<GameObject> Targets;
    GameObject Target = null;

    // Move를 위해 적을 찾기 위한 변수입니다.
    const float DetectRange = 200.0f;
    public Vector3 movePosition;

    // Property
    public AnimEvent animEvent
    {
        get
        {
            if (CharacterAnimEvent == null)
                CharacterAnimEvent = GetComponentInChildren<AnimEvent>();

            return CharacterAnimEvent;
        }
    }

    public CharacterStat statInfo
    {
        get 
        {
            if (_statInfo == null)
            {
                if (GetComponent<CharacterStat>() != null)
                {
                    _statInfo = GetComponent<CharacterStat>();
                    return _statInfo;
                }
                else
                    return null;
            }
            return _statInfo;
        }
    }

    public SkillController skillController
    {
        get 
        { 
            if (_skillController == null)
            {
                if (GetComponent<SkillController>() != null)
                {
                    _skillController = GetComponent<SkillController>();
                    return _skillController;
                }
                else
                    return null;
            }
                

            return _skillController;
        }
    }

    public bool isInField
    {
        get { return _isInField; }
        // jdh : Map 이동시 Set
        set { _isInField = value; }
    }

    public bool isInBossRoom
    {
        get { return _isInBossRoom; }
        // 보스룸 이동시 true Set
        // 보스룸 나오면 false Set
        set { _isInBossRoom = value; }
    }

    public float detectRange
    {
        get { return DetectRange; }
    }

    // Test
    [ContextMenu("CoolTime")]
    public void SetCoolTime()
    {
        if (!_isInField)
            return;

        CharUtils.SetSkillCoolDownTrigger(true);
    }


    // Basic Functions
    void Start()
    {
        ChangeState(BASICSTATE.CREATE);
    }

    void Update()
    {
        StateProcess();
    }



    // public Function
    [ContextMenu("Attatch To Field")]
    public void AttatchField()
    {
        if (characterState != BASICSTATE.WAIT)
            return;

        _isInField = true;
    }

    [ContextMenu("Detach from Field")]
    public void DetachField()
    {
        if (characterState != BASICSTATE.DETECT
            && characterState != BASICSTATE.ATTACK)
            return;

        _isInField = false;
    }

    public void OnDead()
    {
        ChangeState(BASICSTATE.DEAD);
        CharacterAnimator.SetTrigger("T_Dead");
    }

    public void CharacterMove(Vector3 movePos)
    {
        if (!_isInField)
            return;

        movePosition = movePos;
        navAgent.enabled = true;
        ChangeState(BASICSTATE.MOVE);
    }

    // private Functions
    void ChangeState(BASICSTATE state)
    {
        if (characterState == state)
            return;

        characterState = state;
        switch (characterState)
        {
            case BASICSTATE.CREATE:
                Debug.Log("CREATE");
                InitComponents();
                break;
            case BASICSTATE.POSTCREATE:
                Debug.Log("POSTCREATE");
                break;
            case BASICSTATE.WAIT:
                //_isInField = false;
                Debug.Log("WAIT");
                break;
            case BASICSTATE.ATTACHFIELD:
                // To Do SomeThing
                ChangeState(BASICSTATE.DETECT);
                Debug.Log("ATTATCH");
                break;
            case BASICSTATE.DETACHFIELD:
                StopAllCoroutines();
                ResetComp();
                ChangeState(BASICSTATE.WAIT);
                Debug.Log("DETACH");
                break;
            case BASICSTATE.DETECT:
                Debug.Log("DETECT");
                break;
            case BASICSTATE.MOVE:
                CharacterAnimator.SetBool("B_Move", true);
                navAgent.SetDestination(movePosition);
                if (navAgent.isStopped)
                    navAgent.isStopped = false;
                break;
            case BASICSTATE.ATTACK:
                CharacterAnimator.SetTrigger("T_Attack");
                break;
            case BASICSTATE.READYSKILL:
                Debug.Log("READY To SKILL");
                break;
            case BASICSTATE.USESKILL:
                CharacterAnimator.SetTrigger("T_Skill01");
                break;
            case BASICSTATE.DEAD:
                navAgent.enabled = false;
                Destroy(this.gameObject, destroyDelay);
                break;
        }
    }

    void StateProcess()
    {
        switch (characterState)
        {
            case BASICSTATE.CREATE:
                ChangeState(BASICSTATE.POSTCREATE);
                break;
            case BASICSTATE.POSTCREATE:
                ChangeState(BASICSTATE.WAIT);
                break;
            case BASICSTATE.WAIT:
                if (_isInField)
                    ChangeState(BASICSTATE.ATTACHFIELD);
                break;
            case BASICSTATE.DETECT:
                if (!isInField)
                {
                    ChangeState(BASICSTATE.DETACHFIELD);
                    return;
                }    

                // Find Attack Target
                if (Target != null)
                {
                    if (!CharUtils.IsInRange(this.transform, Target.transform, statInfo.attackRange, enemyLayer)
                        || Target.GetComponent<Damageable>().IsDead)
                        Target = null;
                }
                else
                {
                    if (CharUtils.FindTargetAll(this, enemyLayer, ref Targets))
                        Target = CharUtils.GetCloseTarget(this, Targets);
                    //else
                        //CharacterAnimator.SetBool("B_NowAttack", false);
                }

                // skill 사용이 가능하면 READYSKILL로 변경
                if (_skillController != null)
                {
                    if (_skillController.canUseSkill)
                    {
                        ChangeState(BASICSTATE.READYSKILL);
                        break;
                    }
                }

                // 보스방에 있으면, Move할 수 있도록함.
                if (_isInBossRoom)
                {
                    if (Target == null)
                    {
                        GameObject obj = null;
                        if (CharUtils.FindTarget(transform, enemyLayer, DetectRange, out obj))
                        {
                            if (obj != null)
                            {
                                Debug.Log(obj.name);
                                Vector3 pos = obj.transform.position;
                                pos.y = transform.position.y;
                                CharacterMove(pos);
                                break;
                            }
                        }
                    }
                }

                if (Target != null && _attackDelay < Mathf.Epsilon)
                    ChangeState(BASICSTATE.ATTACK);
                break;
            case BASICSTATE.MOVE:
                if (CharUtils.AnyObjInRange(transform, statInfo.attackRange, enemyLayer) || navAgent.remainingDistance < 0.1f)
                {
                    CharacterAnimator.SetBool("B_Move", false);
                    navAgent.isStopped = true;
                    ChangeState(BASICSTATE.DETECT);
                }
                break;
            case BASICSTATE.ATTACK:
                if (Target == null)
                {
                    ChangeState(BASICSTATE.DETECT);
                }
                else
                {
                    // Rotate to Target (CharacterKit에 하나 만들자)
                    Vector3 TargetPos = Target.transform.position;
                    TargetPos.y = transform.position.y;
                    Vector3 dir = TargetPos - transform.position;
                    dir.Normalize();
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
                }
                break;
            case BASICSTATE.READYSKILL:
                // break Point =====> 스킬 쿨다운이 멈췄거나, 필드에서 나갔을 때
                if (!_skillController.startCoolDown || !isInField)
                {
                    ChangeState(BASICSTATE.DETECT);
                    break;
                }
                
                if (_skillController.PrepareSkill())
                    ChangeState(BASICSTATE.USESKILL);
                else
                {
                    if (_isInBossRoom)
                    {
                        GameObject obj = null;
                        if (CharUtils.FindTarget(transform, enemyLayer, DetectRange, out obj))
                        {
                            if (obj != null)
                            {
                                Debug.Log(obj.name);
                                Vector3 pos = obj.transform.position;
                                pos.y = transform.position.y;
                                CharacterMove(pos);
                                break;
                            }
                        }
                    }
                }
                break;
            case BASICSTATE.USESKILL:
                if (!_skillController.canUseSkill)
                    ChangeState(BASICSTATE.DETECT);
                break;
            case BASICSTATE.DEAD:
                break;
        }
    }

   


    // @Summary: 공격 애니메이션의 Call Function에서 실행될 델리게이트에 붙여질 함수입니다.
    //           기본적으로는 BasicAttack()의 OnAttack함수를 여기서 호출합니다.
    void OnAttack()
    {
        if (Target == null)
            return;
        
        GetComponent<BasicAttack>().OnAttack(Target);
        StartCoroutine(StartAttack());
    }

    // @Summary: OnAttack이 호출되는 동시에 바로 공격을 할 수 없게 Delay를 시켜주는 코루틴 함수.
    IEnumerator StartAttack()
    {
        // attackDelay = 1초에 몇번 공격할 것인지 따라 다르다.
        _attackDelay = 1.0f / statInfo.attackSpeed;
        yield return new WaitForSeconds(_attackDelay);
        _attackDelay = 0.0f;
        ChangeState(BASICSTATE.DETECT);
    }


    // Init
    void InitComponents()
    {
        // GetStat Script
        _statInfo = GetComponent<CharacterStat>();
        _skillController = GetComponentInChildren<SkillController>();

        // Get Animator
        CharacterAnimator = GetComponentInChildren<Animator>();
        CharacterAnimator.SetFloat("AttackSpeed", statInfo.attackSpeed);

        // Get AnimEvent
        CharacterAnimEvent = GetComponentInChildren<AnimEvent>();
        CharacterAnimEvent.AttackDel += OnAttack;
        
        // Get Basic Components
        CharacterRigidbody = GetComponent<Rigidbody>();
        InitNavAgent();

        // Get Damageable Script
        GetComponent<Damageable>().onDeadDel += OnDead;

        // Set Character Grade And Set Grade Ring 
        statInfo.grade = CharUtils.SetCharacterGrade(statInfo.GetID());
        CharUtils.SettingGradeRing(statInfo.grade, this.transform);

        // Set List of Targets
        Targets = new List<GameObject>();
    }

    void InitNavAgent()
    {
        // Setting Nav Agnet
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.angularSpeed = 200.0f;
        navAgent.acceleration = 60.0f;
        navAgent.speed = statInfo.moveSpeed;
        navAgent.radius = 1.25f;
        navAgent.enabled = false;
    }

    void ResetComp()
    {
        _isInBossRoom = false;
        _attackDelay = 0.0f;
        Target = null;
        Targets.Clear();
        _skillController?.ResetAll();
    }

    public void InBossRoom(bool isInBoss)
    {
        navAgent.enabled = isInBoss;
        isInField = isInBoss;
        isInBossRoom = isInBoss;
    }
}


// BasicAttack.cs를 새로 만들자.
