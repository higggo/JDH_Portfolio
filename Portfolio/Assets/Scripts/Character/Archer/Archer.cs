using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public enum STATE
    {
        NORMAL, LOOKAROUND, ROAMING, BATTLE, ESCAPE
    }
    public STATE myState = STATE.NORMAL;
    Animator Anim;
    float LookAroundTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.NORMAL:
                break;
            case STATE.LOOKAROUND:
                LookAroundTime = 0.0f;
                break;
            case STATE.ROAMING:
                break;
            case STATE.BATTLE:
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
                break;
            case STATE.LOOKAROUND:
                LookAroundTime += Time.deltaTime;
                if(LookAroundTime > 1.5f)
                {
                    ChangeState(STATE.NORMAL);
                }
                break;
            case STATE.ROAMING:
                Anim.SetFloat("Speed", gameObject.GetComponentInParent<CharacterStat>().Speed);
                break;
            case STATE.BATTLE:
                break;
            case STATE.ESCAPE:
                break;
        }
    }

}
