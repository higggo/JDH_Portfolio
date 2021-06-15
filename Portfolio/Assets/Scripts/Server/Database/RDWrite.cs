using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class RDWrite : RDReference
{
    public void UpdateCharacterLocation(Dictionary<string, object> characterInfo, Callback callback)
    {
        reference.Child("users").Child("Town").Child(FAuth.CurrentUser.UserId).
            UpdateChildrenAsync(characterInfo).ContinueWith(task =>
        {
            ThreadDispatcher.I.RunOnMainThread(()=> { callback(task); return 0; });
            
        });
    }
    public void RemoveUpdate()
    {
        reference.Child("users/Town").Child(FAuth.CurrentUser.UserId).RemoveValueAsync();
    }

    public void Test(Dictionary<string, object> characterInfo, Callback callback)
    {

        reference.Child("Test").UpdateChildrenAsync(characterInfo).ContinueWith(task =>
            {
                ThreadDispatcher.I.RunOnMainThread(() => { callback(task); return 0; });

            });
    }
}
