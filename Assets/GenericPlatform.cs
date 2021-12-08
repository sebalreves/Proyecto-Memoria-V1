using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlatform : MonoBehaviour {
    PointEffector2D attractPointEffector;
    public bool presionado = false;
    // public GameObject grabedObject;
    public Color ActivatedColor, DeactivatedColor;
    SpriteRenderer spriteReference;



    private void Awake() {
        attractPointEffector = GetComponent<PointEffector2D>();
        spriteReference = GetComponent<SpriteRenderer>();
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
}
