using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI
{
    public abstract class ButtonUtil : MouseEvent
    {
        MouseEvent MouseEvent;
        public enum STATE
        {
            None,
            Create,
            Active,
            Deactive
        }
        public STATE state = STATE.None;
        protected Button button;
        public STATE State
        {
            get { return state; }
        }

        abstract protected void ChangeState(STATE s);
        abstract protected void StateProcess();
        abstract public void OnClick();
        abstract protected void MouseOver(PointerEventData eventData);
        abstract protected void MouseOut(PointerEventData eventData);
        protected virtual void Awake()
        {
            MouseEvent = GetComponent<MouseEvent>();

            //MouseEvent.MouseEnterEvent += MouseOver;
            //MouseEvent.MouseExitEvent += MouseOut;

            button = this.GetComponent<Button>();
            //button.onClick.AddListener(OnClick);
        }

        protected void ActiveOnClick()
        {
            button.onClick.AddListener(OnClick);
            button.interactable = true;
        }

        protected void DeactiveOnClick()
        {
            button.onClick.RemoveListener(OnClick);
            button.interactable = false;
        }

        protected void ActiveMouseEvent()
        {
            MouseEvent.MouseEnterEvent += MouseOver;
            MouseEvent.MouseExitEvent += MouseOut;
        }

        protected void DeactiveMouseEvent()
        {
            MouseEvent.MouseEnterEvent -= MouseOver;
            MouseEvent.MouseExitEvent -= MouseOut;
        }

    }
}
