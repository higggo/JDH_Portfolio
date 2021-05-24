using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class User
{
    public string username;
    public Vector3 pos;

    public User()
    {
    }

    public User(string username, Vector3 pos)
    {
        this.username = username;
        this.pos = pos;
    }
}

public class LeaderboardEntry
{
    public string uid;
    public Vector3 pos;


    public LeaderboardEntry()
    {
    }

    public LeaderboardEntry(string uid, Vector3 pos)
    {
        this.uid = uid;
        this.pos = pos;
    }

    public Dictionary<string, object> ToDictionary()
    {
       Dictionary<string, object> result = new Dictionary<string, object>();
       //result["uid"] = uid;
       result["x"] = pos.x;
       result["y"] = pos.y;
       result["z"] = pos.z;

        return result;
    }
}

public class CharacterMove : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;
    // Start is called before the first frame update
    void Start()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        user = auth.CurrentUser;

        TakeCharacterPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendCharactorPos(user.UserId, transform.position);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 deltaPos = Vector3.zero;
            deltaPos.x += Time.deltaTime;
            PushData(deltaPos);
        }
        if(Input.GetKey(KeyCode.A))
        {
            Vector3 deltaPos = Vector3.zero;
            deltaPos.x -= Time.deltaTime;
            PushData(deltaPos);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {

        }
        if(Input.GetKeyDown(KeyCode.W))
        {

        }
    }
    private void SendCharactorPos(string userId, Vector3 pos)
    {
        // Create new entry at /user-scores/$userid/$scoreid and at
        // /leaderboard/$scoreid simultaneously
        string key = reference.Child("scores").Push().Key;
        LeaderboardEntry entry = new LeaderboardEntry(userId, transform.position);
        Dictionary<string, object> entryValues = entry.ToDictionary();

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
       // childUpdates["/scores/" + key] = entryValues;
        childUpdates["/CharacterInfo/" + userId + "/pos"] = entryValues;
        //childUpdates["/user-scores/" + userId + "/" + key] = entryValues;

        reference.UpdateChildrenAsync(childUpdates);

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