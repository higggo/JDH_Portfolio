using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDConnection : MonoBehaviour
{
    private static RDWrite write;
    private static RDRead read;

    public static RDWrite Write
    {
        get
        {
            if(write == null)
            {
            }
            return write;
        }
    }

    public static RDRead Read
    {
        get
        {
            if (read)
            {

            }
            return read;
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
