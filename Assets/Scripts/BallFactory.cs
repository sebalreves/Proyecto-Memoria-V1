using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;
using TMPro;


public class BallFactory : MonoBehaviour {
    public static BallFactory _instance;
    public int ballCount, cubeCount;
    public int BallCount {
        get {
            return ballCount;
        }
        set {
            if (ballCount == value) return;
            ballCount = value;
            if (onCountChange != null)
                onCountChange(CONST.Ball, ballCount);
        }
    }
    public int CubeCount {
        get {
            return cubeCount;
        }
        set {
            if (cubeCount == value) return;
            cubeCount = value;
            if (onCountChange != null)
                onCountChange(CONST.Cube, cubeCount);
        }
    }
    public Dictionary<int, GameObject> instancedBalls = null;
    // public Dictionary<int, GameObject> instancedCubes;
    public GameObject ballPrefab, cubePrefab;

    private Action<string, int> onCountChange;
    public bool ready = false;
    private TextMeshProUGUI ballCountTMP, cubeCountTMP;


    //TODO check network sync of dictionary
    void Awake() {
        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);

        } else if (_instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);
        }

        onCountChange += onCountChangeCallback;

    }

    void onCountChangeCallback(string _type, int _newCount) {
        if (_type == CONST.Ball)
            ballCountTMP.text = _newCount.ToString();

        else if (_type == CONST.Cube)
            cubeCountTMP.text = _newCount.ToString();

    }

    IEnumerator Start() {
        while (PlayerFactory._instance.localPlayer == null) yield return null;
        var canvas = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.GetChild(4);
        ballCountTMP = canvas.transform.Find("#Variables").transform.Find("LayerGroup").transform.Find("Balls").transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        cubeCountTMP = canvas.transform.Find("#Variables").transform.Find("LayerGroup").transform.Find("Cubes").transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        instancedBalls = new Dictionary<int, GameObject>();
        BallCount = 0;
        CubeCount = 0;
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
            BallCount++;
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
            CubeCount++;
        }
        ready = true;
    }


    public GameObject instantiateBall(Vector2 _instantiatePosition, string _color = null) {
        BallCount++;
        return instantiateObject(_instantiatePosition, ballPrefab, _color);

    }

    public GameObject instantiateCube(Vector2 _instantiatePosition, string _color = null) {
        CubeCount++;
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
            instancedBalls.Add(spawnedObject.transform.GetChild(0).GetComponent<PhotonView>().ViewID, spawnedObject.transform.GetChild(0).gameObject);
        } else {
            spawnedObject = Instantiate(_prefab, _instantiatePosition, Quaternion.identity);
            instancedBalls.Add(spawnedObject.transform.GetChild(0).gameObject.GetInstanceID(), spawnedObject.transform.GetChild(0).gameObject);
        }
        return spawnedObject;
    }

    public IEnumerator deleteGroup(string _shape = null, string _color = null, Action onDeleted = null) {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.LocalPlayer.IsMasterClient) yield return null;
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

        yield return StartCoroutine(iterativeDeleteRoutine(toDeleteList, onDeleted));
    }

    private IEnumerator iterativeDeleteRoutine(List<GameObject> toDeleteList, Action onDeleted) {
        while (toDeleteList.Count > 0) {
            var toDeleteItem = toDeleteList[0];
            toDeleteList.RemoveAt(0);
            DestroyBall(toDeleteItem);
            if (onDeleted != null) onDeleted();
            yield return new WaitForSeconds(CONST.codeLoopVelocity + 0.1f);
        }
    }

    public IEnumerator transformGroup(string _fromShape = null, string _fromColor = null, string _toShape = null, string _toColor = null, Action onTransformAction = null) {
        // if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.LocalPlayer.IsMasterClient) return;
        List<GameObject> toTransformList = new List<GameObject>();
        //BALLS
        if (_fromShape == CONST.Ball) {
            toTransformList = toTransformList.Concat(GameObject.FindGameObjectsWithTag(CONST.Ball).ToList()).ToList();
        }
        //CUBES
        else if (_fromShape == CONST.Cube) {
            toTransformList = toTransformList.Concat(GameObject.FindGameObjectsWithTag(CONST.Cube).ToList()).ToList();
        } else {
            toTransformList = toTransformList.Concat(GameObject.FindGameObjectsWithTag(CONST.Cube).ToList()).ToList();
            toTransformList = toTransformList.Concat(GameObject.FindGameObjectsWithTag(CONST.Ball).ToList()).ToList();
        }

        //FILTER BY COLOR
        if (_fromColor != null)
            for (int i = toTransformList.Count - 1; i >= 0; i--) {
                if (toTransformList[i].GetComponent<GenericBall>().color != _fromColor) {
                    toTransformList.RemoveAt(i);
                }
            }

        yield return StartCoroutine(iterativeTransformRoutine(toTransformList, _toShape, _toColor, onTransformAction));
    }

    private IEnumerator iterativeTransformRoutine(List<GameObject> toTransformList, string _toShape, string _toColor, Action onTransformAction) {
        while (toTransformList.Count > 0) {
            var toTransformItem = toTransformList[0];
            toTransformList.RemoveAt(0);
            GenericBall ballScripReference = toTransformItem.GetComponent<GenericBall>();
            if (ballScripReference != null) {
                if (_toShape == null) _toShape = ballScripReference.shape;
                if (_toColor == null) _toColor = ballScripReference.color;

                ballScripReference.onPortalTransform(_toColor, _toShape);
                if (onTransformAction != null) onTransformAction();
                yield return new WaitForSeconds(CONST.codeLoopVelocity + 0.1f);
            }
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


    public void DestroyBall(GameObject _ball) {
        if (_ball == null) return;
        if (_ball.GetComponent<GenericBall>().shape == CONST.Cube) CubeCount--;
        else BallCount--;
        GameObject ballParent = _ball.transform.parent.gameObject;
        if (PhotonNetwork.IsConnectedAndReady) {
            if (PhotonNetwork.IsMasterClient) {
                _ball.GetComponent<PhotonView>().RPC("ReleaseBallBlackHole", RpcTarget.AllBuffered, _ball.GetComponent<BallGrabScript>().beingCarried, _ball.GetComponent<BallGrabScript>().ActualPlayerWhoGrabId);
                PhotonNetwork.Destroy(_ball.GetComponent<PhotonView>());
            }
        } else {
            _ball.GetComponent<GenericBall>().ReleaseBallBlackHole(_ball.GetComponent<BallGrabScript>().beingCarried, _ball.GetComponent<BallGrabScript>().ActualPlayerWhoGrabId);
            Destroy(_ball);
        }
        Destroy(ballParent);
    }


}
