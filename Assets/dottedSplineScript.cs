using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class dottedSplineScript : MonoBehaviour {
    public GameObject dotsContainer;
    private List<SpriteRenderer> dotsList;
    public Color red, green;
    public bool activeSpline = false;
    public bool repeatPulse = true;

    float pulseTime;
    float remainColorTime;
    float repeatPulseTime;

    private void OnValidate() {
        dotsContainer.SetActive(activeSpline);
    }

    private void Awake() {

    }

    private void Start() {
        // if (gameObject.transform.GetChild(0).childCount > 0)
        //     pulseTime = 0.3f / gameObject.transform.GetChild(0).childCount;
        pulseTime = 0.01f;
        remainColorTime = 0.1f;
        repeatPulseTime = 0.7f;
        dotsList = new List<SpriteRenderer>();
        foreach (Transform child in dotsContainer.transform) {
            dotsList.Add(child.Find("Center").GetComponent<SpriteRenderer>());
        }
    }
    public void pulse(string color = "red") {
        if (activeSpline) {
            StopAllCoroutines();
            StartCoroutine(pulseRoutine(color));
        }
    }

    public void stopPulse() {
        StopAllCoroutines();
        foreach (SpriteRenderer spriteRenderer in dotsList) {
            spriteRenderer.color = Color.clear;
        }
    }

    IEnumerator pulseRoutine(string color) {
        foreach (SpriteRenderer spriteRenderer in dotsList) {
            StartCoroutine(circlePulseRoutine(spriteRenderer, color));
            yield return new WaitForSeconds(pulseTime);
        }
    }

    IEnumerator circlePulseRoutine(SpriteRenderer spriteRenderer, string color) {
        do {
            spriteRenderer.color = red;
            yield return new WaitForSeconds(remainColorTime);
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(repeatPulseTime);
        } while (repeatPulse);

    }
}

