using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //private static Billboard instance;
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cam.forward);
    }
    //public static Billboard I
    //{
    //    get
    //    {
    //        instance = FindObjectOfType(typeof(Billboard)) as Billboard;
    //        if (instance == null)
    //        {
    //            GameObject obj = Instantiate(Resources.Load("Billboard")) as GameObject;
    //            obj.name = "Billboard";
    //            instance = obj.GetComponent<Billboard>();
    //        }
    //        return instance;
    //    }
    //}

    //public void SetMainCamera()
    //{
    //    cam = Camera.main.transform;
    //}
}
