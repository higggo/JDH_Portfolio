using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDRead : RDReference
{
    public void GetTownCharacters(CallbackData callback)
    {
        reference.Child("users/Town").GetValueAsync().ContinueWith(task => {
            ThreadDispatcher.I.RunOnMainThread(()=> { callback(task); return 0; });
        });

        RDConnection.Listener.TownCharacterAddListener();
    }
}
