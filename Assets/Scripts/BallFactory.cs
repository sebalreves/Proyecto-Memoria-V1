using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;


public class BallFactory : MonoBehaviour {
    public static BallFactory _instance;
    public Dictionary<int, GameObject> instancedBalls;
    // public Dictionary<int, GameObject> instancedCubes;
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
        // instancedCubes = new Dictionary<int, GameObject>();

        //register all object with "ball" tag
        //TODO Encontrar instancias de scene, destruirlas y crearlas con photon.instantiate
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")) {
            if (PhotonNetwork.IsConnectedAndReady) {
                object[] customData = new object[] { ball.GetComponent<GenericBall>().color };
                Vector3 position = ball.transform.position;
                Destroy(ball);
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Instantiate(ballPrefab.name, position, Quaternion.identity, 0, customData);
                // instancedBalls.Add(syncBall.GetComponent<PhotonView>().ViewID, syncBall);
            } else
                instancedBalls.Add(ball.GetInstanceID(), ball);
        }
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {
            if (PhotonNetwork.IsConnectedAndReady) {
                object[] customData = new object[] { cube.GetComponent<GenericBall>().color };
                Vector3 position = cube.transform.position;
                Destroy(cube);
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Instantiate(cubePrefab.name, position, Quaternion.identity, 0, customData);
                // instancedBalls.Add(syncCube.GetComponent<PhotonView>().ViewID, syncCube);
            } else
                instancedBalls.Add(cube.GetInstanceID(), cube);
        }
    }


    public GameObject instantiateBall(Vector2 _instantiatePosition, string _color = null) {
        return instantiateObject(_instantiatePosition, ballPrefab, _color);

    }

    public GameObject instantiateCube(Vector2 _instantiatePosition, string _color = null) {
        return instantiateObject(_instantiatePosition, cubePrefab, _color);
    }

    private GameObject instantiateObject(Vector2 _instantiatePosition, GameObject _prefab, string _color = null) {
        if (_color == null) {
            _color = CONST.Red;
        }
        GameObject spawnedObject;
        if (PhotonNetwork.IsConnectedAndReady) {
            object[] customData = new object[] { _color };
            spawnedObject = PhotonNetwork.Instantiate(_prefab.name, _instantiatePosition, Quaternion.identity, 0, customData);
            instancedBalls.Add(spawnedObject.GetComponent<PhotonView>().ViewID, spawnedObject);
        } else {
            spawnedObject = Instantiate(_prefab, _instantiatePosition, Quaternion.identity);
            instancedBalls.Add(spawnedObject.GetInstanceID(), spawnedObject);
        }
        return spawnedObject;
    }

    public void deleteGroup(string _shape = null, string _color = null) {
        List<GameObject> toDeleteList = new List<GameObject>();
        //BALLS
        if (_shape == CONST.Ball) {
            toDeleteList = toDeleteList.Concat(GameObject.FindGameObjectsWithTag(CONST.Ball).ToList()).ToList();
        }
        //CUBES
        else if (_shape == CONST.Cube) {
            toDeleteList = toDeleteList.Concat(GameObject.FindGameObjectsWithTag(CONST.Cube).ToList()).ToList();
        } else {
            toDeleteList = toDeleteList.Concat(GameObject.FindGameObjectsWithTag(CONST.Cube).ToList()).ToList();
            toDeleteList = toDeleteList.Concat(GameObject.FindGameObjectsWithTag(CONST.Ball).ToList()).ToList();
        }

        //FILTER BY COLOR
        if (_color != null)
            for (int i = toDeleteList.Count - 1; i >= 0; i--) {
                if (toDeleteList[i].GetComponent<GenericBall>().color != _color) {
                    toDeleteList.RemoveAt(i);
                }
            }

        StartCoroutine(iterativeDeleteRoutine(toDeleteList));
    }

    private IEnumerator iterativeDeleteRoutine(List<GameObject> toDeleteList) {
        while (toDeleteList.Count > 0) {
            var toDeleteItem = toDeleteList[0];
            toDeleteList.RemoveAt(0);
            DestroyBall(toDeleteItem);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public GameObject findBallById(int _id) {
        GameObject temp;
        if (PhotonNetwork.IsConnectedAndReady) {
            return PhotonView.Find(_id).gameObject;
        }
        if (instancedBalls.TryGetValue(_id, out temp)) {
            return temp;
        } else {
            Debug.LogWarning("Bola no encontrada");
            return null;
        }
    }

    public int getBallId(GameObject ball) {
        if (PhotonNetwork.IsConnectedAndReady) {
            return ball.GetComponent<PhotonView>().ViewID;
        } else {
            return ball.GetInstanceID();
        }
    }



    //TODO destroy
    public void DestroyBall(GameObject _ball) {
        if (PhotonNetwork.IsConnectedAndReady) {
            if (PhotonNetwork.IsMasterClient) {
                _ball.GetComponent<PhotonView>().RPC("ReleaseBallBlackHole", RpcTarget.AllBuffered, _ball.GetComponent<BallGrabScript>().beingCarried, _ball.GetComponent<BallGrabScript>().ActualPlayerWhoGrabId);
                PhotonNetwork.Destroy(_ball.GetComponent<PhotonView>());
            }
        } else {
            _ball.GetComponent<GenericBall>().ReleaseBallBlackHole(_ball.GetComponent<BallGrabScript>().beingCarried, _ball.GetComponent<BallGrabScript>().ActualPlayerWhoGrabId);
            Destroy(_ball);
        }
    }


}
