using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PathFinding : MonoBehaviour
{
    //public Animator myAnim;
    NavMeshPath path;
    Coroutine move = null;
    public bool isMoving = false;
    public float StopLerp = 0.25f;
    public float StartLerp = 0.1f;

    public float RotLerp = 0.02f;
    public float MoveLerp = 0.0f;

    public Vector3 nextDest;

    public float CurrentSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
    }
    public void StopMoving()
    {
        if (move != null) StopCoroutine(move);
        CurrentSpeed = 0.0f;
        isMoving = false;
    }
    public void MoveTo(Vector3 pos, float maxSpeed)
    {
        if (NavMesh.CalculatePath(this.transform.position, pos, NavMesh.AllAreas, path))
        {
            if (move != null) StopMoving();
            move = StartCoroutine(Moving(path, maxSpeed));
        }
    }

    IEnumerator Moving(NavMeshPath path, float maxSpeed)
    {
        isMoving = true;
        float movespeed = maxSpeed;
        int cornerNum = 1;
        Vector3 target = path.corners.Length > 1 ? path.corners[cornerNum] : transform.position;

        Vector3 dir = target - this.transform.position;
        dir.y = 0.0f;
        float dist = dir.magnitude;
        dir.Normalize();
        while (dist > 0.0f)
        {
            if (dist < 1.5f && (cornerNum == path.corners.Length - 1))
            {
                MoveLerp -= Time.deltaTime * movespeed * StopLerp;
                MoveLerp = Mathf.Clamp(MoveLerp, 0.25f, 0.95f);
            }
            else
            {
                MoveLerp += Time.deltaTime * movespeed * StartLerp;
                MoveLerp *= (Vector3.Dot(this.transform.forward, dir) + 1) / 2;
                MoveLerp = Mathf.Clamp(MoveLerp, 0.55f, 1.0f);
            }
            float delta = Mathf.Lerp(0.0f, Time.deltaTime * movespeed, MoveLerp);
            delta = dist - delta <= 0.0f ? dist : delta;
            dist -= delta;
            CurrentSpeed = delta / Time.deltaTime;
            this.transform.Translate(dir * delta, Space.World);
            this.transform.forward = Vector3.Slerp(this.transform.forward, dir, RotLerp);
            if (dist <= 0.0f && ++cornerNum < path.corners.Length)
            {
                target = path.corners[cornerNum];
                dir = target - this.transform.position;
                dir.y = 0.0f;
                dist = dir.magnitude;
                dir.Normalize();
            }
            yield return null;

        }
        isMoving = false;
        {
            //while (cornerNum < path.corners.Length)
            //{
            //    if (dist < 1.5f && (cornerNum == path.corners.Length - 1))
            //    {
            //        MoveLerp -= Time.deltaTime * movespeed * StopLerp;
            //        MoveLerp = Mathf.Clamp(MoveLerp, 0.25f, 0.95f);
            //    }
            //    else
            //    {
            //        MoveLerp += Time.deltaTime * movespeed * StartLerp;
            //        MoveLerp *= (Vector3.Dot(this.transform.forward, dir) + 1) / 2;
            //        MoveLerp = Mathf.Clamp(MoveLerp, 0.55f, 1.0f);
            //    }
            //    float delta = Mathf.Lerp(0.0f, Time.deltaTime * movespeed, MoveLerp);
            //    //myAnim.SetFloat("Speed", delta / Time.deltaTime / movespeed);

            //    if (dist - delta <= 0.0f)
            //    {
            //        delta = dist;
            //        ++cornerNum;
            //        if (cornerNum == path.corners.Length)
            //        {
            //            this.transform.Translate(dir * delta, Space.World);
            //            CurrentSpeed = 0.0f;
            //            continue;
            //        }
            //        target = path.corners[cornerNum];
            //        dir = target - this.transform.position;
            //        dir.y = 0.0f;
            //        dist = dir.magnitude;
            //        dir.Normalize();
            //        //this.transform.forward = dir;


            //    }
            //    dist -= delta;
            //    CurrentSpeed = delta / Time.deltaTime;
            //    //this.transform.rotation = Quaternion.Slerp(Quaternion.Euler(this.transform.forward), Quaternion.Euler(dir), Time.deltaTime * rotspeed);

            //    //this.transform.position += Vector3.Lerp(Vector3.zero, dir * delta, Time.deltaTime);
            //    this.transform.Translate(dir * delta, Space.World);
            //    //this.transform.localPosition += dir * delta;
            //    //this.transform.Translate(
            //    //    Vector3.Lerp(Vector3.zero, dir * delta, Time.deltaTime)
            //    //    ,
            //    //    Space.World);
            //    this.transform.forward = Vector3.Slerp(this.transform.forward, dir, RotLerp);
            //    //this.transform.forward = dir;
            //    //Vector3.Lerp(this.transform.position, dir * delta, Time.smoothDeltaTime);
            //    //this.transform.Translate(Vector3.Lerp(this.transform.position, dir * delta, Time.smoothDeltaTime), Space.World);

            //    yield return null;
            //}
            //isMoving = false;
            ////myAnim.SetFloat("Speed", 0.0f);
            ///
        }
    }

}
