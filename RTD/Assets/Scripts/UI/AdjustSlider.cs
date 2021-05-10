using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdjustSlider : MonoBehaviour
{
    public Transform Enemy;
    enum STATE
    {
        STOP,
        Move
    }

    STATE myState = STATE.STOP;
    float dir = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        // Update Slider Position
        if(Enemy != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(Enemy.position);
            Vector3 pos = transform.position;
            pos.y += 20f;
            transform.position = pos;
        }
        //

        StateProcess();
    }

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.STOP:
                break;
            case STATE.Move:
                dir *= -1.0f;
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.STOP:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangeState(STATE.Move);
                }
                break;
            case STATE.Move:
                float delta = Time.deltaTime * 1.0f * dir;
                this.GetComponent<Slider>().value = Mathf.Clamp(this.GetComponent<Slider>().value + delta, 0.0f, 1.0f);
                if (this.GetComponent<Slider>().value >= 1.0f || this.GetComponent<Slider>().value <= 0.0f)
                {
                    ChangeState(STATE.STOP);
                }
                break;
        }
    }
}
