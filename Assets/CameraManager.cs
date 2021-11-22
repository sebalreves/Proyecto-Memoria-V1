using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {
    public Camera mainCamera;
    public GameObject virtualCam1GM, virtualCam2GM;
    private CinemachineVirtualCamera virtualCam1, virtualCam2;
    public CinemachineConfiner confiner1, confiner2;
    void Start() {
        virtualCam1 = virtualCam1GM.GetComponent<CinemachineVirtualCamera>();
        virtualCam2 = virtualCam2GM.GetComponent<CinemachineVirtualCamera>();
        confiner1 = virtualCam1GM.GetComponent<CinemachineConfiner>();
        confiner2 = virtualCam2GM.GetComponent<CinemachineConfiner>();
        virtualCam1GM.SetActive(true);
        virtualCam2GM.SetActive(false);
    }
}
