using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CodeDescription : MonoBehaviour {
    GameObject linePrefab;
    public string titulo;
    public List<string> codeLines;
    GameObject codeLayout;
    TextMeshProUGUI tituloTMP;
    GameObject codeContainer;
    public Color color1, color2;


    IEnumerator Start() {
        // codeLines = new List<string>();
        linePrefab = Resources.Load("CodeLine") as GameObject;
        while (PlayerFactory._instance.localPlayer == null) {
            yield return null;
        }

        codeLayout = PlayerFactory._instance.localPlayer.transform.Find("Camera").transform.Find("Main Camera").transform.GetChild(0).transform.Find("#Codigo").gameObject;
        tituloTMP = codeLayout.transform.Find("Header").GetChild(0).GetComponent<TextMeshProUGUI>();
        codeContainer = codeLayout.transform.Find("Lines").gameObject;
        initializeDescription();
    }

    void initializeDescription() {

    }

    void onExecute() {

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

    public void onUpdateFocusObject() {
        tituloTMP.text = titulo;
        //Borrar lineas anteriores
        clearCodeLines();
        for (int i = 0; i < codeLines.Count; i++) {
            var temp = codeLines[i].Replace("#", "    ");
            GameObject newLine = Instantiate(linePrefab, codeContainer.transform);
            newLine.GetComponent<Image>().color = i % 2 == 0 ? color1 : color2;
            newLine.transform.Find("numero").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            newLine.transform.Find("codigo").GetComponent<TextMeshProUGUI>().text = temp;
        }
        // Debug.Log("UpdateFocusedObject");
    }

    // Update is called once per frame
    void Update() {

    }
}
