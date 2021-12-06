using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWindInteraction : MonoBehaviour {

    private Rigidbody2D myRb;
    // Start is called before the first frame update
    private void Awake() {
        myRb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("WindZone")) {
            // if (!grabbed)
            myRb.velocity *= 0.15f;
        }
    }
}
