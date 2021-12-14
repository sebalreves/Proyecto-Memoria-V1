using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDoor : MonoBehaviour {
    public bool opened;
    public Color openedColor, closedColor;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public bool interacting;

    public Action onInteractEvent;



    public void openOrClose() {
        if (opened) {
            spriteRenderer.color = closedColor;
            boxCollider.enabled = true;
        } else {
            spriteRenderer.color = openedColor;
            boxCollider.enabled = false;
        }
        opened = !opened;
    }


}
