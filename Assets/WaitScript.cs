using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitScript : MonoBehaviour {
    public static WaitScript _instance;
    void Awake() {
        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);

        } else if (_instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    public IEnumerator waitCodeLineAction(float waitTime, Action<float> _onWait = null) {
        if (_onWait != null) _onWait(waitTime - CONST.codeVelocity);
        yield return new WaitForSeconds(waitTime - CONST.codeVelocity);
    }
}
