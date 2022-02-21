using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformTrigger : MonoBehaviour {
    public bool presionado = false;
    public bool activado = false;
    public bool pressedPrevFrame = false;
    public bool activePrevFrame = false;
    public GenericPlatform platformScriptReference;
    public CircleCollider2D innerCollider;

    public bool cubeInteract = false;
    public bool ballInteract = false;
    public bool playerInteract = false;

    private void OnTriggerEnter2D(Collider2D other) {
        // pressedPrevFrame = presionado;
        // activePrevFrame = activado;
        // Debug.Log(other.name);
        if (other.CompareTag("BallPlatformInteraction") || other.CompareTag("Player")) {
            Debug.Log(innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer)));
            activado = innerCollider.IsTouchingLayers(((cubeInteract ? 1 : 0) * CONST.cubeLayer) | ((ballInteract ? 1 : 0) * CONST.ballLayer) | ((playerInteract ? 1 : 0) * CONST.playerLayer));
            presionado = innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer));
            // Debug.Log("Ball enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // pressedPrevFrame = presionado;
        // activePrevFrame = activado;
        if (other.CompareTag("BallPlatformInteraction") || other.CompareTag("Player")) {
            activado = innerCollider.IsTouchingLayers(((cubeInteract ? 1 : 0) * CONST.cubeLayer) | ((ballInteract ? 1 : 0) * CONST.ballLayer) | ((playerInteract ? 1 : 0) * CONST.playerLayer));
            presionado = innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer));
        }
    }

}
