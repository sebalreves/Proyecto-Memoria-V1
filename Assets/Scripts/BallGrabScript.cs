using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class BallGrabScript : MonoBehaviourPun {
    public CircleCollider2D CollisionCollider;
    public bool grabable = true;
    public bool beingCarried = false;
    public Rigidbody2D ballRb;
    public float throwForce;
    public GenericBall genericBallScript;

    private float originalMass;
    private int originalMask;

    // public void OnEvent(EventData photonEvent) {
    //     byte eventCode = photonEvent.Code;

    //     if (eventCode == CONST.GrabBallEventCode) {
    //         object[] data = (object[])photonEvent.CustomData;

    //         int playerID = (int)data[0];
    //         int ballID = (int)data[1];

    //         if (ballID == photonView.ViewID) {
    //             BallTryGrab(playerID);
    //         }
    //     }
    // }

    // private void OnEnable() {
    //     PhotonNetwork.AddCallbackTarget(this);
    // }

    // private void OnDisable() {
    //     PhotonNetwork.RemoveCallbackTarget(this);
    // }



    //TODO ajustar colidders para que sean recogibles a traves de paredes simples y no dobles
    [PunRPC]
    public void BallTryGrab(int grabbingPlayerId) {

        //check if it's inside the windZone
        if (!grabable) return;
        //Check if ball is carried by other player
        if (beingCarried) {
            gameObject.transform.parent.transform.parent.GetComponent<PlayerGrab>().TryRelease();
        } else {
            gameObject.layer = LayerMask.NameToLayer("ObjectGrabed");
            ballRb.mass = CONST.ballMass;
            beingCarried = true;
        }

        //Find new player
        GameObject newPlayerWhoGrab;
        if (PhotonNetwork.IsConnectedAndReady)
            newPlayerWhoGrab = PhotonView.Find(grabbingPlayerId).gameObject;
        else
            newPlayerWhoGrab = PlayerFactory._instance.findPlayer(grabbingPlayerId);


        GameObject newGrabPosition = newPlayerWhoGrab.transform.Find("GrabPosition").gameObject;
        newGrabPosition.GetComponent<SpringJointBreakScript>().createSpringComponent(ballRb);
        gameObject.transform.SetParent(newGrabPosition.transform, true);
    }

    [PunRPC]
    public void BallTryRelease(float _x = 0f, float _y = 0f) {
        Vector2 _velocity = new Vector2(_x, _y);
        //Habilitar collider despues de 0.1s
        // CollisionCollider.enabled = true;
        beingCarried = false;
        // ballRb.isKinematic = false;
        // ballRb.simulated = true;
        GameObject actualPlayerGrabPosition = gameObject.transform.parent.gameObject;
        gameObject.transform.SetParent(null, true);
        try {
            Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
        } catch (System.Exception) {
            Debug.LogWarning("Player carry not found");
        }
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().connectedBody = null;
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().breakForce = 1000f;
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().enabled = false;
        ballRb.velocity += _velocity * throwForce;
        if (genericBallScript.shape == CONST.Cube) {
            ballRb.mass = CONST.cubeMass;
            gameObject.layer = LayerMask.NameToLayer("Cubes");
        } else if (genericBallScript.shape == CONST.Ball) {
            ballRb.mass = CONST.ballMass;
            gameObject.layer = LayerMask.NameToLayer("Balls");
        }
        // ballRb.mass = originalMass;
        // gameObject.layer = originalMask;
    }

    [PunRPC]
    public void OnWindEnter() {
        // if (PhotonNetwork.IsConnectedAndReady) {
        //     photonView.RPC("BallTryRelease", RpcTarget.AllBuffered, 0f, 0f);
        // } else {
        grabable = false;
        if (beingCarried) {
            gameObject.transform.parent.transform.parent.GetComponent<PlayerGrab>().TryRelease();
            BallTryRelease();
        }

        // BallTryRelease(0f, 0f);

    }

    [PunRPC]
    public void OnWindExit() {
        grabable = true;
    }
}
