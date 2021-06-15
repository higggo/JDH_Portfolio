using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBReference : MonoBehaviour
{
    protected FirebaseFirestore db;
    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
}
