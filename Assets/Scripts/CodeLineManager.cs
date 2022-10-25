using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CodeLineManager : MonoBehaviour {
    public GameObject codeContainer, aux, fixedLinesContainer;

    private Animator codeAnimator, codeShineAnimator;
    public TextMeshProUGUI titulo;
    public static CodeLineManager _instance;
    GameObject linePrefab, separatorPrefab;
    private static Color amarillo_1, amarillo_2, gris1, gris2;
    public AnimationCurve fillAnimationCurve, fillAnimationLine;
    public AnimationCurve fillAlphaAnimationCurve;



    // public string currentTitle = null;

    public GameObject currentTargetCodeDisplayed = null;


    private IEnumerator Start() {
        amarillo_1 = PlaygroundManager.instance.yellow1;
        amarillo_2 = PlaygroundManager.instance.yellow2;
        gris1 = PlaygroundManager.instance.grey1;
        gris2 = PlaygroundManager.instance.grey2;

        linePrefab = Resources.Load("CodeLine") as GameObject;
        separatorPrefab = Resources.Load("CodeSeparator") as GameObject;
        while (!PlayerFactory._instance.localPlayer) {
            yield return null;
        }

        aux = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).gameObject;
        codeContainer = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.GetChild(4).transform.Find("#Codigo").transform.Find("Lines").gameObject;


        fixedLinesContainer = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.GetChild(4).transform.Find("#Codigo").transform.Find("LinesFixed").gameObject;
        titulo = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.GetChild(4).transform.Find("#Codigo").transform.Find("Header").transform.Find("Titulo").GetComponent<TextMeshProUGUI>();
        codeAnimator = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.GetChild(4).transform.Find("#Codigo").GetComponent<Animator>();
        codeShineAnimator = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.GetChild(4).transform.Find("CodeShine").GetComponent<Animator>();

        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
    }




    int getIndexSeparatorOffset(int _lineIndex) {
        int linesPassed = -1;
        int returnValue = -1;
        foreach (Transform child in codeContainer.transform) {
            returnValue++;
            if (child.transform.Find("Slider") != null) {
                linesPassed++;
            }
            if (linesPassed == _lineIndex) return returnValue;
        }
        return 0;
    }

    public void ShineAnimation() {
        codeShineAnimator.Play("code_shine");
    }

    public IEnumerator trySetColorLine(GameObject animationTarget, int _fromLineIndex, int _toLineIndex, Color _color, float _time = 1f, bool fadeUp = true, bool lineal = false) {
        if (_fromLineIndex > _toLineIndex) {
            Debug.LogWarning("_fromline parameter > toLine");
        }
        //Ejecutar accion independiente si se anima la linea
        // StartCoroutine(executeLine(_time, _action));

        //Para que solo se anime si la linea de codigo en cuestien esta targeteada
        if (!GameObject.ReferenceEquals(animationTarget, currentTargetCodeDisplayed)) {
            Debug.Log(animationTarget);
            Debug.Log(currentTargetCodeDisplayed);

            Debug.Log("Break set code color");
            yield break;
        }
        GameObject codeLine;

        float offsetTime = 0f;
        for (int i = _fromLineIndex; i <= _toLineIndex; i++) {
            try {
                codeLine = codeContainer.transform.GetChild(getIndexSeparatorOffset(i)).gameObject;
            } catch (System.Exception e) {
                Debug.LogWarning("Line not found" + e);
                yield break;
            }

            if (i == _toLineIndex)
                yield return fillCodeLine(codeLine, _color, _time, fadeUp, lineal, offsetTime);
            else
                StartCoroutine(fillCodeLine(codeLine, _color, _time, fadeUp, lineal, offsetTime));
            offsetTime += CONST.fillLoopOffsetTime;
        }
    }


    public IEnumerator trySetColorLine(GameObject animationTarget, int _lineIndex, Color _color, float _time = CONST.codeVelocity, bool fadeUp = true, bool lineal = false) {
        // StartCoroutine(executeLine(_time, _action));

        //Para que solo se anime si la linea de codigo en cuestien esta targeteada
        if (!GameObject.ReferenceEquals(animationTarget, currentTargetCodeDisplayed)) {
            // Debug.Log(animationTarget, currentTargetCodeDisplayed);
            // Debug.Log("Break set code color");
            yield break;
        }
        GameObject codeLine;

        try {
            codeLine = codeContainer.transform.GetChild(getIndexSeparatorOffset(_lineIndex)).gameObject;
        } catch (System.Exception e) {
            // Debug.LogWarning("Line not found" + e);
            yield break;
        }
        // codeLine.GetComponent<Image>().color = _color;
        yield return fillCodeLine(codeLine, _color, _time, fadeUp, lineal);

    }



    public IEnumerator fillCodeLine(GameObject codeLine, Color _color, float _time, bool fadeUp, bool lineal, float offsetTime = 0f) {
        yield return new WaitForSeconds(offsetTime);
        if (codeLine == null) yield break;


        Slider _slider = codeLine.transform.Find("Slider").GetComponent<Slider>();
        Image _fillImage = codeLine.transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
        _fillImage.color = _color;
        _slider.GetComponent<Animator>().Play("line_execution");

        float fillTime = fadeUp ? _time * 0.7f : _time;
        float alphaTime = _time * 0.3f;
        float auxTime = 0f;
        while (auxTime < fillTime) {
            if (lineal) _slider.value = fillAnimationLine.Evaluate(auxTime / fillTime);
            else _slider.value = fillAnimationCurve.Evaluate(auxTime / fillTime);
            auxTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (fadeUp)
            yield return codeLineAlphaRoutine(_fillImage, alphaTime);

        // yield return new WaitForSeconds(CONST.codeVelocity + 0.1f);


    }

    public void fadeOutCodeColor() {
        foreach (Transform child in codeContainer.transform) {
            if (child.transform.Find("Slider") == null) continue;
            Image _fillImage = child.transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
            _fillImage.color = gris1;
            StartCoroutine(codeLineAlphaRoutine(_fillImage, 0.5f));
        }
        // codeAnimator.Play("end_button_execution");
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
            if (child.transform.Find("Slider") == null) continue;
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

        //si el nuevo target es igual al anterior
        if (GameObject.ReferenceEquals(newTargetGameObject, currentTargetCodeDisplayed)) return;

        //si el anterior es de un grupo de botones y el nuevo es boton
        if (currentTargetCodeDisplayed != null && newTargetGameObject.GetComponent<GenericButton>() != null && currentTargetCodeDisplayed.GetComponent<ButtonWrapper>() != null)
            //si ambos son del mismo grupo de botones
            if (newTargetGameObject.GetComponent<GenericButton>().wrappGroup == currentTargetCodeDisplayed.GetComponent<ButtonWrapper>().my_groupId)
                return;


        //si el nuevo es un boton que no pertenece al grupo default
        if (newTargetGameObject.GetComponent<GenericButton>() != null && newTargetGameObject.GetComponent<GenericButton>().wrappGroup != 0)
            currentTargetCodeDisplayed = newTargetGameObject.GetComponent<GenericButton>().onPressParameter;
        else
            //para cualquier otro objeto o boton
            currentTargetCodeDisplayed = newTargetGameObject;

        titulo.text = _titulo;
        //Borrar lineas anteriores
        clearCodeLines();
        int codeIndex = 1;

        for (int i = 0; i < codeLines.Count; i++) {
            if (codeLines[i] == "#") {
                //agregar separador
                Instantiate(separatorPrefab, fixedLinesContainer.transform);
                Instantiate(separatorPrefab, codeContainer.transform);
                continue;
            }
            GameObject newLine = Instantiate(linePrefab, codeContainer.transform);
            Instantiate(linePrefab, fixedLinesContainer.transform);
            //achicar linea si hay tabulacion
            newLine.GetComponent<LayoutElement>().preferredWidth -= CONST.codeTabSize * codeLines[i].Count(f => f == '#');

            var temp = codeLines[i].Replace("#", "");
            newLine.transform.Find("Slider").transform.Find("Background").GetComponent<Image>().color = codeIndex % 2 == 0 ? amarillo_1 : amarillo_2;
            newLine.transform.Find("numero").GetComponent<TextMeshProUGUI>().text = (codeIndex++).ToString();
            newLine.transform.Find("codigo").GetComponent<TextMeshProUGUI>().text = temp;
            StartCoroutine(callLineAnimations(newLine.transform.Find("Slider").GetComponent<Animator>(), (codeLines.Count - 1) * 0.08f - i * 0.08f));

        }

        for (int i = codeIndex; i <= 5; i++) {
            GameObject newLine = Instantiate(linePrefab, fixedLinesContainer.transform);
            newLine.transform.Find("Slider").transform.Find("Background").GetComponent<Image>().color = i % 2 == 0 ? gris1 : gris2;
            newLine.transform.Find("numero").GetComponent<TextMeshProUGUI>().text = (i).ToString();
            // StartCoroutine(callLineAnimations(newLine.transform.Find("Slider").GetComponent<Animator>(), 0f));
        }

        //Animacion 
        codeAnimator.Play("new_code");
    }

    IEnumerator callLineAnimations(Animator lineAnimator, float delay) {
        yield return new WaitForSeconds(delay);
        lineAnimator.Play("new_line_animation");
    }

    public void clearCodeLines() {

        StopAllCoroutines();
        List<GameObject> Children = new List<GameObject>();
        foreach (Transform child in codeContainer.transform) {
            Children.Add(child.gameObject);
        }
        foreach (GameObject child in Children) {
            Destroy(child);
        }

        //fixed lines

        List<GameObject> ChildrenFixed = new List<GameObject>();
        foreach (Transform child in fixedLinesContainer.transform) {
            ChildrenFixed.Add(child.gameObject);
        }
        foreach (GameObject child in ChildrenFixed) {
            Destroy(child);
        }
    }
}
