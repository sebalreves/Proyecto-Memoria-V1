using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using TMPro;


public class PlaygroundManager : MonoBehaviourPunCallbacks {

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

    #region UI ELEMENTS
    public GameObject ExitgamePopUp;
    public GameObject ResetgamePopUp;
    public GameObject PlayerDisconectedPopUp;
    public GameObject LevelEndPopUp;
    public GameObject LeftgamePopUp;

    public Button exitButton;
    public Button exitButtonConfirm;
    public Button exitButtonCanell;
    public Button resetButton;
    public Button resetButtonConfirm;
    public Button resetButtonCancell;
    public Button playerDisconnectedHomeButton;
    public Button endLevelNextButton;


    public Image UIMaskImage;

    public bool exitingRoom = false;

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
        PhotonNetwork.AutomaticallySyncScene = true;

        while (PlayerFactory._instance == null) yield return null;
        #region INSTANCIAR OBJETOS  
        // var editorPlayer = GameObject.Find("Player");
        // if (editorPlayer != null)
        //     Destroy(editorPlayer);
        GameObject localPlayer;
        if (PhotonNetwork.IsConnectedAndReady) {
            //TODO spawn player 1 y player 2 dependiendo quien es el owner
            // object playerSelectionNumber;
            // if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber)) {
            // }

            // Debug.Log((int)playerSelectionNumber);

            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 instantiatePosition = spawnPositions[actorNumber - 1].position;
            localPlayer = PlayerFactory._instance.instantiatePlayer(instantiatePosition);

        } else {
            localPlayer = PlayerFactory._instance.instantiatePlayer(spawnPositions[0].position);

        }
        #endregion

        #region UI BUTTON ASSIGN

        var PopUpContainer = localPlayer.transform.Find("Camera").Find("PopUpCamera").GetChild(0).gameObject;
        ExitgamePopUp = PopUpContainer.transform.Find("ExitgamePopUp").gameObject;
        ResetgamePopUp = PopUpContainer.transform.Find("ResetgamePopUp").gameObject;
        LevelEndPopUp = PopUpContainer.transform.Find("LevelEndPopUp").gameObject;
        LeftgamePopUp = PopUpContainer.transform.Find("LeftgamePopUp").gameObject;


        var buttonReferences = localPlayer.GetComponent<CanvasManager>();
        exitButton = buttonReferences.exitButton;
        resetButton = buttonReferences.resetButton;
        exitButtonCanell = buttonReferences.exitButtonCanell;
        exitButtonConfirm = buttonReferences.exitButtonConfirm;
        resetButtonCancell = buttonReferences.resetButtonCancell;
        resetButtonConfirm = buttonReferences.resetButtonConfirm;
        endLevelNextButton = buttonReferences.endLevelNextButton;
        playerDisconnectedHomeButton = buttonReferences.playerDisconnectedHomeButton;
        exitButton.onClick.AddListener(onExitLevel);
        resetButton.onClick.AddListener(onRestartLevel);
        exitButtonCanell.onClick.AddListener(onExitLevelCancell);
        exitButtonConfirm.onClick.AddListener(onExitLevelConfirm);
        resetButtonCancell.onClick.AddListener(onRestartLevelCancell);
        resetButtonConfirm.onClick.AddListener(onRestartLevelConfirm);
        endLevelNextButton.onClick.AddListener(onNextLevel);
        playerDisconnectedHomeButton.onClick.AddListener(onPlayerDisconnectedConfirm);

        endLevelNextButton.interactable = PhotonNetwork.IsConnectedAndReady ? PhotonNetwork.LocalPlayer.IsMasterClient : true;
        endLevelNextButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.IsConnectedAndReady ?
        (PhotonNetwork.LocalPlayer.IsMasterClient ? "Siguiente nivel" : "Esperando...")
         : "Siguiente nivel";


        UIMaskImage = localPlayer.GetComponent<CanvasManager>().UIMaskImage;
        if (PhotonNetwork.IsConnectedAndReady) {
            resetButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }
        #endregion

        Invoke("ActivateMask", 0.15f);
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

    private void ActivateMask() {
        if (UIMaskImage) {
            UIMaskImage.enabled = false;
            UIMaskImage.enabled = true;
        }
    }



    [PunRPC]
    public void RPC_onResetLevel(string sceneName) {
        if (sceneName == SceneManager.GetActiveScene().name) {
            SceneManager.LoadScene(sceneName);
            return;
        }

        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    [PunRPC]
    void onEndLevelRPC() {
        //desabilitar controles de ambos jugadores


        //Cuando ambos jugadores llegan a la meta

        //if ultimo nivel de la unidad
        // // Sig unidad o volver a la sala
        exitButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        LevelEndPopUp.GetComponent<Animator>().Play("open_pop_up");

        PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = false;


    }



    public void onEndLevel() {
        //llamado desde el script Endlevel.cs
        if (PhotonNetwork.IsConnectedAndReady) {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
                photonView.RPC("onEndLevelRPC", RpcTarget.AllViaServer);
        } else {
            onEndLevelRPC();
        }

    }

    public void onNextLevel() {

        //solo accesible para el host
        //esperando al host para continuar
        // onRestartLevelConfirm();
        LevelManager.instance.nextLevel();
        endLevelNextButton.interactable = false;

    }

    public void onRestartLevel() {
        exitButton.interactable = false;
        resetButton.interactable = false;
        ResetgamePopUp.GetComponent<Animator>().Play("open_pop_up");
        //solo accesible para el host
        //desabilitar controles del host

    }

    public void onRestartLevelConfirm() {
        Scene scene = SceneManager.GetActiveScene();
        if (PhotonNetwork.IsConnectedAndReady) {
            // if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("RPC_onResetLevel", RpcTarget.AllViaServer, scene.name);

        } else {
            SceneManager.LoadScene(scene.buildIndex);
        }
    }

    public void onRestartLevelCancell() {
        exitButton.interactable = true;
        resetButton.interactable = true;
        ResetgamePopUp.GetComponent<Animator>().Play("close_pop_up");
        //habilitar controles
    }

    #region EXIT 
    public void onExitLevel() {
        //desabilitar controles de quien se quiere ir
        exitButton.interactable = false;
        resetButton.interactable = false;
        ExitgamePopUp.GetComponent<Animator>().Play("open_pop_up");
    }

    public void onExitLevelConfirm() {
        if (PhotonNetwork.IsConnectedAndReady) {
            // if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("RPC_onExitLevel", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.ActorNumber);

        } else {
            SceneManager.LoadScene("LobbyScene");
        }
    }

    public void onExitLevelCancell() {
        exitButton.interactable = true;
        resetButton.interactable = true;
        ExitgamePopUp.GetComponent<Animator>().Play("close_pop_up");

        //habilitar controles

    }
    [PunRPC]
    public void RPC_onExitLevel(int actorNumber) {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber) {
            exitingRoom = true;
            PhotonNetwork.LeaveRoom();

        } else
            //tu compañero se ha desconectado, volveras al menu
            LeftgamePopUp.GetComponent<Animator>().Play("open_pop_up");
    }
    #endregion

    #region DISCONNECT
    public void onPlayerDisconnected() {
        //visible para el jugador que se quedo solo en la room, popup notificando con boton para volver a la sala/inicio
        //si el jugador local se desconecta, popupconexion perdida
        LeftgamePopUp.GetComponent<Animator>().Play("open_pop_up");

        exitButton.interactable = false;
        resetButton.interactable = false;

    }



    public override void OnLeftRoom() {
        if (exitingRoom || LevelManager.instance.returningMenu)
            SceneManager.LoadScene("LobbyScene");
        else {
            //desconexion accidental
            onPlayerDisconnected();
        }

        // onPlayerDisconnected();
        // SceneManager.LoadScene("LobbyScene");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if (exitingRoom) return;
        onPlayerDisconnected();
    }

    public void onPlayerDisconnectedConfirm() {
        exitingRoom = true;
        PhotonNetwork.LeaveRoom();

    }


    #endregion
}
