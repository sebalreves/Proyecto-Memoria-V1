using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun {
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
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        // if (!controllEnabled) return;
        movementInput = _context.ReadValue<Vector2>();
        if (movementInput.x != 0)
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
    }


    private void FixedUpdate() {
        if (!controllEnabled) return;
        playerRB.AddForce(movementInput * CONST.playerAcc);
        if (playerRB.velocity.sqrMagnitude > CONST.playerMaxSpeed * CONST.playerMaxSpeed)
            playerRB.velocity = playerRB.velocity.normalized * CONST.playerMaxSpeed;
    }

    public void playerMoveTo(Vector2 direction, float time) {
        StartCoroutine(playerMoveToRoutine(direction, time));
    }

    private IEnumerator playerMoveToRoutine(Vector2 direction, float time) {
        controllEnabled = false;
        movementInput = direction.normalized;
        yield return new WaitForSeconds(time);
        controllEnabled = true;
        // movementInput = Vector2.zero;
    }
}
