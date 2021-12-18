using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;


public class PlayerInteract : MonoBehaviour {
    // Start is called before the first frame update
    PlayerGrab playerGrabScrip;
    TargetingScript targetingScriptReference;
    Keyboard kb;


    private void Awake() {
        playerGrabScrip = GetComponent<PlayerGrab>();
        targetingScriptReference = GetComponent<TargetingScript>();
        kb = InputSystem.GetDevice<Keyboard>();
    }


    private void FixedUpdate() {
        //TODO actualizar interfaz dependiendo de la accion disponible
        //TODO se puede activar un boton teniendo un objeto cargado?
        if (!(gameObject.GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady)) return;

        targetingScriptReference.UpdateTargetedObject();



    }

    private void Update() {

        #region GRAB INTERACTION ONRELEASE 
        if (playerGrabScrip.grabCdTimer >= 0f) {
            playerGrabScrip.grabCdTimer -= Time.deltaTime;
        }

        if (!playerGrabScrip.grabingBall)
            if (playerGrabScrip.grabCdTimer <= 0f && kb.spaceKey.wasReleasedThisFrame) {
                var focusedElement = targetingScriptReference.GetFirstTarget();
                if (focusedElement != null && (focusedElement.CompareTag("Ball") || focusedElement.CompareTag("Cube")))
                    if (gameObject.GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady) {
                        playerGrabScrip.TryGrab();
                    }
            }
        if (kb.spaceKey.wasReleasedThisFrame && playerGrabScrip.grabCdTimer <= 0) {
            if (playerGrabScrip.grabingBall) {
                playerGrabScrip.TryReleaseAndThrow();
            }
        }
        #endregion

        #region botones
        if (kb.spaceKey.wasPressedThisFrame) {
            var focusedElement = targetingScriptReference.GetFirstTarget();
            if (focusedElement != null && focusedElement.CompareTag("Button")) {
                focusedElement.GetComponent<GenericButton>().Presionar();
            }
        }
        #endregion
    }

}
