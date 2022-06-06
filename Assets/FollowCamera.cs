using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public Transform cameraTransform;
    // Update is called once per frame
    void Update() {
        gameObject.transform.position = cameraTransform.position;
    }
}
