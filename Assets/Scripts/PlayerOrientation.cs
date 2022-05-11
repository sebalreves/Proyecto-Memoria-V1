using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour {
    public Transform playerSprites;
    // public Transform signalTransform;
    public Rigidbody2D rb;
    private void FixedUpdate() {
        playerSprites.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        // var aux = signalTransform.localPosition;
        // aux.x = Mathf.Abs(aux.x) * Mathf.Sign(rb.velocity.x);
        // signalTransform.localPosition = aux;
        // Debug.Log(signalTransform.position);
    }
}
