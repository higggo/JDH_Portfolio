using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

namespace CharacterKit
{
    public enum ENEMYSTATE
    { 
        NONE, CREATE, RUN, STUN, GOAL, DEAD
    }
}


public class EnemyController : MonoBehaviour
{
    Rigidbody EnemyRigidbody;
    CharacterStat _statInfo;
    Animator EnemyAnimator;
    AnimEvent EnemyAnimEvent;
    ENEMYSTATE enemyState = ENEMYSTATE.NONE;

    [SerializeField] float destroyDelay = 0.0f;
    [SerializeField] float moveSpeed = 0.0f;
    [SerializeField] float RotateSpeed = 0.0f;
    float defaultMoveSpeed;
    bool isDead = false;
    public bool canMove { get; set; }
    Coroutine StunNow;
    void Awake()
    {
        ChangeState(ENEMYSTATE.CREATE);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void ChangeState(ENEMYSTATE state)
    {
        if (enemyState == state)
            return;

        enemyState = state;

        switch (enemyState)
        {
            case ENEMYSTATE.CREATE:
                EnemyRigidbody = GetComponent<Rigidbody>();
                _statInfo = GetComponent<CharacterStat>();
                EnemyAnimator = GetComponentInChildren<Animator>();
                EnemyAnimEvent = GetComponentInChildren<AnimEvent>();
                EnemyAnimEvent.DeadDel += DestroyEnemy;
                GetComponent<Damageable>().onDeadDel += OnDead;
                canMove = true;
                break;
            case ENEMYSTATE.RUN:
                EnemyAnimator.SetBool("B_Run", true);
                break;
            case ENEMYSTATE.STUN:
                EnemyAnimator.SetBool("B_Run", false);
                break;
            case ENEMYSTATE.GOAL:
                DestroyEnemy();
                break;
            case ENEMYSTATE.DEAD:
                EnemyAnimator.SetTrigger("T_Dead");
                break;
        }
    }

    void StateProcess()
    {
        switch (enemyState)
        {
            case ENEMYSTATE.CREATE:
                ChangeState(ENEMYSTATE.RUN);
                break;
            case ENEMYSTATE.RUN:
                if (!canMove)
                    ChangeState(ENEMYSTATE.STUN);
                break;
            case ENEMYSTATE.STUN:
                if (canMove)
                    ChangeState(ENEMYSTATE.RUN);
                break;
            case ENEMYSTATE.GOAL:
                break;
            case ENEMYSTATE.DEAD:
                break;
        }
    }

    void OnDead()
    {
        isDead = true;
        ChangeState(ENEMYSTATE.DEAD);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject, destroyDelay);
    }

    public ENEMYSTATE GetState()
    {
        return enemyState;
    }

    // @Summary 적들을 스턴 상태로 만들떄 호출하십시오.
    public void SetStun(float time)
    {
        if (StunNow == null)
            StopCoroutine(StunNow);

        StunNow = StartCoroutine(StartStun(time));
    }

    IEnumerator StartStun(float time)
    {
        canMove = false;
        while (time > Time.deltaTime)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        canMove = true;
    }
}
