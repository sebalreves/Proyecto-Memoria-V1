using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//same code
//unified execution
public class ButtonWrapper : MonoBehaviour {
    public static int buttonGroupId = 0;
    public int my_groupId;
    // public Func<GameObject, IEnumerator> onPressEvent;
    private void Start() {
        buttonGroupId++;
        my_groupId = buttonGroupId;
        // onPressEvent += pressAllButtons;
        // foreach (Transform button in gameObject.transform) {<
        // }
        foreach (Transform button in gameObject.transform) {
            button.GetChild(0).GetComponent<GenericButton>().wrappGroup = buttonGroupId;
            button.GetChild(0).GetComponent<GenericButton>().onPressParameter = gameObject;
            // button.GetChild(0).GetComponent<GenericButton>().onPressEvent += onPressEvent;

        }
    }

    public void setFunc(Func<GameObject, IEnumerator> _func) {
        foreach (Transform button in gameObject.transform) {
            button.GetChild(0).GetComponent<GenericButton>().onPressEvent += _func;

        }
    }
    // private IEnumerator pressAllButtons(GameObject wrapperGameObject) {
    //     // foreach (Transform button in gameObject.transform) {

    //     // }
    //     yield break;
    // }
}
