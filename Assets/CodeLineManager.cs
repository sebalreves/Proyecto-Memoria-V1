using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class CodeLineManager : MonoBehaviour {
    public GameObject codeContainer, aux;
    public TextMeshProUGUI titulo;
    public static CodeLineManager _instance;
    GameObject linePrefab;
    public Color amarillo_1, amarillo_2, verde, gris;
    public AnimationCurve fillAnimationCurve, fillAnimationLine;
    public AnimationCurve fillAlphaAnimationCurve;
    // public string currentTitle = null;

    public GameObject currentTargetCodeDisplayed = null;


    private IEnumerator Start() {
        linePrefab = Resources.Load("CodeLine") as GameObject;
        while (!PlayerFactory._instance.localPlayer) {
            yield return null;
        }

        aux = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).gameObject;
        codeContainer = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).transform.Find("#Codigo").transform.Find("Lines").gameObject;
        titulo = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).transform.Find("#Codigo").transform.Find("Header").transform.Find("Titulo").GetComponent<TextMeshProUGUI>();


        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }

    }




    public void trySetColorLine(GameObject animationTarget, int _fromLineIndex, int _toLineIndex, Color _color, float _time = 1f, Action _action = null, bool fadeUp = true, bool lineal = false) {
        if (_fromLineIndex > _toLineIndex) {
            Debug.LogWarning("_fromline parameter > toLine");
        }
        //Ejecutar accion independiente si se anima la linea
        StartCoroutine(executeLine(_time, _action));

        //Para que solo se anime si la linea de codigo en cuestien esta targeteada
        if (animationTarget != currentTargetCodeDisplayed) return;
        GameObject codeLine;
        float offsetTime = 0f;
        for (int i = _fromLineIndex; i <= _toLineIndex; i++) {
            try {
                codeLine = codeContainer.transform.GetChild(i).gameObject;
            } catch (System.Exception e) {
                Debug.LogWarning("Line not found" + e);
                return;
            }
            // codeLine.GetComponent<Image>().color = _color;
            try {
                StartCoroutine(fillCodeLine(codeLine, _color, _time, _action, fadeUp, lineal, offsetTime));
            } catch (System.Exception) {

                Debug.LogWarning("Error al animar linea");
            }
            offsetTime += CONST.fillLoopOffsetTime;
        }
    }


    public void trySetColorLine(GameObject animationTarget, int _lineIndex, Color _color, float _time = CONST.codeVelocity, Action _action = null, bool fadeUp = true, bool lineal = false) {
        StartCoroutine(executeLine(_time, _action));

        //Para que solo se anime si la linea de codigo en cuestien esta targeteada
        if (animationTarget != currentTargetCodeDisplayed) return;
        GameObject codeLine;
        try {
            codeLine = codeContainer.transform.GetChild(_lineIndex).gameObject;
        } catch (System.Exception e) {
            // Debug.LogWarning("Line not found" + e);
            return;
        }
        // codeLine.GetComponent<Image>().color = _color;
        try {
            StartCoroutine(fillCodeLine(codeLine, _color, _time, _action, fadeUp, lineal));
        } catch (System.Exception) {

            Debug.LogWarning("Error al animar linea");
        }
    }

    public IEnumerator executeLine(float _time, Action _action) {
        yield return new WaitForSeconds(_time);
        if (_action != null)
            _action();
    }

    public IEnumerator fillCodeLine(GameObject codeLine, Color _color, float _time, Action _action, bool fadeUp, bool lineal, float offsetTime = 0f) {
        yield return new WaitForSeconds(offsetTime);

        Slider _slider = codeLine.transform.Find("Slider").GetComponent<Slider>();
        Image _fillImage = codeLine.transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
        _fillImage.color = _color;

        float fillTime = fadeUp ? _time * 0.7f : _time;
        float alphaTime = _time * 0.3f;
        float auxTime = 0f;
        while (auxTime < fillTime) {
            if (lineal) _slider.value = fillAnimationLine.Evaluate(auxTime / fillTime);
            else _slider.value = fillAnimationCurve.Evaluate(auxTime / fillTime);
            auxTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // yield return new WaitForSeconds(_time);
        // codeLine.GetComponent<Image>().color = _color;
        if (fadeUp)
            try {
                StartCoroutine(codeLineAlphaRoutine(_fillImage, alphaTime));
            } catch (System.Exception) {
                Debug.LogWarning("Error al animar alpha");
            }

    }

    public void fadeOutCodeColor() {
        foreach (Transform child in codeContainer.transform) {
            Image _fillImage = child.transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
            _fillImage.color = gris;
            StartCoroutine(codeLineAlphaRoutine(_fillImage, 0.5f));
        }
    }

    public IEnumerator codeLineAlphaRoutine(Image _fillImage, float _alphaTime) {
        // _fillImage
        float auxTime = 0f;
        while (_fillImage && auxTime < _alphaTime) {
            _fillImage.color = new Color(_fillImage.color.r, _fillImage.color.g, _fillImage.color.b, 1 - fillAnimationCurve.Evaluate(auxTime / _alphaTime));
            auxTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

    public void resetCodeColor() {
        List<GameObject> Children = new List<GameObject>();
        int i = 0;
        foreach (Transform child in codeContainer.transform) {
            child.transform.Find("Slider").GetComponent<Slider>().value = 0;
            Image image = child.transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
            var temp = image.color;
            temp.a = 1f;
            image.color = temp;
            child.transform.Find("Slider").transform.Find("Background").GetComponent<Image>().color = i % 2 == 0 ? amarillo_1 : amarillo_2;
            i++;
        }
    }

    public void newTargetResetCodeLines(string _titulo, List<string> codeLines, GameObject newTargetGameObject) {
        //Para evitar que se interrumpa la visualizacion de codigo por targetear pelotas
        if (GameObject.ReferenceEquals(newTargetGameObject, currentTargetCodeDisplayed)) return;
        currentTargetCodeDisplayed = newTargetGameObject;

        titulo.text = _titulo;
        //Borrar lineas anteriores
        clearCodeLines();
        for (int i = 0; i < codeLines.Count; i++) {
            var temp = codeLines[i].Replace("#", "    ");
            GameObject newLine = Instantiate(linePrefab, codeContainer.transform);
            newLine.transform.Find("Slider").transform.Find("Background").GetComponent<Image>().color = i % 2 == 0 ? amarillo_1 : amarillo_2;
            newLine.transform.Find("numero").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            newLine.transform.Find("codigo").GetComponent<TextMeshProUGUI>().text = temp;
        }

        //reanudar animacion de plataformas activas
        // GenericPlatform targetPlatformScript = newTargetGameObject.GetComponent<GenericPlatform>();
        // if (targetPlatformScript) {
        //     if (targetPlatformScript.currentAnimation != null)
        //         targetPlatformScript.currentAnimation();
        // }
    }

    public void clearCodeLines() {
        // Debug.Log("limpiando");
        StopAllCoroutines();
        List<GameObject> Children = new List<GameObject>();
        foreach (Transform child in codeContainer.transform) {
            Children.Add(child.gameObject);
        }
        foreach (GameObject child in Children) {
            Destroy(child);
            // child.SetActive(false);
        }
        // PlaygroundEvents.instance.stopAnimations();
        // if (PlaygroundEvents.instance.currentRoutine != null)
        //     StopCoroutine(PlaygroundEvents.instance.currentRoutine);
    }
}
