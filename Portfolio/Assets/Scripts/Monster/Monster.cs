using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // Start is called before the first frame update
    public string ID;
    public string resourcePath;
    public string location;
    public Vector3 initPos;

    public MonsterAnimEvent AnimEvent = null;
    public MonsterEventHandler EventHandler = null;
    public MonsterStateMachine StateMachine = null;
    public MonsterTaskData TaskData = null;
    public NavMeshAgent NavMeshAgent = null;
    public Animator Animator = null;
    public MonsterRangeSystem RangeSys = null;

    DatabaseReference reference;

    void Start()
    {
        AnimEvent = this.GetComponent<MonsterAnimEvent>();
        EventHandler = this.GetComponent<MonsterEventHandler>();
        StateMachine = this.GetComponent<MonsterStateMachine>();
        TaskData = this.GetComponent<MonsterTaskData>();
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
        Animator = this.GetComponent<Animator>();
        RangeSys = this.GetComponent<MonsterRangeSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddListener()
    {
        if (reference == null)
        {
            reference = FirebaseDatabase.DefaultInstance.GetReference("Destination/Town/Monster/" + ID);
            reference.ChildAdded += EventHandler.HandleDestinationChildAdded;
        }
    }
    public void RemoveListener()
    {
        reference.ChildAdded -= EventHandler.HandleDestinationChildAdded;
        reference = null;
    }
}

