using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    public LayerMask CrashMask;
    public Transform myCam;

    public float Rotspeed = 1.0f;
    public float RotSmoothspeed = 10.0f;
    public Vector3 TargetRot;

    public float ZoomSpeed = 3.0f;
    public float ZoomSmoothSpeed = 10.0f;
    public float TargetDist = 0.0f;
    public float CurDist = 0.0f;
    public float CollisionOffset = 1.0f;

    public Vector2 LookUpArea;
    public Vector2 ZoomArea;

    public bool ControllerRotate = true;

    private void Awake()
    {
        // Update와 Start중에 뭐가 먼저 실행될지 알 수 없다. 그래서 Awake문에서 실행
        //myCam = Camera.main.transform;
        myCam.position = this.transform.position + (-this.transform.forward * ZoomArea.y);
        //TargetDist = Mathf.Clamp(TargetDist, ZoomArea.x, ZoomArea.y);
        CurDist = TargetDist = Vector3.Distance(this.transform.position, myCam.position);

        if (ControllerRotate)
        {
            TargetRot.x = this.transform.localRotation.eulerAngles.x;
            TargetRot.y = this.transform.parent.eulerAngles.y;
        }
        else
        {
            TargetRot = this.transform.rotation.eulerAngles;

        }
        //TargetRot = this.transform.rotation.eulerAngles;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // Look
            //Debug.Log("1 : " + this.transform.localRotation.eulerAngles);

            //Vector3 rot = this.transform.localRotation.eulerAngles;
            TargetRot.x += -Input.GetAxis("Mouse Y") * Rotspeed * Time.smoothDeltaTime;

            if (TargetRot.x > 180.0f)
            {
                TargetRot.x -= 360.0f;
            }

            TargetRot.x = Mathf.Clamp(TargetRot.x, LookUpArea.x, LookUpArea.y);
            //this.transform.localRotation = Quaternion.Euler(TargetRot);

            //Debug.Log("2 : " + this.transform.localRotation.eulerAngles);

            // Turn
            //this.transform.Rotate(Vector3.right * -Input.GetAxis("Mouse Y") * Rotspeed, Space.Self);
            //this.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Rotspeed, Space.World);
            TargetRot.y += Input.GetAxis("Mouse X") * Rotspeed * Time.smoothDeltaTime;

        }

        if (ControllerRotate)
        {
            this.transform.parent.rotation =
                Quaternion.Slerp
                (
                    this.transform.parent.rotation,
                    Quaternion.Euler(new Vector3(0f, TargetRot.y, 0f)), Time.smoothDeltaTime * RotSmoothspeed
                );
            this.transform.localRotation
                = Quaternion.Slerp
                (
                    this.transform.localRotation,
                    Quaternion.Euler(new Vector3(TargetRot.x, 0f, 0f)), Time.smoothDeltaTime * RotSmoothspeed
                );

        }
        else
        {
            // Smooth Rotating
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(TargetRot), Time.smoothDeltaTime * RotSmoothspeed);
        }


        // Not Zero
        if (Input.GetAxis("Mouse ScrollWheel") > Mathf.Epsilon || Input.GetAxis("Mouse ScrollWheel") < -Mathf.Epsilon)
        {
            TargetDist += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
            TargetDist = Mathf.Clamp(TargetDist, ZoomArea.x, ZoomArea.y);
            //myCam.position = this.transform.position + (-this.transform.forward * TargetDist);
        }

        CurDist = Mathf.Lerp(CurDist, TargetDist, Time.smoothDeltaTime * ZoomSmoothSpeed);
        //myCam.position = this.transform.position + (-this.transform.forward * CurDist);

        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = -this.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, CurDist + CollisionOffset, CrashMask))
        {
            myCam.position = hit.point - ray.direction * CollisionOffset;
            if (CurDist > Vector3.Distance(this.transform.position, hit.point))
                CurDist = Vector3.Distance(this.transform.position, hit.point);
        }
        else
        {
            myCam.position = this.transform.position + (-this.transform.forward * CurDist);
        }
    }
}
