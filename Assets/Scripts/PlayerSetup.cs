using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks {
    public GameObject PlayerCamera;
    public TextMeshProUGUI PlayerNameText;
    // Start is called before the first frame update

    void Start() {
        if (gameObject.name == "Player") {
            Destroy(gameObject);
            return;
        }
        if (!PhotonNetwork.IsConnectedAndReady) {
            PlayerFactory._instance.localPlayer = gameObject;
            Debug.Log("OFFLINE");
            return;
        }
        if (photonView.IsMine) {
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerGrab>().enabled = true;
            GetComponent<TargetingScript>().enabled = true;
            GetComponent<PlayerInteract>().enabled = true;
            gameObject.transform.Find("GrabPosition").GetComponent<SpringJointBreakScript>().enabled = true;
            PlayerCamera.SetActive(true);
            gameObject.transform.Find("Camera").GetComponent<CameraManager>().enabled = true;
            PlayerFactory._instance.localPlayer = gameObject;

        } else {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerGrab>().enabled = false;
            GetComponent<TargetingScript>().enabled = false;
            GetComponent<PlayerInteract>().enabled = false;
            gameObject.transform.Find("GrabPosition").GetComponent<SpringJointBreakScript>().enabled = false;
            PlayerCamera.SetActive(false);

            // gameObject.transform.Find("Camera").GetComponent<CameraManager>().enabled = false;
            Destroy(gameObject.transform.Find("Camera").gameObject);
            PlayerFactory._instance.noLocalPlayer = gameObject;
        }
        // if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        // {
        //     if (photonView.IsMine)
        //     {

        //         //enable carMovement script and camera
        //         GetComponent<CarMovement>().enabled = true;
        //         GetComponent<LapController>().enabled = true;
        //         PlayerCamera.enabled = true;

        //     }
        //     else
        //     {
        //         //Player is remote. Disable CarMovement script and camera.
        //         // GetComponent<CarMovement>().enabled = false;
        //         // GetComponent<LapController>().enabled = false;
        //         PlayerCamera.enabled = false;

        //     }

        // }else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        // {
        //     if (photonView.IsMine)
        //     {

        //         //enable carMovement script and camera
        //         GetComponent<CarMovement>().enabled = true;
        //         GetComponent<CarMovement>().controlsEnabled = true;
        //         PlayerCamera.enabled = true;

        //     }
        //     else
        //     {
        //         //Player is remote. Disable CarMovement script and camera.
        //         GetComponent<CarMovement>().enabled = false;

        //         PlayerCamera.enabled = false;

        //     }
        // }




        // SetPlayerUI();
    }


    // private void SetPlayerUI() {
    //     if (PlayerNameText != null) {
    //         PlayerNameText.text = photonView.Owner.NickName;

    //         if (photonView.IsMine) {
    //             PlayerNameText.gameObject.SetActive(false);
    //         }
    //     }
    // }
}
