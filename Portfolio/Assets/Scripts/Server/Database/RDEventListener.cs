using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDEventListener : RDReference
{
    public void TownCharacterAddListener(System.EventHandler<Firebase.Database.ChildChangedEventArgs> handle)
    {
        reference.Child("Users").Child("Town").ChildAdded += handle;
    }
    public void TownCharacterRemoveListener(System.EventHandler<Firebase.Database.ChildChangedEventArgs> handle)
    {
        reference.Child("Users").Child("Town").ChildRemoved += handle;
    }

    public void CharacterDestinationAddListener(ref DatabaseReference reference, System.EventHandler<Firebase.Database.ChildChangedEventArgs> handle)
    {
        reference.LimitToLast(1).ChildAdded += handle;

    }
    public void CharacterDestinationRemoveListener(ref DatabaseReference reference, System.EventHandler<Firebase.Database.ChildChangedEventArgs> handle)
    {
        if (reference != null)
        {
            reference.LimitToLast(1).ChildAdded -= handle;
            reference = null;

            Debug.Log("Remove");
        }
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
