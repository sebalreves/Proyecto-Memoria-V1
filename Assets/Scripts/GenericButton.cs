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

    public void activateButton(bool activate) {
        if (activate) {
            actualSprite.sprite = activableSprite;
            ejecutando = true;
            activable = false;
        } else {
            ejecutando = false;
            activable = true;
            actualSprite.sprite = noActivableSprite;
        }

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
        if (!activable) return;

        activateButton(true);
        if (onPressEvent != null) {
            onPressEvent();
        }
        //reiniciar el estado del boton cuando termina su ejecucion (opcional)
        // StartCoroutine(rutinaPresionar());
    }


    // private IEnumerator rutinaPresionar() {
    //     if (!activable) yield break;

    //     activateButton(true);
    //     if (onPressEvent != null) {
    //         onPressEvent();
    //     }
    //     yield return new WaitForSeconds(2);
    //     // activateButton(false);


    // }

    void Update() {

    }
}
