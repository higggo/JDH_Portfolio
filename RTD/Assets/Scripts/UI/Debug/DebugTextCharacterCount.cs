using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTextCharacterCount : MonoBehaviour
{
    public TileManager TileManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.name == "GroundCharacterCount")
            GetComponent<TMPro.TextMeshPro>().text = TileManager.GetCountGroundCharacter().ToString();
        if(gameObject.name == "StorageCharacterCount")
            GetComponent<TMPro.TextMeshPro>().text = TileManager.GetCountStorageCharacter().ToString();
    }
}
