using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        GameObject GameController = GameObject.Find("GamePlayManager");
        if (GameController != null)
        {
            GameController.GetComponent<CameraManager>().CameraChangeDel += ChangeBillboardCam;

            if (GameController.GetComponent<CameraManager>().breakTime)
                cam = GameController.GetComponent<CameraManager>().DirectionCamera.transform;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    void ChangeBillboardCam(Camera camObj)
    {
        cam = camObj.transform;
    }
}
