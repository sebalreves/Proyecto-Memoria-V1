using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GenericButton : MonoBehaviourPun {
    public SpriteRenderer actualSprite;
    public Sprite activableSprite, noActivableSprite;
    public bool activable;
    public bool ejecutando;
    public Transform pressingPosition;
    public TextMeshProUGUI HUDText;
    [HideInInspector]
    public bool spawner = false;

    public bool freezePlayer = true;

    private string activatedButtonHUD = "Ejecución en curso";
    private string deactivatedButtonHUD = "Mantener <Espacio>";

    // public bool lockMovementOnPress = false;
    public Func<GameObject, IEnumerator> onPressEvent;
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
            HUDText.text = activatedButtonHUD;

        } else {
            ejecutando = false;
            activable = true;
            actualSprite.sprite = noActivableSprite;
            HUDText.text = deactivatedButtonHUD;
        }

    }


    public void Presionar(GameObject playerGameObject) {
        int playerId = PlayerFactory._instance.GetPlayerId(playerGameObject);
        if (activable && !ejecutando) {
            if (PhotonNetwork.IsConnectedAndReady) {
                photonView.RPC("PresionarRPC", RpcTarget.AllViaServer, playerId);
            } else
                PresionarRPC(playerId);
        }
    }

    private void OnDisable() {
        if (onPressEvent != null)
            foreach (var d in onPressEvent.GetInvocationList())
                onPressEvent -= (d as Func<GameObject, IEnumerator>);
    }

    [PunRPC]
    private void PresionarRPC(int playerId) {
        StartCoroutine(rutinaPresionar(playerId));


        //reiniciar el estado del boton cuando termina su ejecucion (opcional)
        // StartCoroutine(rutinaPresionar());
    }


    private IEnumerator rutinaPresionar(int playerId) {
        if (!activable) yield break;

        //mover jugador
        GameObject player = PlayerFactory._instance.findPlayer(playerId);
        if (PhotonNetwork.IsConnectedAndReady) {
            if (player.GetComponent<PhotonView>().IsMine) {
                if (freezePlayer)
                    player.GetComponent<PlayerMovement>().controllEnabled = false;
                player.GetComponent<PlayerMovement>().playerTeleportTo(pressingPosition.position);
            }
        } else {
            if (freezePlayer)
                player.GetComponent<PlayerMovement>().controllEnabled = false;
            player.GetComponent<PlayerMovement>().playerTeleportTo(pressingPosition.position);
        }

        if (onPressEvent != null) {
            CodeLineManager._instance.resetCodeColor();

            yield return onPressEvent(gameObject);
        }

        activateButton(false);
        if (freezePlayer)
            player.GetComponent<PlayerMovement>().controllEnabled = true;

        yield return new WaitForSeconds(1f);
        CodeLineManager._instance.fadeOutCodeColor();

        // if (spawner) {
        //     activateButton(false);
        //     if (freezePlayer)
        //         player.GetComponent<PlayerMovement>().controllEnabled = true;
        // }

    }


}
