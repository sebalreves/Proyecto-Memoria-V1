using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.InputSystem;

public class NetworkManager : MonoBehaviourPunCallbacks {

    Keyboard kb;
    Mouse mouse;

    [Header("Login UI")]
    public TextMeshProUGUI loginConnectingText;
    public GameObject LoginUIPanel;
    // public InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("GameOptions  Panel")]
    public GameObject GameOptionsUIPanel;


    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    // public InputField RoomNameInputField;
    public string GameMode = CONST.level_1;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;
    public TextMeshProUGUI RoomInfoText;
    public GameObject PlayerListPrefab;
    public GameObject PlayerListContent;
    public GameObject StartGameButton;
    public GameObject LevelSelectPanel;
    public TextMeshProUGUI LevelSelectionText;

    // public Text GameModeText;
    // public Image PanelBackground;
    // public Sprite RacingBackground;
    // public Sprite DeathRaceBackground;
    // public GameObject[] PlayerSelectionUIGameObjects;
    // public Player[] DeathRacePlayers;
    // public RacingPlayer[] RacingPlayers;


    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;

    private List<string> levels;
    private int actualLevel = 0;


    private Dictionary<int, GameObject> playerListGameObjects;

    public Dictionary<string, RoomInfo> cachedRoomList;
    public Dictionary<string, GameObject> roomListGameobjects;
    public GameObject roomListEntryPrefab;
    public GameObject roomListEntryContainer;


    #region UNITY Methods
    private void Awake() {
        Application.targetFrameRate = 60;
        roomListEntryPrefab = Resources.Load("RoomList") as GameObject;
        kb = InputSystem.GetDevice<Keyboard>();
        mouse = InputSystem.GetDevice<Mouse>();
        ActivatePanel(LoginUIPanel.name);
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameobjects = new Dictionary<string, GameObject>();
        levels = new List<string>(){
            CONST.level_1,
            CONST.level_2,
            "Playground",
            "Level 4 Tutorial 1"
        };




        // LevelManager.instance.initializeButtons();
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(2f);
        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.LocalPlayer.NickName = "Jugador #" + Random.Range(50, 200);
            PhotonNetwork.ConnectUsingSettings();

        }
    }

    public override void OnEnable() {
        base.OnEnable();
    }
    public override void OnDisable() {
        base.OnDisable();
    }

    private void Update() {
        if (kb.anyKey.wasPressedThisFrame || mouse.leftButton.wasPressedThisFrame) {
            OnInteract_LoginButton();
        }
    }
    #endregion


    #region UI Callback Methods

    public void onClickCreateRoom() {
        ActivatePanel(CreatingRoomInfoUIPanel.name);

        string roomName = PhotonNetwork.LocalPlayer.NickName;
        // if (string.IsNullOrEmpty(roomName)) {
        //     roomName = "Room" + Random.Range(1000, 10000);
        // }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.EmptyRoomTtl = 0;

        string[] roomPropsInLobby = { CONST.levelProp }; //gm = game mode
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { CONST.levelProp, GameMode }
        };

        roomOptions.IsVisible = true;
        roomOptions.BroadcastPropsChangeToAll = true;

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    public void OnInteract_LoginButton() {
        if (PhotonNetwork.IsConnectedAndReady && LoginUIPanel.activeInHierarchy) {

            ActivatePanel(GameOptionsUIPanel.name);

        }

    }
    private void ChangeLevel(string newLevel) {
        LevelSelectionText.text = newLevel;
    }
    public void OnSelectNextLevel() {
        actualLevel = (actualLevel + 1) % levels.Count;
        string newLevel = levels[actualLevel];
        ChangeLevel(newLevel);
    }

    public void OnSelectPrevLevel() {
        actualLevel = (actualLevel - 1) % levels.Count;
        string newLevel = levels[actualLevel];
        ChangeLevel(newLevel);
    }

    public void OnCancelButtonClicked() {
        ActivatePanel(GameOptionsUIPanel.name);
    }


    // public void OnCreateRoomButtonClicked() {
    //     ActivatePanel(CreatingRoomInfoUIPanel.name);

    //     string roomName = "Sala de " + PhotonNetwork.LocalPlayer.NickName;
    //     // if (string.IsNullOrEmpty(roomName)) {
    //     //     roomName = "Room" + Random.Range(1000, 10000);
    //     // }
    //     RoomOptions roomOptions = new RoomOptions();
    //     roomOptions.MaxPlayers = 2;
    //     roomOptions.CleanupCacheOnLeave = true;
    //     roomOptions.EmptyRoomTtl = 0;

    //     string[] roomPropsInLobby = { CONST.levelProp }; //gm = game mode
    //     ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable()
    //     {
    //         { CONST.levelProp, GameMode }
    //     };

    //     roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
    //     roomOptions.CustomRoomProperties = customRoomProperties;

    //     PhotonNetwork.CreateRoom(roomName, roomOptions);
    // }


    public void OnJoinRandomRoomButtonClicked() {
        // GameMode = _gameMode;

        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnBackButtonClicked() {
        ActivatePanel(GameOptionsUIPanel.name);
    }


    public void OnLeaveGameButtonClicked() {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGameButtonClicked() {

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm")) {
            PhotonNetwork.LoadLevel("Level 4 Tutorial 1");

            // if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc")) {
            //     //Racing game mode


            // } else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr")) {
            //     //Death race mode
            //     PhotonNetwork.LoadLevel("DeathRaceScene");

            // }

        }
    }


    #endregion



    #region Photon Callbacks
    // public override void OnConnected() {
    //     Debug.Log("We connected to internet");
    // }

    public override void OnConnectedToMaster() {
        // Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon.");
        loginConnectingText.text = "Presione una tecla para continuar";
        ActivatePanel(GameOptionsUIPanel.name);
        TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
        PhotonNetwork.JoinLobby(customLobby);
    }

    public override void OnCreatedRoom() {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }


    public override void OnJoinedRoom() {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + "Player count:" + PhotonNetwork.CurrentRoom.PlayerCount);

        // LevelSelectPanel.SetActive(PhotonNetwork.IsMasterClient);

        ActivatePanel(InsideRoomUIPanel.name);
        LevelManager.instance.initializeButtons();

        // if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm")) {

        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
            " Players/Max.Players: " +
            PhotonNetwork.CurrentRoom.PlayerCount + " / " +
            PhotonNetwork.CurrentRoom.MaxPlayers;



        // if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc")) {
        // } 
        //Unique Game Mode
        // GameModeText.text = "LET'S RACE!";
        // PanelBackground.sprite = RacingBackground;


        // for (int i = 0; i < PlayerSelectionUIGameObjects.Length; i++) {
        //     PlayerSelectionUIGameObjects[i].transform.Find("PlayerName").GetComponent<Text>().text = RacingPlayers[i].playerName;
        //     PlayerSelectionUIGameObjects[i].GetComponent<Image>().sprite = RacingPlayers[i].playerSprite;
        //     PlayerSelectionUIGameObjects[i].transform.Find("PlayerProperty").GetComponent<Text>().text = "";
        // }


        // else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr")) {
        //     //Death race game mode
        //     GameModeText.text = "DEATH RACE!";
        //     PanelBackground.sprite = DeathRaceBackground;

        //     for (int i = 0; i < PlayerSelectionUIGameObjects.Length; i++) {
        //         PlayerSelectionUIGameObjects[i].transform.Find("PlayerName").GetComponent<Text>().text = DeathRacePlayers[i].playerName;
        //         PlayerSelectionUIGameObjects[i].GetComponent<Image>().sprite = DeathRacePlayers[i].playerSprite;
        //         PlayerSelectionUIGameObjects[i].transform.Find("PlayerProperty").GetComponent<Text>().text = DeathRacePlayers[i].weaponName +
        //             ": " + "Damage: " + DeathRacePlayers[i].damage + " FireRate: " + DeathRacePlayers[i].fireRate;

        //     }



        // }


        if (playerListGameObjects == null) {
            playerListGameObjects = new Dictionary<int, GameObject>();

        }


        //cargar datos de los jugadores conectados
        foreach (Player player in PhotonNetwork.PlayerList) {
            GameObject playerListGameObject = Instantiate(PlayerListPrefab);
            playerListGameObject.transform.SetParent(PlayerListContent.transform);
            playerListGameObject.transform.localScale = Vector3.one;
            playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(player.ActorNumber, player.NickName);


            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(CONST.PLAYER_READY, out isPlayerReady)) {
                playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
            playerListGameObjects.Add(player.ActorNumber, playerListGameObject);



        }



        StartGameButton.SetActive(false);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps) {
        GameObject playerListGameObject;
        if (playerListGameObjects.TryGetValue(target.ActorNumber, out playerListGameObject)) {
            object isPlayerReady;
            if (changedProps.TryGetValue(CONST.PLAYER_READY, out isPlayerReady)) {
                playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
        }
        StartGameButton.SetActive(CheckPlayersReady());
    }



    public override void OnPlayerEnteredRoom(Player newPlayer) {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                  " Players/Max.Players: " +
                  PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                  PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameObject = Instantiate(PlayerListPrefab);
        playerListGameObject.transform.SetParent(PlayerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;
        playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);

        StartGameButton.SetActive(CheckPlayersReady());

    }


    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if (otherPlayer.ActorNumber == 1) {
            PhotonNetwork.LeaveRoom();
        }
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                " Players/Max.Players: " +
                PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        StartGameButton.SetActive(CheckPlayersReady());

    }



    public override void OnLeftRoom() {
        ActivatePanel(GameOptionsUIPanel.name);

        foreach (GameObject playerListGameobject in playerListGameObjects.Values) {
            Destroy(playerListGameobject);

        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;




    }


    public override void OnMasterClientSwitched(Player newMasterClient) {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber) {
            StartGameButton.SetActive(CheckPlayersReady());
        }
    }



    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.LogWarning("Join Random");
        //if there is no room, create one
        if (GameMode != null) {
            string roomName = "Sala creada por error al encontrar";
            if (string.IsNullOrEmpty(roomName)) {
                roomName = "Sala" + Random.Range(1000, 10000);

            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            string[] roomPropsInLobby = { "lvl" }; //gm = game mode 

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.BroadcastPropsChangeToAll = true;
            roomOptions.IsVisible = true;
            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;

            PhotonNetwork.CreateRoom("Sala" + Random.Range(1000, 10000), roomOptions);
        }
    }

    // public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {

    // }




    public override void OnRoomListUpdate(List<RoomInfo> roomList) {

        foreach (GameObject oldEntry in roomListGameobjects.Values) {
            Destroy(oldEntry);
        }
        roomListGameobjects.Clear();
        foreach (RoomInfo room in roomList) {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList) {
                if (cachedRoomList.ContainsKey(room.Name)) {
                    cachedRoomList.Remove(room.Name);
                }
            } else {
                if (cachedRoomList.ContainsKey(room.Name)) {
                    cachedRoomList[room.Name] = room;
                } else {
                    cachedRoomList.Add(room.Name, room);
                }
            }
        }

        foreach (RoomInfo room in cachedRoomList.Values) {
            GameObject newEntry = Instantiate(roomListEntryPrefab, roomListEntryContainer.transform);
            // newEntry.transform.SetParent(roomListEntryContainer.transform);
            // newEntry.transform.localScale = Vector3.one;

            newEntry.transform.Find("Owner").GetComponent<TextMeshProUGUI>().text = "Sala del jugador " + room.Name;
            newEntry.transform.Find("Players").GetComponent<TextMeshProUGUI>().text = room.PlayerCount + " / " + room.MaxPlayers;
            newEntry.transform.Find("Unirse").GetComponent<Button>().onClick.AddListener(() => {
                OnJoinRoomButtonClick(room.Name);
            });

            roomListGameobjects.Add(room.Name, newEntry);
        }
    }

    public void spawnPrefab() {
        GameObject newEntry = Instantiate(roomListEntryPrefab, roomListEntryContainer.transform);
        // newEntry.transform.SetParent(roomListEntryContainer.transform);  
        // newEntry.transform.localScale = Vector3.one;

        newEntry.transform.Find("Owner").GetComponent<TextMeshProUGUI>().text = "Sala del jugador ";
        newEntry.transform.Find("Players").GetComponent<TextMeshProUGUI>().text = " / ";
        // newEntry.transform.Find("Unirse").GetComponent<Button>().onClick.AddListener(() => {
        //     OnJoinRoomButtonClick(room.Name);
        // });
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarning("Jugador desconectado");

    }


    #endregion


    #region Public Methods
    public void ActivatePanel(string panelNameToBeActivated) {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelNameToBeActivated));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
        InsideRoomUIPanel.SetActive(InsideRoomUIPanel.name.Equals(panelNameToBeActivated));

        if (GameOptionsUIPanel.name.Equals(panelNameToBeActivated)) {
            onMainMenu();
        }
    }

    public void SetGameMode(string _gameMode) {
        GameMode = _gameMode;

    }
    #endregion



    #region Private Methods



    private void onMainMenu() {
        Debug.Log("on mainmenu");
        foreach (GameObject oldEntry in roomListGameobjects.Values) {
            Destroy(oldEntry);
        }
        roomListGameobjects.Clear();
        cachedRoomList.Clear();
    }
    private void OnJoinRoomButtonClick(string _roomName) {
        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);

    }

    private bool CheckPlayersReady() {

        if (!PhotonNetwork.IsMasterClient) {
            return false;
        }
        if (PhotonNetwork.PlayerList.Length != 2) return false;
        foreach (Player player in PhotonNetwork.PlayerList) {

            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(CONST.PLAYER_READY, out isPlayerReady)) {

                if (!(bool)isPlayerReady) {
                    return false;
                }
            } else {
                return false;
            }
        }
        return true;
    }
    #endregion



}
