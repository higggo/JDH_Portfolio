using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
public class CreateCharacters : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;
    // Start is called before the first frame update
    void Start()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        user = auth.CurrentUser;


        FirebaseDatabase.DefaultInstance
          .GetReference("users")
          .GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted)
              {
                  // Handle the error...
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  // Do something with snapshot...
              }
          });
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
