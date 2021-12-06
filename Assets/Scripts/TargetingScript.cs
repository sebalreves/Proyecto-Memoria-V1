using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;


public class TargetingScript : MonoBehaviourPun {

    List<GameObject> grabableObjects;
    public PlayerGrab playerGrabReference;

    private void Awake() {
        grabableObjects = new List<GameObject>();
    }

    #region TARGETING
    public GameObject GetFirstTarget() {
        if (grabableObjects.Count == 0) return null;
        List<GameObject> tempList = grabableObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        return tempList[0];
    }

    //usado cuando se grabea un objeto
    public GameObject GetFirstTargetAndClearTargetList() {
        if (grabableObjects.Count == 0) return null;
        List<GameObject> tempList = grabableObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        foreach (GameObject collider in tempList) {
            collider.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
        }
        GameObject tempTarget = tempList[0];
        grabableObjects.Clear();
        return tempTarget;
    }

    public void UpdateTargetedObject() {
        if (grabableObjects.Count > 0) {
            if (!playerGrabReference.grabingBall) {
                List<GameObject> temp = grabableObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
                foreach (GameObject collider in temp) {
                    collider.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
                }
                temp[0].transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(true);
            } else {
                foreach (GameObject collider in grabableObjects) {
                    collider.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //add collider
        if (!playerGrabReference.grabingBall && other.gameObject.CompareTag("TargetZone") && (photonView.IsMine || !PhotonNetwork.IsConnectedAndReady)) {
            // pushObjectFromTargetList(other.gameObject);
            // Debug.Log(other.gameObject);
            var parent = other.gameObject.transform.parent.gameObject;
            if (!grabableObjects.Contains(parent)) {
                grabableObjects.Add(parent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //pop object
        if (!playerGrabReference.grabingBall && other.gameObject.CompareTag("TargetZone") && (photonView.IsMine || !PhotonNetwork.IsConnectedAndReady)) {
            // Debug.Log(other.gameObject);
            var parent = other.gameObject.transform.parent.gameObject;
            if (grabableObjects.Contains(parent)) {
                grabableObjects.Remove(parent);
                parent.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
