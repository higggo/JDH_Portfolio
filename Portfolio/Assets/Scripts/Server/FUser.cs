using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUser : FAuth
{
    public static Firebase.Auth.FirebaseUser I
    {
        get
        {
            return user;
        }
    }
}
