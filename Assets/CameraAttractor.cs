using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAttractor : MonoBehaviour {
    public Transform CameraAnchorTransform;
    Transform playerTransform;
    Transform LastAttractor;
    // Start is called before the first frame update
    void Start() {
        playerTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update() {
        if (LastAttractor != null)
            CameraAnchorTransform.position = Vector3.Lerp(CameraAnchorTransform.position, LastAttractor.position, 3f * Time.deltaTime);
        else
            CameraAnchorTransform.position = Vector3.Lerp(CameraAnchorTransform.position, playerTransform.position, 3f * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "CameraAttractor") {
            LastAttractor = other.transform.GetChild(0).transform;

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "CameraAttractor") {
            LastAttractor = null;
        }
    }
}
