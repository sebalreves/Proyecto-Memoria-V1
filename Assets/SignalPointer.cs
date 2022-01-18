using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SignalPointer : MonoBehaviourPun {
    public static int signalCount = 0;
    public QuestPointer questPointerReference;
    Color transparent = new Color(1f, 1f, 1f, 0f);
    private void Start() {
        // transparent = new Color
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
            signalCount++;
        questPointerReference = transform.GetChild(0).GetComponent<QuestPointer>();
        questPointerReference.gameObject.transform.position = gameObject.transform.position;
        questPointerReference.targetPosition = gameObject.transform.position;
        questPointerReference.pointerVelocity = 1f;
        Destroy(gameObject, 1f);
    }

    private void OnDestroy() {
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
            signalCount--;
    }

    private void Update() {
        questPointerReference.pointerRenderer.color = Color.Lerp(questPointerReference.pointerRenderer.color, transparent, 4f * Time.deltaTime);
        questPointerReference.pointerVelocity = Mathf.Lerp(questPointerReference.pointerVelocity, 0f, Time.deltaTime * 0.5f);
    }
}
