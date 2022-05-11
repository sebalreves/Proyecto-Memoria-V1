using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Events : PlaygroundEvents {
    private int AorBPressed = 0;
    public override void subscribeMethods() {
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonARoutine;

        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformAB;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformAB;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStatePressed += pressPlatformAB;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStateReleased += releasePlatformAB;

        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStatePressed += pressPlatformC;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStateReleased += releasePlatformC;

        PlatformsList[CONST.D].GetComponent<GenericPlatform>().setStatePressed += pressPlatformD;
        PlatformsList[CONST.D].GetComponent<GenericPlatform>().setStateReleased += releasePlatformD;

        WindAreasList[CONST.A].GetComponent<GenericWindArea>().activar();
    }

    #region BUTTON A
    IEnumerator pressButtonARoutine(GameObject buttonObject) {
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);
        DoorsList[CONST.A].GetComponent<GenericDoor>().open();
    }
    #endregion

    #region PLATFORM A AND B
    IEnumerator pressPlatformAB(GameObject platformObject) {
        AorBPressed++;
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
    }

    IEnumerator releasePlatformAB(GameObject platformObject) {
        AorBPressed--;
        if (AorBPressed == 0)
            DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        yield return null;
    }
    #endregion

    #region PLATFORM C
    IEnumerator pressPlatformC(GameObject platformObject) {
        if (platformObject.GetComponent<GenericPlatform>().activado) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
            DoorsList[CONST.C].GetComponent<GenericDoor>().open();
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
        } else {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, red);
        }
    }

    IEnumerator releasePlatformC(GameObject platformObject) {
        DoorsList[CONST.C].GetComponent<GenericDoor>().close();
        yield break;
    }
    #endregion

    #region PLATFORM D
    IEnumerator pressPlatformD(GameObject platformObject) {
        if (platformObject.GetComponent<GenericPlatform>().activado) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
            DoorsList[CONST.D].GetComponent<GenericDoor>().open();
            while (true)
                yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
        } else {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, red);
        }
    }

    IEnumerator releasePlatformD(GameObject platformObject) {
        DoorsList[CONST.D].GetComponent<GenericDoor>().close();
        yield return null;
    }
    #endregion
}
