using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextField;
    // Start is called before the first frame update
    void Start()
    {
        TextField.text = MyInfo.I.cid;
        TextField.text += "\n" + MyInfo.I.uid;
        TextField.text += "\n" + MyInfo.I.resourcePath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetCharactger()
    {
    }
}
