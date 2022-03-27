using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePivot : MonoBehaviour {
    public AnimationCurve rotateAnimation;
    private Transform playerTransform;
    private RectTransform rectOutlineTransform;
    GameObject scenario;
    public static bool rotating = false;
    public static bool ready = false;

    public static RotatePivot _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        }

       //If intance already exists and it is not !this!
       else if (_instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);
        }
    }

    IEnumerator Start() {
        scenario = gameObject;
        while (PlayerFactory._instance.localPlayer == null) {
            yield return null;
        }
        playerTransform = PlayerFactory._instance.localPlayer.transform;
        rectOutlineTransform = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).transform.GetChild(0).transform.Find("GameObject (1)").GetComponent<RectTransform>();
        // yield return new WaitForSeconds(2f);
        // rotateCamera(0f);
        ready = true;
    }


    public void setRotation(float _angle) {
        rotateCamera(scenario.transform.rotation.z - _angle);
    }
    public void rotateCamera(float _angulos) {
        return;
        StartCoroutine(rotateCameraRoutine(_angulos));
    }

    IEnumerator rotateCameraRoutine(float _angulos) {
        rotating = true;
        float time = 0f;
        while (time < 1.5f) {
            var newRotation = (rotateAnimation.Evaluate(time + Time.deltaTime) - rotateAnimation.Evaluate(time)) * CONST.cameraInclination * _angulos;
            rectOutlineTransform.rotation = Quaternion.Euler(0, 0, rotateAnimation.Evaluate(time) * CONST.cameraInclination * _angulos);
            scenario.transform.Rotate(new Vector3(0f, 0f, newRotation));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rotating = false;

        // while();

    }
}
