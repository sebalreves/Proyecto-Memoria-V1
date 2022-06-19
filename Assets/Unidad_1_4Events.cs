using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unidad_1_4Events : PlaygroundEvents {
    public override void subscribeMethods() {
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButton;
    }

    IEnumerator pressButton(GameObject buttonObject) {
        if (BallFactory._instance.ballCount == 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, _time: 1f);
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green, _time: 3f);
            DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red, _time: 2f);
            BallFactory._instance.shineVariables();
        }
        yield break;
    }
}
