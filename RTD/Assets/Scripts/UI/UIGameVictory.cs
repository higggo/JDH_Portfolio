using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameVictory : MonoBehaviour
{
    public bool EndVictoryMovement = false;

    public Image Clear = null;
    public Image VictoryTouch = null;
    bool VictoryFlag = false;
    Vector3 ClearOrigin;
    Vector3 VictoryTouchScaleOrigin;
    Color VictoryTouchColorOrigin;

    // Start is called before the first frame update
    void Start()
    {
        // Victory
        if (Clear == null) Clear = GameObject.Find("Clear").GetComponent<Image>();
        if (VictoryTouch == null) VictoryTouch = GameObject.Find("VictoryTouch").GetComponent<Image>();

        ClearOrigin = Clear.rectTransform.localPosition;
        VictoryTouchScaleOrigin = VictoryTouch.rectTransform.localScale;
        VictoryTouchColorOrigin = VictoryTouch.color;
        gameObject.SetActive(true);
    }

    public void StopVictoryMovement()
    {
        VictoryFlag = false;
        Clear.rectTransform.localPosition = ClearOrigin;
        VictoryTouch.rectTransform.localScale = VictoryTouchScaleOrigin;
        VictoryTouch.color = VictoryTouchColorOrigin;
        gameObject.SetActive(false);
    }
    public IEnumerator VictoryMovement()
    {
        Clear.rectTransform.parent.gameObject.SetActive(true);
        VictoryFlag = true;
        EndVictoryMovement = false;

        float fallingdelta = 0f;
        float fallingdist = 0f;
        float fallingdir = -1f;
        float fallingspeed = 90f;
        float maxdist = 500;
        bool fallingflag = true;

        // Fall down clear
        while (fallingflag)
        {
            if (fallingdist < maxdist)
            {
                fallingdelta = Time.smoothDeltaTime * fallingspeed * fallingdir;

                Vector3 clearpos = Clear.rectTransform.localPosition;
                clearpos.y += fallingdelta;
                Clear.rectTransform.localPosition = clearpos;

                fallingdist = Mathf.Clamp(fallingdist + Mathf.Abs(fallingdelta), 0f, maxdist);
                fallingspeed *= 1.4f;
            }
            else
            {
                fallingflag = false;
            }
            yield return null;
        }

        // Quake
        float time = 0f;
        float movedelta = 0f;
        float movedist = 0f;
        float movedir = -1f;
        float movespeed = 400f;
        float movemax = 35f;
        float movemin = -35f;
        while (time < 1.1f)
        {
            if (movedist >= movemax || movedist <= movemin)
                movedir *= -1f;

            movedelta = Time.smoothDeltaTime * movespeed * movedir;

            Vector3 clearpos = Clear.rectTransform.localPosition;

            clearpos.y += movedelta;
            Clear.rectTransform.localPosition = clearpos;
            movedist = Mathf.Clamp(movedist + movedelta, movemin, movemax);

            time += Time.smoothDeltaTime;
            movespeed *= 0.997f;
            movemax *= 0.99f;
            movemin *= 0.99f;

            yield return null;
        }

        // 이때부터 화면터치로 처음화면 이동 가능
        EndVictoryMovement = true;

        // Touch Alpha up
        Color color = VictoryTouch.color;
        while (color.a < 1f)
        {
            color.a += Time.smoothDeltaTime;
            VictoryTouch.color = color;
            yield return null;
        }

        // Touch Scale
        float scaledelta = 0f;
        float scaledist = 0f;
        float scaledir = -1f;
        float scalespeed = 0.28f;
        float scalemax = 0.04f;
        float scalemin = -0.1f;
        while (VictoryFlag)
        {
            if (scaledist >= scalemax || scaledist <= scalemin)
                scaledir *= -1f;

            scaledelta = Time.smoothDeltaTime * scalespeed * scaledir;

            Vector3 touchpos = VictoryTouch.rectTransform.localScale;

            touchpos.x += scaledelta;
            touchpos.y += scaledelta;
            VictoryTouch.rectTransform.localScale = touchpos;
            scaledist = Mathf.Clamp(scaledist + scaledelta, scalemin, scalemax);

            yield return null;
        }
    }
}
