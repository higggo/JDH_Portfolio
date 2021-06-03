using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
public class CharacterCollection
{
    public GameObject Character;
    public string path;
}
public class CreateCharacters : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;
    FirebaseFirestore db;

    public Transform CollectionPool;
    public TMPro.TMP_InputField ID_Inputfield;

    public List<CharacterCollection> CollectionCharacters;

    int selectNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        CollectionCharacters = new List<CharacterCollection>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        if (CollectionPool == null) CollectionPool = GameObject.Find("Collection").transform;
        if (ID_Inputfield == null) ID_Inputfield = GameObject.Find("ID_InputField").GetComponent<TMPro.TMP_InputField>();

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/BasicCharacters");
        FileInfo[] info = dir.GetFiles("*.prefab");

        string path;
        foreach (FileInfo f in info)
        {
            CharacterCollection c = new CharacterCollection();
            path = "BasicCharacters/" + f.Name.Replace(".prefab", "");
            c.Character = Instantiate(Resources.Load(path), CollectionPool) as GameObject;
            c.Character.SetActive(false);
            c.path = path;
            CollectionCharacters.Add(c);
        }
        CollectionCharacters[selectNum].Character.SetActive(true);
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
    }
}
