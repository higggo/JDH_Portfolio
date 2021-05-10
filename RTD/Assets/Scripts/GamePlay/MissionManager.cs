using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterKit;
using UnityEngine.Events;


/**
 * 미션
 * 1. 수집
 * Ex)N법사7
 * Ex)M법사3 R법사3
 * 
 * 2. 몬스터사냥
 * Ex)헤비유닛 10마리 처치
 * Ex)몬스터 15마리 잡기
 * 
 * 
 * 
 * 
 * */


public class MissionManager : MonoBehaviour
{
    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    public List<Mission> MissionList = new List<Mission>();
    public List<Mission> CurrentMissions = new List<Mission>();
    ResponseMessage.Trade.CODE response;
    public GameObject MissionsVertical = null;

    private void Start()
    {
        if(MissionsVertical == null) MissionsVertical = GameObject.Find("Missions");
        

    }

    private void Update()
    {
        if(MissionsVertical.transform.childCount > 0)
        {
            foreach (Transform info in MissionsVertical.transform)
            {
                Mission mission = info.GetChild(0).GetComponent<MissionDeleteButton>().LinkedObj;
                mission.UpdateVerify();
                
                if (mission.State == Mission.STATE.Succeed)
                {
                    MissionReward(mission.Reward());
                    mission.MissionFinished();
                }
                info.Find("Message").GetComponent<TMPro.TextMeshProUGUI>().text = mission.UpdateMessage() + "\n" + "보상 " + mission.Reward().ToString() + "골드";
                //info.GetComponent<AutoSizing>().SetText(mission.UpdateMessage() + "\n" + "보상 " + mission.Reward().ToString() + "골드");
            }
        }
    }

    public void PushNewMission()
    {
        if(MissionsVertical.transform.childCount >= 2)
        {
            Debug.Log("미션이 가득 찼습니다.");
            return;
        }

        if (GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Pay, 50, response, "Buy Mission"))
        {
            bool none = true;
            MissionList = Shuffle<Mission>(MissionList);
            foreach (Mission mission in MissionList)
            {
                if (mission.State == Mission.STATE.Waiting)
                {
                    // 미션정보, 버튼 생성
                    GameObject MissionInfoUI = Instantiate(Resources.Load("UI/MissionInfo"), MissionsVertical.transform) as GameObject;

                    mission.Init(gameObject);

                    MissionInfoUI.transform.Find("Button").GetComponent<MissionDeleteButton>().LinkedObj = mission;
                    MissionInfoUI.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
                        if (mission.State != Mission.STATE.Finished)
                            mission.State = Mission.STATE.Waiting;
                        Destroy(MissionInfoUI);
                    });

                    none = true;
                    break;
                }
            }
            if (!none)
            {
                Debug.Log("추가할 수 있는 미션이 없습니다.");
                GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, 50, response, "Mission Pay Refund");
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다.[미션]");
        }
    }

    public void MissionReward(uint gold)
    {
        GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, gold, response, "Reward");
    }

    public void Init()
    {
        foreach(Transform child in MissionsVertical.transform)
        {
            Destroy(child.gameObject);
        }
        MissionList.Clear();
        MissionList.Add(new Mission_GetChar_NM7());
        MissionList.Add(new Mission_GetChar_MW2MM2MA2());
        MissionList.Add(new Mission_GetChar_MM3RM3());
        MissionList.Add(new Mission_GetChar_QM3());
        MissionList.Add(new Mission_GetChar_MM1RA1());
        MissionList.Add(new Mission_GetChar_RW4());
        MissionList.Add(new Mission_GetChar_RM2RA3());
        MissionList.Add(new Mission_GetChar_MW3MA2());
        MissionList.Add(new Mission_GetChar_RW2RM2());
        MissionList.Add(new Mission_GetChar_QW1QM1QA1());
        MissionList.Add(new Mission_GetChar_NW2NM2NA2());
        MissionList.Add(new Mission_GetChar_MW1MM2());
        MissionList.Add(new Mission_GetChar_RW1RM1RA1());
        MissionList.Add(new Mission_GetChar_RW2RM2QA2());
        MissionList.Add(new Mission_AllKillNextRound());
        MissionList.Add(new Mission_Kill_H10());
        MissionList.Add(new Mission_Kill_Monster15());
        MissionList.Add(new Mission_Kill_Monster20());
        MissionList.Add(new Mission_Kill_Monster30());
        MissionList.Add(new Mission_Kill_L15());
        MissionList.Add(new Mission_Kill_M15());
        MissionList.Add(new Mission_Kill_H5L5M5());
    }
    
}
