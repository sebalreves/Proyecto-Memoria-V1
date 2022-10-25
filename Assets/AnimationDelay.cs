using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour {
    // Start is called before the first frame update
    IEnumerator Start() {
        Animator myAnimator = GetComponent<Animator>();
        myAnimator.enabled = false;
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        myAnimator.enabled = true;

    }

    // Update is called once per frame
    void Update() {

    }
}
