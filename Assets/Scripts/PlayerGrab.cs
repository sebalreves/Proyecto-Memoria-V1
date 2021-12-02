using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;


public class PlayerGrab : MonoBehaviourPun {
    // Start is called before the first frame update
    Keyboard kb;
    public float grabCdTimer;
    public int grabPosition;
    bool grabingBall = false;
    GameObject ObjectGrabbed;
    int objectGrabbedPhotonId;

    GameObject focusedObject;
    List<GameObject> grabableObjects;
    public CircleCollider2D playerCollider;

    // private void TryGrabEvent(GameObject other) {
    //     //TODO check si hay mas de una pelota seleccionable, recoger solo una
    //     //player id, ball id
    //     grabingBall = true;
    //     object[] content = new object[] { photonView.ViewID, other.GetComponent<PhotonView>().ViewID };
    //     RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
    //     PhotonNetwork.RaiseEvent(CONST.GrabBallEventCode, content, raiseEventOptions, SendOptions.SendReliable);

    // }

    void Start() {
        kb = InputSystem.GetDevice<Keyboard>();
        grabableObjects = new List<GameObject>();
    }


    #region GRAB RELEASE
    void TryGrab() {
        Debug.Log("TryGrabPlayer");
        grabCdTimer = CONST.playerGrabCD;

        grabingBall = true;
        ObjectGrabbed = GetFirstTargetAndClearTargetList();

        if (PhotonNetwork.IsConnectedAndReady) {
            ObjectGrabbed.GetComponent<PhotonView>().RPC("BallTryGrab", RpcTarget.AllBuffered, photonView.ViewID);
            // TryGrabEvent(ObjectGrabbed);
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
        playerCollider.enabled = false;
        playerCollider.enabled = true;
        ObjectGrabbed = null;
    }
    #endregion


    #region TARGETING
    private GameObject GetFirstTargetAndClearTargetList() {
        List<GameObject> tempList = grabableObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        foreach (GameObject collider in tempList) {
            collider.transform.Find("FocusedSprite").gameObject.SetActive(false);
        }
        GameObject tempTarget = tempList[0];
        grabableObjects.Clear();
        return tempTarget;
    }

    void UpdateTargetedObject() {
        if (!grabingBall && grabableObjects.Count > 0) {
            List<GameObject> temp = grabableObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
            foreach (GameObject collider in temp) {
                collider.transform.Find("FocusedSprite").gameObject.SetActive(false);
            }
            temp[0].transform.Find("FocusedSprite").gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //add collider
        if (!grabingBall && other.gameObject.CompareTag("GrabCollider") && (photonView.IsMine || !PhotonNetwork.IsConnectedAndReady)) {
            // pushObjectFromTargetList(other.gameObject);
            // Debug.Log(other.gameObject);
            grabableObjects.Add(other.gameObject.transform.parent.gameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //pop object
        if (!grabingBall && other.gameObject.CompareTag("GrabCollider") && (photonView.IsMine || !PhotonNetwork.IsConnectedAndReady)) {
            // Debug.Log(other.gameObject);
            grabableObjects.Remove(other.gameObject);
        }
    }

    #endregion

    private void FixedUpdate() {

        if (grabCdTimer >= 0f) {
            grabCdTimer -= Time.fixedDeltaTime;
        }

        if (!grabingBall) {
            UpdateTargetedObject();
            if (grabCdTimer <= 0f && kb.spaceKey.isPressed) {
                if (gameObject.GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady) {
                    TryGrab();
                }
            }
        }

        if (kb.spaceKey.wasReleasedThisFrame && grabCdTimer <= 0) {
            if (grabingBall) {
                TryRelease();
            }
        }
    }

    // private void OnTriggerStay2D(Collider2D other) {
    //     //TODO globalizar teclado
    //     if (other.gameObject.CompareTag("GrabCollider")) {
    //         //GRAB
    //         if (!grabingBall && grabCdTimer <= 0f && kb.spaceKey.isPressed) {
    //             if (gameObject.GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady) {
    //                 grabCdTimer = 1f;
    //                 TryGrab(other);
    //             }
    //         }
    //     }
    // }
}
