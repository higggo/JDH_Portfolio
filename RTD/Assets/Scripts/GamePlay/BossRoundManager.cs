using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossRound : Round
{
    public GameObject boss;     // Instantiate obj
    public bool bossClear;         // flag Boss is Dead


    public BossRound(string addr, int enemyCount, int breakTime, uint reward)
        : base(addr, enemyCount, breakTime, reward)
    {
        battle = Type.Boss;
    }
}

public class BossRoundManager : MonoBehaviour
{
    public int Round { get; set; }

    List<BossRound> BossRoundList = new List<BossRound>();

    bool AllClear = true;

    public enum STATE
    {
        StandBy,
        GameStart,
        BossSpawn,
        RoundStart,
        BreakTime,
        BossClear,
        GameOver
    }
    public STATE state = STATE.StandBy;
    public STATE State
    {
        get { return state; }
        set { ChangeState(value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Round = 0;

        //BossRoundList.Add(new BossRound(GetComponent<GameDB>().Boss[0], 3, 5));

        ChangeState(STATE.GameStart);

        //GetComponent<CameraManager>().MoveDone += SpawnBoss;

    }

    public void Init()
    {
        Round = 0;
        for (int i = 0; i < BossRoundList.Count; i++)
        {
            if (BossRoundList[i].boss != null)
            {
                Destroy(BossRoundList[i].boss);
            }
            BossRoundList[i].boss = null;
            BossRoundList[i].bossClear = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (state)
        {
            case STATE.StandBy:
                break;
            case STATE.GameStart:
                break;
            case STATE.BossSpawn:
                break;
            case STATE.RoundStart:
                //for (int i = 0; i < BossRoundList.Count; i++)
                //{
                //    if(BossRoundList[i].boss != null)
                //        BossRoundList[i].boss.GetComponent<BossController>().SetCanAction(true);
                //}
                break;
            case STATE.BreakTime:
                //for (int i = 0; i < BossRoundList.Count; i++)
                //{
                //    // Boss Spawn
                //    if (BossRoundList[i].spawnRound == Round+1)
                //    {
                //        int num = i;
                //        StartCoroutine(GetComponent<CameraManager>().BossSpawn(()=> {
                //            Transform spawnSpot = GameObject.Find("BossSpawn").transform;
                //            BossRoundList[num].boss = Instantiate(Resources.Load(GetComponent<GameDB>().Boss[0])) as GameObject;
                //            BossRoundList[num].boss.transform.SetParent(spawnSpot.parent);
                //            BossRoundList[num].boss.transform.position = spawnSpot.position;
                //            StartCoroutine(GetComponent<CameraManager>().LookAroundBoss(BossRoundList[num].boss.transform));
                //            }));
                //        }

                //    // Stop Boss Attack
                //        if (BossRoundList[i].boss != null)
                //        BossRoundList[i].boss.GetComponent<BossController>().SetCanAction(false);

                //    //
                //    if (BossRoundList[i].endRound == Round)
                //    {
                //        // GameOver
                //        ChangeState(STATE.GameOver);
                //    }
                //}
                break;
            case STATE.BossClear:
                break;
            case STATE.GameOver:
                break;
        }
    }

     void StateProcess()
    {
        switch (state)
        {
            case STATE.StandBy:
                break;
            case STATE.GameStart:
                break;
            case STATE.BossSpawn:
                break;
            case STATE.RoundStart:
                {
                    // Boss Dead
                    //for (int i = 0; i < BossRoundList.Count; i++)
                    //{
                    //    if(BossRoundList[i].boss != null)
                    //    {
                    //        if (BossRoundList[i].boss.GetComponent<BossController>().isDead &&
                    //            !BossRoundList[i].bossClear)
                    //        {
                    //            // [i] Boss Clear
                    //            BossRoundList[i].bossClear = true;
                    //        }
                    //    }
                    //    if (!BossRoundList[i].bossClear)
                    //        AllClear = false;
                    //}
                    //if(AllClear)
                    //    ChangeState(STATE.BossClear);
                }
                break;
            case STATE.BreakTime:
                break;
            case STATE.BossClear:
                break;
            case STATE.GameOver:
                break;
        }
    }

    public void SpawnBoss(int i)
    {
    }

}
