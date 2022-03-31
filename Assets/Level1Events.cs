using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Events : PlaygroundEvents {


    public override void subscribeMethods() {
        ButtonGroupList[CONST.A].GetComponent<ButtonWrapper>().setFunc(pressButtonSwitchDoors);
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonD;
    }

    #region  SWITCH PUERTAS
    /*
    0 Si las puertas estan cerradas
    1     Abrir puertas
    2 Esperar 1s
    3 Si las puertas estan abiertas
    4     Cerrar puertas
    5 En caso contrario 
    6    Abrir puertas
    */

    private void openDoors() {
        DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        DoorsList[CONST.C].GetComponent<GenericDoor>().open();
        DoorsList[CONST.D].GetComponent<GenericDoor>().open();
    }

    private void closeDoors() {
        DoorsList[CONST.A].GetComponent<GenericDoor>().close();
        DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        DoorsList[CONST.C].GetComponent<GenericDoor>().close();
        DoorsList[CONST.D].GetComponent<GenericDoor>().close();
    }

    public IEnumerator pressButtonSwitchDoors(GameObject buttonObject) {

        if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, grey, fadeUp: false);

        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, green, fadeUp: false);
            openDoors();
        }
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: 4f, fadeUp: false, lineal: true);

        if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 3, 4, green, fadeUp: false);
            closeDoors();
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 5, 6, grey, fadeUp: false);
        } else {
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 3, 4, grey, fadeUp: false);
            yield return CodeLineManager._instance.trySetColorLine(buttonObject, 5, 6, green, fadeUp: false);
            openDoors();
        }
    }
    #endregion

    #region CERRAR PUERTAS
    IEnumerator pressButtonD(GameObject buttonObject) {
        //cerrar puertas
        yield return CodeLineManager._instance.trySetColorLine(buttonObject, 0, green, fadeUp: false);
        closeDoors();
    }
    #endregion

}
