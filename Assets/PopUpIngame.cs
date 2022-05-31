using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpIngame : MonoBehaviour {
    public GameObject messageContainer;
    public Animator messageContainerAnimator;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (!messageContainer.activeInHierarchy) {
                messageContainerAnimator.Play("open_message");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            messageContainerAnimator.Play("close_message");
        }
    }
}
