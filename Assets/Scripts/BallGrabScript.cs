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
        GameObject playerWhoGrab;
        if (PhotonNetwork.IsConnectedAndReady)
            playerWhoGrab = PhotonView.Find(grabbingPlayerId).gameObject;
        else
            playerWhoGrab = PlayerFactory._instance.findPlayer(grabbingPlayerId);

        GameObject grabPosition = playerWhoGrab.transform.Find("GrabPosition").gameObject;
        ballRb.velocity = Vector2.zero;
        beingCarried = true;
        CollisionCollider.enabled = false;
        ballRb.isKinematic = true;
        ballRb.simulated = false;
        ballRb.position = Vector2.zero;
        gameObject.transform.SetParent(grabPosition.transform, true);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    [PunRPC]
    public void BallTryRelease(float _x, float _y) {
        Vector2 _velocity = new Vector2(_x, _y);
        //Habilitar collider despues de 0.1s
        CollisionCollider.enabled = true;
        beingCarried = false;
        ballRb.isKinematic = false;
        ballRb.simulated = true;
        gameObject.transform.SetParent(null, true);
        ballRb.velocity = _velocity * throwForce;
    }
}
