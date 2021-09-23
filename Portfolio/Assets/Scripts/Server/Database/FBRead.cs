using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
public class FBRead : FBReference
{
    public void GetMyCharacterList(CallbackQuery callback)
    {
        Firebase.Firestore.Query capitalQuery = db.Collection("Users").WhereEqualTo("UID", FAuth.CurrentUser.UserId);
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            UnityMainThread.wkr.AddJob(() => { callback(task);});
        });
    }

    public void GetAllcharacter(CallbackQuery callback)
    {
        Firebase.Firestore.Query capitalQuery = db.Collection("Users");
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            UnityMainThread.wkr.AddJob(() => { callback(task);});
        });
    }
}