using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public delegate void CallbackQuery(Task<QuerySnapshot> task);
public class FBConnection : MonoBehaviour
{
    private static FBWrite write;
    private static FBRead read;
    public static FBWrite Write
    {
        get
        {
            write = FindObjectOfType(typeof(FBWrite)) as FBWrite;
            if (write == null)
            {
                write = GameObject.Find("Server").AddComponent<FBWrite>();
            }
            return write;
        }
    }

    public static FBRead Read
    {
        get
        {
            read = FindObjectOfType(typeof(FBRead)) as FBRead;
            if (read == null)
            {
                read = GameObject.Find("Server").AddComponent<FBRead>();
            }
            return read;
        }
    }
}
