using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDRead : RDReference
{
    public void GetTownCharacters(CallbackData callback)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child("Town").GetValueAsync().ContinueWith(task => {
            UnityMainThread.wkr.AddJob(()=> { callback(task); });
        });
    }

    public void GetTownMosters(CallbackData callback)
    {
        FirebaseDatabase.DefaultInstance.GetReference("monster").GetValueAsync().ContinueWith(task =>
        {
            UnityMainThread.wkr.AddJob(() => { callback(task); });
        });
    }
}
