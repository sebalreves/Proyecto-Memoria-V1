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
            //TODO spawn player 1 y player 2 dependiendo quien es el owner
            // object playerSelectionNumber;
            // if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber)) {
            // }

            // Debug.Log((int)playerSelectionNumber);

            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 instantiatePosition = spawnPositions[actorNumber - 1].position;
            PlayerFactory._instance.instantiatePlayer(instantiatePosition);

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                BallFactory._instance.instantiateBall(new Vector2(-4, -1));
        } else {
            // PlayerFactory._instance.instantiatePlayer(spawnPositions[0].position);
            BallFactory._instance.instantiateBall(new Vector2(-4, -1));
        }
    }


    void Update() {
    }
    //TODO if player disconnects, end game

}
