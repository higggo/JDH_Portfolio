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
    public void HandleDestination(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        //foreach (DataSnapshot child in args.Snapshot.Children)
        //{
        //}

        if (args.Snapshot.Key == "Pos")
        {
            foreach (DataSnapshot pos in args.Snapshot.Children)
            {
                if (pos.Key == "X") float.TryParse(pos.Value.ToString(), out DestHandlerData.pos.x);
                if (pos.Key == "Y") float.TryParse(pos.Value.ToString(), out DestHandlerData.pos.y);
                if (pos.Key == "Z") float.TryParse(pos.Value.ToString(), out DestHandlerData.pos.z);
            }
        }
        if (args.Snapshot.Key == "UID") DestHandlerData.uid = args.Snapshot.Value.ToString();
        OnDestReceive?.Invoke();
        //Debug.Log("HandleCharacterDestinationChildAdded : " + args.Snapshot);
        //Debug.Log("HandleCharacterDestinationChildAdded Pos : " + DestHandlerData.pos);
    }

    public Broadcast OnAttackTargetReceive = null;
    public string AttackTarget = "";
    public void HandleAttackTarget(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if(args.Snapshot.Exists)
        {
            AttackTarget = args.Snapshot.Value.ToString(); 
            Debug.Log(args.Snapshot.Value.ToString());
            OnAttackTargetReceive?.Invoke();
        }
    }
}
