using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {
    public Camera mainCamera;
    public GameObject virtualCam1GM, virtualCam2GM;
    private CinemachineVirtualCamera virtualCam1, virtualCam2;
    private CinemachineConfiner confiner1, confiner2;



    void Awake() {
        virtualCam1 = virtualCam1GM.GetComponent<CinemachineVirtualCamera>();
        virtualCam2 = virtualCam2GM.GetComponent<CinemachineVirtualCamera>();
        confiner1 = virtualCam1GM.GetComponent<CinemachineConfiner>();
        confiner2 = virtualCam2GM.GetComponent<CinemachineConfiner>();
        virtualCam1GM.SetActive(true);
        virtualCam2GM.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //TODO bug cuando se devuelve
        if (other.gameObject.CompareTag("CamZone")) {
            //get zone collider
            PlayerMovement._instance.playerMoveTo(PlayerMovement._instance.playerRB.velocity, .2f);
            PolygonCollider2D poly = other.gameObject.GetComponent<PolygonCollider2D>();
            if (virtualCam1GM.activeSelf) {
                virtualCam2GM.SetActive(true);
                confiner2.m_BoundingShape2D = poly;
                confiner1.InvalidatePathCache();
                virtualCam1GM.SetActive(false);
            } else if (virtualCam2GM.activeSelf) {
                virtualCam1GM.SetActive(true);
                confiner1.m_BoundingShape2D = poly;
                confiner2.InvalidatePathCache();
                virtualCam2GM.SetActive(false);
            }
        }
    }
}
