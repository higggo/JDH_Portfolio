using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class Moving : MonoBehaviour
{
    public GameObject slider;
    public GameObject Canvas;
    //GameObject HpBar;

    public Vector3[] wayPoints;     //이동할 포인트들을 배열에 저장
    public Vector3 CurrentPosition; //현재위치
    private int wayPointIndex = 0;  //이동 포인트 인덱스
    private float speed = 10f;       //속도

    float rotTime = 1.0f;
    float rotSumDelta = 0.0f;

    public UnityAction DestroySpawnDelegate;

    // Start is called before the first frame update
    void Start()
    {
        wayPoints = new Vector3[12];

        wayPoints.SetValue(new Vector3(-17.09f, 0, -4.71f), 0);
        wayPoints.SetValue(new Vector3(-8.668f, 0, -4.71f), 1);
        wayPoints.SetValue(new Vector3(14.86f, 0, -4.71f), 2);
        wayPoints.SetValue(new Vector3(14.86f, 0, 3.61f), 3);
        wayPoints.SetValue(new Vector3(6.89f, 0, 3.61f), 4);
        wayPoints.SetValue(new Vector3(6.89f, 0, -21.22f), 5);
        wayPoints.SetValue(new Vector3(14.77f, 0, -21.22f), 6);
        wayPoints.SetValue(new Vector3(14.77f, 0, -12.73f), 7);
        wayPoints.SetValue(new Vector3(-8.668f, 0, -12.73f), 8);
        wayPoints.SetValue(new Vector3(-8.668f, 0, -21.22f), 9);
        wayPoints.SetValue(new Vector3(-0.78f, 0, -21.22f), 10);
        wayPoints.SetValue(new Vector3(-0.78f, 0, 10.08f), 11);
        speed = GetComponent<CharacterStat>().moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPosition = transform.position;
        speed = GetComponent<CharacterStat>().moveSpeed;
        if (wayPointIndex < wayPoints.Length &&
            GetComponent<EnemyController>().GetState() != ENEMYSTATE.DEAD)
        {
            float step = Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(CurrentPosition, wayPoints[wayPointIndex], step);

            Vector3 dir = (wayPoints[wayPointIndex] - CurrentPosition).normalized;
            if (transform.forward != dir)
            {
                rotSumDelta += Time.deltaTime;      // 1초에 dir방향으로 회전
                // rot speed를 적용하려면? rotSumDelta * (rotspeed / 360.0f)
                transform.forward = Vector3.Slerp(transform.forward, dir, rotSumDelta / Mathf.Clamp(rotTime, 0.1f, 1.0f));
            }
            else
            {
                rotSumDelta = 0.0f;
            }

            if (Vector3.Distance(wayPoints[wayPointIndex], CurrentPosition) == 0f)
            {
                wayPointIndex++;
                if (wayPointIndex == wayPoints.Length)  //목적지에 도착시 에네미 캐릭터를 삭제합니다.
                {
                    DestroySpawnDelegate?.Invoke();
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}