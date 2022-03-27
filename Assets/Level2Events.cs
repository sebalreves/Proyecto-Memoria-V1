using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Events : PlaygroundEvents {
    private bool rotated = false;
    private bool rotated2 = false;
    private int door1Count = 0;
    private int door2Count = 0;
    private int lastRoomCount = 0;
    public override void subscribeMethods() {
        // PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStatePressed += pressPlatformA;
        PlatformsList[CONST.A].GetComponent<GenericPlatform>().setStateReleased += releasePlatformA;
        //door 1
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStatePressed += pressDoor1Activator;
        PlatformsList[CONST.B].GetComponent<GenericPlatform>().setStateReleased += releaseDoor1Activator;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStatePressed += pressDoor1Activator;
        PlatformsList[CONST.C].GetComponent<GenericPlatform>().setStateReleased += releaseDoor1Activator;

        //door 2
        PlatformsList[CONST.D].GetComponent<GenericPlatform>().setStatePressed += pressDoor2Activator;
        PlatformsList[CONST.D].GetComponent<GenericPlatform>().setStateReleased += releaseDoor2Activator;

        //door3
        PlatformsList[CONST.E].GetComponent<GenericPlatform>().setStatePressed += pressDoor3Activator;
        PlatformsList[CONST.E].GetComponent<GenericPlatform>().setStateReleased += releaseDoor3Activator;
        ButtonsList[CONST.A].GetComponent<GenericButton>().onPressEvent += pressDoor3Button;

        //last room
        PlatformsList[CONST.F].GetComponent<GenericPlatform>().setStatePressed += pressLastPlatform;
        PlatformsList[CONST.G].GetComponent<GenericPlatform>().setStatePressed += pressLastPlatform;
        PlatformsList[CONST.H].GetComponent<GenericPlatform>().setStatePressed += pressLastPlatform;
        PlatformsList[CONST.I].GetComponent<GenericPlatform>().setStatePressed += pressLastPlatform;
        PlatformsList[CONST.F].GetComponent<GenericPlatform>().setStateReleased += releaseLastPlatform;
        PlatformsList[CONST.G].GetComponent<GenericPlatform>().setStateReleased += releaseLastPlatform;
        PlatformsList[CONST.H].GetComponent<GenericPlatform>().setStateReleased += releaseLastPlatform;
        PlatformsList[CONST.I].GetComponent<GenericPlatform>().setStateReleased += releaseLastPlatform;

        ButtonsList[CONST.B].GetComponent<GenericButton>().onPressEvent += pressLastButton;


    }

    IEnumerator pressDoor1Activator(GameObject platformObject) {
        door1Count++;
        if (door1Count == 2) {
            DoorsList[CONST.A].GetComponent<GenericDoor>().open();
        }
        yield break;
    }
    IEnumerator releaseDoor1Activator(GameObject platformObject) {
        door1Count--;
        if (door1Count < 2) {
            DoorsList[CONST.A].GetComponent<GenericDoor>().close();
        }
        yield break;
    }

    IEnumerator pressDoor2Activator(GameObject platformObject) {
        if (platformObject.GetComponent<GenericPlatform>().activado)
            DoorsList[CONST.B].GetComponent<GenericDoor>().open();
        yield break;
    }
    IEnumerator releaseDoor2Activator(GameObject platformObject) {
        DoorsList[CONST.B].GetComponent<GenericDoor>().close();
        if (!rotated2) {
            RotatePivot._instance.rotateCamera(-1f);
        }
        rotated2 = true;
        yield break;
    }

    #region DOOR3
    IEnumerator pressDoor3Activator(GameObject platformObject) {
        door2Count++;
        yield break;
    }
    IEnumerator releaseDoor3Activator(GameObject platformObject) {
        door2Count--;
        yield break;
    }

    IEnumerator pressDoor3Button(GameObject platformObject) {
        if (door2Count == 2) {
            DoorsList[CONST.C].GetComponent<GenericDoor>().open();
        }
        yield break;
    }
    #endregion

    #region Door4
    IEnumerator pressLastPlatform(GameObject platformObject) {
        lastRoomCount++;
        if (lastRoomCount == 4) {
            DoorsList[CONST.D].GetComponent<GenericDoor>().open();
        }
        yield break;
    }

    IEnumerator releaseLastPlatform(GameObject platformObject) {
        DoorsList[CONST.D].GetComponent<GenericDoor>().close();
        lastRoomCount--;
        yield break;
    }

    IEnumerator pressLastButton(GameObject buttonObject) {
        if (lastRoomCount == 4 || BallFactory._instance.ballCount == 0) {
            DoorsList[CONST.D].GetComponent<GenericDoor>().open();
        }
        yield break;
    }
    #endregion

    IEnumerator releasePlatformA(GameObject platformObject) {
        if (!rotated)
            RotatePivot._instance.rotateCamera(1f);
        rotated = true;
        yield break;
    }
}

