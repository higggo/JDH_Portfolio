using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateThreadPoll : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        ThreadDispatcher.I.PollJobs();
    }
}
