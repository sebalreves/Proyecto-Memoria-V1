using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {
    public Camera mainCamera;
    public GameObject virtualCam1GM, virtualCam2GM;
    private CinemachineVirtualCamera virtualCam1, virtualCam2;
    private CinemachineConfiner confiner1, confiner2;

    private PlayerMovement playerMovementScript;

    private bool firstEnteringCameraZone = false;

    void Awake() {
        virtualCam1 = virtualCam1GM.GetComponent<CinemachineVirtualCamera>();
        virtualCam2 = virtualCam2GM.GetComponent<CinemachineVirtualCamera>();
        confiner1 = virtualCam1GM.GetComponent<CinemachineConfiner>();
        confiner2 = virtualCam2GM.GetComponent<CinemachineConfiner>();
        virtualCam1GM.SetActive(true);
        virtualCam2GM.SetActive(false);
        playerMovementScript = gameObject.transform.parent.GetComponent<PlayerMovement>();

        // Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //TODO bug cuando se devuelve
        if (other.gameObject.CompareTag("CamZone")) {
            //quitar controles al jugador un momento al entrar a una zona
            if (firstEnteringCameraZone) playerMovementScript.playerMoveTo(playerMovementScript.playerRB.velocity, .2f);
            else firstEnteringCameraZone = true;

            //get zone collider
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
