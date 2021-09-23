using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RealtimeUpdate_Core : MonoBehaviour
{
    public abstract void SetCharacterCamera(GameObject target, string uid);
    public abstract void CleanDatabase();
    public abstract void SetDestination();
#if UNITY_ANDROID
    public AndroidJavaClass unityClass;
    public AndroidJavaObject unityActivity;
    public AndroidJavaObject unityContext;
    public AndroidJavaClass customClass;
#endif
    DatabaseReference reference;

    public LayerMask ClickMask;

    public GameObject users = null;
    public GameObject monsters = null;
    public GameObject myCharacter = null;
    FirebaseAuth auth;
    FirebaseUser user;
    protected void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        StartCoroutine(UpdatePosServer());
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            sendActivityReference("com.higggo.service.mylibrary");

            startService();
        }
#endif
        if (users == null) users = GameObject.Find("Users");
        if (monsters == null) monsters = GameObject.Find("Monsters");
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
    protected void Update()
    {
        SetDestination();
    }
    public void GameQuit()
    {
        foreach (Transform character in users.transform)
        {
            character.GetComponent<Character>().Status.StopAllListener();
        }

        foreach (Transform monster in monsters.transform)
        {
            monster.GetComponent<Monster>().Status.StopAllListener();
        }
        reference.ChildAdded -= HandleTownCharacterChildAdded;
        reference.ChildRemoved -= HandleTownCharacterChildRemoved;
        CleanDatabase();
        myCharacter = null;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void OnApplicationQuit()
    {
        GameQuit();
    }

    //void OnApplicationFocus(bool hasFocus)
    //{
    //    foreach(Transform character in users.transform)
    //    {
    //        character.GetComponent<CharacterMove>().AddListener();
    //    }

    //    foreach (Transform monster in monsters.transform)
    //    {
    //        monster.GetComponent<Monster>().AddListener();
    //    }
    //    if (reference == null)
    //    {
    //        reference.ChildAdded += HandleTownCharacterChildAdded;
    //        reference.ChildRemoved += HandleTownCharacterChildRemoved;
    //    }
    //    isPaused = !hasFocus;
    //}
    //void OnApplicationPause(bool pauseStatus)
    //{
    //    foreach (Transform character in users.transform)
    //    {
    //        character.GetComponent<CharacterMove>().RemoveListener();
    //    }

    //    foreach (Transform monster in monsters.transform)
    //    {
    //        monster.GetComponent<Monster>().RemoveListener();
    //    }
    //    reference.ChildAdded -= HandleTownCharacterChildAdded;
    //    reference.ChildRemoved -= HandleTownCharacterChildRemoved;
    //    reference = null;
    //    isPaused = pauseStatus;
    //}

    void ServerAysnInitialize()
    {
        //RDConnection.Listener.TownCharacterAddListener(HandleTownCharacterChildAdded);
        //RDConnection.Listener.TownCharacterRemoveListener(HandleTownCharacterChildRemoved);

        reference = FirebaseDatabase.DefaultInstance.GetReference("Users").Child("Town");
        reference.ChildAdded += HandleTownCharacterChildAdded;
        reference.ChildRemoved += HandleTownCharacterChildRemoved;

        // Init Town Monsters
        RDConnection.Read.GetTownMosters((task) => {
            foreach (DataSnapshot monsterIDData in task.Result.Children)
            {
                string monsterID = monsterIDData.Key;
                string resourcePath = "";
                string location = "";
                Vector3 pos = Vector3.zero;
                float maxHP = 0.0f;
                Debug.Log("monsterID : " + monsterIDData);
                foreach (DataSnapshot monsterData in monsterIDData.Children)
                {
                    Debug.Log("monsterData : " + monsterData);
                    if (monsterData.Key == "ResourcePath")
                    {
                        resourcePath = monsterData.Value.ToString();
                    }
                    if (monsterData.Key == "Location")
                    {
                        location = monsterData.Value.ToString();
                    }
                    if (monsterData.Key == "Pos")
                    {
                        foreach (DataSnapshot monsterPos in monsterData.Children)
                        {
                            if (monsterPos.Key == "X") float.TryParse(monsterPos.Value.ToString(), out pos.x);
                            if (monsterPos.Key == "Y") float.TryParse(monsterPos.Value.ToString(), out pos.y);
                            if (monsterPos.Key == "Z") float.TryParse(monsterPos.Value.ToString(), out pos.z);
                        }
                    }
                    if (monsterData.Key == "MaxHP")
                    {
                       float.TryParse(monsterData.Value.ToString(), out maxHP);
                    }
                }
                GameObject monster = Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, monsters.transform) as GameObject;
                monster.GetComponent<Monster>().Initialize(monsterID, resourcePath, location, pos, maxHP);
                Units.Monsters.Add(monsterID, monster);
            }
        });
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
        UnityMainThread.wkr.AddJob(() => {
            OnPlaceCharacters(args.Snapshot);
        });
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
            if (character.Key == "CID") cid = character.Value.ToString();
            if (character.Key == "UID") uid = character.Value.ToString();
        }
        foreach(string ChasedID in Units.Users[cid].GetComponent<Character>().ChasedObjects)
        {
            Units.Monsters[ChasedID].GetComponent<Monster>().AttackTarget = null;
        }
        Units.Users[cid].GetComponent<Character>().Logout();
        Units.Users.Remove(cid);
    }
    void OnPlaceCharacters(DataSnapshot dataSnapshot)
    {
        Debug.Log("OnPlaceCharacters");
        Vector3 pos = Vector3.zero;
        string cid = "";
        string uid = "";
        string resourcePath = "";
        foreach (DataSnapshot characterData in dataSnapshot.Children)
        {
            if (characterData.Key == "Pos")
            {
                foreach (DataSnapshot p in characterData.Children)
                {
                    if (p.Key == "X") float.TryParse(p.Value.ToString(), out pos.x);
                    if (p.Key == "Y") float.TryParse(p.Value.ToString(), out pos.y);
                    if (p.Key == "Z") float.TryParse(p.Value.ToString(), out pos.z);
                }
            }
            if (characterData.Key == "CID") cid = characterData.Value.ToString();
            if (characterData.Key == "UID") uid = characterData.Value.ToString();
            if (characterData.Key == "ResourcePath") resourcePath = "PlayCharacter/" + characterData.Value.ToString();
        }
        GameObject character = Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, users.transform) as GameObject;
        character.GetComponent<Character>().Initialize(uid, cid);
        Units.Users.Add(cid, character);
    }
    IEnumerator UpdatePosServer()
    {
        while (true)
        {
            if (Units.MyCharacter != null)
            {
                Dictionary<string, object> childUpdates = new Dictionary<string, object>();
                childUpdates["Users/Town/" + FAuth.CurrentUser.UserId + "/Pos/X"] = Units.MyCharacter.transform.position.x;
                childUpdates["Users/Town/" + FAuth.CurrentUser.UserId + "/Pos/Y"] = Units.MyCharacter.transform.position.y;
                childUpdates["Users/Town/" + FAuth.CurrentUser.UserId + "/Pos/Z"] = Units.MyCharacter.transform.position.z;
                FirebaseDatabase.DefaultInstance.RootReference.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
                {
                    //UnityMainThread.wkr.AddJob(() => { });
                });
            }
            yield return new WaitForSeconds(1);
        }
    }
}
