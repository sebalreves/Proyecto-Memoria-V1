using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallBlackHoleInteraction : MonoBehaviour {
    private PhotonView myPhotonView;
    private BallGrabScript grabBallScript;
    private void Start() {
        myPhotonView = gameObject.transform.parent.GetComponent<PhotonView>();
        grabBallScript = gameObject.transform.parent.GetComponent<BallGrabScript>();
    }



    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("BlackHoleCenter")) {
            // Debug.Log("Hola");

            BallFactory._instance.DestroyBall(gameObject.transform.parent.gameObject);

        }
    }
}
