using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    private static Dictionary<string, GameObject> _monsters = null;
    private static Dictionary<string, GameObject> _users = null;

    private static GameObject _myCharacter = null;

    public static Dictionary<string, GameObject> Monsters
    {
        get
        {
            if (_monsters == null)
            {
                _monsters = new Dictionary<string, GameObject>();
            }
            return _monsters;
        }
    }
    public static Dictionary<string, GameObject> Users
    {
        get
        {
            if (_users == null)
            {
                _users = new Dictionary<string, GameObject>();
            }
            return _users;
        }
    }
    public static GameObject MyCharacter
    {
        get
        {
            return _myCharacter;
        }
        set
        {
            _myCharacter = value;
        }
    }
}
