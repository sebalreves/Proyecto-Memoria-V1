using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpIngame : MonoBehaviour {
    public GameObject messageContainer;
    public Animator messageContainerAnimator;
    public int actualPlayers = 0;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            actualPlayers++;
            if (!messageContainer.activeInHierarchy) {
                messageContainerAnimator.Play("open_message");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            actualPlayers--;
            if (actualPlayers == 0)
                messageContainerAnimator.Play("close_message");
        }
    }
}
