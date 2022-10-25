using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad_1_2Events : PlaygroundEvents {
    public override void subscribeMethods() {
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformA;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStatePressed += pressPlatformB;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStateReleased += releasePlatformB;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStatePressed += pressPlatformC;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStateReleased += releasePlatformC;

        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButton1;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButton2;

    }

    #region PRIMERA HABITACION
    IEnumerator pressPlatformA(GameObject platformObject) {
        if (platformObject.GetComponent<GenericPlatform>().activado) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
            DoorsList[CONST.B].GetComponent<GenericDoor>().open();
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
        } else {
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, red, _time: CONST.codeLoopVelocity);
        }

        // yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        // DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        // while (true)
        //     yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);

    }
    IEnumerator releasePlatformA(GameObject platformObject) {
        DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        yield break;
    }
    IEnumerator pressPlatformB(GameObject platformObject) {
        if (platformObject.GetComponent<GenericPlatform>().activado) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
            DoorsList[CONST.A].GetComponent<GenericDoor>().open();
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
        } else {
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, red, _time: CONST.codeLoopVelocity);
        }
    }
    IEnumerator releasePlatformB(GameObject platformObject) {
        DoorsList[CONST.A].GetComponent<GenericDoor>().close();
        yield break;
    }


    #endregion

    #region BOTONES
    IEnumerator pressButton1(GameObject buttonObject) {
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 3f);

        DoorsList[CONST.D].GetComponent<GenericDoor>().open();

        yield break;
    }

    IEnumerator pressButton2(GameObject buttonObject) {
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 3f);

        DoorsList[CONST.C].GetComponent<GenericDoor>().open();

        yield break;
    }
    #endregion

    #region LAST PLATFORM
    IEnumerator pressPlatformC(GameObject platformObject) {
        //SI PRESIONA CON EL CUBO
        if (platformObject.GetComponent<GenericPlatform>().activado) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 3, green);
            DoorsList[CONST.E].GetComponent<GenericDoor>().open();
            DoorsList[CONST.F].GetComponent<GenericDoor>().open();
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 4, green, _time: CONST.codeLoopVelocity);

        } else {
            //SI PRESIONA SOLO CON PERSONAJE

            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
            DoorsList[CONST.E].GetComponent<GenericDoor>().open();
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
        }
    }
    IEnumerator releasePlatformC(GameObject platformObject) {
        DoorsList[CONST.E].GetComponent<GenericDoor>().close();
        DoorsList[CONST.F].GetComponent<GenericDoor>().close();
        yield break;
    }
    #endregion

}
