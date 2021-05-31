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

        GameObject obj;
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
        reference.Child("users").Child(user.UserId).RemoveValueAsync();
    } 

    public void NextCharacter()
    {
        CollectionCharacters[selectNum].Character.SetActive(false);
        selectNum++;
        if(selectNum > CollectionCharacters.Count - 1)
            selectNum = 0;
        CollectionCharacters[selectNum].Character.SetActive(true);
    }

    public void PreCharacter()
    {
        CollectionCharacters[selectNum].Character.SetActive(false);
        selectNum--;
        if (selectNum < 0)
            selectNum = CollectionCharacters.Count-1;
        CollectionCharacters[selectNum].Character.SetActive(true);
    }

    public void SelectCharacter()
    {
            DocumentReference docRef = db.Collection("user").Document("info");
            Dictionary<string, object> userInfo = new Dictionary<string, object>
             {
                { "CharacterPath", CollectionCharacters[selectNum].path },
                { "uid", user.UserId },
                { "cid", ID_Inputfield.text }
            };

             docRef.SetAsync(userInfo).ContinueWithOnMainThread(task => {
                Debug.Log("Create User Characater");
            });
    }
}
