using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAuth : MonoBehaviour
{
    FirebaseAuth auth;
    protected static FirebaseUser user;
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        //user = auth.CurrentUser;
        //Debug.Log("FAuth.cs__ user : " + user.UserId);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
