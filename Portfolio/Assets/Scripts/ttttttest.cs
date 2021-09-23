using Firebase.Database;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttttttest : MonoBehaviour
{
    DatabaseReference reference = null;
    DatabaseReference reference1 = null;

    public StatContainer<string> Destination;
    Vector3 pos;
    float x = 0.0f;
    string str;
    Firebase.Database.Query query;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseFirestore.DefaultInstance.Settings.PersistenceEnabled = false;
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        pos = Vector3.zero;
        str = "a";
        reference = FirebaseDatabase.DefaultInstance.GetReference("Destination1").Child(nameof(Destination));
        //SetAttribute(ref Destination, reference.Child(nameof(Destination)));
        //Destination.AddListener();

        //Destination.OnUpdate = () => {
        //    Debug.Log(Destination.GetValue());

        //};
        reference1 = FirebaseDatabase.DefaultInstance.GetReference("Destination1").Child(nameof(Destination));
        query = FirebaseDatabase.DefaultInstance.GetReference("Destination1").Child(nameof(Destination)).LimitToLast(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            Debug.Log(Firebase.Database.ServerValue.Timestamp);
        }

    }
    public void SetAttribute<T>(ref StatContainer<T> attr, DatabaseReference reference)
    {
        attr = new StatContainer<T>();
        attr.Reference(reference);
    }
    public class test
    {
        public Vector3 pos;
        public object timestamp;
        public test(Vector3 pos)
        {
            this.pos = pos;
            timestamp = ServerValue.Timestamp;
        }
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;
            result["pos/x"] = x;
            result["pos/y"] = y;
            result["pos/z"] = z;
            result["timestamp"] = ServerValue.Timestamp;

            return result;
        }
    }

    public class test2
    {
        public string str;

        public test2(string str)
        {
            this.str = str;
        }
    }
    public void UpdateCharacterDestination()
    {
        x += 1;
        pos.x = x;
        pos.y = x;
        pos.z = x;
        str += "a";
        
        Attribute<string> Item = new Attribute<string>(str);
        test t = new test(pos);
        test2 t2 = new test2(str);
        t.pos = pos;
        string json = JsonUtility.ToJson(t);
        string json2 = JsonUtility.ToJson(t2);
        string json3 = JsonUtility.ToJson(Item);

        json = json.Substring(0, json.Length - 1);

        string timestampAdd = @" , ""timestamp"": {"".sv"" : ""timestamp""} } ";
        json = json + timestampAdd;

        string key = reference.Push().Key;
        //reference.SetRawJsonValueAsync(t.ToDictionary().jso);
        reference.RemoveValueAsync();
        reference.Child(key).SetRawJsonValueAsync(json).ContinueWith((task) =>{
            //AddListener();
        });
    }
    public void RemoveListener()
    {
        query.ChildAdded -= FuncA;
        //reference3.ValueChanged -= FuncB;
    }
    public void AddListener()
    {
        query.ChildAdded += FuncA;
        //reference3.ValueChanged += FuncB;
    }
   void FuncA(object sender, ChildChangedEventArgs args)
    {
            test Task = JsonUtility.FromJson<test>(args.Snapshot.GetRawJsonValue());

        Debug.Log("Func AAAAA, "+Task.pos);
        //RemoveListener();
    }
   void FuncB(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("Func BBBBB");
    }

}
