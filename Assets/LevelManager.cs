using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class LevelManager : MonoBehaviourPunCallbacks {
    // Start is called before the first frame update

    static LevelManager _instance;
    public static LevelManager instance {
        get {
            if (_instance == null) {
                // * El ShopCameraControl se crea dinámicamente. No existe en la escena previamente *

                Object obj = Resources.Load("LevelManager");
                GameObject go = Instantiate(obj) as GameObject;
                _instance = go.GetComponent<LevelManager>();

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public List<LevelData> niveles;
    public LevelData currentLevel;
    // public LevelData level_1 = new LevelData(2, 1, "Nivel 1", 3, false);
    // public LevelData level_2 = new LevelData(3, 1, "Nivel 2", 4);
    // public LevelData level_3 = new LevelData(4, 1, "Nivel 3", 5);
    // public LevelData level_4 = new LevelData(5, 1, "Nivel 4", -1);

    public Button level_1_button, level_2_button, level_3_button, level_4_button;
    public bool returningMenu;
    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        niveles = new List<LevelData>();
        niveles.Add(new LevelData(2, 1, "Nivel 1", 3, false));
        niveles.Add(new LevelData(3, 1, "Nivel 2", 4));
        niveles.Add(new LevelData(4, 1, "Nivel 3", 5));
        niveles.Add(new LevelData(5, 1, "Nivel 4", -1));
        currentLevel = getcurrentLevel();

    }

    public LevelData getcurrentLevel() {
        foreach (LevelData data in niveles) {
            if (data.buildIndex == SceneManager.GetActiveScene().buildIndex) {
                return data;
            }
        }
        return null;
    }

    public void initializeButtons() {
        initializeButton(level_1_button, niveles[0]);
        initializeButton(level_2_button, niveles[1]);
        initializeButton(level_3_button, niveles[2]);
        initializeButton(level_4_button, niveles[3]);
    }

    void initializeButton(Button button, LevelData levelData) {

        // button.interactable = !levelData.locked;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelData.nombre.Substring(levelData.nombre.Length - 1);
        button.onClick.AddListener(() => {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                currentLevel = levelData;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(levelData.buildIndex);
                button.interactable = false;
            }

            // SceneManager.LoadScene(levelData.buildIndex);
        });

        if (!PhotonNetwork.IsMasterClient)
            button.gameObject.SetActive(false);
    }

    public void LevelCompleted() {

    }

    public void nextLevel() {
        //lobby al llegar al ultimo de la unidad
        int nextLevel = 0;

        //desbloquear el nivel al completarlo
        niveles[niveles.IndexOf(currentLevel)].locked = false;

        //si no es el ultimo de la unidad
        if (currentLevel.nextLevel != -1) {
            currentLevel = niveles[niveles.IndexOf(currentLevel) + 1];
            nextLevel = currentLevel.buildIndex;
        }
        if (PhotonNetwork.IsConnectedAndReady) {
            if (nextLevel == 0) {
                returningMenu = true;
                PhotonNetwork.LeaveRoom();
                return;
            }
            PhotonNetwork.LoadLevel(nextLevel);

        } else {

            SceneManager.LoadScene(nextLevel);
        }
    }

    public void selectLevel() {

    }

    public void activar_botones_seleccion_nivel() {

    }
}

// [SerializeField]
// public class LevelList(){

//     LevelList(){

//     }
// }

public class LevelData {
    public int buildIndex;
    public int unidad;
    public string nombre;
    public int nextLevel;
    public bool locked;
    public LevelData(int _buildIndex, int _unidad, string _nombre, int _nextLevel, bool _locked = true) {
        buildIndex = _buildIndex;
        unidad = _unidad;
        nombre = _nombre;
        nextLevel = _nextLevel;
        locked = _locked;
    }
}
