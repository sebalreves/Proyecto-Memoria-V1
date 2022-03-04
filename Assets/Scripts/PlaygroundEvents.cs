using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonA;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonB;
        ButtonsList[CONST.C].GetComponent<GenericButton>().onPressEvent += pressButtonC;
        ButtonsList[CONST.D].GetComponent<GenericButton>().onPressEvent += pressButtonD;

        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformA;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;
    }
    #endregion

    #region BUTTON C
    void pressButtonC() {
        CodeLineManager._instance.resetCodeColor();
        StartCoroutine(pressButtonCRoutine());
    }

    IEnumerator pressButtonCRoutine() {
        bool ejecutando = true;
        int _lineIndex = 0;
        int prevLine = _lineIndex;
        GameObject buttonObject = ButtonsList[CONST.C];
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = false;
        while (ejecutando) {
            // Debug.Log("Ejecutando" + prevLine + "   " + _lineIndex);
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
                        // CodeLineManager._instance.trySetColorLine(0, 1, grey, time);
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
                    // CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, time);
                    ejecutando = false;
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
            // if (prevLine == _lineIndex) ejecutando = false;
            // else prevLine = _lineIndex;
        }
        ButtonsList[CONST.C].GetComponent<GenericButton>().activateButton(false);
        terminarEjecucion();
    }

    #endregion

    #region BUTTON D
    public void pressButtonD() {
        //asi, aunque se este mirando otra animacion, no se solapan
        CodeLineManager._instance.resetCodeColor();
        StartCoroutine(pressButtonDRoutine());
    }

    public IEnumerator pressButtonDRoutine(int _lineIndex = 0) {
        bool ejecutando = true;
        int prevLine = _lineIndex;
        GameObject buttonObject = ButtonsList[CONST.D];
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = false;
        while (ejecutando) {
            Debug.Log("Ejecutando" + prevLine + "   " + _lineIndex);
            switch (_lineIndex) {
                case 0:
                    PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = true;
                    yield return WaitScript._instance.waitCodeLineAction(10f,
                    (time) => {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: time, fadeUp: false, lineal: true);
                    }
                    );
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
                    yield return WaitScript._instance.waitCodeLineAction(5f,
                   (time) => {
                       CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: time, fadeUp: false, lineal: true);
                   }
                   );
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
            if (prevLine == _lineIndex) ejecutando = false;
            else prevLine = _lineIndex;
        }
        ButtonsList[CONST.D].GetComponent<GenericButton>().activateButton(false);
        terminarEjecucion();
    }

    #endregion


    #region PLATFORM A
    public void releasePlatformA() {
        WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();
        if (PlatformsList[CONST.A].GetComponent<GenericPlatform>().loopingCodeRoutine != null) {
            StopCoroutine(PlatformsList[CONST.A].GetComponent<GenericPlatform>().loopingCodeRoutine);
        }
        // PlatformsList[CONST.A].GetComponent<GenericPlatform>().keepLoopingCode = false;
        // if (PlatformsList[CONST.A].transform.Find("TargetZone").GetComponent<CodeDescription>().targeted)
        // stopAnimations();
    }

    public void pressPlatformA() {
        // Debug.Log("pressss");
        //TODO reiniciar animacion cuando se vuelve a llamar sin dejar de rpesionar el boton

        //callback para reanudar animación sin tener que volver a colocar la bola cuando se retargetea la platform
        // if (PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation == null)
        //     PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation += pressPlatformA;
        // else {
        //     stopAnimations();
        // }
        CodeLineManager._instance.resetCodeColor();

        //para evitar que se anime la ejecucion de la plataforma si se activa cuando n ose esta viendo
        // WindAreasList[CONST.A].SetActive(PlatformsList[CONST.A].GetComponent<GenericPlatform>().activado);
        if (PlatformsList[CONST.A].GetComponent<GenericPlatform>().activado) WindAreasList[CONST.A].GetComponent<GenericWindArea>().activar();
        else WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();

        // WindAreasList[CONST.A].SetActive(true);
        // if (PlatformsList[CONST.A].transform.Find("TargetZone").GetComponent<CodeDescription>().targeted)
        if (PlatformsList[CONST.A].GetComponent<GenericPlatform>().loopingCodeRoutine != null) {
            StopCoroutine(PlatformsList[CONST.A].GetComponent<GenericPlatform>().loopingCodeRoutine);
        }
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().loopingCodeRoutine = StartCoroutine(pressPlatformARoutine());

    }
    public IEnumerator pressPlatformARoutine(int _lineIndex = 0) {
        // Debug.Log("AAAAAAAAAAA");
        bool ejecutando = true;
        GenericPlatform platformReference = PlatformsList[CONST.A].GetComponent<GenericPlatform>();
        GameObject platformObject = PlatformsList[CONST.A];

        // platformReference.keepLoopingCode = true;
        while (ejecutando) {
            // if (!platformReference.presionado) yield break;
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
                    // WindAreasList[CONST.A].SetActive(true);
                    CodeLineManager._instance.trySetColorLine(platformObject, 1, green);
                    break;

                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);

            // if (platformReference.keepLoopingCode == false) ejecutando = false;
        }
        // PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation = null;

        terminarEjecucion();
    }

    #endregion
    #region  BUTTON A
    public void pressButtonA() {
        // Debug.Log("Boton A presionado");
        // DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().playerTeleportTo(new Vector2(32, 11));
        // WindAreasList[CONST.A].GetComponent<GenericWindArea>().startLoop(4f, 2f);

        //asi, aunque se este mirando otra animacion, no se solapan
        CodeLineManager._instance.resetCodeColor();
        StartCoroutine(pressButtonARoutine());
    }

    public IEnumerator pressButtonARoutine(int _lineIndex = 0) {
        bool ejecutando = true;
        int prevLine = _lineIndex;
        GameObject buttonObject = ButtonsList[CONST.A];
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = false;
        while (ejecutando) {
            Debug.Log("Ejecutando" + prevLine + "   " + _lineIndex);
            switch (_lineIndex) {
                case 0:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);
                        _lineIndex = 1;
                    } else {
                        _lineIndex = 2;
                        // CodeLineManager._instance.trySetColorLine(0, 1, grey, time);
                    }
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 1, green,
                    _action: () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                        // ejecutando = false;
                    });
                    // CodeLineManager._instance.trySetColorLine(2, 3, grey);
                    break;
                case 2:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 2, green);
                    _lineIndex = 3;
                    break;
                case 3:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 3, green,
                    _action: () => {
                        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
                        // ejecutando = false;
                    });
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
            if (prevLine == _lineIndex) ejecutando = false;
            else prevLine = _lineIndex;
        }
        ButtonsList[CONST.A].GetComponent<GenericButton>().activateButton(false);
        terminarEjecucion();
    }
    #endregion
    public void terminarEjecucion() {
        Debug.Log("ejecucion terminada");
        PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = true;
    }

    public void pressButtonB() {
        // Debug.Log("Boton B presionado");
        // BallFactory._instance.deleteGroup(CONST.Cube, CONST.Blue);
        StartCoroutine(pressButtonBRoutine());
    }

    IEnumerator pressButtonBRoutine() {
        bool ejecutando = true;
        int _lineIndex = 0;
        // int prevLine = _lineIndex;
        GameObject buttonObject = ButtonsList[CONST.B];
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = false;
        while (ejecutando) {
            // Debug.Log("Ejecutando" + prevLine + "   " + _lineIndex);
            switch (_lineIndex) {
                case 0:
                    if (BallFactory._instance.CubeCount > 0) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);
                        _lineIndex = 1;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, red,
                        _action: () => {
                            ejecutando = false;
                        });
                        // CodeLineManager._instance.trySetColorLine(0, 1, grey, time);
                    }
                    break;
                case 1:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
                    _lineIndex = 2;
                    break;
                case 2:
                    yield return BallFactory._instance.transformGroup(CONST.Cube, null, null, CONST.Blue, onTransformAction:
                        () => {
                            CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: CONST.codeLoopVelocity);
                        });
                    // CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, time);
                    ejecutando = false;
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
            // if (prevLine == _lineIndex) ejecutando = false;
            // else prevLine = _lineIndex;
        }
        ButtonsList[CONST.B].GetComponent<GenericButton>().activateButton(false);
        terminarEjecucion();
    }
}
