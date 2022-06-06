using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectNoRoom : MonoBehaviour {
    public GameObject message;
    public Transform ListContainer;


    // Start is called before the first frame update
    void Start() {
        message.SetActive(ListContainer.childCount == 0);
    }

    // Update is called once per frame
    void Update() {
        // Debug.Log(ListContainer.childCount);
        message.SetActive(ListContainer.childCount == 0);
    }
}
