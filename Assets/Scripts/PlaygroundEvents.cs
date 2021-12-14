using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundEvents : MonoBehaviour {
    private List<GameObject> ButtonsList;
    private List<GameObject> WindAreasList;
    private List<GameObject> DoorsList;
    private List<GameObject> PlatformsList;

    private void Awake() {
    }

    IEnumerator Start() {
        yield return new WaitUntil(() => PlaygroundManager.instance.isReady);
        WindAreasList = PlaygroundManager.instance.WindAreasList;
        DoorsList = PlaygroundManager.instance.DoorsList;
        PlatformsList = PlaygroundManager.instance.PlatformsList;
        ButtonsList = PlaygroundManager.instance.ButtonsList;
        #region SUSCRIBIR METODOS
        // Debug.Log(ButtonsList.Count);
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonA;
        #endregion
    }

    private void OnEnable() {
    }



    public void pressButtonA() {
        Debug.Log("Boton A presionado");
    }
}
