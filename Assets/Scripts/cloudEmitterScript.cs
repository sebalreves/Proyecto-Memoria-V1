using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudEmitterScript : MonoBehaviour {
    public int fps = 4;
    ParticleSystem particle;
    private float timeElapsed = 0f;
    private float displayTime = 0f;

    private void OnEnable() {
        particle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    void LateUpdate() {
        timeElapsed += Time.deltaTime;

        if ((timeElapsed - displayTime) > 1f / fps) {
            displayTime = timeElapsed;
            particle.Simulate(.1f, true, false, false);
            particle.Pause();
        }

    }
}
