using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallBlackHoleInteraction : MonoBehaviour {
    private PhotonView myPhotonView;
    private BallGrabScript grabBallScript;
    private GameObject signalPointerPrefab;
    private void Start() {
        signalPointerPrefab = Resources.Load("SignalPointer") as GameObject;

        myPhotonView = gameObject.transform.parent.GetComponent<PhotonView>();
        grabBallScript = gameObject.transform.parent.GetComponent<BallGrabScript>();
    }



    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("BlackHoleCenter")) {
            // Debug.Log("Hola");
            // Debug.Log(other.name);
            other.transform.parent.Find("MainSprite").GetComponent<Animator>().Play("ActionBasurero");
            if (PhotonNetwork.IsConnectedAndReady) {
                // object[] customData = new object[] { _color };
                PhotonNetwork.Instantiate(signalPointerPrefab.name, other.transform.position, Quaternion.identity);
            } else {
                Instantiate(signalPointerPrefab, other.transform.position, Quaternion.identity);
            }
            BallFactory._instance.DestroyBall(gameObject.transform.parent.gameObject);

        }
    }
}
