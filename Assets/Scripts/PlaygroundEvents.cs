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
        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
    }
    public IEnumerator pressButtonARoutine(int _lineIndex, float time = 0.5f) {
        bool ejecutando = true;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(0, green);
                        _lineIndex = 1;
                    } else {
                        _lineIndex = 2;
                        CodeLineManager._instance.trySetColorLine(0, 1, grey);
                    }
                    break;
                case 1:
                    DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                    CodeLineManager._instance.trySetColorLine(1, green);
                    ejecutando = false;
                    break;
                case 2:
                    CodeLineManager._instance.trySetColorLine(2, green);
                    _lineIndex = 3;
                    break;
                case 3:
                    CodeLineManager._instance.trySetColorLine(3, green);
                    DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                    ejecutando = false;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(time);
        }

    }

    public void pressButtonB() {
        Debug.Log("Boton B presionado");
        // BallFactory._instance.deleteGroup(CONST.Cube, CONST.Blue);
        BallFactory._instance.transformGroup(CONST.Cube, null, null, CONST.Blue);
    }
    #endregion
}
