using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameover : MonoBehaviour
{
    public bool EndGameoverMovement = false;

    public Image Game = null;
    public Image Over = null;
    public Image GameoverTouch = null;
    bool GameoverFlag = false;
    Vector3 GamePosOrigin;
    Vector3 GameRotOrigin;
    Vector3 OverOrigin;
    Vector3 GameoverTouchScaleOrigin;
    Color GameoverTouchColorOrigin;

    
    // Start is called before the first frame update
    void Start()
    {
        if (Game == null) Game = GameObject.Find("Game").GetComponent<Image>();
        if (Over == null) Over = GameObject.Find("Over").GetComponent<Image>();
        if (GameoverTouch == null) GameoverTouch = GameObject.Find("GameoverTouch").GetComponent<Image>();

        GamePosOrigin = Game.rectTransform.localPosition;
        GameRotOrigin = Game.rectTransform.rotation.eulerAngles;
        OverOrigin = Over.rectTransform.localPosition;
        GameoverTouchScaleOrigin = GameoverTouch.rectTransform.localScale;
        GameoverTouchColorOrigin = GameoverTouch.color;
        gameObject.SetActive(true);
    }

    public void StopGameoverMovement()
    {
        GameoverFlag = false;
        Game.rectTransform.localPosition = GamePosOrigin;
        Game.rectTransform.rotation = Quaternion.Euler(GameRotOrigin);
        Over.rectTransform.localPosition = OverOrigin;
        GameoverTouch.rectTransform.localScale = GameoverTouchScaleOrigin;
        GameoverTouch.color = GameoverTouchColorOrigin;
        gameObject.SetActive(false);
    }

    public IEnumerator GameoverMovement()
    {
        GameoverFlag = true;
        EndGameoverMovement = false;

        gameObject.SetActive(true);

        float time = 0f;
        float movedelta = 0f;
        float movedir = 1f;
        float movespeed = 70f;

        float movemax = 7f;
        float movemin = -7f;

        float movedist = 0f;

        // quake
        while (time < 1.4f)
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
            if (fallingdist < 100f)
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

}
