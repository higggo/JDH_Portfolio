using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;


public class BtnLevelUpMage : ButtonUtil
{
    Sprite MouseOverImage;
    Sprite DefaultImage;
    public UnityAction OnclickDelegate;

    public static int Level { get; set; }
    public uint Price { get { return (uint)(Level * 100); } }      // LJH: (Level + 1) * 10 => Level * 100

    protected override void Awake()
    {
        base.Awake();
        MouseOverImage = Resources.Load<Sprite>("UI/btn_6_MouseOver");
        DefaultImage = Resources.Load<Sprite>("UI/btn_6_Default");
        Level = 1;
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
                UpdateText();
                ChangeState(STATE.Active);
                break;
            case STATE.Active:
                ActiveOnClick();
                ActiveMouseEvent();
                break;
            case STATE.Deactive:
                DeactiveOnClick();
                DeactiveMouseEvent();
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
        Level = 1;
    }

    public override void OnClick()
    {
        OnclickDelegate?.Invoke();
        UpdateText();
    }
    protected override void MouseOver(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = MouseOverImage;
    }
    protected override void MouseOut(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = DefaultImage;
    }
    void UpdateText()
    {
        gameObject.transform.Find("Lv").GetComponent<TMPro.TextMeshProUGUI>().text = "Lv."+Level.ToString();
        gameObject.transform.Find("Price").GetComponent<TMPro.TextMeshProUGUI>().text = Price.ToString();
    }
}
