using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    Rigidbody2D myRB;
    Transform myAvatar;

    // [SerializeField] InputAction WASD_input;
    public Vector2 movementInput;

    [SerializeField] float movementSpeed;

    // private void OnEnable() {
    //     WASD_input.Enable();
    // }

    // private void OnDisable() {
    //     WASD_input.Disable();
    // }


    void Awake() {
        //TODO BLEND TREES
        myRB = GetComponent<Rigidbody2D>();
        myAvatar = transform.GetChild(0);
    }

    public void OnMovement(InputAction.CallbackContext _context) {
        movementInput = _context.ReadValue<Vector2>();
        if (movementInput.x != 0)
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
    }


    private void FixedUpdate() {
        myRB.velocity = movementInput * movementSpeed;
    }
}
