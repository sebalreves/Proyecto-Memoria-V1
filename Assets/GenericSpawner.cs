using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GenericSpawner : MonoBehaviourPun {
    public bool spawnBall;
    public bool spawnCube;
    public Transform spawnPoint;
    public GenericButton genericButtonScript;
    // Start is called before the first frame update
    void Start() {
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        if (!(spawnBall ^ spawnCube)) {
            Debug.LogWarning("Spawner cube y ball al mismo tiempo");
            return;
        }
        genericButtonScript.onPressEvent += spawnObject;
    }

    private void spawnObject() {
        if (spawnBall) {
            // Debug.Log("ball spawn");
            BallFactory._instance.instantiateBall(spawnPoint.position);
        } else if (spawnCube) {
            // Debug.Log("cube spawn");
            BallFactory._instance.instantiateCube(spawnPoint.position);

        }
    }
}
