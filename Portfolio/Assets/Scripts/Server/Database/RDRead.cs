using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDRead : RDReference
{
    public void GetTownCharacters(CallbackData callback)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child("Town").GetValueAsync().ContinueWith(task => {
            Debug.Log(task.Result);
            //callback(task);
            UnityMainThread.wkr.AddJob(()=> { callback(task); });
        });
    }
}
