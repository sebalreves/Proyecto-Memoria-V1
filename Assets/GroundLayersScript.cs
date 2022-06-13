using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLayersScript : MonoBehaviour {
    // Start is called before the first frame update
    private void OnValidate() {
        // Debug.Log("ground script");
        int i = 0;
        foreach (Transform child in gameObject.transform) {
            child.GetComponent<SpriteRenderer>().sortingOrder = i++;
        }
    }
}
