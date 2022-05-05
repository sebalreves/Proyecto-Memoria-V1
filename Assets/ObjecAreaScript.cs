using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjecAreaScript : MonoBehaviour {
    public bool detectBalls = false;
    public bool detectCubes = false;

    // [SerializeField]
    public List<GameObject> detectedObjects;
    // Start is called before the first frame update

    private void Awake() {
        detectedObjects = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "BallPlatformInteraction") return;
        GameObject objectDetected = other.transform.parent.gameObject;
        if ((detectBalls && objectDetected.CompareTag("Ball")) || (detectCubes && objectDetected.CompareTag("Cube"))) {
            detectedObjects.Add(objectDetected);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "BallPlatformInteraction") return;
        GameObject objectDetected = other.transform.parent.gameObject;
        if ((detectBalls && objectDetected.CompareTag("Ball")) || (detectCubes && objectDetected.CompareTag("Cube"))) {
            detectedObjects.Remove(objectDetected);
        }
    }
}
