using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class CanvasManager : MonoBehaviourPun {
    //ingame buttons functions and references
    public Button exitButton, resetButton;
    public Image UIMaskImage;
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.AutomaticallySyncScene = true;
        Invoke("ActivateMask", 0.1f);
        exitButton.onClick.AddListener(
        // PlaygroundEvents.test
        () => {
            Debug.Log("Exit");
        }
            );

        resetButton.onClick.AddListener(onResetLevel);
    }

    private void ActivateMask() {
        if (UIMaskImage) {

            UIMaskImage.enabled = false;
            UIMaskImage.enabled = true;
        }
    }

    public void onResetLevel() {
        Scene scene = SceneManager.GetActiveScene();
        if (PhotonNetwork.IsConnectedAndReady) {
            // if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("RPC_onResetLevel", RpcTarget.AllViaServer, scene.name);

        } else {
            SceneManager.LoadScene(scene.name);
        }
    }

    [PunRPC]
    public void RPC_onResetLevel(string sceneName) {
        if (sceneName == SceneManager.GetActiveScene().name) {
            SceneManager.LoadScene(sceneName);
            return;
        }

        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}
