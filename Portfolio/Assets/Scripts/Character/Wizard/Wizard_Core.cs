using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wizard_Core : Character
{
    public Transform ArrowShootPoint;
    public WizardStateMachine StateMachine;
    public bool IsShootArrow = false;

    private new void Start()
    {
        base.Start();
    }
    protected new void Update()
    {
        //DoReserveAction();
        base.Update();
    }
    public void ShootMotionStart()
    {
        IsShootArrow = false;
    }
    public void ShootArrow()
    {
        IsShootArrow = true;
        string arrowPath = "PlayCharacter/Magic";
        GameObject arrow = Instantiate(Resources.Load(arrowPath), ArrowShootPoint.position, ArrowShootPoint.rotation) as GameObject;
        arrow.GetComponent<MagicMovement>().Shoot(gameObject, AttackTarget, Status.damage);
    }

    public override void ProjectileHit(GameObject target, float damage)
    {
        if (target.tag == "Monster" && !target.GetComponent<BattleSystem>().IsDead())
            target.GetComponent<BattleSystem>()?.Damaged(gameObject, damage);
    }
    public override void InitDestinationUpdate()
    {
        StateMachine.ChangeState(WizardStateMachine.STATE.MOVING);
    }
    public override void InitChasingTargetUpdate()
    {
        if (Status.ChasingTarget.GetValue() == "")
        {
            StopChasing();
            return;
        }
        if (StateMachine.myState == WizardStateMachine.STATE.BATTLE && IsShootArrow)
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
        Debug.Log("AttackTarget.OnUpdate");
        if (StateMachine.myState == WizardStateMachine.STATE.BATTLE && IsShootArrow)
        {
            ReserveAction = () => { };
        }
        if (Status.AttackTarget.GetValue() != null)
        {
            ReserveAction = () => { };
            StateMachine.ChangeState(WizardStateMachine.STATE.BATTLE);
            PathFinding.StopMoving();
        }
    }
    public override void Dead()
    {
        StateMachine.ChangeState(WizardStateMachine.STATE.DEAD);
    }
}