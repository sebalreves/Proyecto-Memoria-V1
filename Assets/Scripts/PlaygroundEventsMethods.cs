using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundEventsMethods : PlaygroundEvents {
    public override void subscribeMethods() {
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonARoutine;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonBRoutine;
        ButtonsList[CONST.C].GetComponent<GenericButton>().onPressEvent += pressButtonCRoutine;
        ButtonsList[CONST.D].GetComponent<GenericButton>().onPressEvent += pressButtonDRoutine;

        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformARoutine;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;

        //area circulos roja
        ObjectAreasList[CONST.A].GetComponent<ObjecAreaScript>().detectBalls = true;
        ButtonsList[CONST.E].GetComponent<GenericButton>().onPressEvent += pressButtonERoutine;
        // ButtonsList[CONST.F].GetComponent<GenericButton>().onPressEvent += pressButtonFRoutine;


    }

    #region BUTTON C
    IEnumerator pressButtonCRoutine(GameObject buttonObject) {

        if (BallFactory._instance.BallCount > 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);

        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
            yield break;
        }


        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
        yield return BallFactory._instance.deleteGroup(_shape: CONST.Ball, onDeleted:
            () => {
                CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: CONST.codeLoopVelocity);
            });
    }

    #endregion

    #region BUTTON D
    public IEnumerator pressButtonDRoutine(GameObject buttonObject) {
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 10f, fadeUp: false, lineal: true);


        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
        DoorsList[CONST.A].GetComponent<GenericDoor>().open();

        //esperar 5 s
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 5f, fadeUp: false, lineal: true);

        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 3, green);
        DoorsList[CONST.A].GetComponent<GenericDoor>().close();
    }

    #endregion

    #region BUTTON E
    public IEnumerator pressButtonERoutine(GameObject buttonObject) {

        if (ObjectAreasList[CONST.A].GetComponent<ObjecAreaScript>().detectedObjects.Count == 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
        }
        yield return BallFactory._instance.deleteGroup(customGroup: ObjectAreasList[CONST.A].GetComponent<ObjecAreaScript>().detectedObjects,
        onDeleted: () => {
            Debug.Log("AAAAAAAA");
            StartCoroutine(CodeLineManager._instance.trySetColorLine(buttonObject, 0, 2, green, _time: CONST.codeLoopVelocity));
        });
        // yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 10f, fadeUp: false, lineal: true);


        // yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
        // DoorsList[CONST.A].GetComponent<GenericDoor>().open();

        // //esperar 5 s
        // yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 5f, fadeUp: false, lineal: true);

        // yield return CodeLineManager._instance.trySetColorLine(buttonObject, 3, green);
        // DoorsList[CONST.A].GetComponent<GenericDoor>().close();
    }

    #endregion




    #region PLATFORM A
    public IEnumerator releasePlatformA(GameObject platformObject) {
        WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();
        yield return null;
    }

    public IEnumerator pressPlatformARoutine(GameObject platformObject) {

        GenericPlatform platformReference = platformObject.GetComponent<GenericPlatform>();

        if (platformReference.activado) WindAreasList[CONST.A].GetComponent<GenericWindArea>().activar();
        else WindAreasList[CONST.A].GetComponent<GenericWindArea>().desactivar();


        if (platformReference.activado) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);

        } else {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, red);
            yield break;
        }

        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green);

    }

    #endregion
    #region  BUTTON A
    public IEnumerator pressButtonARoutine(GameObject buttonObject) {

        if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, fadeUp: false);
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
            DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();
            yield break;
        }

        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, grey, fadeUp: false);

        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 2, 3, grey, fadeUp: false);

        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, fadeUp: false);

        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 3, green, fadeUp: false);
        DoorsList[CONST.A].GetComponent<GenericDoor>().openOrClose();


    }
    #endregion
    #region BUTTON B
    IEnumerator pressButtonBRoutine(GameObject buttonObject) {

        if (BallFactory._instance.CubeCount > 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, fadeUp: false);
            yield return BallFactory._instance.transformGroup(CONST.Cube, null, null, CONST.Blue, onTransformAction:
        () => {
            CodeLineManager._instance.trySetColorLine(buttonObject, 1, 2, green, _time: CONST.codeLoopVelocity);
        });
        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
            yield break;
        }
        #endregion
    }
}
