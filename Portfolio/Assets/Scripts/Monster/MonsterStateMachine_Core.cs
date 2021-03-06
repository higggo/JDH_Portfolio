using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterStateMachine_Core : MonoBehaviour
{
    public abstract void Revive();
    public abstract void WaitingRespawn();
    public abstract void WaitingRoaming();
    public abstract void InitWaitingRoaming();
    public enum STATE
    {
        NORMAL, IDLE, LOOKAROUND, ROAMING, MOVING, BATTLE, DEAD, RESPAWN, ESCAPE
    }
    public STATE myState = STATE.NORMAL;



    // 원형으로 해보자
    public float radius = 10.0f;

    public float AttackDelay = 1.0f;
    float OrgAttackDelay = 0.0f;
    float LookAroundTime = 1.5f;
    float LookAroundCount = 0.0f;
    protected float DeadTime = 0.0f;
    protected float DeadTimeMax = 3.0f;

    public Monster Monster;

    protected void Awake()
    {
        Debug.Log("MonsterStateMachine_Core Awake");

        Monster = this.GetComponent<Monster>();

        //GetComponent<MonsterStatus>().Destination.OnUpdate += MoveTo;

        // 람다식
        //myRangeSys.battle = () => { ChangeState(STATE.BATTLE); };


        OrgAttackDelay = AttackDelay;

    }
    // Start is called before the first frame update
    protected void Start()
    {
        Debug.Log("MonsterStateMachine_Core Start");

        //StartCoroutine(ChildChagedTest());
    }
    private void Update()
    {
        StateProcess();
    }
    void OnBattle()
    {
        ChangeState(STATE.BATTLE);
    }
    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        // out
        switch (myState)
        {
            case STATE.NORMAL:
                break;
            case STATE.IDLE:
                break;
            case STATE.LOOKAROUND:
                Monster.Animator.SetBool("LookAround", false);
                break;
            case STATE.ROAMING:
                InitWaitingRoaming();
                Monster.Animator.SetFloat("Speed", 0.0f);
                Monster.NavMeshAgent.enabled = false;
                Monster.Animator.SetBool("Roaming", false);
                break;
            case STATE.MOVING:
                InitWaitingRoaming();
                Monster.Animator.SetFloat("Speed", 0.0f);
                Monster.Animator.SetBool("Moving", false);
                break;
            case STATE.BATTLE:
                Monster.Animator.SetBool("Battle", false);
                break;
            case STATE.DEAD:
                Monster.Animator.SetBool("Dead", false);
                break;
            case STATE.RESPAWN:
                DeadTime = 0.0f;
                break;
            case STATE.ESCAPE:
                break;
        }
        // in
        myState = s;
        switch (myState)
        {
            case STATE.NORMAL:
                Monster.Animator.SetFloat("Speed", 0.0f);
                InitWaitingRoaming();
                break;
            case STATE.IDLE:
                break;
            case STATE.LOOKAROUND:
                LookAroundCount = 0.0f;
                Monster.Animator.SetBool("LookAround", true);
                break;
            case STATE.ROAMING:
                Monster.Animator.SetBool("Roaming", true);
                break;
            case STATE.MOVING:
                Monster.Animator.SetBool("Moving", true);
                break;
            case STATE.BATTLE:
                Monster.Animator.SetBool("Battle", true);
                Monster.NavMeshAgent.stoppingDistance = 1.5f;
                AttackDelay = 0.0f;
                break;
            case STATE.DEAD:
                Monster.StopChasing();
                Monster.Animator.SetBool("Dead", true);
                break;
            case STATE.RESPAWN:
                Revive();
                Monster.Animator.SetTrigger("Spawn");
                break;
            case STATE.ESCAPE:
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NORMAL:
                ChangeState(STATE.IDLE);
                //Monster.Animator.SetFloat("Speed", 0.0f);
                break;
            case STATE.IDLE:
                WaitingRoaming();
                break;
            case STATE.LOOKAROUND:
                WaitingRoaming();
                LookAroundCount += Time.deltaTime;
                if (LookAroundCount > LookAroundTime)
                {
                    ChangeState(STATE.IDLE);
                    Monster.Animator.SetTrigger("StableIdle");
                }
                break;
            case STATE.ROAMING:
                Monster.Animator.SetFloat("Speed", Monster.NavMeshAgent.velocity.magnitude / Monster.NavMeshAgent.speed);
                if (Monster.NavMeshAgent.velocity == Vector3.zero)
                {
                    ChangeState(STATE.LOOKAROUND);
                }
                break;
            case STATE.MOVING:
                Monster.Animator.SetFloat("Speed", Monster.NavMeshAgent.velocity.magnitude / Monster.NavMeshAgent.speed);
                if (Monster.NavMeshAgent.velocity == Vector3.zero)
                {
                    ChangeState(STATE.LOOKAROUND);
                }
                break;
            case STATE.BATTLE:
                {
                    if(Monster.AttackTarget != null)
                    {
                        if (Monster.AttackTarget.GetComponent<BattleSystem>().IsDead())
                        {
                            ChangeState(STATE.LOOKAROUND);
                            Monster.AttackTarget = null;
                        }

                        Vector3 lookPos = Monster.AttackTarget.transform.position - transform.position;
                        lookPos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = rotation;
                    }
                    else
                    {
                        ChangeState(STATE.LOOKAROUND);
                        Monster.AttackTarget = null;
                    }
                }
                break;
            case STATE.DEAD:
                DeadTime += Time.deltaTime;
                WaitingRespawn();
                break;
            case STATE.RESPAWN:
                ChangeState(STATE.LOOKAROUND);
                break;
            case STATE.ESCAPE:
                break;
        }
    }

    void OnAttack()
    {
        if (Monster.NavMeshAgent.remainingDistance <= Monster.NavMeshAgent.stoppingDistance)
        {
            Monster.Animator.SetTrigger("Attack");
            AttackDelay = OrgAttackDelay;
        }
    }

    public IEnumerator MoveTo()
    {
        if (!Monster.NavMeshAgent.enabled) Monster.NavMeshAgent.enabled = true;
        Monster.NavMeshAgent.SetDestination(Monster.Status.Destination.GetValue());
        yield return null;
        if(Monster.AttackTarget == null)
        {
            ChangeState(STATE.ROAMING);
        }
        else
        {
            ChangeState(STATE.MOVING);
        }
    }

}
