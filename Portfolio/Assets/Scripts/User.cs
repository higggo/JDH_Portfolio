using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://twnkls.com/en/blogs/howto-native-android-plugins-for-unity3d-2/
public class MyInfo
{
    private static MyInfo instance;
    public string cid;
    public string uid;
    public string resourcePath;
    public static MyInfo I
    {
        get
        {
            if (instance == null)
                instance = new MyInfo();
            return instance;
        }
    }

    public static void SetInfo(string cid, string uid, string resourcePath)
    {
        I.cid = cid;
        I.uid = uid;
        I.resourcePath = resourcePath;
    }
}

public class User : MonoBehaviour
{
    public string username;
    public string uid;
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
