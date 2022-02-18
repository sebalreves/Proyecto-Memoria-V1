using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlatform : MonoBehaviour {
    public PointEffector2D attractPointEffector;
    public bool presionado = false;
    public bool activado = false;
    // public GameObject grabedObject;
    public Color ActivatedColor, DeactivatedColor;
    public SpriteRenderer spriteReference;
    public CircleCollider2D innerCollider;
    public Action setStatePressed;
    public Action setStateReleased;
    public Action currentAnimation = null;

    public bool cubeInteract = false;
    public bool ballInteract = false;
    public bool playerInteract = false;

    private int interactMask;


    private void Awake() {
        // attractPointEffector = GetComponent<PointEffector2D>();
        // spriteReference = GetComponent<SpriteRenderer>();
        if (presionado) ActivatePlatform();
        else DeactivatePlatform();
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
        presionado = true;
        spriteReference.color = ActivatedColor;
        attractPointEffector.enabled = false;
        if (setStatePressed != null)
            setStatePressed();
    }


    public void DeactivatePlatform() {
        presionado = false;
        spriteReference.color = DeactivatedColor;
        attractPointEffector.enabled = true;
        if (setStateReleased != null)
            setStateReleased();
    }

    public void resumeAnimation() {
        if (presionado) {
            if (currentAnimation != null)
                currentAnimation();
        }
    }

    private void FixedUpdate() {
        //Set mask
        // Debug.Log(activado + " - " + presionado);
        // interactMask = ((cubeInteract ? 1 : 0) * CONST.cubeLayer) | ((ballInteract ? 1 : 0) * CONST.ballLayer) | ((playerInteract ? 1 : 0) * CONST.playerLayer);
        // presionado = innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer));
        activado = innerCollider.IsTouchingLayers(((cubeInteract ? 1 : 0) * CONST.cubeLayer) | ((ballInteract ? 1 : 0) * CONST.ballLayer) | ((playerInteract ? 1 : 0) * CONST.playerLayer));
        if (!presionado && innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer))) {
            ActivatePlatform();
        } else if (presionado && !innerCollider.IsTouchingLayers((CONST.cubeLayer) | (CONST.ballLayer) | (CONST.playerLayer))) {
            DeactivatePlatform();
        }
    }
}
