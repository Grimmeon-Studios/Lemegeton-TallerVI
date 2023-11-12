using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class EndCanva : MonoBehaviour
{
    [SerializeField] HighscoreHandler highscoreHandler;
    private string playerName;
    [SerializeField] private GameObject playerNameInput;
    [SerializeField] private GameObject joystick;
    private ScoreBoard scoreBoard;
    [SerializeField] TextMeshProUGUI pointsText;

    private AsmodeusScript boss;

    private bool running;
    // Start is called before the first frame update
    private void Start()
    {
        running = true;
        scoreBoard = UnityEngine.Object.FindObjectOfType<ScoreBoard>();
        boss = FindObjectOfType<AsmodeusScript>();
    }

    private void Update()
    {
        if (boss.EndCanva && running)
        {
            Time.timeScale = 0f;
            playerNameInput.SetActive(true);
            pointsText.text = "" + scoreBoard.GetScore();
            joystick.SetActive(false);
        }
    }

    public void ReadStringInput(string name)
    {
        playerName = name;
        Debug.Log(playerName);
        ChangeScene();
    }

    
    private void ChangeScene()
    {
        running = false;
        highscoreHandler.AddHighscoreIfPossible (new HighscoreElement (playerName, scoreBoard.GetScore()));
        Time.timeScale = 1f;
        SceneManager.LoadScene("HUB");
    }
}
