using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointer : MonoBehaviour {
    // [SerializeField] private Camera uiCamera;
    private RectTransform maskRect;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite squareSprite;
    public Vector3 targetPosition;
    private Transform pointerTransform;
    public SpriteRenderer pointerRenderer;
    public Action onReachEvent;
    public float objectiveRadius = 10f;
    private float borderSize = 100f;
    private GameObject playerGameObjectReference = null;
    Vector3 playerPosition;
    public float pointerVelocity = 0f;

    private float startTime;
    // public string type;

    // Start is called before the first frame update
    private void Awake() {
        // targetPosition = new Vector3(5f, 4f, 0f);
        pointerTransform = transform.Find("Pointer").GetComponent<Transform>();
        pointerRenderer = transform.Find("Pointer").GetComponent<SpriteRenderer>();
    }
    IEnumerator Start() {
        while (PlayerFactory._instance.localPlayer == null) {
            yield return null;
        }
        playerGameObjectReference = PlayerFactory._instance.localPlayer;
        maskRect = playerGameObjectReference.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).transform.Find("#Mask").GetComponent<RectTransform>();

        pointerTransform.position = playerGameObjectReference.transform.position;
    }

    private void OnEnable() {
        startTime = Time.time;
    }


    Vector3 rotarSprite() {
        Vector3 dir = (targetPosition - playerPosition).normalized;
        float angle = GetAngleFromVector(dir); //flecha apuntando a la derecha
        pointerTransform.localEulerAngles = Vector3.Lerp(pointerTransform.localEulerAngles, new Vector3(0, 0, angle + 90), 07f);
        return dir;
    }

    // Update is called once per frame
    private void OnDisable() {
        if (onReachEvent != null)
            foreach (var d in onReachEvent.GetInvocationList())
                onReachEvent -= (d as Action);
    }

    private void checkReach() {
        if (Vector2.SqrMagnitude(playerPosition - targetPosition) < objectiveRadius * objectiveRadius) {
            onReachEvent();
        }
    }

    void Update() {
        if (!playerGameObjectReference) return;
        playerPosition = playerGameObjectReference.transform.position;
        playerPosition.z = 0f;
        //Checkear si llegó el 
        // checkReach();
        //rotar puntero

        //objetivo dentro de la camara
        bool offScreen = isOffGameScreen(targetPosition);

        if (offScreen) {
            pointerRenderer.sprite = arrowSprite;
            Vector3 dir = rotarSprite();
            Vector3 cappedTargetPosition = targetPosition;
            Vector3 _center = maskRect.position;
            cappedTargetPosition = _center + dir * maskRect.rect.width * 0.4f;
            pointerTransform.position = Vector3.Lerp(pointerTransform.position, cappedTargetPosition, Mathf.Max(Time.deltaTime * 6f, pointerVelocity));
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);
        } else {
            pointerRenderer.sprite = squareSprite;
            pointerTransform.localEulerAngles = Vector3.zero;
            pointerTransform.position = Vector3.Lerp(pointerTransform.position, targetPosition, Mathf.Max(Time.deltaTime * 6f, pointerVelocity));
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);
        }
    }

    private bool isOffGameScreen(Vector3 target) {
        Vector3 _center = playerPosition;
        if (_center.x - maskRect.rect.width / 2 > target.x) {
            return true;
        }
        if (_center.x + maskRect.rect.width / 2 < target.x) {
            return true;
        }
        if (_center.y - maskRect.rect.height / 2 > target.y) {
            return true;
        }
        if (_center.y + maskRect.rect.height / 2 < target.y) {
            return true;
        }
        return false;
        // return target.x <= borderSize || target.x >= Screen.width - borderSize || target.y <= borderSize || target.y >= Screen.height - borderSize;

    }

    private float GetAngleFromVector(Vector2 dir) {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle % 360;

    }
}
