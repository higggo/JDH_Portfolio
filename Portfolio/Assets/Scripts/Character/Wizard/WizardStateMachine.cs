using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateMachine : MonoBehaviour
{
    public enum STATE
    {
        NORMAL, IDLE, LOOKAROUND, MOVING, BATTLE, DEAD
    }
    public STATE myState = STATE.NORMAL;
    Wizard Player = null;
    float LookAroundTime = 1.5f;
    float LookAroundCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Wizard>();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    public void ChangeState(STATE s)
    {
        if (myState == s)
        {
            switch (myState)
            {
                case STATE.NORMAL:
                    break;
                case STATE.IDLE:
                    break;
                case STATE.LOOKAROUND:
                    break;
                case STATE.MOVING:
                    Player.PathFinding.MoveTo(Player.Status.Destination.GetValue(), Player.Status.MaxSpeed);
                    break;
                case STATE.BATTLE:
                    break;
                case STATE.DEAD:
                    break;
            }
            return;
        }
        // out
        switch (myState)
        {
            case STATE.NORMAL:
                break;
            case STATE.IDLE:
                break;
            case STATE.LOOKAROUND:
                Player.Animator.SetBool("LookAround", false);
                break;
            case STATE.MOVING:
                Player.Animator.SetBool("Moving", false);
                Player.PathFinding.StopMoving();
                Player.Animator.SetFloat("Speed", 0.0f);
                break;
            case STATE.BATTLE:
                if (Player.IsShootArrow)
                {
                    Player.ReserveAction = () =>
                    {
                        Player.IsShootArrow = false;
                        ChangeState(s);
                    };
                    return;
                }
                else
                {
                    Player.Animator.SetBool("Battle", false);
                }
                break;
            case STATE.DEAD:
                break;
        }
        // in
        myState = s;
        switch (myState)
        {
            case STATE.NORMAL:
                break;
            case STATE.IDLE:
                break;
            case STATE.LOOKAROUND:
                Player.Animator.SetBool("LookAround", true);
                LookAroundCount = 0.0f;
                break;
            case STATE.MOVING:
                Player.Animator.SetBool("Moving", true);
                Player.PathFinding.MoveTo(Player.Status.Destination.GetValue(), Player.Status.MaxSpeed);
                break;
            case STATE.BATTLE:
                Player.Animator.SetBool("Battle", true);
                break;
            case STATE.DEAD:
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NORMAL:
                ChangeState(STATE.IDLE);
                break;
            case STATE.IDLE:
                break;
            case STATE.LOOKAROUND:
                LookAroundCount += Time.deltaTime;
                if (LookAroundCount > LookAroundTime)
                {
                    ChangeState(STATE.IDLE);
                    Player.Animator.SetTrigger("StableIdle");
                }
                break;
            case STATE.MOVING:
                Player.Animator.SetFloat("Speed", Player.PathFinding.CurrentSpeed / Player.Status.MaxSpeed);
                if (!Player.PathFinding.isMoving)
                {
                    ChangeState(STATE.LOOKAROUND);
                }
                break;
            case STATE.BATTLE:
                if (Player.AttackTarget.GetComponent<BattleSystem>().IsDead())
                {
                    Player.IsShootArrow = false;
                    ChangeState(STATE.LOOKAROUND);
                }

                Vector3 lookPos = Player.AttackTarget.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;

                break;
            case STATE.DEAD:
                break;
        }
    }

}

