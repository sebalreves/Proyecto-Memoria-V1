using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Events : PlaygroundEvents {
    public override void subscribeMethods() {
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonARoutine;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonBRoutine;
        ButtonsList[CONST.C].GetComponent<GenericButton>().onPressEvent += pressButtonCRoutine;
        ButtonsList[CONST.D].GetComponent<GenericButton>().onPressEvent += pressButtonDRoutine;

        ObjectAreasList[CONST.A].GetComponent<ObjecAreaScript>().detectCubes = true;
        ObjectAreasList[CONST.B].GetComponent<ObjecAreaScript>().detectBalls = true;

    }

    #region  BUTTON A
    //Area naranja transformar a caja
    IEnumerator pressButtonARoutine(GameObject buttonObject) {
        if (BallFactory._instance.ballCount == 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
            yield break;
        }
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, green);
        yield return BallFactory._instance.transformGroup(_toShape: CONST.Cube, customGroup: ObjectAreasList[CONST.B].GetComponent<ObjecAreaScript>().detectedObjects,
        onTransformAction: () => {
            StartCoroutine(CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: CONST.codeLoopVelocity));
        });
    }
    #endregion

    #region  BUTTON B
    //area azul eliminar cajas
    IEnumerator pressButtonBRoutine(GameObject buttonObject) {
        if (BallFactory._instance.cubeCount == 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
            yield break;
        }
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, green);
        yield return BallFactory._instance.deleteGroup(customGroup: ObjectAreasList[CONST.A].GetComponent<ObjecAreaScript>().detectedObjects,
        onDeleted: () => {
            StartCoroutine(CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: CONST.codeLoopVelocity));
        });
    }
    #endregion

    #region  BUTTON C
    //puerta 1 cajas = globos
    IEnumerator pressButtonCRoutine(GameObject buttonObject) {
        if (BallFactory._instance.ballCount == BallFactory._instance.cubeCount) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
            DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
        }
    }
    #endregion

    #region  BUTTON D
    //puerta 2  globos = 0
    IEnumerator pressButtonDRoutine(GameObject buttonObject) {
        if (BallFactory._instance.ballCount == 0) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green);
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 1, green);
            DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, red);
        }
    }
    #endregion
}
