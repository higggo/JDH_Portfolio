using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
