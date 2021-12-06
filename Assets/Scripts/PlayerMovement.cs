using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public Rigidbody2D playerRB;
    Transform myAvatar;
    public bool controllEnabled;
    public Vector2 movementInput;

    [SerializeField] float movementSpeed;



    void Awake() {
        //TODO BLEND TREES
        playerRB = GetComponent<Rigidbody2D>();
        myAvatar = transform.GetChild(0);
        controllEnabled = true;
    }

    public void OnMovement(InputAction.CallbackContext _context) {
        if (!controllEnabled) return;
        movementInput = _context.ReadValue<Vector2>();
        if (movementInput.x != 0)
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
    }


    private void FixedUpdate() {
        playerRB.velocity = movementInput * movementSpeed;
    }

    public void playerMoveTo(Vector2 direction, float time) {
        StartCoroutine(playerMoveToRoutine(direction, time));
    }

    private IEnumerator playerMoveToRoutine(Vector2 direction, float time) {
        controllEnabled = false;
        movementInput = direction.normalized;
        yield return new WaitForSeconds(time);
        controllEnabled = true;
        movementInput = Vector2.zero;
    }
}
