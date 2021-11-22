using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoneManager : MonoBehaviour {
    //TODO local player tag
    //TODO Pasar Zonas a Tile Map
    public PolygonCollider2D selfColider;
    private CameraManager camManager;
    private void Start() {
        camManager = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject.GetComponent<CameraManager>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {

            if (camManager.virtualCam1GM.activeSelf) {
                camManager.virtualCam2GM.SetActive(true);
                camManager.confiner2.m_BoundingShape2D = selfColider;
                camManager.confiner1.InvalidatePathCache();
                camManager.virtualCam1GM.SetActive(false);
            } else if (camManager.virtualCam2GM.activeSelf) {
                camManager.virtualCam1GM.SetActive(true);
                camManager.confiner1.m_BoundingShape2D = selfColider;
                camManager.confiner2.InvalidatePathCache();
                camManager.virtualCam2GM.SetActive(false);
            }
        }
    }
}
