using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageDrag : MonoBehaviour
{
    MouseEvent MouseEvent;
    bool IsMove;

    // Start is called before the first frame update
    private void Awake()
    {
        IsMove = false;
        MouseEvent = this.GetComponent<MouseEvent>();
        MouseEvent.MouseDownEvent += this.MoveStart;
        MouseEvent.MouseClickEvent += this.MoveEnd;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MoveStart(PointerEventData eventData)
    {
        IsMove = true;
        StartCoroutine(Moving());

    }
    void MoveEnd(PointerEventData eventData)
    {
        IsMove = false;
    }

    IEnumerator Moving()
    {
        while (IsMove)
        {
            Vector3 pos = this.transform.position;

            pos = Input.mousePosition;

            this.transform.position = pos;
            yield return null;
        }
    }
}
