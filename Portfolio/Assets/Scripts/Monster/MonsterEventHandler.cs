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
    public void HandleDestinationChildAdded(object sender, ValueChangedEventArgs args)
    {
        //Debug.Log("HandleDestinationChildAdded");
        if (args.DatabaseError != null)
        {
            //Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if (args.Snapshot.Key == "Pos")
        {
            foreach (DataSnapshot pos in args.Snapshot.Children)
            {
                //if (pos.Key == "X") float.TryParse(pos.Value.ToString(), out TaskData.NextDestination.x);
                //if (pos.Key == "Y") float.TryParse(pos.Value.ToString(), out TaskData.NextDestination.y);
                //if (pos.Key == "Z") float.TryParse(pos.Value.ToString(), out TaskData.NextDestination.z);
            }
        }
        OnDestReceive?.Invoke();
        //Debug.Log("HandleCharacterDestinationChildAdded : " + args.Snapshot);
    }
}
