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
    public int ActualPlayerWhoGrabId;

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
    public void BallTryGrab(int newGrabPlayerId) {
        // if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        //check if it's inside the windZone
        if (!grabable) return;
        //Check if ball is carried by other player
        if (beingCarried && photonView.IsMine) {
            PlayerFactory._instance.findPlayer(ActualPlayerWhoGrabId).GetComponent<PlayerGrab>().TryRelease();
            // gameObject.transform.parent.transform.parent.GetComponent<PlayerGrab>().TryRelease();
        } else {
            gameObject.layer = LayerMask.NameToLayer("ObjectGrabed");
            ballRb.mass = CONST.ballMass;
            beingCarried = true;
        }
        ActualPlayerWhoGrabId = newGrabPlayerId;

        //Find new player
        GameObject newPlayerWhoGrab;
        newPlayerWhoGrab = PlayerFactory._instance.findPlayer(newGrabPlayerId);

        GameObject newGrabPosition = newPlayerWhoGrab.transform.Find("GrabPosition").gameObject;
        newGrabPosition.GetComponent<SpringJointBreakScript>().createSpringComponent(ballRb);
        // newGrabPosition.GetComponent<SpringJointBreakScript>().createSpringComponent(ballRb);
        // gameObject.transform.SetParent(newGrabPosition.transform, true);
    }

    [PunRPC]
    public void BallTryRelease(float _x = 0f, float _y = 0f) {
        Vector2 _velocity = new Vector2(_x, _y);
        //Habilitar collider despues de 0.1s
        // CollisionCollider.enabled = true;
        beingCarried = false;
        // ballRb.isKinematic = false;
        // ballRb.simulated = true;
        // GameObject actualPlayerGrabPosition = gameObject.transform.parent.gameObject;
        GameObject actualPlayerGrabPosition = PlayerFactory._instance.findPlayer(ActualPlayerWhoGrabId).transform.Find("GrabPosition").gameObject;
        // gameObject.transform.SetParent(null, true);
        try {
            // Debug.Log("destry spring");
            Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
            // Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
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
        // Debug.Log("Holaa");

        grabable = false;
        if (beingCarried) {
            PlayerFactory._instance.findPlayer(ActualPlayerWhoGrabId).GetComponent<PlayerGrab>().TryRelease();
            // gameObject.transform.parent.transform.parent.GetComponent<PlayerGrab>().TryRelease();
            BallTryRelease();
        }
        // BallTryRelease(0f, 0f);
    }

    [PunRPC]
    public void OnWindExit() {
        grabable = true;
    }
}
