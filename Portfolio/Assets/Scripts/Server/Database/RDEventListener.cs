﻿using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDEventListener : RDReference
{
    public void TownCharacterAddListener()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child("Town").ChildAdded += HandleTownCharacterChildAdded;
    }
    void HandleTownCharacterChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        Debug.Log("HandleChildAdded : " + args.Snapshot);
        foreach(DataSnapshot child in args.Snapshot.Children)
        {
            Debug.Log("Town Handle : {" + child.Key + ", " + child.Value + "}");
        }
    }

    public void CharacterDestinationAddListener(string uid, string cid, System.EventHandler<Firebase.Database.ChildChangedEventArgs> handle)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Destination").Child(uid).Child(cid).LimitToLast(1).ChildAdded += handle;
        
    }
    void HandleCharacterDestinationChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        Debug.Log("HandleCharacterDestinationChildAdded : " + args.Snapshot);
    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
    }

    void HandleChildRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        Debug.Log("HandleChildRemoved : " + args.Snapshot);
    }

    void HandleChildMoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
    }
}
