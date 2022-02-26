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
    public void stopAnimations() {
        //detiene animaciones localmente
        StopAllCoroutines();
    }

    IEnumerator Start() {
        yield return new WaitUntil(() => PlaygroundManager.instance.isReady);
        WindAreasList = PlaygroundManager.instance.WindAreasList;
        DoorsList = PlaygroundManager.instance.DoorsList;
        PlatformsList = PlaygroundManager.instance.PlatformsList;
        ButtonsList = PlaygroundManager.instance.ButtonsList;
        subscribeMethods();
    }

    public void subscribeMethods() {
        #region SUSCRIBIR METODOS
        // Debug.Log(ButtonsList.Count);
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonA;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonB;

        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformA;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;
        #endregion
    }

    #region BUTTON CALLBACKS
    public void releasePlatformA() {
        WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation = null;
        if (PlatformsList[CONST.A].transform.Find("TargetZone").GetComponent<CodeDescription>().targeted)
            stopAnimations();
    }

    public void pressPlatformA() {
        // Debug.Log("pressss");
        //TODO reiniciar animacion cuando se vuelve a llamar sin dejar de rpesionar el boton

        //callback para reanudar animación sin tener que volver a colocar la bola cuando se retargetea la platform
        if (PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation == null)
            PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation += pressPlatformA;
        else {
            stopAnimations();
        }
        CodeLineManager._instance.resetCodeColor();

        //para evitar que se anime la ejecucion de la plataforma si se activa cuando n ose esta viendo
        // WindAreasList[CONST.A].SetActive(PlatformsList[CONST.A].GetComponent<GenericPlatform>().activado);
        if (PlatformsList[CONST.A].GetComponent<GenericPlatform>().activado) WindAreasList[CONST.A].GetComponent<GenericWindArea>().activar();
        else WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();

        // WindAreasList[CONST.A].SetActive(true);
        if (PlatformsList[CONST.A].transform.Find("TargetZone").GetComponent<CodeDescription>().targeted)
            StartCoroutine(pressPlatformARoutine(0));

    }
    public IEnumerator pressPlatformARoutine(int _lineIndex, float time = 1f) {
        // Debug.Log("AAAAAAAAAAA");
        bool ejecutando = true;
        GenericPlatform platformReference = PlatformsList[CONST.A].GetComponent<GenericPlatform>();
        while (ejecutando) {
            if (!platformReference.presionado) yield break;
            switch (_lineIndex) {
                case 0:
                    if (platformReference.activado) {
                        CodeLineManager._instance.trySetColorLine(0, green, time);
                        _lineIndex = 1;
                    } else {
                        CodeLineManager._instance.trySetColorLine(0, red, time);
                        ejecutando = false;
                    }
                    break;
                case 1:
                    // WindAreasList[CONST.A].SetActive(true);
                    while (true) {
                        CodeLineManager._instance.trySetColorLine(1, green, time);
                        yield return new WaitForSeconds(time + 0.1f);
                    }
                default:
                    break;
            }
            yield return new WaitForSeconds(time + 0.1f);
        }
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().currentAnimation = null;

        terminarEjecucion();
    }

    public void pressButtonA() {
        // Debug.Log("Boton A presionado");
        // DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().playerTeleportTo(new Vector2(32, 11));
        // WindAreasList[CONST.A].GetComponent<GenericWindArea>().startLoop(4f, 2f);
        CodeLineManager._instance.resetCodeColor();
        StartCoroutine(pressButtonARoutine(0));
    }

    public IEnumerator pressButtonARoutine(int _lineIndex, float time = 1f) {
        bool ejecutando = true;
        int prevLine = _lineIndex;
        // PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = false;
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
        ButtonsList[CONST.A].GetComponent<GenericButton>().activateButton(false);
        terminarEjecucion();
    }

    public void terminarEjecucion() {
        Debug.Log("ejecucion terminada");
        PlayerFactory._instance.localPlayer.GetComponent<PlayerMovement>().controllEnabled = true;
    }

    public void pressButtonB() {
        Debug.Log("Boton B presionado");
        // BallFactory._instance.deleteGroup(CONST.Cube, CONST.Blue);
        BallFactory._instance.transformGroup(CONST.Cube, null, null, CONST.Blue);
    }
    #endregion
}
