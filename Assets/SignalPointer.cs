using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalPointer : MonoBehaviour {
    public static int signalCount = 0;
    public QuestPointer questPointerReference;
    Color transparent = new Color(1f, 1f, 1f, 0f);
    private void Start() {
        // transparent = new Color
        signalCount++;
        questPointerReference = transform.GetChild(0).GetComponent<QuestPointer>();
        questPointerReference.gameObject.transform.position = gameObject.transform.position;
        questPointerReference.targetPosition = gameObject.transform.position;
        Destroy(gameObject, 1f);
    }

    private void OnDestroy() {
        signalCount--;
    }

    private void Update() {
        questPointerReference.pointerRenderer.color = Color.Lerp(questPointerReference.pointerRenderer.color, transparent, 3f * Time.deltaTime);
    }
}
