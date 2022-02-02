using System.Collections;
using System.Collections.Generic;
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


    public void trySetColorLine(int _fromLineIndex, int _toLineIndex, Color _color) {
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
            codeLine.GetComponent<Image>().color = _color;
        }
    }

    public void trySetColorLine(int _lineIndex, Color _color) {
        GameObject codeLine;
        try {
            codeLine = codeContainer.transform.GetChild(_lineIndex).gameObject;
        } catch (System.Exception e) {
            Debug.LogWarning("Line not found" + e);
            return;
        }
        codeLine.GetComponent<Image>().color = _color;
    }

    public void resetCodeColor() {
        List<GameObject> Children = new List<GameObject>();
        int i = 0;
        foreach (Transform child in codeContainer.transform) {
            child.GetComponent<Image>().color = i % 2 == 0 ? amarillo_1 : amarillo_2;
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
            newLine.GetComponent<Image>().color = i % 2 == 0 ? amarillo_1 : amarillo_2;
            newLine.transform.Find("numero").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            newLine.transform.Find("codigo").GetComponent<TextMeshProUGUI>().text = temp;
        }
        // GameObject finalLine = Instantiate(linePrefab, codeContainer.transform);
        // finalLine.GetComponent<Image>().color = verde;
        // finalLine.transform.Find("numero").GetComponent<TextMeshProUGUI>().text = (codeLines.Count + 1).ToString();
        // finalLine.transform.Find("codigo").GetComponent<TextMeshProUGUI>().text = "Presionar <sprite=1> para activar";
        // Debug.Log("UpdateFocusedObject");

    }

    public void clearCodeLines() {
        List<GameObject> Children = new List<GameObject>();
        foreach (Transform child in codeContainer.transform) {
            Children.Add(child.gameObject);
        }
        foreach (GameObject child in Children) {
            Destroy(child);
        }
    }
}
