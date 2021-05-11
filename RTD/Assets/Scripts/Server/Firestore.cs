using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
public class Firestore : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Text;

    Firebase.Auth.FirebaseAuth auth;
    string googleIdToken;
    string googleAccessToken;

    // Start is called before the first frame update
    void Start()
    {
    //    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //    CollectionReference citiesRef = db.Collection("cities");
    //    citiesRef.Document("SF").SetAsync(new Dictionary<string, object>(){
    //    { "Name", "San Francisco" },
    //    { "State", "CA" },
    //    { "Country", "USA" },
    //    { "Capital", false },
    //    { "Population", 860000 }
    //}).ContinueWithOnMainThread(task =>
    //    citiesRef.Document("LA").SetAsync(new Dictionary<string, object>(){
    //        { "Name", "Los Angeles" },
    //        { "State", "CA" },
    //        { "Country", "USA" },
    //        { "Capital", false },
    //        { "Population", 3900000 }
    //    })
    //    ).ContinueWithOnMainThread(task =>
    //        citiesRef.Document("DC").SetAsync(new Dictionary<string, object>(){
    //        { "Name", "Washington D.C." },
    //        { "State", null },
    //        { "Country", "USA" },
    //        { "Capital", true },
    //        { "Population", 680000 }
    //        })
    //    ).ContinueWithOnMainThread(task =>
    //        citiesRef.Document("TOK").SetAsync(new Dictionary<string, object>(){
    //        { "Name", "Tokyo" },
    //        { "State", null },
    //        { "Country", "Japan" },
    //        { "Capital", true },
    //        { "Population", 9000000 }
    //        })
    //    ).ContinueWithOnMainThread(task =>
    //        citiesRef.Document("BJ").SetAsync(new Dictionary<string, object>(){
    //        { "Name", "Beijing" },
    //        { "State", null },
    //        { "Country", "China" },
    //        { "Capital", true },
    //        { "Population", 21500000 }
    //        })
    //    );

    //    FirebaseDatabase.DefaultInstance
    // .GetReference("GamePlay/System/Money")
    // .GetValueAsync().ContinueWith(task => {
    //     if (task.IsFaulted)
    //     {
    //         Debug.Log("server connect fail");
    //     }
    //     else if (task.IsCompleted)
    //     {
    //         DataSnapshot result = task.Result;
    //         Debug.Log(result.Value.ToString());
    //         uint _money = uint.Parse(result.Value.ToString());
    //     }
    // });
        
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential =
    Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    private void Update()
    {

    }

    public void ButtonClick()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;

            Text.text = name + ", " + email + ", " + uid;
            Debug.Log(name + ", " + email + ", " + uid);
        }
    }
}
