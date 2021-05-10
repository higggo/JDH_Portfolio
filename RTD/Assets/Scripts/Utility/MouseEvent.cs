using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void MouseEventData(PointerEventData data);
public class MouseEvent : MonoBehaviour,
    IPointerClickHandler, IPointerDownHandler,
    IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    // event를 붙여서 외부에서 추가할수있지만 실행은 안되게
    public event MouseEventData MouseClickEvent;
    public event MouseEventData MouseDownEvent;
    public event MouseEventData MouseEndDragEvent;
    public event MouseEventData MouseDragEvent;
    public event MouseEventData MouseEnterEvent;
    public event MouseEventData MouseExitEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        MouseClickEvent?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseDownEvent?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MouseDragEvent?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MouseEndDragEvent?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEvent?.Invoke(eventData);
    }
     
     
    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExitEvent?.Invoke(eventData);
    }

}
