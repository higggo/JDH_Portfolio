using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInfo : MonoBehaviour
{
    private static MyInfo instance;
    string cid;
    string resourcePath;
    public static MyInfo I
    {
        get
        {
            return instance;
        }
    }

    public MyInfo(string cid, string resourcePath)
    {
        this.cid = cid;
        this.resourcePath = resourcePath;
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
