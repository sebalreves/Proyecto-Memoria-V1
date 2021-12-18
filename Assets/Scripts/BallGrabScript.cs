using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class BallGrabScript : MonoBehaviourPun {
    public CircleCollider2D CollisionCollider;
    public bool grabable = false;
    public bool beingCarried = false;
    public Rigidbody2D ballRb;
    public float throwForce;

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
        //Check if ball is carried by other player
        if (beingCarried) {
            GameObject actualPlayerGrabPosition = gameObject.transform.parent.gameObject;
            GameObject actualPlayer = actualPlayerGrabPosition.transform.parent.gameObject;

            Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
            // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().breakForce = 1000f;
            actualPlayer.GetComponent<PlayerGrab>().grabingBall = false;
        } else {
            originalMask = gameObject.layer;
            originalMass = ballRb.mass;
            gameObject.layer = LayerMask.NameToLayer("ObjectGrabed");
            ballRb.mass = 0.5f;
            beingCarried = true;
        }

        //Find new player
        GameObject newPlayerWhoGrab;
        if (PhotonNetwork.IsConnectedAndReady)
            newPlayerWhoGrab = PhotonView.Find(grabbingPlayerId).gameObject;
        else
            newPlayerWhoGrab = PlayerFactory._instance.findPlayer(grabbingPlayerId);


        GameObject newGrabPosition = newPlayerWhoGrab.transform.Find("GrabPosition").gameObject;
        // ballRb.velocity = Vector2.zero;
        // CollisionCollider.enabled = false;
        // ballRb.isKinematic = true;
        newGrabPosition.GetComponent<SpringJointBreakScript>().createSpringComponent(ballRb);
        // newGrabPosition.GetComponent<SpringJoint2D>().enabled = true;
        // newGrabPosition.GetComponent<SpringJoint2D>().connectedBody = ballRb;
        // ballRb.simulated = false;
        // ballRb.position = Vector2.zero;
        gameObject.transform.SetParent(newGrabPosition.transform, true);
        // gameObject.transform.localPosition = new Vector3(0, 0, 0);


    }

    [PunRPC]
    public void BallTryRelease(float _x, float _y) {
        Vector2 _velocity = new Vector2(_x, _y);
        //Habilitar collider despues de 0.1s
        // CollisionCollider.enabled = true;
        beingCarried = false;
        // ballRb.isKinematic = false;
        // ballRb.simulated = true;
        GameObject actualPlayerGrabPosition = gameObject.transform.parent.gameObject;
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().connectedBody = null;
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().breakForce = 1000f;
        // actualPlayerGrabPosition.GetComponent<SpringJoint2D>().enabled = false;
        Destroy(actualPlayerGrabPosition.GetComponent<SpringJoint2D>());
        gameObject.transform.SetParent(null, true);
        ballRb.velocity += _velocity * throwForce;
        ballRb.mass = originalMass;
        gameObject.layer = originalMask;
    }
}
