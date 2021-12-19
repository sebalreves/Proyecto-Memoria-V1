using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallWindInteraction : MonoBehaviour {

    public Rigidbody2D myRb;
    public BallGrabScript myGrabScript;
    public PhotonView myPhotonView;
    // Start is called before the first frame update
    // private void Awake() {
    //     myRb = gameObject.GetComponent<Rigidbody2D>();
    // }
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("WindZone")) {
            myRb.velocity *= CONST.FRENADO_VIENTO;
            if (PhotonNetwork.IsConnectedAndReady) {
                if (myPhotonView.IsMine) {
                    myPhotonView.RPC("OnWindEnter", RpcTarget.AllBuffered);
                }
            } else {
                myGrabScript.OnWindEnter();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("WindZone")) {
            if (PhotonNetwork.IsConnectedAndReady) {

                if (myPhotonView.IsMine) {
                    myPhotonView.RPC("OnWindExit", RpcTarget.AllBuffered);
                }
            } else {
                myGrabScript.OnWindExit();
            }
        }
    }
}
