using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossWarning : MonoBehaviour
{
    public Image WarningImage = null;
    public Image BossRoundImage = null;
    public Image BorderImage = null;
    // Start is called before the first frame update
    void Start()
    {
        if (WarningImage == null) WarningImage = transform.Find("Warning").GetComponent<Image>();
        if (BossRoundImage == null) BossRoundImage = transform.Find("BossRound").GetComponent<Image>();
        if (BorderImage == null) BorderImage = transform.Find("Border").GetComponent<Image>();

    }

    public IEnumerator BossAlarm()
    {

        bool timeOver = false;
        float time = 0.0f;
        float speed = 0.5f;
        float colorDelta = 0f;
        float min = 0.65f;
        float max = 0.9f;
        float direction = 1f;

        WarningImage.color = new Color(max, max, max, max);
        BossRoundImage.color = new Color(max, max, max, max);
        BorderImage.color = new Color(max, max, max, max);
        Color color = WarningImage.color;


        while (!(timeOver && color.r == 0))
        {
            if (color.r <= min || color.r >= max)
                direction = -direction;
            if(time > 3.5f && color.r >= max)
            {
                direction = direction < 0f ? direction : -direction;
                min = 0f;
                speed = 1f;
                timeOver = true;
            }
            colorDelta = Time.smoothDeltaTime * direction * speed;

            color.r = Mathf.Clamp(color.r + colorDelta, min, max);
            color.g = Mathf.Clamp(color.g + colorDelta, min, max);
            color.b = Mathf.Clamp(color.b + colorDelta, min, max);
            color.a = Mathf.Clamp(color.a + colorDelta, min, max);
            WarningImage.color = color;
            BossRoundImage.color = color;
            BorderImage.color = color;
            time += Time.smoothDeltaTime;
            yield return null;
        }
    }
}
