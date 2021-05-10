using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
   
    public string Timer = @"00:00";
    private bool IsPlaying;
    public KeyCode KcdPlay = KeyCode.Space;
    public float Seconds = 15;
    [SerializeField] Text CountDownText;

    // Start is called before the first frame update

    void Start()
    {
        Timer = CountdownTimer(false);
        GetComponent<TMPro.TextMeshProUGUI>().text = Timer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KcdPlay))
            IsPlaying = !IsPlaying;
        if (IsPlaying)
        {
            Timer = CountdownTimer();
        }

        if (Seconds <= 0)
        {
            SetZero();
        }
        if (CountDownText)
            CountDownText.text = Timer;
        GetComponent<TMPro.TextMeshProUGUI>().text = Timer.ToString();
    }

    private string CountdownTimer(bool IsUpdate = true)
    {
        if (IsUpdate)
            Seconds -= Time.deltaTime;

        TimeSpan timespan = TimeSpan.FromSeconds(Seconds);
        string timer = string.Format("{00:00}",
            timespan.Seconds);
        return timer;
    }
    private void SetZero()
    {
        Timer = @"00:00";
        Seconds = 0;
        IsPlaying = false;
    }


}
