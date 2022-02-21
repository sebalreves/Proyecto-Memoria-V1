using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWindArea : MonoBehaviour {
    // Start is called before the first frame update
    public bool activated;
    public GameObject effector;
    public ParticleSystem particles;
    void Start() {
        effector = gameObject.transform.GetChild(0).gameObject;
        particles = gameObject.transform.Find("Particles").GetComponent<ParticleSystem>();
    }

    public void activar() {
        particles.Play();
        effector.SetActive(true);
    }
    public void desactivar() {
        effector.SetActive(false);
        particles.Stop();
    }

    public void stopLooping(bool stayActive = false) {
        StopAllCoroutines();
        if (stayActive) activar();
        else desactivar();

    }

    public void startLoop(float activatedTime, float deactivatedTime) {
        StartCoroutine(loopRoutine(activatedTime, deactivatedTime));
    }

    public IEnumerator loopRoutine(float activatedTime, float deactivatedTime) {
        while (true) {
            activar();
            yield return new WaitForSeconds(activatedTime);
            desactivar();
            yield return new WaitForSeconds(deactivatedTime);
        }
    }
}
