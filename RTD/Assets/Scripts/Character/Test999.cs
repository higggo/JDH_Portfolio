using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test999 : MonoBehaviour
{
    float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (deltaTime > 5.0f)
        {
            Instantiate(Resources.Load("TEST/TestPrefab"), this.transform, true);
            deltaTime = 0.0f;
        }
        deltaTime += Time.deltaTime;
    }
}
