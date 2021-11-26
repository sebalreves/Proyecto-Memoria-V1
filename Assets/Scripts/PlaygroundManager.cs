using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlaygroundManager : MonoBehaviour {

    public GameObject playerPrefab;
    public static PlaygroundManager instance;
    public Transform[] spawnPositions;

    private void Awake() {
        //check if instance already exists
        if (instance == null) {
            instance = this;
        }

        //If intance already exists and it is not !this!
        else if (instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);

        }

        //To not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    void Start() {

        if (PhotonNetwork.IsConnectedAndReady) {
            // object playerSelectionNumber;
            // if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber)) {
            // }

            // Debug.Log((int)playerSelectionNumber);

            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 instantiatePosition = spawnPositions[actorNumber - 1].position;

            PhotonNetwork.Instantiate(playerPrefab.name, instantiatePosition, Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update() {

    }
}
