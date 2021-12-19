using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GenericButton : MonoBehaviourPun {
    public SpriteRenderer actualSprite;
    public Sprite activableSprite, noActivableSprite;
    public bool activable;
    public bool ejecutando;
    public Action onPressEvent;
    void Start() {
        switchActivableState();
        actualSprite.sprite = noActivableSprite;
    }

    public void switchActivableState() {
        activable = true;
        ejecutando = false;
    }


    public void Presionar() {
        if (activable && !ejecutando) {
            if (PhotonNetwork.IsConnectedAndReady) {
                photonView.RPC("PresionarRPC", RpcTarget.AllBuffered);
            } else
                PresionarRPC();
        }
    }

    private void OnDisable() {
        if (onPressEvent != null)
            foreach (var d in onPressEvent.GetInvocationList())
                onPressEvent -= (d as Action);
    }

    [PunRPC]
    private void PresionarRPC() {
        StartCoroutine(rutinaPresionar());
    }


    private IEnumerator rutinaPresionar() {
        actualSprite.sprite = activableSprite;
        ejecutando = true;
        activable = false;

        if (onPressEvent != null) {
            onPressEvent();
        }
        yield return new WaitForSeconds(2);
        ejecutando = false;
        activable = true;
        actualSprite.sprite = noActivableSprite;

    }

    void Update() {

    }
}
