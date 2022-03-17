using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Events : PlaygroundEvents {
    // protected override void Awake() {
    //     base.Awake();
    // }

    // protected override IEnumerator Start() {
    //     return base.Start();
    // }

    // protected override void Awake() {
    //     base.Awake();
    // }

    public override void subscribeMethods() {
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressButtonSwitchDoors;
        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressButtonSwitchDoors;
        ButtonsList[CONST.C].GetComponent<GenericButton>().onPressEvent += pressButtonSwitchDoors;
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

    public IEnumerator pressButtonSwitchDoorsV2(GameObject buttonObject) {
        bool ejecutando = true;
        int _lineIndex = 0;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, grey, fadeUp: false);
                        _lineIndex = 2;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, green, fadeUp: false,
                        _action: () => { openDoors(); }
                        );
                        _lineIndex = 2;
                    }
                    break;
                case 2:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: 4f, fadeUp: false, lineal: true);
                    yield return new WaitForSeconds(4f);
                    _lineIndex = 3;
                    break;
                case 3:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 3, 4, green, fadeUp: false,
                        _action: () => {
                            closeDoors();
                        });
                        yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
                        CodeLineManager._instance.trySetColorLine(buttonObject, 5, 6, grey, fadeUp: false);
                        ejecutando = false;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 3, 4, grey, fadeUp: false);
                        _lineIndex = 5;
                    }
                    break;

                case 5:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 5, 6, green, fadeUp: false,
                    _action: () => { openDoors(); });

                    ejecutando = false;
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }

    public IEnumerator pressButtonSwitchDoors(GameObject buttonObject) {
        bool ejecutando = true;
        int _lineIndex = 0;
        while (ejecutando) {
            switch (_lineIndex) {
                case 0:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, grey, fadeUp: false);
                        _lineIndex = 2;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 0, 1, green, fadeUp: false,
                        _action: () => { openDoors(); }
                        );
                        _lineIndex = 2;
                    }
                    break;
                case 2:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 2, green, _time: 4f, fadeUp: false, lineal: true);
                    yield return new WaitForSeconds(4f);
                    _lineIndex = 3;
                    break;
                case 3:
                    if (DoorsList[CONST.A].GetComponent<GenericDoor>().opened) {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 3, 4, green, fadeUp: false,
                        _action: () => {
                            closeDoors();
                        });
                        yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
                        CodeLineManager._instance.trySetColorLine(buttonObject, 5, 6, grey, fadeUp: false);
                        ejecutando = false;
                    } else {
                        CodeLineManager._instance.trySetColorLine(buttonObject, 3, 4, grey, fadeUp: false);
                        _lineIndex = 5;
                    }
                    break;

                case 5:
                    CodeLineManager._instance.trySetColorLine(buttonObject, 5, 6, green, fadeUp: false,
                    _action: () => { openDoors(); });

                    ejecutando = false;
                    break;
                default:
                    break;
            }
            if (ejecutando)
                yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);
        }
    }
    #endregion

}
