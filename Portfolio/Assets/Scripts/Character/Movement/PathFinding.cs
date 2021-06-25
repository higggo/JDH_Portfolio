using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    //public Animator myAnim;
    NavMeshPath path;
    Coroutine move = null;

    public float StopLerp = 0.25f;
    public float StartLerp = 0.1f;

    public float RotLerp = 0.02f;
    float MoveLerp = 0.0f;

    public Vector3 nextDest;

    CharacterEventHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponent<CharacterEventHandler>();
        handler.OnDestReceive += MoveTo;
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }

    public void MoveTo()
    {
        if (NavMesh.CalculatePath(this.transform.position, handler.DestHandlerData.pos, NavMesh.AllAreas, path))
        {
            Debug.Log("CalculatePath");
            if (move != null) StopCoroutine(move);
            move = StartCoroutine(Moving(path));
        }
    }

    IEnumerator Moving(NavMeshPath path)
    {
        float movespeed = 7.0f;
        int curpos = 1;
        Vector3 target = path.corners[curpos];
        Vector3 dir = target - this.transform.position;
        dir.y = 0.0f;
        float dist = dir.magnitude;
        dir.Normalize();

        while (curpos < path.corners.Length)
        {
            if (dist < 1.5f && (curpos == path.corners.Length - 1))
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
            float delta = Mathf.Lerp(Mathf.Epsilon, Time.deltaTime * movespeed, MoveLerp);
            //myAnim.SetFloat("Speed", delta / Time.deltaTime / movespeed);

            if (dist - delta <= Mathf.Epsilon)
            {
                delta = dist;
                ++curpos;
                if (curpos == path.corners.Length)
                {
                    this.transform.Translate(dir * delta, Space.World);
                    continue;
                }
                target = path.corners[curpos];
                dir = target - this.transform.position;
                dir.y = 0.0f;
                dist = dir.magnitude;
                dir.Normalize();
                //this.transform.forward = dir;


            }
            dist -= delta;
            //this.transform.rotation = Quaternion.Slerp(Quaternion.Euler(this.transform.forward), Quaternion.Euler(dir), Time.deltaTime * rotspeed);

            //this.transform.position += Vector3.Lerp(Vector3.zero, dir * delta, Time.deltaTime);
            this.transform.position += dir * delta;
            //this.transform.Translate(
            //    Vector3.Lerp(Vector3.zero, dir * delta, Time.deltaTime)
            //    ,
            //    Space.World);
            this.transform.forward = Vector3.Slerp(this.transform.forward, dir, RotLerp);
            //this.transform.forward = dir;
            //Vector3.Lerp(this.transform.position, dir * delta, Time.smoothDeltaTime);
            //this.transform.Translate(Vector3.Lerp(this.transform.position, dir * delta, Time.smoothDeltaTime), Space.World);

            yield return null;
        }
        //myAnim.SetFloat("Speed", 0.0f);
    }

}
