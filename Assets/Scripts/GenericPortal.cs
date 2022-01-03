using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class GenericPortal : MonoBehaviour {
    // string[] colorList = new string[] { CONST.Red, CONST.Blue };
    // string[] shapeList = new string[] { CONST.Ball, CONST.Cube };

    // [Dropdown("colorList")]
    public bool colorOutRed;
    public bool colorOutBlue;

    // [Dropdown("shapeList")]
    public bool shapeOutCube;
    public bool shapeOutBall;

    private void Start() {
        if (!(colorOutRed ^ colorOutBlue)) {
            Debug.LogWarning("Portal con colores mal asignados");
        }
        if (!(shapeOutBall ^ shapeOutCube)) {
            Debug.LogWarning("Portal con shapes mal asignadas");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Cube") || other.gameObject.CompareTag("Ball")) {
            // Debug.Log("Cube");
            if (PhotonNetwork.IsConnectedAndReady) {
                PhotonView otherPhotonView = other.GetComponent<PhotonView>();
                if (otherPhotonView.IsMine)
                    otherPhotonView.RPC("onPortalTransform", RpcTarget.AllBuffered, colorOutRed ? CONST.Red : CONST.Blue, shapeOutCube ? CONST.Cube : CONST.Ball);
            } else
                other.gameObject.GetComponent<GenericBall>().onPortalTransform(colorOutRed ? CONST.Red : CONST.Blue, shapeOutCube ? CONST.Cube : CONST.Ball);
        }
        // if (other.gameObject.CompareTag("Ball")) {
        //     Debug.Log("Ball");
        // }
    }
}
