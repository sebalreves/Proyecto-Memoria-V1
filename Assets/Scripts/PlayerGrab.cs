using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;


public class PlayerGrab : MonoBehaviourPun {
    // Start is called before the first frame update
    public float grabCdTimer;
    public bool grabingBall = false;
    // public GameObject ObjectFocused;

    GameObject focusedObject;
    public CircleCollider2D playerCollider;
    public TargetingScript targetingScriptReference;

    //TODO habilitar events para evitar bugs en el online
    // private void TryGrabEvent(GameObject other) {
    //     //TODO check si hay mas de una pelota seleccionable, recoger solo una
    //     //player id, ball id
    //     grabingBall = true;
    //     object[] content = new object[] { photonView.ViewID, other.GetComponent<PhotonView>().ViewID };
    //     RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
    //     PhotonNetwork.RaiseEvent(CONST.GrabBallEventCode, content, raiseEventOptions, SendOptions.SendReliable);

    // }

    void Start() {
    }


    #region GRAB RELEASE
    public void TryGrab() {
        // Debug.Log("TryGrabPlayer");
        grabCdTimer = CONST.playerGrabCD;

        GameObject ObjectFocused = (GameObject)targetingScriptReference.GetFirstTargetAndClearTargetList();
        //check if is grabable
        if (ObjectFocused.CompareTag("Ball"))
            if (!ObjectFocused.GetComponent<BallGrabScript>().grabable) return;

        grabingBall = true;
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine) {
            ObjectFocused.GetComponent<BallGrabScript>().BallTryGrab(photonView.ViewID);
            ObjectFocused.GetComponent<PhotonView>().RPC("BallTryGrab", RpcTarget.Others, photonView.ViewID);
            // TryGrabEvent(ObjectGrabbed);
        } else {
            ObjectFocused.GetComponent<BallGrabScript>().BallTryGrab(gameObject.GetInstanceID());
        }
    }

    public void TryReleaseAndThrow() {
        // Debug.Log("TryReleasePlayer");
        grabingBall = false;
        Vector2 movementInput = gameObject.GetComponent<PlayerMovement>().movementInput;
        GameObject grabedBall = gameObject.transform.Find("GrabPosition").transform.GetChild(0).gameObject;
        if (PhotonNetwork.IsConnectedAndReady) {
            grabedBall.GetComponent<PhotonView>().RPC("BallTryRelease", RpcTarget.AllBuffered, movementInput.x, movementInput.y);
        } else {
            grabedBall.GetComponent<BallGrabScript>().BallTryRelease(movementInput.x, movementInput.y);
        }
        playerCollider.enabled = false;
        playerCollider.enabled = true;
        // ObjectFocused = null;
    }

    public void TryRelease() {
        grabingBall = false;
        GameObject actualPlayerGrabPosition = gameObject.transform.Find("GrabPosition").gameObject;
        GameObject actualPlayer = gameObject;
        Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().breakForce = 1000f;
    }
    #endregion





}
