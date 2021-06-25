using Firebase.Database;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public delegate void CallbackData(Task<DataSnapshot> task);
public delegate void Callback(Task task);
public class RDConnection : MonoBehaviour
{
    private static RDWrite write;
    private static RDRead read;
    private static RDEventListener listener;
    public static RDWrite Write
    {
        get
        {
            write = FindObjectOfType(typeof(RDWrite)) as RDWrite;
            if (write == null)
            {
                write = GameObject.Find("Server").AddComponent<RDWrite>();
            }
            return write;
        }
    }

    public static RDRead Read
    {
        get
        {
            read = FindObjectOfType(typeof(RDRead)) as RDRead;
            if (read == null)
            {
                read = GameObject.Find("Server").AddComponent<RDRead>();
            }
            return read;
        }
    }
    public static RDEventListener Listener
    {
        get
        {
            //GameObject obj = Instantiate(Resources.Load("RDListener")) as GameObject;
            //return obj.GetComponent<RDEventListener>();
            listener = FindObjectOfType(typeof(RDEventListener)) as RDEventListener;
            if (listener == null)
            {
                listener = GameObject.Find("Server").AddComponent<RDEventListener>();
            }
            return listener;
        }
    }
}
