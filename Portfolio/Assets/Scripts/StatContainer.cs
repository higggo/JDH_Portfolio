using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public class Utility
//{
//    public static Vector3 StrToVector(StringVector3 stringVector)
//    {
//        Vector3 vec = new Vector3();
//        float.TryParse(stringVector.x.ToString(), out vec.x);
//        float.TryParse(stringVector.y.ToString(), out vec.y);
//        float.TryParse(stringVector.z.ToString(), out vec.z);
//        return vec;
//    }
//}
//[System.Serializable]
//public struct StringVector3
//{
//    public string x;
//    public string y;
//    public string z;
//    public StringVector3(Vector3 vector)
//    {
//        x = vector.x.ToString();
//        y = vector.y.ToString();
//        z = vector.z.ToString();
//    }
//}
public struct Attribute<T>
{
    //public object updateTime;
    public T value;
    public Attribute(T value)
    {
        this.value = value;
    }
}

public class StatContainer<T>
{
    private Attribute<T> Item;
    private Attribute<T> Task;
    //private string timestamp = "";
    private DatabaseReference ReceiveReference;
    private DatabaseReference SendReference;
    private Firebase.Database.Query ListenerQuery;
    public UnityAction OnUpdate;
    public bool OnListener = false;
    public void Reference(DatabaseReference reference)
    {
        ReceiveReference = reference;
        SendReference = reference;
        ListenerQuery = ReceiveReference.LimitToLast(1);
    }

    public void Load(string savedData)
    {
        Task = JsonUtility.FromJson<Attribute<T>>(savedData);
    }

    public void HandlerValueChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if (args.Snapshot.Exists)
        {
            Load(args.Snapshot.GetRawJsonValue());
            OnUpdate?.Invoke();
        }
    }

    public void AddListener()
    {
        if (!OnListener)
        {
            ReceiveReference.ChildAdded += HandlerValueChanged;
            //ListenerQuery.ChildAdded += HandlerValueChanged;
            OnListener = true;
        }
    }

    public void RemoveListener()
    {
        if (OnListener)
        {
            ReceiveReference.ChildAdded -= HandlerValueChanged;
            //ListenerQuery.ChildAdded -= HandlerValueChanged;
            OnListener = false;
        }
    }

    public void SetValue(T value, Callback callback)
    {
        Item = new Attribute<T>(value);
        UpdateServer(callback);
    }
    
    public void SetValue(T value)
    {
        Item = new Attribute<T>(value);
        UpdateServer();
    }
    
    public T GetValue()
    {
        return Task.value;
    }

    void UpdateServer(Callback callback)
    {
        string json = JsonUtility.ToJson(Item);

        json = json.Substring(0, json.Length - 1);

        string timestampAdd = @" , ""timestamp"": {"".sv"" : ""timestamp""} } ";
        json = json + timestampAdd;

        string key = SendReference.Push().Key;
        SendReference.RemoveValueAsync();
        SendReference.Child(key).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            callback(task);
            //if (!task.IsFaulted)
            //{
            //    Debug.Log(json);
            //}
        });
    }
    void UpdateServer()
    {
        string json = JsonUtility.ToJson(Item);

        json = json.Substring(0, json.Length - 1);

        string timestampAdd = @" , ""timestamp"": {"".sv"" : ""timestamp""} } ";
        json = json + timestampAdd;

        string key = SendReference.Push().Key;
        SendReference.RemoveValueAsync();
        SendReference.Child(key).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            //callback(task);
            //UnityMainThread.wkr.AddJob(() => { callback(task); });
            //if(!task.IsFaulted)
            //{
            //    Debug.Log(json);
            //}
        });
    }
    public void Clear()
    {
        SendReference.RemoveValueAsync();
    }
}
