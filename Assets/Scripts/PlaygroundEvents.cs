using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundEvents : MonoBehaviour {
    private List<GameObject> ButtonsList;
    private List<GameObject> WindAreasList;
    private List<GameObject> DoorsList;
    private List<GameObject> PlatformsList;

    public Color grey, green;

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
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonB;

        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressButtonA;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += pressButtonA;
        #endregion
    }

    #region BUTTON CALLBACKS
    public void pressButtonA() {
        Debug.Log("Boton A presionado");
        // DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
        CodeLineManager._instance.resetCodeColor();
        StartCoroutine(pressButtonARoutine(0));
    }
    public IEnumerator pressButtonARoutine(int _lineIndex, float time = 1f) {
        bool ejecutando = true;
        int prevLine = _lineIndex;
        while (ejecutando) {
            Debug.Log("Ejecutando" + prevLine + "   " + _lineIndex);
            switch (_lineIndex) {
                case 0:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(0, green, time);
                        _lineIndex = 1;
                    } else {
                        _lineIndex = 2;
                        // CodeLineManager._instance.trySetColorLine(0, 1, grey, time);
                    }
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(1, green, time,
                    () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                        // ejecutando = false;
                    });
                    // CodeLineManager._instance.trySetColorLine(2, 3, grey);
                    break;
                case 2:
                    CodeLineManager._instance.trySetColorLine(2, green, time);
                    _lineIndex = 3;
                    break;
                case 3:
                    CodeLineManager._instance.trySetColorLine(3, green, time,
                    () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                        // ejecutando = false;
                    });
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(time + 0.1f);
            if (prevLine == _lineIndex) ejecutando = false;
            else prevLine = _lineIndex;
        }
        terminarEjecucion();

    }

    void terminarEjecucion() {
        Debug.Log("ejecucion terminada");
    }

    public void pressButtonB() {
        Debug.Log("Boton B presionado");
        // BallFactory._instance.deleteGroup(CONST.Cube, CONST.Blue);
        BallFactory._instance.transformGroup(CONST.Cube, null, null, CONST.Blue);
    }
    #endregion
}
