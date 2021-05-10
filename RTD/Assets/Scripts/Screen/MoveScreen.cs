using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 카메라 무브먼트를 다루는 클래스입니다.
/// 빈 오브젝트에 해당 스크립트를 붙여도 무방하게 로직을 짰습니다.
/// </summary>
public class MoveScreen : MonoBehaviour
{
    public enum COORDSTATE
    {
        NONEMOVE, MOVEMINUS, MOVEPLUS
    }

    public struct FCamPosInfo
    {
        public Vector3 savePoint;
        public float zoomDepth;
        public Vector3 rot;
    }

    FCamPosInfo[] camSavePoint = new FCamPosInfo[10];
    KeyCode[] hotKeys = new KeyCode[10];

    [SerializeField, Tooltip("카메라 이동속도")] float camMoveSpeed = 20.0f;
    [SerializeField, Tooltip("카메라 줌 속도")] float camZoomSpeed = 100.0f;
    [SerializeField, Tooltip("최대 줌 거리")] float MaxZoomDepth = 10.0f;
    [SerializeField, Tooltip("스크린 이동시 여백의 거리")] float screenSpace = 20.0f;

    float ScrollInput;
    float TargetZoomDepth = 0.0f;
    float CurZoomDepth = 0.0f;

    Vector2 mousePos;
    Camera mainCam;
    COORDSTATE MoveX = COORDSTATE.NONEMOVE;
    COORDSTATE MoveZ = COORDSTATE.NONEMOVE;
    COORDSTATE Zoom = COORDSTATE.NONEMOVE;



    void Awake()
    {
        mainCam = Camera.main;
        for (int i = 0; i < hotKeys.Length; i++)
        {
            FCamPosInfo saveinfo = new FCamPosInfo();
            saveinfo.savePoint = Vector3.zero;
            saveinfo.zoomDepth = CurZoomDepth;
            camSavePoint[i] = saveinfo;

            KeyCode code = (KeyCode)(i + 49);
            if (i == 9)
                code = KeyCode.Alpha0;
            hotKeys[i] = code;
        }
        
        camSavePoint[0].savePoint = mainCam.transform.position;
        camSavePoint[0].rot = mainCam.transform.rotation.eulerAngles;
        camSavePoint[1].savePoint = new Vector3(94.66f, -48.88f, -40.85f);
        camSavePoint[1].rot = new Vector3(40.874f, -110.73f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        //CheckMoveCoord();
        ProcessCoord();
        CheckSavePoint();
    }

    /// <summary>
    /// Change STATE MACHINE
    /// </summary>
    void CheckMoveCoord()
    {
        ScrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (mousePos.x < screenSpace)
            MoveX = COORDSTATE.MOVEMINUS;
        else if (mousePos.x > Screen.width - screenSpace)
            MoveX = COORDSTATE.MOVEPLUS;
        else
            MoveX = COORDSTATE.NONEMOVE;

        if (mousePos.y < screenSpace)
            MoveZ = COORDSTATE.MOVEMINUS;
        else if (mousePos.y > Screen.height - screenSpace)
            MoveZ = COORDSTATE.MOVEPLUS;
        else
            MoveZ = COORDSTATE.NONEMOVE;

        if (ScrollInput > Mathf.Epsilon)
            Zoom = COORDSTATE.MOVEPLUS;
        else if (ScrollInput < -Mathf.Epsilon)
            Zoom = COORDSTATE.MOVEMINUS;
        else
            Zoom = COORDSTATE.NONEMOVE;

    }

    /// <summary>
    /// 카메라 이동을 다룹니다.
    /// </summary>
    void ProcessCoord()
    {
        float moveDelta = Time.deltaTime * camMoveSpeed;
        Vector3 ScreenDelta = Vector3.zero;

        switch (MoveX)
        {
            case COORDSTATE.MOVEMINUS:
                ScreenDelta.x -= moveDelta;
                break;
            case COORDSTATE.MOVEPLUS:
                ScreenDelta.x += moveDelta;
                break;
        }

        switch (MoveZ)
        {
            case COORDSTATE.MOVEMINUS:
                ScreenDelta.z -= moveDelta;
                break;
            case COORDSTATE.MOVEPLUS:
                ScreenDelta.z += moveDelta;
                break;
        }
        mainCam.transform.Translate(ScreenDelta, Space.World);

        // Check Need Zoom In Out
        ZoomInOut();
    }


    /// <summary>
    /// Lerp로 Zoom합니다.
    /// </summary>
    void ZoomInOut()
    {
        float zoomDelta = Time.deltaTime * camZoomSpeed;
        Vector3 ScreenZoomDelta = Vector3.zero;
        switch (Zoom)
        {
            // Zoom OUT
            case COORDSTATE.MOVEMINUS:
                zoomDelta *= -1.0f;
                ScreenZoomDelta = mainCam.transform.forward * zoomDelta;
                TargetZoomDepth = Mathf.Clamp(TargetZoomDepth + ScreenZoomDelta.magnitude * -1.0f, 0.0f, MaxZoomDepth);
                break;

            // Zoom IN
            case COORDSTATE.MOVEPLUS:
                ScreenZoomDelta = mainCam.transform.forward * zoomDelta;
                TargetZoomDepth = Mathf.Clamp(TargetZoomDepth + ScreenZoomDelta.magnitude, 0.0f, MaxZoomDepth);
                break;
        }
        float LerpSpeed = 5.0f;
        float depthDist = Mathf.Abs(CurZoomDepth - Mathf.Lerp(CurZoomDepth, TargetZoomDepth, Time.smoothDeltaTime * LerpSpeed));

        if (Mathf.Abs(TargetZoomDepth - CurZoomDepth) < 0.05f)
        {
            depthDist = Mathf.Abs(TargetZoomDepth - CurZoomDepth);
            if (CurZoomDepth > TargetZoomDepth)
                depthDist *= -1.0f;

            CurZoomDepth = TargetZoomDepth;
        }
        else
        {
            if (CurZoomDepth > TargetZoomDepth)
                depthDist *= -1.0f;
        }
        CurZoomDepth = Mathf.Lerp(CurZoomDepth, TargetZoomDepth, Time.smoothDeltaTime * LerpSpeed);
        ScreenZoomDelta = mainCam.transform.forward * depthDist;
        mainCam.transform.Translate(ScreenZoomDelta, Space.World);
    }


    /// <summary>
    /// 지정된 곳으로 카메라 이동, 지정된 위치를 저장하는 함수 (현재 카메라의 Depth에 맞춰 움직입니다.)
    /// </summary>
    void CheckSavePoint()
    {
        if (!Input.anyKeyDown)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            foreach (KeyCode k in hotKeys)
            {
                if (Input.GetKeyDown(k))
                {
                    int idx = (int)k - 49;
                    if (k == KeyCode.Alpha0)
                        idx = 9;

                    camSavePoint[idx].savePoint = mainCam.transform.position;
                    camSavePoint[idx].zoomDepth = CurZoomDepth;
                }
            }
        }
        else
        {
            foreach (KeyCode k in hotKeys)
            {
                if (Input.GetKeyDown(k))
                {
                    int idx = (int)k - 49;
                    if (k == KeyCode.Alpha0)
                        idx = 9;

                    if (camSavePoint[idx].savePoint != Vector3.zero)
                    {
                        float depthDist = CurZoomDepth - camSavePoint[idx].zoomDepth;
                        Vector3 ScreenZoomDelta = mainCam.transform.forward * depthDist;
                        mainCam.transform.position = camSavePoint[idx].savePoint;
                        mainCam.transform.rotation = Quaternion.Euler(camSavePoint[idx].rot);
                        mainCam.transform.Translate(ScreenZoomDelta, Space.World);
                    }
                        
                }
            }
        }

    }

    public void TranslatePoint(int i)
    {
        float depthDist = CurZoomDepth - camSavePoint[i].zoomDepth;
        Vector3 ScreenZoomDelta = mainCam.transform.forward * depthDist;
        mainCam.transform.position = camSavePoint[i].savePoint;
        mainCam.transform.rotation = Quaternion.Euler(camSavePoint[i].rot);
        mainCam.transform.Translate(ScreenZoomDelta, Space.World);

    }
}