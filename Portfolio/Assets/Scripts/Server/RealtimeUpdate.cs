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
    DatabaseReference reference;
    public LayerMask ClickMask;
    public string sendID = "higggo";
    GameObject users;
    private void Start()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            sendActivityReference("com.higggo.service.mylibrary");

            startService();
        }
#endif
        users = GameObject.Find("Users");
        ClickMask = 1 << 8;
        ServerAysnInitialize();
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
    }
    
    public void OnApplicationQuit()
    {
        RDConnection.Write.RemoveCharacter();
        RDConnection.Write.RemoveDestination();
        reference.ChildAdded -= HandleTownCharacterChildAdded;
        reference.ChildRemoved -= HandleTownCharacterChildRemoved;
        foreach (Transform c in users.transform)
        {
            c.GetComponent<CharacterMove>().RemoveListener();
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        foreach(Transform c in users.transform)
        {
            c.GetComponent<CharacterMove>().AddListener();
        }
        if(reference == null)
        {
            reference.ChildAdded += HandleTownCharacterChildAdded;
            reference.ChildRemoved += HandleTownCharacterChildRemoved;
        }
        isPaused = !hasFocus;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        foreach (Transform c in users.transform)
        {
            c.GetComponent<CharacterMove>().RemoveListener();
        }
        reference.ChildAdded -= HandleTownCharacterChildAdded;
        reference.ChildRemoved -= HandleTownCharacterChildRemoved;
        reference = null;
        isPaused = pauseStatus;
        pauseCnt = 0.0f;
    }

    void ServerAysnInitialize()
    {
        //RDConnection.Listener.TownCharacterAddListener(HandleTownCharacterChildAdded);
        //RDConnection.Listener.TownCharacterRemoveListener(HandleTownCharacterChildRemoved);

        reference = FirebaseDatabase.DefaultInstance.GetReference("users/Town");
        reference.ChildAdded += HandleTownCharacterChildAdded;
        reference.ChildRemoved += HandleTownCharacterChildRemoved;
    }
    public void HandleTownCharacterChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        Debug.Log("HandleChildAdded : " + args.Snapshot);
        UnityMainThread.wkr.AddJob(() => { OnPlaceCharacters(args.Snapshot); });
        //OnPlaceCharacters(args.Snapshot);
    }
    public void HandleTownCharacterChildRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        Debug.Log("HandleChildAdded : " + args.Snapshot);

        RemoveCharacters(args.Snapshot);
    }
    void RemoveCharacters(DataSnapshot dataSnapshot)
    {
        string cid = "";
        string uid = "";
        foreach (DataSnapshot character in dataSnapshot.Children)
        {
            if (character.Key == "cid") cid = character.Value.ToString();
            if (character.Key == "uid") uid = character.Value.ToString();
        }
        GameObject obj = GameObject.Find(cid);
        obj.GetComponent<CharacterMove>().RemoveListener();
        Destroy(obj);
    }
    void OnPlaceCharacters(DataSnapshot dataSnapshot)
    {
        Vector3 pos = Vector3.zero;
        string cid = "";
        string uid = "";
        string resourcePath = "";
        foreach (DataSnapshot characterData in dataSnapshot.Children)
        {
            if (characterData.Key == "pos")
            {
                foreach (DataSnapshot p in characterData.Children)
                {
                    if (p.Key == "x") float.TryParse(p.Value.ToString(), out pos.x);
                    if (p.Key == "y") float.TryParse(p.Value.ToString(), out pos.y);
                    if (p.Key == "z") float.TryParse(p.Value.ToString(), out pos.z);
                }
            }
            if (characterData.Key == "cid") cid = characterData.Value.ToString();
            if (characterData.Key == "uid") uid = characterData.Value.ToString();
            if (characterData.Key == "ResourcePath") resourcePath = "PlayCharacter/" + characterData.Value.ToString();
        }

        GameObject character = Instantiate(Resources.Load(resourcePath), GameObject.Find("Users").transform) as GameObject;
        character.transform.position = pos; 
        character.name = cid;
        character.GetComponent<CharacterMove>().uid = uid;
        character.GetComponent<CharacterMove>().cid = cid;
        character.transform.Find("CharacterCanvas").Find("ID").GetComponent<TMPro.TextMeshProUGUI>().text = cid;
        if(uid == FAuth.CurrentUser.UserId)
        {
            Debug.Log("Camera Change");
            character.transform.Find("SpringArm").Find("PlayerCamera").gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
            Billboard.I.SetMainCamera();
        }
    }
}
