using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointer : MonoBehaviour {
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RectTransform maskRect;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite squareSprite;
    private Vector3 targetPosition;
    private Transform pointerTransform;
    private SpriteRenderer pointerRenderer;


    private float borderSize = 100f;
    // Start is called before the first frame update
    private void Awake() {
        targetPosition = new Vector3(5f, 4f, 0f);
        pointerTransform = transform.Find("Pointer").GetComponent<Transform>();
        pointerRenderer = transform.Find("Pointer").GetComponent<SpriteRenderer>();
    }
    void Start() {

    }
    void OnGUI() {
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }

    Vector3 rotarSprite() {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = gameObject.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVector(dir); //flecha apuntando a la derecha
        pointerTransform.localEulerAngles = new Vector3(0, 0, angle + 90);
        return dir;
    }

    // Update is called once per frame
    void Update() {
        //rotar puntero

        //objetivo dentro de la camara
        bool offScreen = isOffGameScreen(targetPosition);

        if (offScreen) {
            Vector3 dir = rotarSprite();
            pointerRenderer.sprite = arrowSprite;
            Vector3 cappedTargetPosition = targetPosition;
            Vector3 _center = gameObject.transform.position;
            cappedTargetPosition = _center + dir * maskRect.rect.width * 0.4f;
            pointerTransform.position = Vector3.Lerp(pointerTransform.position, cappedTargetPosition, Time.deltaTime * 7f);
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);
        } else {
            pointerRenderer.sprite = squareSprite;
            pointerTransform.position = Vector3.Lerp(pointerTransform.position, targetPosition, Time.deltaTime * 7f);
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);
            pointerTransform.localEulerAngles = Vector3.zero;

        }
    }

    private bool isOffGameScreen(Vector3 target) {
        Vector3 _center = gameObject.transform.position;
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
