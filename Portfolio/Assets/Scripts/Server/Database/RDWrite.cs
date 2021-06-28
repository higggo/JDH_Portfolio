using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class RDWrite : RDReference
{
    public void UpdateCharacterLocation(Dictionary<string, object> dictionary, Callback callback)
    {
        reference.Child("users").Child("Town").Child(FAuth.CurrentUser.UserId).
            UpdateChildrenAsync(dictionary).ContinueWith(task =>
        {
            //
            //callback(task);
            UnityMainThread.wkr.AddJob(()=> { callback(task);});
        });
    }
    public void RemoveCharacter()
    {
        reference.Child("users").Child("Town").Child(FAuth.CurrentUser.UserId).RemoveValueAsync();
    }
    public void RemoveDestination()
    {
        reference.Child("Destination").Child(FAuth.CurrentUser.UserId).Child(FAuth.CID).RemoveValueAsync();
    }
    public void Test(Dictionary<string, object> dictionary, Callback callback)
    {

        reference.Child("Test").UpdateChildrenAsync(dictionary).ContinueWith(task =>
            {
                UnityMainThread.wkr.AddJob(() => { callback(task);});
            });
    }

    public void UpdateCharacterDestination(Vector3 dest, Callback callback)
    {
        string key = FirebaseDatabase.DefaultInstance.RootReference.Child("Destination").Child("Destination").Push().Key;

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["Destination/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/" + key + "/pos/x"] = dest.x;
        childUpdates["Destination/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/" + key + "/pos/y"] = dest.y;
        childUpdates["Destination/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/" + key + "/pos/z"] = dest.z;
        reference.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
            {
                UnityMainThread.wkr.AddJob(() => { callback(task);});
            });
    }
}
