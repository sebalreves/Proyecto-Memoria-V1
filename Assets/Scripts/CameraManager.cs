using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraManager : MonoBehaviour {
    public Camera mainCamera;
    public GameObject virtualCam1GM, virtualCam2GM;
    private CinemachineVirtualCamera virtualCam1, virtualCam2;
    private CinemachineConfiner confiner1, confiner2;
    public RectTransform inGameSquareRect;
    private float leftMargin, rightMargin, topMargin, bottomMargin;
    public CutOutMaskUI cutOutMaskUI;

    private PlayerMovement playerMovementScript;

    private bool firstEnteringCameraZone = false;
    public GameObject actualCamZone = null;
    Vector3 originalCamZoneScale;
    public PhotonView myPhotonView;
    // private IEnumerator maskBugRoutine() {
    //     yield return new WaitForSeconds(0.01f);
    //     cutOutMaskUI.enabled = false;
    //     cutOutMaskUI.enabled = true;
    // }
    void Awake() {
        // StartCoroutine(maskBugRoutine());
        virtualCam1 = virtualCam1GM.GetComponent<CinemachineVirtualCamera>();
        virtualCam2 = virtualCam2GM.GetComponent<CinemachineVirtualCamera>();
        confiner1 = virtualCam1GM.GetComponent<CinemachineConfiner>();
        confiner2 = virtualCam2GM.GetComponent<CinemachineConfiner>();
        virtualCam1GM.SetActive(true);
        virtualCam2GM.SetActive(false);
        playerMovementScript = gameObject.transform.parent.GetComponent<PlayerMovement>();
        // Screen.SetResolution(1920, 1080, Screen.fullScreen);

        leftMargin = inGameSquareRect.offsetMin.x;
        rightMargin = inGameSquareRect.offsetMax.x;
        topMargin = inGameSquareRect.offsetMax.y;
        bottomMargin = inGameSquareRect.offsetMin.y;
    }

    private void resizeEnteringCamZone(GameObject _newPoly) {
        // float xScale = _newPoly.gameObject.transform.localScale.x;
        // float yScale = _newPoly.gameObject.transform.localScale.y;

        float xOffset = 2 * Mathf.Abs(leftMargin);
        float yOffset = 2 * Mathf.Abs(topMargin);
        // Debug.Log("offset" + xOffset + " // " + yOffset);

        // RectTransform camZoneRect = (RectTransform)_newPoly.transform;
        Vector3 newObjectSize = _newPoly.GetComponent<Renderer>().bounds.size;
        float xFactor = (newObjectSize.x + xOffset) / newObjectSize.x;
        float yFactor = (newObjectSize.y + yOffset) / newObjectSize.y;


        _newPoly.transform.localScale = new Vector3(_newPoly.transform.localScale.x * xFactor,
                                                        _newPoly.transform.localScale.y * yFactor,
                                                        _newPoly.transform.localScale.z);
    }

    private void resizeExitingCamZone(GameObject _oldPoly, Vector3 _newPolyScale) {
        StartCoroutine(resizeExitingCamZoneRoutine(_oldPoly, _newPolyScale));
    }

    private IEnumerator resizeExitingCamZoneRoutine(GameObject _oldPoly, Vector3 _newPolyScale) {
        yield return new WaitForSeconds(CONST.waitTimeEnterCamZone);
        if (_oldPoly != null) {
            _oldPoly.transform.localScale = originalCamZoneScale;
        }
        originalCamZoneScale = _newPolyScale;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        //TODO bug cuando se devuelve
        if (other.gameObject.CompareTag("CamZone")) {
            if (PhotonNetwork.IsConnectedAndReady && !myPhotonView.IsMine) return;
            GameObject newCamZone = other.gameObject;
            Vector3 newPolyOriginalScale = newCamZone.transform.localScale;
            resizeEnteringCamZone(newCamZone);

            PolygonCollider2D newPoly = newCamZone.GetComponent<PolygonCollider2D>();
            if (actualCamZone == newCamZone) return;

            //quitar controles al jugador un momento al entrar a una zona
            if (firstEnteringCameraZone) playerMovementScript.playerMoveTo(playerMovementScript.playerRB.velocity, CONST.waitTimeEnterCamZone);
            else firstEnteringCameraZone = true;

            //get zone collider
            if (virtualCam1GM.activeSelf) {
                virtualCam2GM.SetActive(true);
                confiner2.m_BoundingShape2D = newPoly;
                confiner1.InvalidatePathCache();
                virtualCam1GM.SetActive(false);
            } else if (virtualCam2GM.activeSelf) {
                virtualCam1GM.SetActive(true);
                confiner1.m_BoundingShape2D = newPoly;
                confiner2.InvalidatePathCache();
                virtualCam2GM.SetActive(false);
            }

            resizeExitingCamZone(actualCamZone, newPolyOriginalScale);

            actualCamZone = newCamZone;
        }
    }
}
