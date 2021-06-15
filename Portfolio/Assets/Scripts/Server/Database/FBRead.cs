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
        Firebase.Firestore.Query capitalQuery = db.Collection("user").WhereEqualTo("uid", FAuth.CurrentUser.UserId);
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            ThreadDispatcher.I.RunOnMainThread(() => { callback(task); return 0; });
        });
    }

    public void GetAllcharacter(CallbackQuery callback)
    {
        Firebase.Firestore.Query capitalQuery = db.Collection("user");
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            ThreadDispatcher.I.RunOnMainThread(() => { callback(task); return 0; });
        });
    }
}