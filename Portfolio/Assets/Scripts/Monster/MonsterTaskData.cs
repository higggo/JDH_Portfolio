using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTaskData : MonoBehaviour
{
    Monster Monster;
    Attribute<float> HP;
    Attribute<float> MP;
    Attribute<Vector3> Destination;
    // Start is called before the first frame update
    void Start()
    {
        HP = new Attribute<float>();
        MP = new Attribute<float>();
        Destination = new Attribute<Vector3>();
    }
    public void Initialize()
    {
        Monster = GetComponent<Monster>();
        Monster.Status.Destination.OnUpdate += SetDestination;
    }
    public void SetHP(Attribute<float> attr)
    {
        HP = attr;
    }
    public void SetDestination()
    {
        //Destination = Monster.Status.Destination.Task;
    }
}
