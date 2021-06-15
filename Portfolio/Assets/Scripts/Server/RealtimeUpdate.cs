using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeUpdate : MonoBehaviour
{
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject unityContext;
    AndroidJavaClass customClass;
    bool isPaused = false;
    float pauseCnt = 0.0f;
    float timer = 0.0f;
    float timer1 = 0.0f;

    private void Start()
    {
        sendActivityReference("com.example.mylibrary");

        startService();
    }

    void sendActivityReference(string packageName)
    {
        //Replace with your full package name
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        customClass = new AndroidJavaClass(packageName);
        customClass.CallStatic("receiveContextInstance", unityActivity);
    }
    void startService()
    {
        customClass.CallStatic("StartCheckerService");
    }
    private void Update()
    {
        timer += Time.deltaTime;
        timer1 += Time.deltaTime;

        if(timer > 5.0f)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["time"] = timer1;
            RDConnection.Write.Test(result, (res) => {
                if (res.IsFaulted)
                {
                    Debug.Log("UpdateCharacterLocation Faild");
                    return;
                }
                else if (res.IsCompleted)
                {
                }
            });
            timer = 0.0f;
        }
        if(isPaused)
        {
            pauseCnt += Time.deltaTime;
        }
        if(pauseCnt > 10.0f)
        {

            RDConnection.Write.RemoveUpdate();
        }
    }
    
    public void OnApplicationQuit()
    {
        RDConnection.Write.RemoveUpdate();
    }
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        pauseCnt = 0.0f;
    }
}
