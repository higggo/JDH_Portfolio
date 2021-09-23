using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Archer_Core : Character
{
    public ArcherStateMachine StateMachine;
    public Transform ArrowShootPoint;
    public bool IsShootArrow = false;

    private new void Start()
    {
        base.Start();
    }
    protected new void Update()
    {
        base.Update();
    }
    public void ShootMotionStart()
    {
        IsShootArrow = false;
    }
    public void ShootArrow()
    {
        IsShootArrow = true;
        string arrowPath = "PlayCharacter/Arrow";
        GameObject arrow = Instantiate(Resources.Load(arrowPath), ArrowShootPoint.position, Quaternion.identity) as GameObject;
        arrow.GetComponent<ArrowMovement>().Shoot(gameObject, AttackTarget, Status.damage);
    }

    public override void ProjectileHit(GameObject target, float damage)
    {
        if (target.tag == "Monster" && !target.GetComponent<BattleSystem>().IsDead())
            target.GetComponent<BattleSystem>()?.Damaged(gameObject, damage);
    }


    public override void InitDestinationUpdate()
    {
        StateMachine.ChangeState(ArcherStateMachine.STATE.MOVING);
    }
    public override void InitChasingTargetUpdate()
    {
        if (Status.ChasingTarget.GetValue() == "")
        {
            StopChasing();
            return;
        }
        if (StateMachine.myState == ArcherStateMachine.STATE.BATTLE && IsShootArrow)
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
    public override void InitAttackTargetUpdate()
    {
        if (StateMachine.myState == ArcherStateMachine.STATE.BATTLE && IsShootArrow)
        {
            ReserveAction = () => { };
        }
        if (Status.AttackTarget.GetValue() != null)
        {
            ReserveAction = () => { };
            StateMachine.ChangeState(ArcherStateMachine.STATE.BATTLE);
        }
    }
    public override void Dead()
    {
        StateMachine.ChangeState(ArcherStateMachine.STATE.DEAD);
    }
}