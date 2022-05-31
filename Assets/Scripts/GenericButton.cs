using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GenericButton : MonoBehaviourPun {
    public Animator animator;
    // public SpriteRenderer actualSprite;
    public Sprite activableSprite, noActivableSprite;
    public bool activable;
    public bool ejecutando;
    public Transform pressingPosition;
    public TextMeshProUGUI HUDText;
    public CodeDescription codeDescription;

    [HideInInspector]
    public bool spawner = false;

    public bool teleportPlayer = true;
    public bool freezePlayer = true;

    private string activatedButtonHUD = "Ejecutando";
    private string deactivatedButtonHUD = "Mantener <Espacio>";
    public GameObject onPressParameter = null;
    public int wrappGroup = 0;
    public dottedSplineScript SplineScript;

    public void ChangeAnimation(string newAnimationName) {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(newAnimationName)) return;
        animator.Play(newAnimationName);
    }

    // public bool lockMovementOnPress = false;
    public Func<GameObject, IEnumerator> onPressEvent;

    void Start() {
        codeDescription.titulo = codeDescription.titulo + "<sprite=0>";
        switchActivableState();
        animator = GetComponent<Animator>();
        ChangeAnimation("Button");
        // actualSprite.sprite = noActivableSprite;
    }

    public void switchActivableState() {
        activable = true;
        ejecutando = false;
    }

    public void activateButton(bool activate) {
        //todo animate when executing
        if (activate) {
            ChangeAnimation("Button_Working");

            // actualSprite.sprite = activableSprite;
            ejecutando = true;
            activable = false;
            HUDText.text = activatedButtonHUD;

        } else {
            ejecutando = false;
            activable = true;
            ChangeAnimation("Button");

            // actualSprite.sprite = noActivableSprite;
            HUDText.text = deactivatedButtonHUD;
        }
    }

    private void activateButtonGroup(bool activate) {
        if (wrappGroup == 0) {
            activateButton(activate);
            return;
        }
        GameObject parent = gameObject.transform.parent.transform.parent.gameObject;
        foreach (Transform child in parent.transform) {
            child.transform.GetChild(0).GetComponent<GenericButton>().activateButton(activate);
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
    }

    private IEnumerator rutinaPresionar(int playerId) {
        if (!activable) yield break;

        //mover jugador
        GameObject player = PlayerFactory._instance.findPlayer(playerId);
        if (PhotonNetwork.IsConnectedAndReady) {
            if (player.GetComponent<PhotonView>().IsMine) {
                if (freezePlayer)
                    player.GetComponent<PlayerMovement>().controllEnabled = false;
                if (teleportPlayer)
                    player.GetComponent<PlayerMovement>().playerTeleportTo(pressingPosition.position);
            }
        } else {
            if (freezePlayer)
                player.GetComponent<PlayerMovement>().controllEnabled = false;
            if (teleportPlayer)
                player.GetComponent<PlayerMovement>().playerTeleportTo(pressingPosition.position);
        }

        activateButtonGroup(true);

        //activa la spline punteada si es que esta disponible
        SplineScript.pulse();
        if (onPressEvent != null) {
            CodeLineManager._instance.resetCodeColor();

            //para que todas los botones de un grupo ejecuten el mismo codigo en cada uno de los miembros del grupo
            var parameter = onPressParameter == null ? gameObject : onPressParameter;
            yield return onPressEvent(parameter);
        }


        activateButtonGroup(false);
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
