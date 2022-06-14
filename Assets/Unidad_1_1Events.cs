using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad_1_1Events : PlaygroundEvents {

    public override void subscribeMethods() {
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressDoor1Activator;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releaseDoor1Activator;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStatePressed += platform2Press;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStateReleased += platform1Press;
    }

    IEnumerator pressDoor1Activator(GameObject platformObject) {
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        while (true)
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);

    }
    IEnumerator releaseDoor1Activator(GameObject platformObject) {
        DoorsList[CONST.A].GetComponent<GenericDoor>().close();
        yield break;
    }

    IEnumerator platform2Press(GameObject platformObject) {
        yield return CodeLineManager._instance.trySetColorLine(platformObject, 0, green);
        DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        DoorsList[CONST.C].GetComponent<GenericDoor>().open();
        while (true) {
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 1, green, _time: CONST.codeLoopVelocity);
            yield return CodeLineManager._instance.trySetColorLine(platformObject, 2, green, _time: CONST.codeLoopVelocity);
        }

        yield break;
    }

    IEnumerator platform1Press(GameObject platformObject) {
        DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        DoorsList[CONST.C].GetComponent<GenericDoor>().close();
        yield break;
    }
}
