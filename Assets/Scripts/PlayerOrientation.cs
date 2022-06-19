using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour {
    public Transform playerSprites;
    // public Transform signalTransform;
    public Transform rb;
    private float lastPosition = -1000f;
    private void FixedUpdate() {
        if (Mathf.Abs(rb.position.x - lastPosition) < 0.02f) return;
        if (rb.position.x - lastPosition > 0) {
            playerSprites.localScale = new Vector2(1, 1);
        } else {
            playerSprites.localScale = new Vector2(-1, 1);
        }

        lastPosition = rb.position.x;
        // playerSprites.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        // var aux = signalTransform.localPosition;
        // aux.x = Mathf.Abs(aux.x) * Mathf.Sign(rb.velocity.x);
        // signalTransform.localPosition = aux;
        // Debug.Log(signalTransform.position);
    }
}
