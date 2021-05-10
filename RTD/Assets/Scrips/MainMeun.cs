using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeun : MonoBehaviour
{
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNewGame()
    {

    }

    public void OnClickLoad()
    {


    }

    public void OnClickOption()
    {


    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
