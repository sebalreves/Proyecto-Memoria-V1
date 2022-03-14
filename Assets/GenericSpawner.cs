using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GenericSpawner : MonoBehaviour {
    public bool spawnBall;
    public bool spawnCube;
    public Transform spawnPoint;
    public GenericButton genericButtonScript;
    public Color green, red;
    public CodeDescription codeDescription;
    public PhotonView photonView;



    IEnumerator Start() {
        // if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) yield break;
        yield return new WaitUntil(() => codeDescription.codeLines != null);
        if (!(spawnBall ^ spawnCube)) {
            Debug.LogWarning("Spawner cube y ball al mismo tiempo");
            yield break;
        }

        if (spawnBall) {
            codeDescription.codeLines[0] = "Spawn nuevo cubo";
        } else if (spawnCube) {
            codeDescription.codeLines[0] = "Spawn nueva pelota";
        }
        genericButtonScript.onPressEvent += spawnObject;
        genericButtonScript.spawner = true;
    }

    private IEnumerator spawnObject(GameObject buttonObject) {
        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green,
        _action: () => {
            if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
            if (spawnBall) {
                // Debug.Log("ball spawn");
                BallFactory._instance.instantiateBall(spawnPoint.position);

            } else if (spawnCube) {
                // Debug.Log("cube spawn");
                BallFactory._instance.instantiateCube(spawnPoint.position);
            }
        });

        yield return null;
    }
}
