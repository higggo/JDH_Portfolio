using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Firebase.Database;
using Firebase.Auth;

public class CharacterMove : MonoBehaviour
{
    PathFinding MovePath;
    CharacterEventHandler handler;
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;

    public string uid;
    public string cid;

    // Start is called before the first frame update
    void Start()
    {
        MovePath = gameObject.AddComponent<PathFinding>();
        handler = gameObject.AddComponent<CharacterEventHandler>();
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        user = auth.CurrentUser;

        RDConnection.Listener.CharacterDestinationAddListener(uid, cid, GetComponent<CharacterEventHandler>().HandleCharacterDestinationChildAdded);

        //TakeCharacterPos();
    }

    // Update is called once per frame
    void Update()
    {
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        Vector3 deltaPos = Vector3.zero;
    //        deltaPos.x += Time.deltaTime;
    //        PushData(deltaPos);
    //    }
    //    if(Input.GetKey(KeyCode.A))
    //    {
    //        Vector3 deltaPos = Vector3.zero;
    //        deltaPos.x -= Time.deltaTime;
    //        PushData(deltaPos);
    //    }
    //    if(Input.GetKeyDown(KeyCode.W))
    //    {

    //    }


    }
    private void TakeCharacterPos()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users")
        .ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if(args.Snapshot.HasChild(user.UserId))
        {
            User newValue = JsonUtility.FromJson<User>(args.Snapshot.Child(user.UserId).GetRawJsonValue());
            Debug.Log(newValue.pos);
            transform.position = newValue.pos;
        }
    }


    public void PushData(Vector3 delta)
    {
        UnityEngine.Assertions.Assert.IsNotNull(reference, "Database ref is null!");
        User temp = new User(name, transform.position + delta);
        string json = JsonUtility.ToJson(temp);        
        reference.Child("users").Child(user.UserId).SetRawJsonValueAsync(json);
    }

}