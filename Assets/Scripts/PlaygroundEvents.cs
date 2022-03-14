using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// PlayerFactory._instance.localPlayer.transform.Find("Camera").GetComponent<CameraManager>().lookObject(DoorsList[CONST.A].transform, 2f);

public class PlaygroundEvents : MonoBehaviour {
    private List<GameObject> ButtonsList;
    private List<GameObject> WindAreasList;
    private List<GameObject> DoorsList;
    private List<GameObject> PlatformsList;

    public Color grey, green, red;

    public static PlaygroundEvents instance;
    // public Coroutine currentRoutine = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        //If intance already exists and it is not !this!
        else if (instance != this) {
            //Then, destroy this. This enforces our singletton pattern, meaning rhat there can only ever be one instance of a GameManager
            Destroy(gameObject);
        }
    }
    // public void stopAnimations() {
    //     //detiene animaciones localmente
    //     StopAllCoroutines();
    // }

    IEnumerator Start() {
        yield return new WaitUntil(() => PlaygroundManager.instance.isReady);
        WindAreasList = PlaygroundManager.instance.WindAreasList;
        DoorsList = PlaygroundManager.instance.DoorsList;
        PlatformsList = PlaygroundManager.instance.PlatformsList;
        ButtonsList = PlaygroundManager.instance.ButtonsList;
        subscribeMethods();
    }

    #region SUSCRIBIR METODOS
    public void subscribeMethods() {
        // Debug.Log(ButtonsList.Count);
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonARoutine;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonBRoutine;
        ButtonsList[CONST.C].GetComponent<GenericButton>().onPressEvent += pressButtonCRoutine;
        ButtonsList[CONST.D].GetComponent<GenericButton>().onPressEvent += pressButtonDRoutine;

        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformARoutine;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;
    }
    #endregion

    #region BUTTON C
    IEnumerator pressButtonCRoutine(GameObject buttonObject) {
        bool ejecutando = true;
        int _lineIndex = 0;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (BallFactory._instance.BallCount > 0) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);
                        _lineIndex = 1;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, red,
                        _action: () => {
                            ejecutando = false;
                        });
                    }
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
                    _lineIndex = 2;
                    break;
                case 2:
                    yield return BallFactory._instance.deleteGroup(_shape: CONST.Ball, onDeleted:
                        () => {
                            CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: CONST.codeLoopVelocity);
                        });
                    ejecutando = false;
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }

    #endregion

    #region BUTTON D
    public IEnumerator pressButtonDRoutine(GameObject buttonObject) {
        int _lineIndex = 0;
        bool ejecutando = true;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 10f, fadeUp: false, lineal: true);
                    yield return new WaitForSeconds(10f);
                    _lineIndex = 1;
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 1, green,
                    _action: () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().open();
                        // ejecutando = false;
                    });
                    _lineIndex = 2;


                    // CodeLineManager._instance.trySetColorLine(2, 3, grey);
                    break;
                case 2:
                    //esperar 5 s
                    CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 5f, fadeUp: false, lineal: true);
                    yield return new WaitForSeconds(5f);
                    _lineIndex = 3;
                    break;
                case 3:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 3, green,
                    _action: () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().close();
                        ejecutando = false;
                    });
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }

    #endregion


    #region PLATFORM A
    public IEnumerator releasePlatformA(GameObject platformObject) {
        WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();
        yield return null;
    }

    public IEnumerator pressPlatformARoutine(GameObject platformObject) {
        bool ejecutando = true;
        int _lineIndex = 0;
        GenericPlatform platformReference = platformObject.GetComponent<GenericPlatform>();

        if (platformReference.activado) WindAreasList[CONST.A].GetComponent<GenericWindArea>().activar();
        else WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (platformReference.activado) {
                        CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
                        _lineIndex = 1;
                    } else {
                        CodeLineManager._instance.trySetColorLine(platformObject, 0, red);
                        ejecutando = false;
                    }
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(platformObject, 1, green);
                    break;

                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }

    #endregion
    #region  BUTTON A
    public IEnumerator pressButtonARoutine(GameObject buttonObject) {
        bool ejecutando = true;
        int _lineIndex = 0;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, fadeUp: false);
                        _lineIndex = 1;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, grey, fadeUp: false);
                        _lineIndex = 2;
                    }
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 1, green,
                    _action: () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                        ejecutando = false;
                    });
                    yield return new WaitForSeconds(1.2f);
                    CodeLineManager._instance.trySetColorLine(buttonObject, 2, 3, grey, fadeUp: false);
                    break;
                case 2:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, fadeUp: false);
                    _lineIndex = 3;
                    break;
                case 3:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 3, green,
                    _action: () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                        ejecutando = false;
                    }, fadeUp: false);
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }
    #endregion
    #region BUTTON B
    IEnumerator pressButtonBRoutine(GameObject buttonObject) {
        bool ejecutando = true;
        int _lineIndex = 0;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (BallFactory._instance.CubeCount > 0) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, fadeUp: false);
                        _lineIndex = 1;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, red,
                        _action: () => {
                            ejecutando = false;
                        });
                        // CodeLineManager._instance.trySetColorLine(0, 1, grey, time);
                    }
                    break;
                // case 1:
                //     CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
                //     _lineIndex = 2;
                //     break;
                case 1:
                    yield return BallFactory._instance.transformGroup(CONST.Cube, null, null, CONST.Blue, onTransformAction:
                        () => {
                            CodeLineManager._instance.trySetColorLine(buttonObject, 1, 2, green, _time: CONST.codeLoopVelocity);
                        });
                    ejecutando = false;
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }
    #endregion
}
