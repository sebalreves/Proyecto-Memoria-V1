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
    public Color amarillo_1, amarillo_2, verde;
    public AnimationCurve fillAnimationCurve;
    public AnimationCurve fillAlphaAnimationCurve;


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


    public void trySetColorLine(int _fromLineIndex, int _toLineIndex, Color _color, float _time = 1f, Action _action = null) {
        if (_fromLineIndex > _toLineIndex) {
            Debug.LogWarning("_fromline parameter > toLine");
        }
        GameObject codeLine;
        for (int i = _fromLineIndex; i <= _toLineIndex; i++) {
            try {
                codeLine = codeContainer.transform.GetChild(i).gameObject;
            } catch (System.Exception e) {
                Debug.LogWarning("Line not found" + e);
                return;
            }
            // codeLine.GetComponent<Image>().color = _color;
            try {
                StartCoroutine(fillCodeLine(codeLine, _color, _time, _action));
                StartCoroutine(executeLine(_time, _action));
            } catch (System.Exception) {

                Debug.LogWarning("Error al animar linea");
            }
        }
    }



    public void trySetColorLine(int _lineIndex, Color _color, float _time = 1f, Action _action = null) {
        GameObject codeLine;
        try {
            codeLine = codeContainer.transform.GetChild(_lineIndex).gameObject;
        } catch (System.Exception e) {
            // Debug.LogWarning("Line not found" + e);
            return;
        }
        // codeLine.GetComponent<Image>().color = _color;
        try {
            StartCoroutine(fillCodeLine(codeLine, _color, _time, _action));
            StartCoroutine(executeLine(_time, _action));
        } catch (System.Exception) {

            Debug.LogWarning("Error al animar linea");
        }
    }

    public IEnumerator executeLine(float _time, Action _action) {
        yield return new WaitForSeconds(_time);
        if (_action != null)
            _action();
    }

    public IEnumerator fillCodeLine(GameObject codeLine, Color _color, float _time, Action _action) {
        Slider _slider = codeLine.transform.Find("Slider").GetComponent<Slider>();
        Image _fillImage = codeLine.transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
        _fillImage.color = _color;


        float aux = 0f;
        while (_slider.value < 1) {
            _slider.value = fillAnimationCurve.Evaluate(aux / _time);
            aux += _time * Time.deltaTime;
            yield return null;
        }
        // yield return new WaitForSeconds(_time);
        // codeLine.GetComponent<Image>().color = _color;
        try {
            StartCoroutine(codeLineAlphaRoutine(_fillImage, _time));
        } catch (System.Exception) {
            Debug.LogWarning("Error al animar alpha");
        }

    }

    public IEnumerator codeLineAlphaRoutine(Image _fillImage, float _time) {
        // _fillImage
        float aux = 0f;
        while (_fillImage && _fillImage.color.a > 0f) {
            _fillImage.color = new Color(_fillImage.color.r, _fillImage.color.g, _fillImage.color.b, 1 - fillAnimationCurve.Evaluate(aux));
            aux += Time.deltaTime;
            yield return null;
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

    public void onTargetUpdateUI(string _titulo, List<string> codeLines) {
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
    }

    public void clearCodeLines() {
        List<GameObject> Children = new List<GameObject>();
        foreach (Transform child in codeContainer.transform) {
            Children.Add(child.gameObject);
        }
        foreach (GameObject child in Children) {
            Destroy(child);
            // child.SetActive(false);
        }
    }
}
