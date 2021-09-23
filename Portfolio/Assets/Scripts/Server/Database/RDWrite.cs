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
        reference.Child("Users").Child("Town").Child(FAuth.CurrentUser.UserId).
            UpdateChildrenAsync(dictionary).ContinueWith(task =>
        {
            //
            //callback(task);
            UnityMainThread.wkr.AddJob(()=> { callback(task);});
        });
    }
    public void RemoveCharacter()
    {
        reference.Child("Users").Child("Town").Child(FAuth.CurrentUser.UserId).RemoveValueAsync();
    }
    public void RemoveDestination()
    {
        reference.Child("Destination").Child("Town").Child("Users").Child(FAuth.CurrentUser.UserId).RemoveValueAsync();
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
        Debug.Log("UpdateCharacterDestination");
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["Destination/Town/Users/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/Pos/X"] = dest.x.ToString();
        childUpdates["Destination/Town/Users/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/Pos/Y"] = dest.y.ToString();
        childUpdates["Destination/Town/Users/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/Pos/Z"] = dest.z.ToString();
        reference.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
            {
                //UnityMainThread.wkr.AddJob(() => { callback(task);});
            });
    }
    public void UpdateAttackTarget(string targetID, Callback callback)
    {
        //Debug.Log("UpdateAttackTarget");
        //Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        //childUpdates["Destination/Town/Users/" + FAuth.CurrentUser.UserId + "/" + FAuth.CID + "/AttackTarget"] = targetID;
        reference.Child("Destination").Child("Town").Child("Users").Child(FAuth.CurrentUser.UserId).Child(FAuth.CID).Child("AttackTarget").SetValueAsync(targetID);
        //reference.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
        //    {
        //        //UnityMainThread.wkr.AddJob(() => { callback(task);});
        //    });
    }

    public void UpdateMonsterDestination(string id, Vector3 dest, Callback callback)
    {
        string key = FirebaseDatabase.DefaultInstance.RootReference.Push().Key;

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["Destination/Town/Monsters/" + id + "/" + "RecordTime"] = key;
        childUpdates["Destination/Town/Monsters/" + id + "/" + "/Pos/X"] = dest.x;
        childUpdates["Destination/Town/Monsters/" + id + "/" + "/Pos/Y"] = dest.y;
        childUpdates["Destination/Town/Monsters/" + id + "/" + "/Pos/Z"] = dest.z;
        reference.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
        {
            //UnityMainThread.wkr.AddJob(() => { callback(task); });
        });
    }
}
