using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster_Core : MonoBehaviour, BattleSystem
{

    public abstract void StartChasing();
    public abstract void StopChasing();
    public abstract void ApplyDamage(float damage);

    public abstract void ChaseTarget(GameObject target);

    public abstract void ApplyHit(GameObject target, float damage);

    public abstract void ApplyHP();
    // Start is called before the first frame update
    public string ID;
    public string resourcePath;
    public string location;
    public Vector3 initPos;
    public float maxHP;

    public MonsterAnimEvent AnimEvent;
    public MonsterStateMachine StateMachine;
    public MonsterStatus Status;
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    public MonsterRangeSystem RangeSys;
    public MonsterCanvas Canvas;
    public GameObject AttackTarget;
    public Coroutine ChasingCoroutine;

    public DatabaseReference Reference;
    protected void Awake()
    {
	}
    protected void Start()
    {
        AttackTarget = null;
    }

    public virtual void Initialize(string ID, string resourcePath, string location, Vector3 InitPos, float maxHP)
    {
        this.ID = ID;
        this.resourcePath = resourcePath;
        this.location = location;
        this.initPos = InitPos;
        this.maxHP = maxHP;
        Reference = FirebaseDatabase.DefaultInstance.GetReference("Destination").Child("Town").Child("Monsters").Child(ID);
        Status.Initialize();
        ApplyHP();
        Status.Destination.OnUpdate += () =>
        {
            StartCoroutine(StateMachine.MoveTo());
        };
        Status.HP.OnUpdate += () => {
            string currentHP;
            if (Status.HP.GetValue() <= 0.0f)
            {
                StateMachine.ChangeState(MonsterStateMachine_Core.STATE.DEAD);
                currentHP = "0";
            }
            else
            {
                currentHP = Status.HP.GetValue().ToString();
            }
            string hpBar = currentHP + "/" + maxHP.ToString();
            Canvas.HP.text = hpBar;
        };
        Status.Spawn.OnUpdate += () =>
        {
            if(StateMachine.myState == MonsterStateMachine_Core.STATE.DEAD)
                StateMachine.ChangeState(MonsterStateMachine_Core.STATE.RESPAWN);
        };

        Status.ChasingTarget.OnUpdate += () =>
        {
            if (Status.ChasingTarget.GetValue() != "")
            {
                AttackTarget = Units.Users[Status.ChasingTarget.GetValue()];
                StartChasing();
            }
        };

        Status.AttackTarget.OnUpdate += () =>
        {
            if (Status.AttackTarget.GetValue() != null)
            {
                StateMachine.ChangeState(MonsterStateMachine.STATE.BATTLE);
            }
        };
    }

    public void Damaged(GameObject target, float damage)
    {
        // Attack Target
        ChaseTarget(target);
        Animator.SetTrigger("Damaged");
        ApplyDamage(damage);
    }
    public void Hit()
    {
        AttackTarget.GetComponent<BattleSystem>()?.Damaged(gameObject, Status.damage);
        ApplyHit(AttackTarget, Status.damage);
    }
    public bool IsDead()
    {
        if (Status.HP.GetValue() <= 0.0f)
            return true;
        else
            return false;
    }
    public void ReturnIdle()
    {
        StateMachine.ChangeState(MonsterStateMachine_Core.STATE.IDLE);
    }
}

