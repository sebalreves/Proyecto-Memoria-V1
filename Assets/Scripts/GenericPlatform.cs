using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlatform : MonoBehaviour {
    public PointEffector2D attractPointEffector;
    public bool presionado = false;
    public bool activado = false;
    public Color ActivatedColor, DeactivatedColor;
    private Animator animatorButton;
    private Animator animatorEngine;
    public CodeDescription codeDescription;
    private float lagInicial = 1f;

    public bool isNormalPlatform = true;

    // public SpriteRenderer spriteReference;


    public Func<GameObject, IEnumerator> setStatePressed;
    public Func<GameObject, IEnumerator> setStateReleased;

    public platformTrigger platformTriggerReference;
    public Coroutine loopingCodeRoutine = null;

    public void ChangeAnimation(string action) {
        if (!isNormalPlatform) return;
        if (action == "Press") {
            animatorEngine.enabled = true; // or speed
            animatorButton.Play("Boton_Down");

        } else if (action == "Release") {
            animatorEngine.enabled = false;
            animatorButton.Play("Boton_Up");
        }
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName(newAnimationName)) return;
        // animator.Play(newAnimationName);
    }

    private void Awake() {
        if (isNormalPlatform) {
            animatorButton = gameObject.transform.Find("Base").Find("Button").GetComponent<Animator>();
            animatorEngine = gameObject.transform.Find("Base").Find("Tuerca").GetComponent<Animator>();
            animatorEngine.enabled = false;
            codeDescription.titulo = codeDescription.titulo + "<sprite=1>";
        }


        // ActivatePlatform();
    }

    private void OnDisable() {
        if (setStatePressed != null)
            foreach (var d in setStatePressed.GetInvocationList())
                setStatePressed -= (d as Func<GameObject, IEnumerator>);

        if (setStateReleased != null)
            foreach (var d in setStateReleased.GetInvocationList())
                setStateReleased -= (d as Func<GameObject, IEnumerator>);
    }


    public void ActivatePlatform() {
        //si se presiona la plataforma
        // Debug.Log(platformTriggerReference.presionado + " " + platformTriggerReference.activado);
        if (platformTriggerReference.presionado && !presionado) {
            presionado = true;
            ChangeAnimation("Press");
            // spriteReference.color = ActivatedColor;
            attractPointEffector.enabled = false;
            activado = platformTriggerReference.activado;
            if (setStatePressed != null) {
                if (loopingCodeRoutine != null) StopCoroutine(loopingCodeRoutine);
                CodeLineManager._instance.resetCodeColor();
                loopingCodeRoutine = StartCoroutine(setStatePressed(gameObject));
            }
            return;
        } else if (!platformTriggerReference.presionado && presionado) {
            //si se libera la plataforma
            presionado = false;
            ChangeAnimation("Release");
            // spriteReference.color = DeactivatedColor;
            attractPointEffector.enabled = true;
            activado = false;
            if (setStateReleased != null) {
                if (loopingCodeRoutine != null) StopCoroutine(loopingCodeRoutine);
                CodeLineManager._instance.resetCodeColor();

                StartCoroutine(setStateReleased(gameObject));
            }
            return;
        }

        //si estando presionada, se activa o desactiva
        if (!platformTriggerReference.presionado) return;
        if (platformTriggerReference.activado ^ activado) {
            activado = !activado;
            if (setStatePressed != null) {
                if (loopingCodeRoutine != null) StopCoroutine(loopingCodeRoutine);
                CodeLineManager._instance.resetCodeColor();
                loopingCodeRoutine = StartCoroutine(setStatePressed(gameObject));
            }
        }
    }

    private void FixedUpdate() {
        if (lagInicial > 0f) {
            lagInicial -= Time.fixedDeltaTime;
        } else
            ActivatePlatform();
    }
}
