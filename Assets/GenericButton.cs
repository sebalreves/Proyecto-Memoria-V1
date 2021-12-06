using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour {
    public SpriteRenderer actualSprite;
    public Sprite activableSprite, noActivableSprite;
    public bool activable;
    public bool ejecutando;
    void Start() {
        checkActivable();
        actualSprite.sprite = noActivableSprite;
    }

    public void checkActivable() {
        activable = true;
        ejecutando = false;
    }


    public void Presionar() {
        if (activable && !ejecutando)
            StartCoroutine(rutinaPresionar());
    }

    private IEnumerator rutinaPresionar() {
        actualSprite.sprite = activableSprite;
        ejecutando = true;
        activable = false;
        yield return new WaitForSeconds(2);
        ejecutando = false;
        activable = true;
        actualSprite.sprite = noActivableSprite;

    }

    void Update() {

    }
}
