using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Firebase.Database;
using Firebase.Auth;
public class CreateCharacters : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;

    public Transform Collection;

    int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        if (Collection == null) Collection = GameObject.Find("Collection").transform;
        //Instantiate(Resources.Load("BasicCharacters/BowHitman"), Collection);

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/BasicCharacters");
        FileInfo[] info = dir.GetFiles("*.prefab");

        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
            
            Instantiate(Resources.Load("BasicCharacters/"+ f.Name.Replace(".prefab", "")), Collection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
