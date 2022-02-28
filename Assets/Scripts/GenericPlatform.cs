using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlatform : MonoBehaviour {
    public PointEffector2D attractPointEffector;
    public bool presionado = false;
    public bool activado = false;
    // public bool pressedPrevFrame = false;
    // public bool activePrevFrame = false;
    // public GameObject grabedObject;
    public Color ActivatedColor, DeactivatedColor;
    public SpriteRenderer spriteReference;
    // public CircleCollider2D innerCollider;
    public Action setStatePressed;
    public Action setStateReleased;
    // public Action currentAnimation = null;

    public platformTrigger platformTriggerReference;
    // public bool keepLoopingCode = false;
    public Coroutine loopingCodeRoutine = null;

    // public bool cubeInteract = false;
    // public bool ballInteract = false;
    // public bool playerInteract = false;

    // private int interactMask;





    private void Awake() {
        // attractPointEffector = GetComponent<PointEffector2D>();
        // spriteReference = GetComponent<SpriteRenderer>();
        ActivatePlatform();

    }

    private void OnDisable() {
        if (setStatePressed != null)
            foreach (var d in setStatePressed.GetInvocationList())
                setStatePressed -= (d as Action);

        if (setStateReleased != null)
            foreach (var d in setStateReleased.GetInvocationList())
                setStateReleased -= (d as Action);
    }


    public void ActivatePlatform() {
        //si se presiona la plataforma
        // Debug.Log(platformTriggerReference.presionado + " " + platformTriggerReference.activado);
        if (platformTriggerReference.presionado && !presionado) {
            presionado = true;
            spriteReference.color = ActivatedColor;
            attractPointEffector.enabled = false;
            activado = platformTriggerReference.activado;
            if (setStatePressed != null)
                setStatePressed();
            return;
        } else if (!platformTriggerReference.presionado && presionado) {
            //si se libera la plataforma
            presionado = false;
            spriteReference.color = DeactivatedColor;
            attractPointEffector.enabled = true;
            activado = false;
            if (setStateReleased != null)
                setStateReleased();
            return;
        }

        //si estando presionada, se activa o desactiva
        if (!platformTriggerReference.presionado) return;
        if (platformTriggerReference.activado ^ activado) {
            activado = !activado;
            if (setStatePressed != null)
                setStatePressed();
        }
    }




    private void FixedUpdate() {
        //Set mask
        // Debug.Log(activado + " - " + presionado);
        // interactMask = ((cubeInteract ? 1 : 0) * CONST.cubeLayer) | ((ballInteract ? 1 : 0) * CONST.ballLayer) | ((playerInteract ? 1 : 0) * CONST.playerLayer);
        // presionado = innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer));
        // pressedPrevFrame = presionado;
        // activePrevFrame = activado;
        // activado = innerCollider.IsTouchingLayers(((cubeInteract ? 1 : 0) * CONST.cubeLayer) | ((ballInteract ? 1 : 0) * CONST.ballLayer) | ((playerInteract ? 1 : 0) * CONST.playerLayer));
        // presionado = innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer));
        // presionado = platformTriggerReference.presionado;
        // activado = platformTriggerReference.activado;
        ActivatePlatform();
        // DeactivatePlatform();
    }


}
