using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGrab : MonoBehaviour {
    // Start is called before the first frame update
    Keyboard kb;
    float cdTimer;

    void TryGrab() {
        Debug.Log("Pressed");
    }

    void Start() {
        kb = InputSystem.GetDevice<Keyboard>();
        cdTimer = 1f;
    }



    // Update is called once per frame
    void Update() {
        // spacePressed = kb.spaceKey.wasPressedThisFrame;
        if (cdTimer >= 0f) {
            cdTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        //TODO globalizar teclado
        if (other.gameObject.CompareTag("GrabCollider")) {
            if (cdTimer <= 0f && kb.spaceKey.isPressed) {
                cdTimer = 1f;
                Debug.Log("Recogido");
            }

        }
    }
}
