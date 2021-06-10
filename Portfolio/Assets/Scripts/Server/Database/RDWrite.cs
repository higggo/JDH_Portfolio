using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDWrite : RDReference
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateCharacterLocation(Dictionary<string, object> characterInfo)
    {
        reference.Child("users").Child("Town").Child(FUser.I.UserId).UpdateChildrenAsync(characterInfo);
    }
}
