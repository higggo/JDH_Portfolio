using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAuth : MonoBehaviour
{
    private static string cid;
    public static FirebaseUser CurrentUser
    {
        get
        {
            return FirebaseAuth.DefaultInstance.CurrentUser;
        }
    }
    public static string CID
    {
        get
        {
            return cid;
        }
        set
        {
            cid = value;
        }
    }
}
