using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeUpdate : MonoBehaviour
{
#if UNITY_ANDROID
    public AndroidJavaClass unityClass;
    public AndroidJavaObject unityActivity;
    public AndroidJavaObject unityContext;
    public AndroidJavaClass customClass;
#endif
    bool isPaused = false;
    float pauseCnt = 0.0f;
    //float timer = 0.0f;
    //float timer1 = 0.0f;

    public LayerMask ClickMask;
    private void Start()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            sendActivityReference("com.higggo.service.mylibrary");

            startService();
        }
#endif

        ClickMask = 1 << 8;
        RDConnection.Listener.TownCharacterAddListener();

        OnPlaceCharacters();
    }

    void sendActivityReference(string packageName)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            //Replace with your full package name
            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            customClass = new AndroidJavaClass(packageName);
            customClass.CallStatic("receiveContextInstance", unityActivity);
        }
#endif
    }
    void startService()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            customClass.CallStatic("StartCheckerService");
        }
#endif
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("GetMouseButtonDown");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999.0f, ClickMask))
            {
                Debug.Log("Physics.Raycast");
                RDConnection.Write.UpdateCharacterDestination(hit.point, (task) => { });
            }
        }
        //timer += Time.deltaTime;
        //timer1 += Time.deltaTime;

        //if(timer > 5.0f)
        //{
        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    result["time"] = timer1;
        //    RDConnection.Write.Test(result, (res) => {
        //        if (res.IsFaulted)
        //        {
        //            Debug.Log("UpdateCharacterLocation Faild");
        //            return;
        //        }
        //        else if (res.IsCompleted)
        //        {
        //        }
        //    });
        //    timer = 0.0f;
        //}
        //if(isPaused)
        //{
        //    pauseCnt += Time.deltaTime;
        //}
        //if(pauseCnt > 10.0f)
        //{

        //    RDConnection.Write.RemoveUpdate();
        //}
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

    void OnPlaceCharacters()
    {
        Debug.Log("OnPlaceCharacters");
        RDConnection.Read.GetTownCharacters((task)=>
        {

            Debug.Log("OnPlaceCharacters111");
            if (task.IsFaulted)
            {
                Debug.Log("OnPlaceCharacters Failed");
            }
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot characters in snapshot.Children)
                {
                    Vector3 pos = Vector3.zero;
                    string cid = "";
                    string uid = "";
                    string resourcePath = "";
                    foreach (DataSnapshot character in characters.Children)
                    {
                        if (character.Key == "pos")
                        {
                            foreach (DataSnapshot p in character.Children)
                            {
                                if (p.Key == "x") float.TryParse(p.Value.ToString(), out pos.x);
                                if (p.Key == "y") float.TryParse(p.Value.ToString(), out pos.y);
                                if (p.Key == "z") float.TryParse(p.Value.ToString(), out pos.z);
                            }
                        }
                        if (character.Key == "cid") cid = character.Value.ToString();
                        if (character.Key == "uid") uid = character.Value.ToString();
                        if (character.Key == "ResourcePath") resourcePath = character.Value.ToString();
                    }

                    GameObject obj = Instantiate(Resources.Load("Character"), GameObject.Find("Users").transform) as GameObject;
                    obj.transform.position = pos;
                    obj.transform.Find("Canvas").Find("ID").GetComponent<TMPro.TextMeshProUGUI>().text = cid;
                    obj.AddComponent<CharacterMove>();
                    obj.GetComponent<CharacterMove>().uid = uid;
                    obj.GetComponent<CharacterMove>().cid = cid;
                    GameObject obj1 = Instantiate(Resources.Load(resourcePath), obj.transform) as GameObject;
                    //obj1.AddComponent<CharacterMove>();
                }
                Debug.Log("OnPlaceCharacters222");
            }
        });
    }
}
