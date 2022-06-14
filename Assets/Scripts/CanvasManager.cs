using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System.Collections;


public class CanvasManager : MonoBehaviour {
    //ingame buttons functions and references
    public Button exitButton, resetButton, exitButtonConfirm, exitButtonCanell, resetButtonConfirm, resetButtonCancell, playerDisconnectedHomeButton, endLevelNextButton;








    public TextMeshProUGUI levelTitle;
    public Image UIMaskImage;

    IEnumerator Start() {

        while (LevelManager.instance.currentLevel == null)
            yield return null;
        levelTitle.text = LevelManager.instance.currentLevel.nombre;
        // Debug.Log(LevelManager.instance.currentLevel.nombre);

    }


    // Start is called before the first frame update
    // void Start() {
    //     PhotonNetwork.AutomaticallySyncScene = true;
    //     Invoke("ActivateMask", 0.15f);
    //     exitButton.onClick.AddListener(onExitLevel);
    //     resetButton.onClick.AddListener(onResetLevel);
    //     if (PhotonNetwork.IsConnectedAndReady) {
    //         resetButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    //     }
    // }

    // private void ActivateMask() {
    //     if (UIMaskImage) {
    //         UIMaskImage.enabled = false;
    //         UIMaskImage.enabled = true;
    //     }
    // }

    // public void onResetLevel() {
    //     Scene scene = SceneManager.GetActiveScene();
    //     if (PhotonNetwork.IsConnectedAndReady) {
    //         // if (PhotonNetwork.IsMasterClient) {
    //         photonView.RPC("RPC_onResetLevel", RpcTarget.AllViaServer, scene.name);

    //     } else {
    //         SceneManager.LoadScene(scene.name);
    //     }
    // }

    // public void onExitLevel() {
    //     if (PhotonNetwork.IsConnectedAndReady) {
    //         // if (PhotonNetwork.IsMasterClient) {
    //         photonView.RPC("RPC_onExitLevel", RpcTarget.AllViaServer);

    //     } else {
    //         SceneManager.LoadScene("LobbyScene");
    //     }
    // }

    // [PunRPC]
    // public void RPC_onExitLevel() {
    //     if (PhotonNetwork.IsMasterClient) {
    //         PhotonNetwork.LoadLevel("LobbyScene");
    //     }
    // }

    // [PunRPC]
    // public void RPC_onResetLevel(string sceneName) {
    //     if (sceneName == SceneManager.GetActiveScene().name) {
    //         SceneManager.LoadScene(sceneName);
    //         return;
    //     }

    //     if (PhotonNetwork.IsMasterClient) {
    //         PhotonNetwork.LoadLevel(sceneName);
    //     }
    // }
}
