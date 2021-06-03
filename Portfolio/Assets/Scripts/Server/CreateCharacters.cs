﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
public class CreateCharacterInfo
{
    public GameObject Character;
    public string path;
    public int slot;
    public string cid;
}
public class CreateCharacters : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;
    FirebaseFirestore db;

    public Transform CollectionPool;
    public Transform MyCharacterPool;
    public TMPro.TMP_InputField ID_Inputfield;

    public List<CreateCharacterInfo> CollectionCharacters;
    public List<CreateCharacterInfo> MyCharacters;

    int selectNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        CollectionCharacters = new List<CreateCharacterInfo>();
        MyCharacters = new List<CreateCharacterInfo>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        if (CollectionPool == null) CollectionPool = GameObject.Find("CollectionPool").transform;
        if (MyCharacterPool == null) MyCharacterPool = GameObject.Find("MyCharacterPool").transform;
        if (ID_Inputfield == null) ID_Inputfield = GameObject.Find("ID_InputField").GetComponent<TMPro.TMP_InputField>();

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/BasicCharacters");
        FileInfo[] info = dir.GetFiles("*.prefab");

        string path;
        foreach (FileInfo f in info)
        {
            CreateCharacterInfo c = new CreateCharacterInfo();
            path = "BasicCharacters/" + f.Name.Replace(".prefab", "");
            c.Character = Instantiate(Resources.Load(path), CollectionPool) as GameObject;
            c.Character.SetActive(false);
            c.path = path;
            CollectionCharacters.Add(c);
        }
        CollectionCharacters[selectNum].Character.SetActive(true);

        GetCharacters();
    }

    public void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        reference.Child("users").Child(user.UserId).RemoveValueAsync();
    }

    public void LogOut()
    {
        Debug.Log("LogOut");
        reference.Child("users").Child(user.UserId).RemoveValueAsync();
    }

    public void NextCharacter()
    {
        CollectionCharacters[selectNum].Character.SetActive(false);
        selectNum++;
        if (selectNum > CollectionCharacters.Count - 1)
            selectNum = 0;
        CollectionCharacters[selectNum].Character.SetActive(true);
    }

    public void PreCharacter()
    {
        CollectionCharacters[selectNum].Character.SetActive(false);
        selectNum--;
        if (selectNum < 0)
            selectNum = CollectionCharacters.Count - 1;
        CollectionCharacters[selectNum].Character.SetActive(true);
    }

    public void CreateCharacter()
    {
        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "ResourcePath", CollectionCharacters[selectNum].path },
            { "uid", auth.CurrentUser.UserId },
            { "cid", ID_Inputfield.text }
        };
        db.Collection("user").AddAsync(user).ContinueWithOnMainThread(task => {
            DocumentReference addedDocRef = task.Result;
            Debug.Log(string.Format("Added document with ID: {0}.", addedDocRef.Id));
            Debug.Log("Create User Characater");
            Debug.Log("ResourcePath : " + CollectionCharacters[selectNum].path);
            Debug.Log("uid : " + auth.CurrentUser.UserId);
            Debug.Log("cid : " + ID_Inputfield.text);
        });
    }

    public void CompareID()
    {
        string cid = ID_Inputfield.text;
        if (cid == "")
        {
            Debug.Log("아이디를 입력해 주세요.");
            return;
        }

        Firebase.Firestore.Query allCitiesQuery = db.Collection("user").WhereEqualTo("cid", cid);
        allCitiesQuery.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshotTask =>
        {
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                Debug.Log("해당 아이디가 존재합니다.");
                return;
            }
            CreateCharacter();
        });

        Firebase.Firestore.Query capitalQuery = db.Collection("user").WhereEqualTo("uid", auth.CurrentUser.UserId);
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            QuerySnapshot capitalQuerySnapshot = task.Result;
            if(capitalQuerySnapshot.Count >= 3)
            {
                Debug.Log("슬롯이 가득 찼습니다.");
                return;
            }
        });
    }

    public void GetCharacters()
    {
        Firebase.Firestore.Query capitalQuery = db.Collection("user").WhereEqualTo("uid", auth.CurrentUser.UserId);
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            QuerySnapshot capitalQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
            {
                Debug.Log(string.Format("Document data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> city = documentSnapshot.ToDictionary();

                CreateCharacterInfo c = new CreateCharacterInfo();
                foreach (KeyValuePair<string, object> pair in city)
                {

                    Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                    if(pair.Key == "ResourcePath")
                    {
                        c.path = pair.Value.ToString();
                    }
                    if(pair.Key == "cid")
                    {
                        c.cid = pair.Value.ToString();
                    }
                }
                GameObject obj = Instantiate(Resources.Load("Character"), GetSlot()) as GameObject;
                obj.transform.Find("Canvas").Find("ID").GetComponent<TMPro.TextMeshProUGUI>().text = c.cid;
                c.Character = Instantiate(Resources.Load(c.path), obj.transform) as GameObject;

                MyCharacters.Add(c);

                // Newline to separate entries
                Debug.Log("");
            };
        });
    }

    Transform GetSlot()
    {
        Transform t = transform;
        for (int i=0; i < MyCharacterPool.childCount; i++)
        {
            if(MyCharacterPool.GetChild(i).childCount == 0)
            {
                t = MyCharacterPool.GetChild(i);
                break;
            }
        }

        return t;
    }
}
