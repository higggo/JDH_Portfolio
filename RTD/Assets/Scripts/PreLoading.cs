using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLoading : MonoBehaviour
{
    bool succeed = false;
    bool start = false;
    public GameObject IntroPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IntroPanel.activeSelf && !succeed && !start)
        {
            ExecutePreLoading();
            start = true;
        }

        if (succeed)
        {
            gameObject.SetActive(false);
        }
    }

    void ExecutePreLoading()
    {
        //GameDB DB = GameObject.Find("GamePlayManager").GetComponent<GameDB>();
        //foreach(string addr in DB.PreLoadingPrefabAddr)
        //{
        //    GameObject obj = Instantiate(Resources.Load(addr)) as GameObject;
        //    Destroy(obj);
        //}
        StartCoroutine(WaitLoading());
    }
    

    IEnumerator WaitLoading()
    {
        float time = 0;
        while(time < 2)
        {
            time += Time.deltaTime;
            yield return null;
        }
        succeed = true;
    }
}
