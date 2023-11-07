using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour {
    [SerializeField] GameObject panel;
    [SerializeField] GameObject highscoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;

    List<GameObject> uiElements = new List<GameObject>();

    private void Start()
    {
    }

    private void OnEnable () {
        HighscoreHandler.onHighscoreListChanged += UpdateUI;
    }

    private void OnDisable () {
        HighscoreHandler.onHighscoreListChanged -= UpdateUI;
    }


    public void ShowPanel () {
        panel.SetActive (true);
    }

    public void ClosePanel () {
        panel.SetActive (false);
    }

    public void RestartLeaderBoard()
    {
        
    }
    private void UpdateUI (List<HighscoreElement> list) {
        for (int i = 0; i < list.Count; i++) {
            HighscoreElement el = list[i];

            if (el != null && el.score > 0) {
                if (i >= uiElements.Count) {
                    // instantiate new entry
                    var inst = Instantiate (highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent (elementWrapper, false);

                    uiElements.Add (inst);
                }

                // write or overwrite name & points
                var texts = uiElements[i].GetComponentsInChildren<TextMeshProUGUI> ();
                texts[0].text = (i+1).ToString();
                texts[1].text = el.playerName;
                texts[2].text = el.score.ToString ();
            }
        }
    }

}