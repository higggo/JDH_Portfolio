using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStateMachine : MonoBehaviour, MonsterBattleSystem
{
    //public GameObject demonstrationDot;

    public enum STATE
    {
        NORMAL, ROAMING, BATTLE, ESCAPE
    }
    public STATE myState = STATE.NORMAL;


    float Playtime = 0.0f;
    Vector3 OrgPos = Vector3.zero;

    // 원형으로 해보자
    public float radius = 10.0f;

    Vector3 destination;

    public float AttackDelay = 1.0f;
    float OrgAttackDelay = 0.0f;

    public Monster Monster;

    bool completeSendPos = false;
    bool waitingSendPos = false;
    private void Awake()
    {
        Monster = this.GetComponent<Monster>();


        // 람다식
        //myRangeSys.battle = () => { ChangeState(STATE.BATTLE); };


    }
    // Start is called before the first frame update
    void Start()
    {
        Monster.EventHandler.OnDestReceive += MoveTo;
        OrgPos = this.transform.position;

        OrgAttackDelay = AttackDelay;

    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();

        /*
        myAnim.SetFloat("Speed", myNavAgent.velocity.magnitude);
        if(myNavAgent.remainingDistance <= Mathf.Epsilon)
        {
            //GameObject obj = Instantiate(demonstrationDot) as GameObject;

            // 임의의 각도
            float ran_radian = Random.Range(0.0f, Mathf.PI * 2);
            // 임의의 반지름
            float ran_radius = Random.Range(0.0f, radius);

            // x = r * con()
            destination.x = this.transform.position.x + ran_radius * Mathf.Cos(ran_radian);
            // y = r * sin()
            destination.z = this.transform.position.z + ran_radius * Mathf.Sin(ran_radian);
            destination.y = this.transform.position.y;

            //obj.transform.position = destination;
            myNavAgent.SetDestination(destination);
        }
        */
    }

    void OnBattle()
    {
        ChangeState(STATE.BATTLE);
    }
    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.NORMAL:
                Monster.Animator.SetFloat("Speed", 0.0f);
                Playtime = 0.0f;
                Monster.NavMeshAgent.stoppingDistance = 0.5f;
                completeSendPos = false;
                waitingSendPos = false;
                break;
            case STATE.ROAMING:
                Monster.NavMeshAgent.SetDestination(destination);
                break;
            case STATE.BATTLE:
                Monster.NavMeshAgent.stoppingDistance = 1.5f;
                AttackDelay = 0.0f;
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
                Monster.Animator.SetFloat("Speed", 0.0f);
                Playtime += Time.deltaTime;
                if (Playtime >= 2.0f && !waitingSendPos)
                {
                    waitingSendPos = true;
                    Vector3 dir = Vector3.zero;
                    dir.x = Random.Range(-1.0f, 1.0f);
                    dir.z = Random.Range(-1.0f, 1.0f);
                    dir.Normalize();
                    Vector3 dest = OrgPos + dir * Random.Range(1.0f, 3.0f);
                    RDConnection.Write.UpdateMonsterDestination(GetComponent<Monster>().ID, dest, (task) => {
                        if (task.IsCompleted)
                        {
                            completeSendPos = true;
                        }
                        if (task.IsFaulted)
                        {
                            waitingSendPos = false;
                        }
                    });
                }
                break;
            case STATE.ROAMING:
                Monster.Animator.SetFloat("Speed", Monster.NavMeshAgent.velocity.magnitude / Monster.NavMeshAgent.speed);
                if (Monster.NavMeshAgent.remainingDistance < 0.01f)
                {
                    Monster.NavMeshAgent.SetDestination(this.transform.position);
                    ChangeState(STATE.NORMAL);
                }
                break;
            case STATE.BATTLE:
                {
                    Vector3 dir = Monster.RangeSys.Target.position - this.transform.position;
                    dir.y = 0.0f;
                    dir.Normalize();
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.smoothDeltaTime * 10.0f);
                    Monster.Animator.SetFloat("Speed", Monster.NavMeshAgent.velocity.magnitude / Monster.NavMeshAgent.speed);
                    Monster.NavMeshAgent.SetDestination(Monster.RangeSys.Target.position);

                    if (AttackDelay > Mathf.Epsilon)
                    {
                        AttackDelay -= Time.deltaTime;
                    }
                    else
                    {
                        OnAttack();
                    }
                }
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
    void OnAttackTarget()
    {
        Monster.RangeSys.Target.GetComponent<MonsterBattleSystem>()?.OnDamage();
    }

    public void OnDamage()
    {
        Debug.Log("OnDamage Mon cs");
        Monster.Animator.SetTrigger("Damage");
    }

    public void MoveTo()
    {
        destination = Monster.TaskData.NextDestination;
        ChangeState(STATE.ROAMING);
    }
}
