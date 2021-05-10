using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum STATE
    {
        Hide,
        Normal,
        Impossible,
        Possible

    }
    STATE state = STATE.Normal;
    public STATE State
    {
        get
        {
            return state;
        }
        set
        {
            ChangeState(value);
        }

    }
    [SerializeField, Range(0f, 2)]
    public float CollisionRadiusRate = 1f;

    Sprite Hide;
    Sprite Impossible;
    Sprite Normal;
    Sprite Possible;

    bool isColliding = false;

    private void Awake()
    {
        gameObject.layer = transform.parent.gameObject.layer;
    }
    // Start is called before the first frame update
    void Start()
    {
        float RadiusOrigin = GetComponent<SphereCollider>().radius;
        Vector3 localScale = transform.localScale;
        localScale *= CollisionRadiusRate;
        transform.localScale = localScale;

        Hide = null;
        Impossible = Resources.Load<Sprite>("UI/tileimpossible");
        Normal = Resources.Load<Sprite>("UI/tileormal");
        Possible = Resources.Load<Sprite>("UI/tilepossible");
        ChangeState(STATE.Hide);
    }
    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (state)
        {
            case STATE.Hide:
                GetComponent<SpriteRenderer>().sprite = Hide;
                isColliding = false;
                break;
            case STATE.Normal:
                GetComponent<SpriteRenderer>().sprite = Normal;
                break;
            case STATE.Impossible:
                GetComponent<SpriteRenderer>().sprite = Impossible;
                break;
            case STATE.Possible:
                GetComponent<SpriteRenderer>().sprite = Possible;
                break;
        }
    }

    void StateProcess()
    {
        switch (state)
        {
            case STATE.Hide:
                break;
            case STATE.Normal:
                //if (transform.childCount > 0)
                //{
                //    State = STATE.Impossible;
                //}
                //else if(isColliding)
                //{
                //    State = STATE.Possible;
                //}
                if(isColliding)
                {
                    State = STATE.Possible;
                }

                break;
            case STATE.Impossible:
                if (transform.childCount == 0)
                {
                    State = STATE.Possible;
                }
                break;
            case STATE.Possible:
                //if (transform.childCount > 0)
                //{
                //    State = STATE.Impossible;
                //}
                //else if (!isColliding)
                //{
                //    State = STATE.Normal;
                //}
                if (!isColliding)
                {
                    State = STATE.Normal;
                }
                break;
        }
    }
    void UpdateTile()
    {
    }
    public void AppearTile()
    {
        State = STATE.Normal;
        isColliding = false;
    }
    public void InRange()
    {
        isColliding = true;
    }
    public void OutRange()
    {
        isColliding = false;
    }
}
