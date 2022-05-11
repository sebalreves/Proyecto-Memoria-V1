using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericWindArea : MonoBehaviour {
    // Start is called before the first frame update
    public bool activated;
    private float alfa;
    public GameObject effector;
    public ParticleSystem particles;
    // public SpriteRenderer spriteReference;
    public RawImage spriteReference;
    public float spriteVelocity = 2f;

    public Animator ventilador_1_animator, ventilador_2_animator;

    public void ChangeAnimation(string newAnimationName) {
        if (ventilador_1_animator.GetCurrentAnimatorStateInfo(0).IsName(newAnimationName)) return;
        ventilador_1_animator.Play(newAnimationName);

        if (ventilador_2_animator.GetCurrentAnimatorStateInfo(0).IsName(newAnimationName)) return;
        ventilador_2_animator.Play(newAnimationName);
    }
    public void Start() {
        // effector = gameObject.transform.Find("Effectors").transform.gameObject;
        // effector2 = gameObject.transform.Find("Effectors").transform.GetChild(1).GetComponent<AreaEffector2D>();
        // particles = gameObject.transform.Find("Particles").GetComponent<ParticleSystem>();
        // spriteReference = gameObject.transform.Find("Canvas").transform.Find("GameObject").GetComponent<RawImage>();
        alfa = spriteReference.color.a;
        // startLoop(5f, 3f);
    }

    public void activar() {
        activated = true;
        ChangeAnimation("Ventilador_activado");
    }
    public void desactivar() {
        activated = false;
        ChangeAnimation("Ventilador_apagado");

    }

    private void Update() {
        Rect tempRect = spriteReference.uvRect;
        tempRect.x = (spriteReference.uvRect.x - spriteVelocity * Time.deltaTime) % 1;
        spriteReference.uvRect = tempRect;
        if (activated) {
            // var alfa = 0f;
            spriteVelocity = Mathf.Lerp(spriteVelocity, CONST.spriteVelocity, 1f * Time.deltaTime);
            effector.SetActive(true);
            var main = particles.main;
            main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, CONST.simulationSpeedPlay, 1f * Time.deltaTime);
            if (alfa < 0.25f) {
                alfa += 0.8f * Time.deltaTime;
                Color temp = spriteReference.color;
                temp.a = alfa;
                spriteReference.color = temp;
                // yield return new WaitForFixedUpdate();
            }

        } else {
            // var alfa = 0.6f;
            spriteVelocity = Mathf.Lerp(spriteVelocity, 0f, 1f * Time.deltaTime);
            effector.SetActive(false);
            var main = particles.main;
            main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, CONST.simulationSpeedPause, 1f * Time.deltaTime);
            if (alfa > 0) {
                alfa -= 0.8f * Time.deltaTime;
                Color temp = spriteReference.color;
                temp.a = Mathf.Abs(alfa);
                spriteReference.color = temp;
                // yield return new WaitForFixedUpdate();
            }
        }
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
