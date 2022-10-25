using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;


public class LevelManager : MonoBehaviourPunCallbacks {
    // Start is called before the first frame update

    static LevelManager _instance;
    public Animator transitionAnimator;
    public Animator sceneTransitionAnimator;
    public Image transitionImage;
    public Color rojo, morado, amarillo;
    public bool fadedBool = false;

    public bool changingScene = false;
    public static LevelManager instance {
        get {
            if (_instance == null) {
                // * El ShopCameraControl se crea dinámicamente. No existe en la escena previamente *

                UnityEngine.Object obj = Resources.Load("LevelManager");
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

    // public Button level_1_button, level_2_button, level_3_button, level_4_button;

    // public Button[] buttons;
    public bool returningMenu;
    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        } else {
            Destroy(this);
        }
        // buttons = new Button[] { level_1_button, level_2_button, level_3_button, level_4_button };
        niveles = new List<LevelData>();
        niveles.Add(new LevelData(2, 1, "Nivel 1", 3, false));
        niveles.Add(new LevelData(3, 1, "Nivel 2", 4));
        niveles.Add(new LevelData(4, 1, "Nivel 3", 5));
        niveles.Add(new LevelData(5, 1, "Nivel 4", -1));
        currentLevel = getcurrentLevel();

    }

    private void Update() {
        // Debug.Log();
        if (PhotonNetwork.IsConnectedAndReady) {
            if (PhotonNetwork.LevelLoadingProgress > 0f && changingScene == false) {
                changingScene = true;
                StartCoroutine(FadeAnimation(() => { }, false, true));

            }
            if (PhotonNetwork.LevelLoadingProgress == 0 && changingScene == true) {
                changingScene = false;
                StartCoroutine(FadeAnimation(() => { }, false, true));
            }
        }
    }



    public LevelData getcurrentLevel() {
        foreach (LevelData data in niveles) {
            if (data.buildIndex == SceneManager.GetActiveScene().buildIndex) {
                return data;
            }
        }
        return null;
    }

    // public void initializeButtons() {
    //     // initializeButton(level_1_button, niveles[0]);
    //     // initializeButton(level_2_button, niveles[1]);
    //     // initializeButton(level_3_button, niveles[2]);
    //     // initializeButton(level_4_button, niveles[3]);
    // }

    public void LevelButtonOnClick(string levelName) {

        Debug.Log("SELECCIONADO");
        // button.interactable = !levelData.locked;

        LevelData levelData = null;
        foreach (var nivel in niveles) {
            if (nivel.nombre == levelName) levelData = nivel;
        }
        if (levelData == null) {
            Debug.Log("Nivel no encontrado");
            return;
        }
        // button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelData.nombre.Substring(levelData.nombre.Length - 1);
        // if (button.onClick.GetPersistentEventCount() == 0)
        // button.onClick.AddListener(() => {
        Debug.Log(levelData.nombre);
        currentLevel = levelData;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        GameObject buttonContainer = GameObject.Find("Level Buttons");
        foreach (Transform button in buttonContainer.transform) {
            button.GetComponent<Button>().interactable = false;
        }
        PhotonNetwork.LoadLevel(levelData.buildIndex);
        // button.interactable = false;


        // SceneManager.LoadScene(levelData.buildIndex);
        // });

        // if (!PhotonNetwork.IsMasterClient)
        //     button.gameObject.SetActive(false);
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

            LoadSceneAnimation(nextLevel);
        }
    }

    public IEnumerator LoadSceneAnimation(int nexLevelIndex) {
        //fasein
        transitionAnimator.Play("fade_in_1");
        //wait
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(nexLevelIndex);
        //wait scene load
        while (!sceneLoading.isDone) yield return null;

        //animation fadeout
        transitionAnimator.Play("fade_out_1");

    }

    bool AnimatorIsPlaying() {
        return (transitionAnimator.GetCurrentAnimatorStateInfo(0).length >
               transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) ||
               (sceneTransitionAnimator.GetCurrentAnimatorStateInfo(0).length >
               sceneTransitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
    public IEnumerator FadeAnimation(Action _callback, bool fullLoop = false, bool secondaryTransition = false) {
        //fasein
        if (fadedBool) {
            while (AnimatorIsPlaying()) yield return null;
            if (secondaryTransition) sceneTransitionAnimator.Play("fase_in_transicion"); else transitionAnimator.Play("fade_out_1");
        } else {
            ChangeTransitionColor();
            if (secondaryTransition) sceneTransitionAnimator.Play("fade_out_transicion"); else transitionAnimator.Play("fade_in_1");

        }
        fadedBool = !fadedBool;
        //wait
        yield return new WaitForSeconds(secondaryTransition ? sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length : transitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(nexLevelIndex);
        _callback();
        //wait scene load
        // while (!sceneLoading.isDone) yield return null;


        if (fullLoop) {
            if (fadedBool) {
                if (secondaryTransition) sceneTransitionAnimator.Play("fase_in_transicion"); else transitionAnimator.Play("fade_out_1");
            } else {
                if (secondaryTransition) sceneTransitionAnimator.Play("fade_out_transicion"); else transitionAnimator.Play("fade_in_1");

            }
            fadedBool = !fadedBool;
        }

        //animation fadeout

    }

    private void ChangeTransitionColor() {
        try {


            float random = UnityEngine.Random.Range(0f, 1f);
            Debug.Log(random);
            if (random < 0.3f) {
                transitionImage.color = rojo;

            } else if (random < 0.7f) {
                transitionImage.color = morado;

            } else {
                transitionImage.color = amarillo;

            }
        } catch (System.Exception e) {

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
