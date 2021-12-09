using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlatformInteract : MonoBehaviour {
    //SCRIPT QUE TIENEN BALLS Y CUBES   
    public Rigidbody2D myRb;
    public PointEffector2D objectRepelEffector;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("PlatformInnerCollider")) {
            // other.gameObject.GetComponent<GenericPlatform>().ActivatePlatform();
            objectRepelEffector.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("PlatformInnerCollider")) {
            // other.gameObject.GetComponent<GenericPlatform>().DeactivatePlatform();
            objectRepelEffector.enabled = false;

        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("PlatformInnerCollider")) {
            Vector2 dir = other.transform.position - gameObject.transform.position;
            myRb.AddForce((other.transform.position - gameObject.transform.position) * CONST.PLATFORM_INNER_ATRACTION * myRb.mass);
        }
    }
}
