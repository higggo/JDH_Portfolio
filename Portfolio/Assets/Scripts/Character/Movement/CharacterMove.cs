using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Firebase.Database;
using Firebase.Auth;

public class CharacterMove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //auth = FirebaseAuth.DefaultInstance;

        //user = auth.CurrentUser;

    }

    //void HandleValueChanged(object sender, ValueChangedEventArgs args)
    //{
    //    if (args.DatabaseError != null)
    //    {
    //        Debug.LogError(args.DatabaseError.Message);
    //        return;
    //    }

    //    if(args.Snapshot.HasChild(user.UserId))
    //    {
    //        User newValue = JsonUtility.FromJson<User>(args.Snapshot.Child(user.UserId).GetRawJsonValue());
    //        Debug.Log(newValue.pos);
    //        transform.position = newValue.pos;
    //    }
    //}


}