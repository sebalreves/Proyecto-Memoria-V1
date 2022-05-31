using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorActivator : MonoBehaviour {
    public GameObject florContainer;
    public GameObject indicatorCircle;

    private void Start() {
        indicatorCircle.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !florContainer.activeInHierarchy) {
            florContainer.SetActive(true);
        }
    }
}
