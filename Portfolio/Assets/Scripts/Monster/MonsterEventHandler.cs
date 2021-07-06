using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterEventHandler : MonoBehaviour
{
    public Broadcast OnDestReceive = null;
    MonsterTaskData TaskData;
    private void Start()
    {
        TaskData = GetComponent<MonsterTaskData>();
    }
    public void HandleDestinationChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        foreach (DataSnapshot child in args.Snapshot.Children)
        {
            if (child.Key == "pos")
            {
                foreach (DataSnapshot pos in child.Children)
                {
                    if (pos.Key == "x") float.TryParse(pos.Value.ToString(), out TaskData.NextDestination.x);
                    if (pos.Key == "y") float.TryParse(pos.Value.ToString(), out TaskData.NextDestination.y);
                    if (pos.Key == "z") float.TryParse(pos.Value.ToString(), out TaskData.NextDestination.z);
                }
            }

        }
        OnDestReceive?.Invoke();
        Debug.Log("HandleCharacterDestinationChildAdded : " + args.Snapshot);
    }
}
