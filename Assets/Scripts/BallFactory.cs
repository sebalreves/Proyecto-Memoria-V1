using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallFactory : MonoBehaviour {
    public static BallFactory _instance;
    public Dictionary<int, GameObject> instancedBalls;
    public Dictionary<int, GameObject> instancedCubes;
    public GameObject ballPrefab, cubePrefab;


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
        instancedCubes = new Dictionary<int, GameObject>();
    }


    public GameObject instantiateBall(Vector2 _instantiatePosition) {
        return instantiateObject(_instantiatePosition, ballPrefab);

    }

    public GameObject instantiateCube(Vector2 _instantiatePosition) {
        return instantiateObject(_instantiatePosition, cubePrefab);
    }

    private GameObject instantiateObject(Vector2 _instantiatePosition, GameObject _prefab) {
        GameObject spawnedObject;
        if (PhotonNetwork.IsConnectedAndReady) {
            spawnedObject = PhotonNetwork.Instantiate(_prefab.name, _instantiatePosition, Quaternion.identity);
            instancedBalls.Add(spawnedObject.GetComponent<PhotonView>().ViewID, spawnedObject);
        } else {
            spawnedObject = Instantiate(_prefab, _instantiatePosition, Quaternion.identity);
            instancedBalls.Add(spawnedObject.GetInstanceID(), spawnedObject);
        }
        return spawnedObject;
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
