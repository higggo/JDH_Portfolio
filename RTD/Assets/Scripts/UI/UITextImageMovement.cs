using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITextImageMovement : MonoBehaviour
{
    // Gameover
    public Image Game = null;
    public Image Over = null;
    public Image GameoverTouch = null;
    public bool EndGameoverMovement = false;
    public bool GameoverFlag = false;
    Vector3 GamePosOrigin;
    Vector3 GameRotOrigin;
    Vector3 OverOrigin;
    Vector3 GameoverTouchScaleOrigin;
    Color GameoverTouchColorOrigin;

    // Victory
    public Image Clear = null;
    public Image VictoryTouch = null;
    public bool EndVictoryMovement = false;
    public bool VictoryFlag = false;
    Vector3 ClearOrigin;
    Vector3 VictoryTouchScaleOrigin;
    Color VictoryTouchColorOrigin;

    // Start is called before the first frame update
    void Start()
    {
        // Gameover
        if (Game == null) Game = GameObject.Find("Game").GetComponent<Image>();
        if (Over == null) Over = GameObject.Find("Over").GetComponent<Image>();
        if (GameoverTouch == null) GameoverTouch = GameObject.Find("GameoverTouch").GetComponent<Image>();

        // Victory
        if (Clear == null) Clear = GameObject.Find("Clear").GetComponent<Image>();
        if (VictoryTouch == null) VictoryTouch = GameObject.Find("VictoryTouch").GetComponent<Image>();

        /* Save Origin */
        // Gameover
        GamePosOrigin = Game.rectTransform.localPosition;
        GameRotOrigin = Game.rectTransform.rotation.eulerAngles;
        OverOrigin = Over.rectTransform.localPosition;
        GameoverTouchScaleOrigin = GameoverTouch.rectTransform.localScale;
        GameoverTouchColorOrigin = GameoverTouch.color;

        // Victory
        ClearOrigin = Clear.rectTransform.localPosition;
        VictoryTouchScaleOrigin = VictoryTouch.rectTransform.localScale;
        VictoryTouchColorOrigin = VictoryTouch.color;
    }

    public void StopGameoverMovement()
    {
        GameoverFlag = false;
        Game.rectTransform.localPosition = GamePosOrigin;
        Game.rectTransform.rotation = Quaternion.Euler(GameRotOrigin);
        Over.rectTransform.localPosition = OverOrigin;
        GameoverTouch.rectTransform.localScale = GameoverTouchScaleOrigin;
        GameoverTouch.color = GameoverTouchColorOrigin;
        Game.rectTransform.parent.gameObject.SetActive(false);

    }
    public void StopVictoryMovement()
    {
        VictoryFlag = false;
        Clear.rectTransform.localPosition = ClearOrigin;
        VictoryTouch.rectTransform.localScale = VictoryTouchScaleOrigin;
        VictoryTouch.color = VictoryTouchColorOrigin;
        Clear.rectTransform.parent.gameObject.SetActive(false);
    }

    public IEnumerator GameoverMovement()
    {
        GameoverFlag = true;
        EndGameoverMovement = false;

        Game.rectTransform.parent.gameObject.SetActive(true);

        float time = 0f;
        float movedelta = 0f;
        float movedir = 1f;
        float movespeed = 70f;

        float movemax = 7f;
        float movemin = -7f;

        float movedist = 0f;

        // quake
        while(time < 1.4f)
        {
            if (movedist >= movemax || movedist <= movemin)
                movedir *= -1f;

            movedelta = Time.smoothDeltaTime * movespeed * movedir;

            Vector3 gamepos = Game.rectTransform.localPosition;
            Vector3 overpos = Over.rectTransform.localPosition;

            gamepos.x += movedelta;
            overpos.x += movedelta;
            Game.rectTransform.localPosition = gamepos;
            Over.rectTransform.localPosition = overpos;
            movedist = Mathf.Clamp(movedist + movedelta, movemin, movemax);

            time += Time.smoothDeltaTime;
            movespeed *= 1.008f;

            yield return null;
        }

        movedelta = 0f;
        movedir = -1f;
        movespeed = 110f;

        float rotspeed = 35f;
        float fallingdist = 0f;

        bool breakdownflag = true;
        // Game Image Breakdown
        while (breakdownflag)
        {
            if(fallingdist < 100f)
            {
                movedelta = Time.smoothDeltaTime * movespeed * movedir;

                Vector3 gamepos = Game.rectTransform.localPosition;
                Vector3 gamerot = Game.rectTransform.rotation.eulerAngles;
                gamerot.z += Time.smoothDeltaTime * rotspeed;
                Game.rectTransform.rotation = Quaternion.Euler(gamerot);
                gamepos.y += movedelta;
                Game.rectTransform.localPosition = gamepos;

                fallingdist += Mathf.Abs(movedelta);
                movespeed *= 1.05f;
                rotspeed *= 1.03f;

            }
            else
            {
                breakdownflag = false;
            }
            yield return null;
        }

        // 이때부터 화면터치로 처음화면 이동 가능
        EndGameoverMovement = true;

        // Touch Alpha up
        Color color = GameoverTouch.color;
        while (color.a < 1f)
        {
            color.a += Time.smoothDeltaTime;
            GameoverTouch.color = color;
            yield return null;
        }

        // Touch Scale
        float scaledelta = 0f;
        float scaledist = 0f;
        float scaledir = -1f;
        float scalespeed = 0.28f;
        float scalemax = 0.04f;
        float scalemin = -0.1f;
        while (GameoverFlag)
        {
            if (scaledist >= scalemax || scaledist <= scalemin)
                scaledir *= -1f;

            scaledelta = Time.smoothDeltaTime * scalespeed * scaledir;

            Vector3 touchpos = GameoverTouch.rectTransform.localScale;

            touchpos.x += scaledelta;
            touchpos.y += scaledelta;
            GameoverTouch.rectTransform.localScale = touchpos;
            scaledist = Mathf.Clamp(scaledist + scaledelta, scalemin, scalemax);

            yield return null;
        }
    }

    public IEnumerator VictoryMovement()
    {
        Clear.rectTransform.parent.gameObject.SetActive(true);
        VictoryFlag = true;
        EndVictoryMovement = false;

        float fallingdelta = 0f;
        float fallingdist = 0f;
        float fallingdir = -1f;
        float fallingspeed = 70f;
        bool fallingflag = true;

        // Fall down clear
        while (fallingflag)
        {
            if (fallingdist < 550f)
            {
                fallingdelta = Time.smoothDeltaTime * fallingspeed * fallingdir;

                Vector3 clearpos = Clear.rectTransform.localPosition;
                clearpos.y += fallingdelta;
                Clear.rectTransform.localPosition = clearpos;

                fallingdist += Mathf.Abs(fallingdelta);
                fallingspeed *= 1.05f;
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
            movespeed *= 0.992f;
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
