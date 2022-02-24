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


    public CircleCollider2D playerCollider;
    public TargetingScript targetingScriptReference;


    public int grabedBallId;

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
    private void Update() {
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;

        if (grabCdTimer >= 0f) {
            grabCdTimer -= Time.deltaTime;
        }
    }


    #region GRAB RELEASE
    public void TryGrab(GameObject _ball) {
        // Debug.Log("TryGrabPlayer");
        grabCdTimer = CONST.playerGrabCD;

        // GameObject ObjectFocused = (GameObject)targetingScriptReference.GetFirstTargetAndClearTargetList();
        //check if is grabable
        // if (ObjectFocused.CompareTag("Ball"))
        if (!_ball.GetComponent<BallGrabScript>().grabable) return;

        grabingBall = true;
        targetingScriptReference.deactivateBallFocus();

        grabedBallId = BallFactory._instance.getBallId(_ball);
        // StartCoroutine(disableCollisionRoutine(_ball.GetComponent<CircleCollider2D>()));
        if (PhotonNetwork.IsConnectedAndReady) {
            // ObjectFocused.GetComponent<BallGrabScript>().BallTryGrab(photonView.ViewID);
            if (photonView.IsMine)
                _ball.GetComponent<PhotonView>().RPC("BallTryGrab", RpcTarget.AllBuffered, photonView.ViewID);
            // TryGrabEvent(ObjectGrabbed);
        } else
            _ball.GetComponent<BallGrabScript>().BallTryGrab(gameObject.GetInstanceID());

        blinkCollider();
    }

    IEnumerator disableCollisionRoutine(CircleCollider2D _ballCollider) {

        //para evitar que la bola arrastre al player cuando es tomada
        Physics2D.IgnoreCollision(_ballCollider, playerCollider, true);
        yield return new WaitForSeconds(CONST.playerGrabCollisionIgnoreCD);
        Physics2D.IgnoreCollision(_ballCollider, playerCollider, false);

    }

    void blinkCollider() {
        return;
        playerCollider.enabled = false;
        playerCollider.enabled = true;

    }

    public void TryReleaseAndThrow() {
        // Debug.Log("TryReleasePlayer");
        grabCdTimer = CONST.playerGrabCD;
        grabingBall = false;
        Vector2 movementInput = gameObject.GetComponent<PlayerMovement>().movementInput;
        // GameObject grabedBall = gameObject.transform.Find("GrabPosition").transform.GetChild(0).gameObject;
        GameObject grabedBall = BallFactory._instance.findBallById(grabedBallId);
        if (PhotonNetwork.IsConnectedAndReady) {
            grabedBall.GetComponent<PhotonView>().RPC("BallTryRelease", RpcTarget.AllBuffered, movementInput.x, movementInput.y);
        } else {
            grabedBall.GetComponent<BallGrabScript>().BallTryRelease(movementInput.x, movementInput.y);
        }
        blinkCollider();
        // ObjectFocused = null;
    }

    public void TryRelease() {
        grabingBall = false;
        GameObject actualPlayerGrabPosition = gameObject.transform.Find("GrabPosition").gameObject;
        // GameObject actualPlayer = gameObject;
        Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
        // playerCollider.enabled = false;
        // playerCollider.enabled = true;
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().breakForce = 1000f;
    }
    #endregion





}
