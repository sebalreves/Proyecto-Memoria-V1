using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFactory : MonoBehaviour {

    public static PlayerFactory _instance;
    public Dictionary<int, GameObject> instancedPlayers;
    public GameObject playerPrefab;
    public GameObject localPlayer, noLocalPlayer;


    void Awake() {
        instancedPlayers = new Dictionary<int, GameObject>();

        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);

        } else if (_instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);

        }
    }




    public GameObject instantiatePlayer(Vector2 _instantiatePosition) {
        GameObject spawnedPlayer;
        if (PhotonNetwork.IsConnectedAndReady) {
            spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, _instantiatePosition, Quaternion.identity);
            // if (PhotonNetwork.IsMasterClient) {
            //     if (spawnedPlayer.GetComponent<PhotonView>().IsMine) {
            //         spawnedPlayer.transform.Find("Sprites").Find("BodyAnimated").GetChild(0).gameObject.SetActive(true);
            //     } else
            //         spawnedPlayer.transform.Find("Sprites").Find("BodyAnimated").GetChild(1).gameObject.SetActive(true);

            // } else {
            //     if (spawnedPlayer.GetComponent<PhotonView>().IsMine) {
            //         spawnedPlayer.transform.Find("Sprites").Find("BodyAnimated").GetChild(1).gameObject.SetActive(true);
            //     } else
            //         spawnedPlayer.transform.Find("Sprites").Find("BodyAnimated").GetChild(0).gameObject.SetActive(true);
            // }


            instancedPlayers.Add(spawnedPlayer.GetComponent<PhotonView>().ViewID, spawnedPlayer);
        } else {
            spawnedPlayer = Instantiate(playerPrefab, _instantiatePosition, Quaternion.identity);
            instancedPlayers.Add(spawnedPlayer.GetInstanceID(), spawnedPlayer);

        }
        return spawnedPlayer;
    }

    //FIND LOCAL PLAYER
    //FIND  NOT LOCAL

    public int GetPlayerId(GameObject playerGO) {
        if (PhotonNetwork.IsConnectedAndReady) {
            return playerGO.GetComponent<PhotonView>().ViewID;
        } else {
            return playerGO.GetInstanceID();
        }
    }

    public GameObject findPlayer(int _id) {
        GameObject temp;

        if (PhotonNetwork.IsConnectedAndReady) {
            return PhotonView.Find(_id).gameObject;
        }

        if (instancedPlayers.TryGetValue(_id, out temp)) {
            return temp;
        } else {
            Debug.LogWarning("Jugador no encontrado");
            return null;
        }
    }

    //TODO destroy
    public void DestroyPlayer(int _id) {
        GameObject temp;
        if (instancedPlayers.TryGetValue(_id, out temp)) {
            PhotonNetwork.Destroy(temp.GetComponent<PhotonView>());
        } else {
            Debug.LogWarning("Jugador no encontrado");
        }
    }
}
