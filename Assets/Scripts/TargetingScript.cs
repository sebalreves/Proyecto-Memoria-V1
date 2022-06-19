using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;


public class TargetingScript : MonoBehaviourPun {

    List<GameObject> nearObjects;
    public PlayerGrab playerGrabReference;
    GameObject actualFocus = null;
    GameObject actualBallFocus = null;


    private void Awake() {
        nearObjects = new List<GameObject>();
    }

    #region TARGETING
    public GameObject GetFirstTarget() {
        if (nearObjects.Count == 0) return null;
        List<GameObject> tempList = nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        return tempList[0];
    }

    public GameObject getTargetedBall() {
        var temp = nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        var nearBalls = temp.Where(x => (x.CompareTag("Ball") || x.CompareTag("Cube"))).ToList();
        if (nearBalls.Count > 0) {
            return nearBalls[0];
        }
        return null;
    }

    public GameObject getTargetedButton() {
        var temp = nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        var nearButtons = temp.Where(x => x.CompareTag("Button")).ToList();
        if (nearButtons.Count > 0) {
            return nearButtons[0];
        }
        return null;
    }



    private void FixedUpdate() {

        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        UpdateTargetedObject();
    }
    //usado cuando se grabea un objeto
    // public GameObject GetFirstTargetAndClearTargetList() {
    //     if (nearObjects.Count == 0) return null;
    //     List<GameObject> tempList = nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
    //     foreach (GameObject collider in tempList) {
    //         collider.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
    //     }
    //     GameObject tempTarget = tempList[0];
    //     nearObjects.Clear();
    //     return tempTarget;
    // }

    // private List<GameObject> sortObjects() {
    //     //nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList()
    //     var temp = nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
    //     var recogibles = nearObjects.Where(x => x.CompareTag("Ball") || x.CompareTag("Cube")).ToList();
    //     var activables = nearObjects.Where(x => !(x.CompareTag("Ball") || x.CompareTag("Cube"))).ToList();

    // }

    public void deactivateBallFocus() {
        //llamar cuando se toma una bola
        if (actualBallFocus != null) {
            // Debug.Log(actualBallFocus.name);
            enableTargeted(actualBallFocus, false);
            // actualBallFocus.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
            actualBallFocus = null;
        }
    }

    void enableTargeted(GameObject targetObject, bool newState) {
        // Debug.Log(targetObject.tag);
        targetObject.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(newState);
        if (newState && (targetObject.GetComponent<GenericButton>() != null || targetObject.GetComponent<GenericPlatform>() != null)) {
            //acercarse a boton o plataforma
            CodeLineManager._instance.ShineAnimation();
            // targetObject.transform.Find("TargetZone").Find("Canvas").gameObject.SetActive(true);
        }



        var ingameUI = targetObject.transform.parent.Find("InGameUI");
        if (!ingameUI) return;
        ingameUI.gameObject.SetActive(newState);
        if (newState) {
            Vector3 anchor = ingameUI.gameObject.transform.Find("1stSpring").GetComponent<SpringJoint2D>().anchor;
            ingameUI.gameObject.transform.Find("1stSpring").transform.position = targetObject.transform.position - anchor;
            ingameUI.gameObject.transform.Find("2ndSpring").transform.position = targetObject.transform.position - anchor;
        }
        // actualBallFocus.transform.Find("TargetZone").GetComponent<CodeDescription>().UIIngameReference.SetActive(newState);
    }

    public void UpdateTargetedObject() {
        // Debug.Log(nearObjects.Count);

        for (int i = nearObjects.Count - 1; i >= 0; i--) {
            if (nearObjects[i] == null) nearObjects.Remove(nearObjects[i]);
        }
        if (nearObjects.Count == 0) {
            actualBallFocus = null;
            actualFocus = null;
            return;
        }

        // if (!playerGrabReference.grabingBall || true) {

        var temp = nearObjects.OrderBy(grabable => Vector2.SqrMagnitude(grabable.transform.position - gameObject.transform.position)).ToList();
        var nearBalls = temp.Where(x => x.CompareTag("Ball") || x.CompareTag("Cube")).ToList();
        var nearActives = temp.Where(x => !(x.CompareTag("Ball") || x.CompareTag("Cube"))).ToList();

        if (nearBalls.Count == 0) actualBallFocus = null;
        if (nearActives.Count == 0) actualFocus = null;

        // Debug.Log("balls: " + nearBalls.Count + "actives: " + nearActives.Count);
        //Balls 
        if (nearBalls.Count > 0 && !playerGrabReference.grabingBall && actualBallFocus != nearBalls[0]) {
            //solo si no se esta tomando una bola, desactivar la actual, y target la nueva mas cercana
            if (actualBallFocus != null) {
                // actualBallFocus.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
                enableTargeted(actualBallFocus, false);
            }
            actualBallFocus = nearBalls[0];
            enableTargeted(actualBallFocus, true);
            // actualBallFocus.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(true);
        }

        //Activables (botones y plataformas)
        if (nearActives.Count > 0 && actualFocus != nearActives[0]) {
            if (actualFocus != null) {
                enableTargeted(actualFocus, false);

                // actualFocus.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
            }
            actualFocus = nearActives[0];
            enableTargeted(actualFocus, true);

            // actualFocus.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(true);
            updateCodeDescription();

            // actualFocus.transform.Find("TargetZone").GetComponent<CodeDescription>().targeted = true;

            // updateCodeDescription();
        }

        // else {
        //     if (actualFocus != null) {
        //         actualFocus.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
        //         actualFocus = null;
        //     }

        //     // foreach (GameObject collider in grabableObjects) {
        //     //     collider.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
        //     // }
        // }

        // else {
        //     actualFocus = null;
        // }

    }

    void updateCodeDescription() {
        // if (actualFocus == null) return;

        //las bolas y cubos no actualizan las lineas de codigo
        // if (actualFocus.CompareTag(CONST.ballTag) || actualFocus.CompareTag(CONST.cubeTag)) return;
        CodeDescription descriptionManager = actualFocus.transform.Find("TargetZone").GetComponent<CodeDescription>();
        // descriptionManager.targeted = true;


        CodeLineManager._instance.newTargetResetCodeLines(descriptionManager.titulo, descriptionManager.codeLines, actualFocus);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        //add collider
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        if (other.gameObject.CompareTag("TargetZone")) {
            var parent = other.gameObject.transform.parent.gameObject;
            if (!nearObjects.Contains(parent)) {
                nearObjects.Add(parent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //pop object
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        if (other.gameObject.CompareTag("TargetZone")) {
            var parent = other.gameObject.transform.parent.gameObject;
            if (nearObjects.Contains(parent)) {
                nearObjects.Remove(parent);
                enableTargeted(parent, false);
                // parent.transform.Find("TargetZone").transform.Find("FocusedSprite").gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
