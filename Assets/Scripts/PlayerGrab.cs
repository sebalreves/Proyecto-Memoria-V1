using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;


public class PlayerGrab : MonoBehaviourPun {
    // Start is called before the first frame update
    Keyboard kb;
    public float grabCdTimer;
    public int grabPosition;
    bool grabingBall = false;
    GameObject ObjectGrabbed;
    int objectGrabbedPhotonId;

    private void TryGrabEvent(GameObject other) {
        //TODO check si hay mas de una pelota seleccionable, recoger solo una
        //player id, ball id
        grabingBall = true;
        object[] content = new object[] { photonView.ViewID, other.GetComponent<PhotonView>().ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(CONST.GrabBallEventCode, content, raiseEventOptions, SendOptions.SendReliable);

    }
    void TryGrab(Collider2D other) {
        Debug.Log("TryGrabPlayer");
        grabingBall = true;
        ObjectGrabbed = other.gameObject.transform.parent.gameObject;

        if (PhotonNetwork.IsConnectedAndReady) {
            TryGrabEvent(ObjectGrabbed);
        } else {
            ObjectGrabbed.GetComponent<BallGrabScript>().BallTryGrab(gameObject.GetInstanceID());
        }
    }

    void TryRelease() {
        Debug.Log("TryReleasePlayer");
        grabingBall = false;
        Vector2 movementInput = gameObject.GetComponent<PlayerMovement>().movementInput;
        if (PhotonNetwork.IsConnectedAndReady) {
            ObjectGrabbed.GetComponent<PhotonView>().RPC("BallTryRelease", RpcTarget.AllBuffered, movementInput.x, movementInput.y);

        } else {
            ObjectGrabbed.GetComponent<BallGrabScript>().BallTryRelease(movementInput.x, movementInput.y);
        }
        ObjectGrabbed = null;
    }

    void Start() {
        kb = InputSystem.GetDevice<Keyboard>();
        Debug.Log(photonView.IsMine);
    }



    void Update() {
        if (grabCdTimer >= 0f) {
            grabCdTimer -= Time.deltaTime;
        }
        if (kb.spaceKey.wasReleasedThisFrame && grabCdTimer <= 0) {
            if (grabingBall) {
                TryRelease();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        //TODO globalizar teclado
        if (other.gameObject.CompareTag("GrabCollider")) {
            if (!grabingBall && grabCdTimer <= 0f && kb.spaceKey.isPressed) {
                if (gameObject.GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady) {
                    grabCdTimer = 1f;
                    TryGrab(other);
                }
            }
        }
    }
}
