using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallFactory : MonoBehaviour {
    public static BallFactory _instance;
    public Dictionary<int, GameObject> instancedBalls;
    public GameObject ballPrefab;

    //TODO check network sync of dictionary
    void Awake() {
        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);

        } else if (_instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);

        }
    }

    private void Start() {
        instancedBalls = new Dictionary<int, GameObject>();
    }


    public GameObject instantiateBall(Vector2 _instantiatePosition) {
        GameObject spawnedBall;
        if (PhotonNetwork.IsConnectedAndReady) {
            spawnedBall = PhotonNetwork.Instantiate(ballPrefab.name, _instantiatePosition, Quaternion.identity);
            instancedBalls.Add(spawnedBall.GetComponent<PhotonView>().ViewID, spawnedBall);
        } else {
            spawnedBall = Instantiate(ballPrefab, _instantiatePosition, Quaternion.identity);
            instancedBalls.Add(spawnedBall.GetInstanceID(), spawnedBall);

        }
        return spawnedBall;
    }

    public GameObject findBall(int _id) {
        GameObject temp;
        if (instancedBalls.TryGetValue(_id, out temp)) {
            return temp;
        } else {
            Debug.LogWarning("Bola no encontrada");
            return null;
        }
    }

    //TODO destroy
    public void DestroyBall(int _id) {
        GameObject temp;
        if (instancedBalls.TryGetValue(_id, out temp)) {
            PhotonNetwork.Destroy(temp.GetComponent<PhotonView>());
        } else {
            Debug.LogWarning("Bola no encontrada");
        }
    }


}
