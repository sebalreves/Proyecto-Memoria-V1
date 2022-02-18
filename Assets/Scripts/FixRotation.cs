using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour {
    Quaternion rotation;
    public Transform parentObject;
    Vector3 offset = new Vector3(-0.01f, 0.63f, 0f);
    void Awake() {
        rotation = Quaternion.identity;
    }
    void LateUpdate() {
        transform.rotation = rotation;
        // transform.position = parentObject.position + offset;
    }

}
