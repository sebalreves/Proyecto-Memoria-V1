using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviourPunCallbacks {
    public bool player1Ready = false;
    public bool player2Ready = false;
    public GenericPlatform platform1, platform2;
    void Start() {
        platform1.setStatePressed += playerEnter;
        platform2.setStatePressed += playerEnter;
        platform1.setStateReleased += playerExit;
        platform2.setStateReleased += playerExit;
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    IEnumerator playerEnter(GameObject platformGO) {
        // Debug.Log("A");
        if (platformGO == platform1.gameObject) {
            player1Ready = true;
        }
        if (platformGO == platform2.gameObject) {
            player2Ready = true;
        }
        if (player1Ready && player2Ready) onEndLevel();
        if (!PhotonNetwork.IsConnectedAndReady && (player1Ready || player2Ready))
            onEndLevel();
        yield return null;
    }

    IEnumerator playerExit(GameObject platformGO) {
        if (platformGO == platform1.gameObject) {
            player1Ready = false;
        }
        if (platformGO == platform2.gameObject) {
            player2Ready = false;
        }
        yield return null;
    }



    [PunRPC]
    void onEndLevelRPC() {
        if (PhotonNetwork.IsConnectedAndReady) {
            PhotonNetwork.LeaveRoom();
        } else {
            SceneManager.LoadScene("LobbyScene");
        }

    }

    void onEndLevel() {
        if (PhotonNetwork.IsConnectedAndReady) {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
                photonView.RPC("onEndLevelRPC", RpcTarget.AllViaServer);
        } else {
            onEndLevelRPC();
        }
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("LobbyScene");
    }
}
