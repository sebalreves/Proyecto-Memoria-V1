using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePivot : MonoBehaviour {
    public AnimationCurve rotateAnimation;
    private Transform playerTransform;
    private RectTransform rectOutlineTransform;
    GameObject scenario;

    IEnumerator Start() {
        scenario = gameObject;
        while (PlayerFactory._instance.localPlayer == null) {
            yield return null;
        }
        playerTransform = PlayerFactory._instance.localPlayer.transform;
        rectOutlineTransform = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).transform.GetChild(0).transform.Find("GameObject (1)").GetComponent<RectTransform>();
        // yield return new WaitForSeconds(2f);
        // rotateCamera(0f);
    }

    // Update is called once per frame
    void Update() {

    }
    public void rotateCamera(float _angulos) {
        StartCoroutine(rotateCameraRoutine(_angulos));
    }
    IEnumerator rotateCameraRoutine(float _angulos) {
        float time = 0f;
        while (time < 1.5f) {
            var newRotation = (rotateAnimation.Evaluate(time + Time.deltaTime) - rotateAnimation.Evaluate(time)) * CONST.cameraInclination;
            rectOutlineTransform.rotation = Quaternion.Euler(0, 0, rotateAnimation.Evaluate(time) * CONST.cameraInclination);
            scenario.transform.Rotate(new Vector3(0f, 0f, newRotation));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // while();

    }
}
