using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class EndLevel : MonoBehaviourPunCallbacks {
    public int playerReadyCount = 0;
    public GenericPlatform platform1;
    public TextMeshProUGUI playerCountText;
    public
    void Start() {
        // platform1.setStatePressed += playerEnter;
        // // platform2.setStatePressed += playerEnter;
        // platform1.setStateReleased += playerExit;
        // platform2.setStateReleased += playerExit;
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Player") return;
        // Debug.Log("A");

        playerReadyCount++;
        playerCountText.text = playerReadyCount + playerCountText.text.Substring(1);

        // if (platformGO == platform2.gameObject) {
        //     player2Ready = true;
        // }
        if (playerReadyCount == 2) Invoke("onEndLevel", 1f);
        if (!PhotonNetwork.IsConnectedAndReady && playerReadyCount == 1)
            Invoke("onEndLevel", 1f);

    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "Player") return;


        playerReadyCount--;
        playerCountText.text = playerReadyCount + playerCountText.text.Substring(1);

        // if (platformGO == platform2.gameObject) {
        //     player2Ready = false;
        // }

    }





    void onEndLevel() {
        if (PhotonNetwork.IsConnectedAndReady) {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
                photonView.RPC("onEndLevelRPC", RpcTarget.AllViaServer);
        } else {
            onEndLevelRPC();
        }
    }

    [PunRPC]
    void onEndLevelRPC() {
        if (PhotonNetwork.IsConnectedAndReady) {
            PhotonNetwork.LeaveRoom();
        } else {
            SceneManager.LoadScene("LobbyScene");
        }

    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("LobbyScene");
    }
}
