using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnLevelUpActive: ButtonUtil
{
    Sprite MouseOverImage;
    Sprite DefaultImage;
    Sprite DeactiveImage;

    public AudioClip Audio_Button;

    bool bOpen = false;

    protected override void Awake()
    {
        base.Awake();
        MouseOverImage = Resources.Load<Sprite>("UI/btn_3_MouseOver");
        DefaultImage = Resources.Load<Sprite>("UI/btn_3_Default");
        DeactiveImage = Resources.Load<Sprite>("UI/btn_3_Deactive");
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.Create);


    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    protected override void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (state)
        {
            case STATE.None:
                break;
            case STATE.Create:
                GetComponent<Image>().sprite = DefaultImage;
                ChangeState(STATE.Active);
                break;
            case STATE.Active:
                {
                    ActiveOnClick();
                    ActiveMouseEvent();
                    SetOpen(false);
                }
                break;
            case STATE.Deactive:
                {
                    GetComponent<Image>().sprite = DeactiveImage;
                    DeactiveOnClick();
                    DeactiveMouseEvent();
                    SetOpen(false);
                }
                break;
        }
    }

    protected override void StateProcess()
    {
        switch (state)
        {
            case STATE.None:
                break;
            case STATE.Create:
                break;
            case STATE.Active:
                break;
            case STATE.Deactive:
                break;
        }
    }

    public void Init()
    {
        SetActive();
    }

    public override void OnClick()
    {
        SetOpen(!bOpen);
        SoundManager.I.PlayEffectSound(Audio_Button);
    }
    protected override void MouseOver(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = MouseOverImage;
    }
    protected override void MouseOut(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = DefaultImage;
    }

    void SetOpen(bool bOpen)
    {
        this.bOpen = bOpen;
        foreach (Transform child in transform.parent)
        {
            if (child.gameObject != gameObject)
                child.gameObject.SetActive(this.bOpen);
        }
    }

    public void SetDeactive()
    {
        ChangeState(STATE.Deactive);
    }

    public void SetActive()
    {
        ChangeState(STATE.Active);
    }
}

