using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Firebase.Database;
using Firebase.Auth;

public class CharacterMove : MonoBehaviour
{
    CharacterStat Status;
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
        Status = gameObject.AddComponent<CharacterStat>();
        MovePath = gameObject.AddComponent<PathFinding>();
        handler = gameObject.AddComponent<CharacterEventHandler>();
        // Get the root reference location of the database.
        //reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        user = auth.CurrentUser;

        AddListener();
    }

    public void AddListener()
    {
        if(reference == null)
        {
            reference = FirebaseDatabase.DefaultInstance.GetReference("Destination/" + uid + "/" + cid);
            reference.ChildAdded += GetComponent<CharacterEventHandler>().HandleCharacterDestinationChildAdded;
        }
    }
    public void RemoveListener()
    {
       // GetComponent<PathFinding>().handler.OnDestReceive -= GetComponent<PathFinding>().MoveTo;
        reference.ChildAdded -= GetComponent<CharacterEventHandler>().HandleCharacterDestinationChildAdded;
        reference = null;
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


}