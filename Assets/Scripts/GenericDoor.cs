using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GenericDoor : MonoBehaviourPun {
    public bool opened;
    public Color openedColor, closedColor;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public bool interacting;

    public Action onInteractEvent;

    public void open() {
        if (PhotonNetwork.IsConnectedAndReady) {
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC("RPC_DoorInteract", RpcTarget.AllViaServer, true);
        } else {
            RPC_DoorInteract(true);
        }
    }

    public void close() {
        if (PhotonNetwork.IsConnectedAndReady) {
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC("RPC_DoorInteract", RpcTarget.AllViaServer, false);
        } else {
            RPC_DoorInteract(false);
        }
    }

    public void openOrClose() {
        if (opened) {
            close();
        } else {
            open();
        }
        opened = !opened;
    }

    [PunRPC]
    public void RPC_DoorInteract(bool newState) {
        opened = newState;
        spriteRenderer.color = newState ? openedColor : closedColor;
        boxCollider.enabled = !newState;
    }

    // private void OnValidate() {
    //     if (opened) {
    //         open();
    //     } else {
    //         close();
    //     }
    // }
}
