using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAuth : MonoBehaviour
{
    public static FirebaseUser CurrentUser
    {
        get
        {
            return FirebaseAuth.DefaultInstance.CurrentUser;
        }
    }
}
