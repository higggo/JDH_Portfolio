using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.UI;
using UnityEngine.EventSystems;

public class BossZoneWarp : ButtonUtil
{
    public bool bMouseOver = false;
    public override void OnClick()
    {
        Debug.Log("Bosszone Warp");
    }

    protected override void ChangeState(STATE s)
    {
    }

    protected override void MouseOut(PointerEventData eventData)
    {
        bMouseOver = false;
    }

    protected override void MouseOver(PointerEventData eventData)
    {
        bMouseOver = true;
    }

    protected override void StateProcess()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ActiveMouseEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
