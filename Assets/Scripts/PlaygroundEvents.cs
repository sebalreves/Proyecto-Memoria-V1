using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


// PlayerFactory._instance.localPlayer.transform.Find("Camera").GetComponent<CameraManager>().lookObject(DoorsList[CONST.A].transform, 2f);

public class PlaygroundEvents : MonoBehaviourPun {
    protected List<GameObject> ButtonsList;
    protected List<GameObject> WindAreasList;
    protected List<GameObject> DoorsList;
    protected List<GameObject> PlatformsList;
    protected List<GameObject> ButtonGroupList;
    protected List<GameObject> ObjectAreasList;

    protected Color grey, green, red;

    // public static PlaygroundEvents instance;
    // // public Coroutine currentRoutine = null;

    // protected virtual void Awake() {
    //     if (instance == null) {
    //         instance = this;
    //     }

    //     //If intance already exists and it is not !this!
    //     else if (instance != this) {
    //         //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
    //         Destroy(gameObject);
    //     }
    // }

    protected virtual IEnumerator Start() {
        yield return new WaitUntil(() => PlaygroundManager.instance.isReady);
        WindAreasList = PlaygroundManager.instance.WindAreasList;
        DoorsList = PlaygroundManager.instance.DoorsList;
        PlatformsList = PlaygroundManager.instance.PlatformsList;
        ButtonsList = PlaygroundManager.instance.ButtonsList;
        ButtonGroupList = PlaygroundManager.instance.ButtonGroupList;
        ObjectAreasList = PlaygroundManager.instance.ObjectAreasList;
        grey = PlaygroundManager.instance.grey1;
        green = PlaygroundManager.instance.green;
        red = PlaygroundManager.instance.red;
        subscribeMethods();
    }

    #region SUSCRIBIR METODOS
    public virtual void subscribeMethods() { }
    // Debug.Log(ButtonsList.Count);

    #endregion






}
