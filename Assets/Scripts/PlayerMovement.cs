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
    public Vector2 movementInputScript;
    public bool movingViaScript = false;

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
        // Debug.Log("AAA");
        movementInput = _context.ReadValue<Vector2>();
        if (movementInput.x != 0)
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
    }


    private void FixedUpdate() {
        // if (!controllEnabled && !movingViaScript) return;
        // Debug.Log(movementInput);
        Vector2 currentMove = movementInput * (controllEnabled ? 1 : 0) + movementInputScript * (movingViaScript ? 1 : 0);
        playerRB.AddForce(currentMove * CONST.playerAcc);
        if (playerRB.velocity.sqrMagnitude > CONST.playerMaxSpeed * CONST.playerMaxSpeed)
            playerRB.velocity = playerRB.velocity.normalized * CONST.playerMaxSpeed;
    }

    public void playerMoveTo(Vector2 direction, float time, bool setControllsEnbaled = true) {
        StartCoroutine(playerMoveToRoutine(direction, time, setControllsEnbaled));
    }

    public void playerTeleportTo(Vector2 position) {
        StartCoroutine(teleportRoutine(position));
    }

    public IEnumerator teleportRoutine(Vector2 position) {
        controllEnabled = false;
        movementInput = Vector2.zero;
        playerRB.velocity = Vector2.zero;
        playerRB.position = position;
        yield return new WaitForSeconds(1f);
        controllEnabled = true;
    }

    private IEnumerator playerMoveToRoutine(Vector2 direction, float time, bool setControllsEnbaled) {
        movingViaScript = true;
        controllEnabled = false;
        movementInputScript = direction.normalized;
        yield return new WaitForSeconds(time);
        movementInputScript = Vector2.zero;
        movingViaScript = false;
        yield return new WaitForSeconds(0.5f);

        if (setControllsEnbaled)
            controllEnabled = true;

        // movementInput = Vector2.zero;
    }
}
