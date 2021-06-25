using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct DestHandlerData
{
    public Vector3 pos;
    public string uid;
}
public delegate void Broadcast();
public class CharacterEventHandler : MonoBehaviour
{
    public Broadcast OnDestReceive = null;
    public DestHandlerData DestHandlerData;
    public void HandleCharacterDestinationChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        foreach(DataSnapshot child in args.Snapshot.Children)
        {
            if(child.Key == "pos")
            {
                foreach (DataSnapshot pos in child.Children)
                {
                    if (pos.Key == "x") float.TryParse(pos.Value.ToString(), out DestHandlerData.pos.x);
                    if (pos.Key == "y") float.TryParse(pos.Value.ToString(), out DestHandlerData.pos.y);
                    if (pos.Key == "z") float.TryParse(pos.Value.ToString(), out DestHandlerData.pos.z);
                }
            }
            if(child.Key == "uid") DestHandlerData.uid = child.Value.ToString();
        }
        OnDestReceive?.Invoke();
        Debug.Log("HandleCharacterDestinationChildAdded : " + args.Snapshot);
    }
}
