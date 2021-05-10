using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessageUI : MonoBehaviour
{
    public TMP_Text txt;

    public void SetDamage(float dmg)
    {
        txt = GetComponentInChildren<TMP_Text>();
        int changedDmg = (int)dmg;
        txt.SetText("-" + changedDmg.ToString());
        if (changedDmg < 30)
        {
            txt.color = Color.black;
            txt.fontSize = 25;
        }
        else if (changedDmg < 100)
        {
            txt.color = Color.yellow;
            txt.fontSize = 35;
        }
        else
        {
            txt.color = Color.red;
        }
        
        StartCoroutine(StartPrint());
    }

    public void SetText(string text, Color color)
    {
        txt = GetComponentInChildren<TMP_Text>();
        txt.text = text;
        txt.color = color;
    }

    IEnumerator StartPrint()
    {
        float time = 1.0f;
        while (time > Mathf.Epsilon)
        {
            txt.alpha = time;
            transform.Translate(0.0f, 0.01f, 0.0f);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    
}
