using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad_1_3Events : PlaygroundEvents {
    public override void subscribeMethods() {
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformA;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStatePressed += pressPlatformB;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStateReleased += releasePlatformB;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStatePressed += pressPlatformC;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStateReleased += releasePlatformC;
        PlatformsList[CONST.D].GetComponent<GenericPlatform>().setStatePressed += pressPlatformD;
        PlatformsList[CONST.D].GetComponent<GenericPlatform>().setStateReleased += releasePlatformD;

        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButton;
    }




    IEnumerator pressPlatformA(GameObject platformObject) {
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
    }
    IEnumerator releasePlatformA(GameObject platformObject) {
        if (!PlatformsList[CONST.B].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.A].GetComponent<GenericDoor>().close();
        }

        if (!PlatformsList[CONST.C].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        }
        yield break;
    }
    IEnumerator pressPlatformB(GameObject platformObject) {
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        DoorsList[CONST.C].GetComponent<GenericDoor>().open();
        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
    }
    IEnumerator releasePlatformB(GameObject platformObject) {
        if (!PlatformsList[CONST.A].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.A].GetComponent<GenericDoor>().close();
        }

        if (!PlatformsList[CONST.D].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.C].GetComponent<GenericDoor>().close();
        }
        yield break;

    }
    IEnumerator pressPlatformC(GameObject platformObject) {
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        DoorsList[CONST.D].GetComponent<GenericDoor>().open();
        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
    }
    IEnumerator releasePlatformC(GameObject platformObject) {
        if (!PlatformsList[CONST.A].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        }

        if (!PlatformsList[CONST.D].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.D].GetComponent<GenericDoor>().close();
        }
        yield break;
    }
    IEnumerator pressPlatformD(GameObject platformObject) {
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.C].GetComponent<GenericDoor>().open();
        DoorsList[CONST.D].GetComponent<GenericDoor>().open();
        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
    }
    IEnumerator releasePlatformD(GameObject platformObject) {
        if (!PlatformsList[CONST.B].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.C].GetComponent<GenericDoor>().close();
        }

        if (!PlatformsList[CONST.C].GetComponent<GenericPlatform>().presionado) {
            DoorsList[CONST.D].GetComponent<GenericDoor>().close();
        }
        yield break;
    }

    IEnumerator pressButton(GameObject buttonObject) {
        if (BallFactory._instance.cubeCount == 1) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 1f);
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green, _time: 3f);
            DoorsList[CONST.E].GetComponent<GenericDoor>().open();
        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red, _time: 2f);
            BallFactory._instance.shineVariables();
        }
        yield break;
    }
}
