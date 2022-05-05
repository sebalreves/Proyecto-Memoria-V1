using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlaygroundManager : MonoBehaviour {

    //instancia los objetos de la escena (jugadores y objetos)
    //Asigna funcionalidades a cada boton e interruptor
    [HideInInspector]
    public bool isReady = false;
    public GameObject playerPrefab;
    public static PlaygroundManager instance;
    public Transform[] spawnPositions;
    public GameObject LevelPointers;

    public Color grey1, grey2, yellow1, yellow2, green, red;

    #region STAGE OBJECTS
    public GameObject Buttons;
    public GameObject ButtonGroups;
    public GameObject WindAreas;
    public GameObject Doors;
    public GameObject Platforms;
    public GameObject ObjectAreas;

    // [HideInInspector]
    public List<GameObject> ButtonsList;
    public List<GameObject> ButtonGroupList;
    // [HideInInspector]
    public List<GameObject> WindAreasList;
    // [HideInInspector]
    public List<GameObject> DoorsList;
    // [HideInInspector]
    public List<GameObject> PlatformsList;
    public List<GameObject> ObjectAreasList;
    #endregion

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
        #region LISTAR BUTTONS, DOORS, ETC
        ButtonsList = getChildren(Buttons, "buttons");
        DoorsList = getChildren(Doors);
        WindAreasList = getChildren(WindAreas);
        PlatformsList = getChildren(Platforms);
        ButtonGroupList = getChildren(ButtonGroups);
        ObjectAreasList = getChildren(ObjectAreas);

        #endregion
        // DontDestroyOnLoad(gameObject);

    }

    IEnumerator Start() {
        while (PlayerFactory._instance == null) yield return null;
        #region INSTANCIAR OBJETOS  
        // var editorPlayer = GameObject.Find("Player");
        // if (editorPlayer != null)
        //     Destroy(editorPlayer);
        if (PhotonNetwork.IsConnectedAndReady) {
            //TODO spawn player 1 y player 2 dependiendo quien es el owner
            // object playerSelectionNumber;
            // if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber)) {
            // }

            // Debug.Log((int)playerSelectionNumber);

            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 instantiatePosition = spawnPositions[actorNumber - 1].position;
            PlayerFactory._instance.instantiatePlayer(instantiatePosition);

        } else {
            PlayerFactory._instance.instantiatePlayer(spawnPositions[0].position);
            // BallFactory._instance.instantiateBall(new Vector2(-4, -1));
        }
        #endregion
        isReady = true;
    }

    private List<GameObject> getChildren(GameObject go, string types = "") {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < go.transform.childCount; i++) {
            if (types == "buttons") {
                children.Add(go.transform.GetChild(i).transform.Find("Button").gameObject);
            } else
                children.Add(go.transform.GetChild(i).gameObject);
        }
        // Debug.Log(children.Count);
        return children;
    }
}
