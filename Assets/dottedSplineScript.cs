using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class dottedSplineScript : MonoBehaviour {
    public GameObject dotsContainer;
    private List<SpriteRenderer> dotsList;
    public Color red, green;
    public bool activeSpline = false;
    public bool repeatPulse = false;

    float pulseTime = 0.3f / 7;
    float remainColorTime = 0.2f;
    float repeatPulseTime = 0.5f;

    private void OnValidate() {
        dotsContainer.SetActive(activeSpline);
    }

    private void Start() {
        dotsList = new List<SpriteRenderer>();
        foreach (Transform child in dotsContainer.transform) {
            dotsList.Add(child.Find("Center").GetComponent<SpriteRenderer>());
        }
    }
    public void pulse(string color = "red") {
        if (activeSpline)
            StartCoroutine(pulseRoutine(color));
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
