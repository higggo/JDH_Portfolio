using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class Warrior_Core : Character
{
    public WarriorStateMachine StateMachine;
    public WarriorGeneralAttack GeneralAttack;

    private new void Start()
    {
        base.Start();
    }
    protected new void Update()
    {
        base.Update();
    }
    public override void ProjectileHit(GameObject target, float damage)
    {
        if (target.tag == "Monster" && !target.GetComponent<BattleSystem>().IsDead())
            target.GetComponent<BattleSystem>()?.Damaged(gameObject, damage);
    }
    public override void Hit()
    {
        if (AttackTarget.tag == "Monster" && !AttackTarget.GetComponent<BattleSystem>().IsDead())
            AttackTarget.GetComponent<BattleSystem>()?.Damaged(gameObject, Status.damage);
    }

    public override void InitDestinationUpdate()
    {
        if (StateMachine.myState == WarriorStateMachine.STATE.BATTLE && GeneralAttack.Hit)
        {
            ReserveAction = () =>
            {
                StateMachine.ChangeState(WarriorStateMachine.STATE.MOVING);
            };
        }
        else
        {
            StateMachine.ChangeState(WarriorStateMachine.STATE.MOVING);
            Animator.SetTrigger("GeneralAttackCancled");
        }
    }
    public override void InitChasingTargetUpdate()
    {
        if (Status.ChasingTarget.GetValue() == "")
        {
            StopChasing();
            return;
        }
        if (StateMachine.myState == WarriorStateMachine.STATE.BATTLE && GeneralAttack.Hit)
        {
            ReserveAction = () =>
            {
                if (Status.ChasingTarget.GetValue() != "")
                {
                    if (Status.IsChasing.GetValue())
                    {
                        AttackTarget = Units.Monsters[Status.ChasingTarget.GetValue()];
                        StartChasing();
                    }
                    else if (!Status.IsChasing.GetValue())
                    {
                        StartChasing();
                    }
                }
            };
        }
        else
        {
            if (Status.ChasingTarget.GetValue() != "")
            {
                if (Status.IsChasing.GetValue())
                {
                    if (AttackTarget != Units.Monsters[Status.ChasingTarget.GetValue()])
                    {
                        AttackTarget = Units.Monsters[Status.ChasingTarget.GetValue()];
                        StartChasing();
                    }
                }
                else if (!Status.IsChasing.GetValue())
                {
                    AttackTarget = Units.Monsters[Status.ChasingTarget.GetValue()];
                    StartChasing();
                }
            }
        }
    }
    public override void InitAttackTargetUpdate()
    {
        if (StateMachine.myState == WarriorStateMachine.STATE.BATTLE && GeneralAttack.Hit)
        {
            ReserveAction = () => { };
        }
        if (Status.AttackTarget.GetValue() != null)
        {
            PathFinding.StopMoving();
            StateMachine.ChangeState(WarriorStateMachine.STATE.BATTLE);
        }
    }

    public override void Dead()
    {
        StateMachine.ChangeState(WarriorStateMachine.STATE.DEAD);
    }
}