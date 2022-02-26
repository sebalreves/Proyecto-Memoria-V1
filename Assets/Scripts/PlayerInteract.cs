using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;


public class PlayerInteract : MonoBehaviourPun {
    // Start is called before the first frame update
    PlayerGrab playerGrabScrip;
    TargetingScript targetingScriptReference;
    PlayerMovement playerMovementScript;
    Keyboard kb;
    GameObject signalPointerPrefab;


    private void Awake() {
        playerGrabScrip = GetComponent<PlayerGrab>();
        targetingScriptReference = GetComponent<TargetingScript>();
        playerMovementScript = GetComponent<PlayerMovement>();
        kb = InputSystem.GetDevice<Keyboard>();
    }

    private void Start() {
        signalPointerPrefab = Resources.Load("SignalPointer") as GameObject;
    }


    public void OnInteract(InputAction.CallbackContext _context) {
        // if (_context.phase == InputActionPhase.Canceled && _context.interaction is UnityEngine.InputSystem.Interactions.MultiTapInteraction) {
        //     Debug.Log("Tap");
        // }
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        if (!playerMovementScript.controllEnabled) return;
        if (!(_context.phase == InputActionPhase.Performed)) return;
        if (_context.interaction is UnityEngine.InputSystem.Interactions.SlowTapInteraction) {
            //Presionar botones
            #region BUTTONS
            // if (kb.spaceKey.wasPressedThisFrame) {
            var focusedElement = targetingScriptReference.getTargetedButton();
            if (focusedElement != null) {
                focusedElement.GetComponent<GenericButton>().Presionar(gameObject);
            }

            #endregion

        } else if (_context.interaction is UnityEngine.InputSystem.Interactions.TapInteraction) {
            // Debug.Log("Hola");
            #region SIGNALS
            //Signals que deja el jugador cada vez que hace una interaccion
            if (SignalPointer.signalCount < 5) {
                if (PhotonNetwork.IsConnectedAndReady) {
                    // object[] customData = new object[] { _color };
                    GameObject spawnedObject = PhotonNetwork.Instantiate(signalPointerPrefab.name, transform.position, Quaternion.identity);
                } else {
                    GameObject spawnedObject = Instantiate(signalPointerPrefab, transform.position, Quaternion.identity);
                }
                // SignalPointer.signalCount++;
                // Instantiate(signalPointer, gameObject.transform.position, Quaternion.identity);
            }
            #endregion

            #region GRAB INTERACTION ONRELEASE 
            if (playerGrabScrip.grabCdTimer <= 0f) {
                if (playerGrabScrip.grabingBall) {
                    playerGrabScrip.TryReleaseAndThrow();

                } else {
                    var focusedElement = targetingScriptReference.getTargetedBall();
                    if (focusedElement != null) {
                        playerGrabScrip.grabCdTimer = CONST.playerGrabCD;
                        playerGrabScrip.TryGrab(focusedElement);
                    }
                }
            }
            #endregion
        }
    }






}
