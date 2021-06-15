using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDReference : MonoBehaviour
{
    protected DatabaseReference reference;

    void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
