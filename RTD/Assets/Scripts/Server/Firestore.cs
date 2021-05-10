using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;
public class Firestore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference citiesRef = db.Collection("cities");
        citiesRef.Document("SF").SetAsync(new Dictionary<string, object>(){
        { "Name", "San Francisco" },
        { "State", "CA" },
        { "Country", "USA" },
        { "Capital", false },
        { "Population", 860000 }
    }).ContinueWithOnMainThread(task =>
        citiesRef.Document("LA").SetAsync(new Dictionary<string, object>(){
            { "Name", "Los Angeles" },
            { "State", "CA" },
            { "Country", "USA" },
            { "Capital", false },
            { "Population", 3900000 }
        })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("DC").SetAsync(new Dictionary<string, object>(){
            { "Name", "Washington D.C." },
            { "State", null },
            { "Country", "USA" },
            { "Capital", true },
            { "Population", 680000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("TOK").SetAsync(new Dictionary<string, object>(){
            { "Name", "Tokyo" },
            { "State", null },
            { "Country", "Japan" },
            { "Capital", true },
            { "Population", 9000000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("BJ").SetAsync(new Dictionary<string, object>(){
            { "Name", "Beijing" },
            { "State", null },
            { "Country", "China" },
            { "Capital", true },
            { "Population", 21500000 }
            })
        );

        FirebaseDatabase.DefaultInstance
     .GetReference("GamePlay/System/Money")
     .GetValueAsync().ContinueWith(task => {
         if (task.IsFaulted)
         {
             Debug.Log("server connect fail");
         }
         else if (task.IsCompleted)
         {
             DataSnapshot result = task.Result;
             Debug.Log(result.Value.ToString());
             uint _money = uint.Parse(result.Value.ToString());
         }
     });
    }
}
