using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlatform : MonoBehaviour {
    public PointEffector2D attractPointEffector;
    public bool presionado = false;
    // public GameObject grabedObject;
    public Color ActivatedColor, DeactivatedColor;
    public SpriteRenderer spriteReference;
    public CircleCollider2D innerCollider;



    private void Awake() {
        // attractPointEffector = GetComponent<PointEffector2D>();
        // spriteReference = GetComponent<SpriteRenderer>();
        if (presionado) ActivatePlatform();
        else DeactivatePlatform();
    }


    public void ActivatePlatform() {
        presionado = true;
        spriteReference.color = ActivatedColor;
        attractPointEffector.enabled = false;
    }


    public void DeactivatePlatform() {
        presionado = false;
        spriteReference.color = DeactivatedColor;
        attractPointEffector.enabled = true;
    }

    private void FixedUpdate() {
        if (!presionado && innerCollider.IsTouchingLayers(((1 << 6) | (1 << 7)))) {
            ActivatePlatform();
        } else if (presionado && !innerCollider.IsTouchingLayers(((1 << 6) | (1 << 7)))) {
            DeactivatePlatform();
        }
    }
}
