using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpringJointBreakScript : MonoBehaviour {
    public SpringJoint2D mySpringJoin;
    public float tempBreakForce = 1000f;

    private void Start() {
        mySpringJoin = gameObject.GetComponent<SpringJoint2D>();
    }


    public void createSpringComponent(Rigidbody2D attachedRigidBody = null) {
        // Debug.Log("creando spring");
        Destroy(mySpringJoin);
        SpringJoint2D newJoint = gameObject.AddComponent<SpringJoint2D>() as SpringJoint2D;
        newJoint.autoConfigureDistance = false;
        newJoint.distance = 0f;
        newJoint.frequency = CONST.frequency;
        newJoint.breakForce = 1000f;
        newJoint.enabled = true;
        newJoint.connectedBody = attachedRigidBody;
        mySpringJoin = newJoint;
    }
    public void OnJointBreak2D(Joint2D brokenJoint) {
        // Debug.Log("Broke joint");
        //Crear nueva joint vacia
        // createSpringComponent(); 
        //Notificar al player y a la pelota
        if (PhotonNetwork.IsConnectedAndReady) {
            if (gameObject.transform.parent.gameObject.GetComponent<PhotonView>().IsMine)
                gameObject.transform.parent.gameObject.GetComponent<PlayerGrab>().TryReleaseAndThrow();
        } else
            gameObject.transform.parent.gameObject.GetComponent<PlayerGrab>().TryReleaseAndThrow();
    }

    private void FixedUpdate() {
        if (mySpringJoin != null && mySpringJoin.breakForce > CONST.breakForce + 1) {
            mySpringJoin.breakForce = Mathf.Lerp(mySpringJoin.breakForce, CONST.breakForce, Time.fixedDeltaTime / 0.5f);
        }
    }
}
