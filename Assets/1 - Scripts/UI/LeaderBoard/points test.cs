using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class pointstest : MonoBehaviour
{
    [SerializeField] HighscoreHandler highscoreHandler;
    private string playerName;
    [SerializeField] private GameObject playerNameInput;
    [SerializeField] private GameObject joystick;
    private ScoreBoard scoreBoard;
    [SerializeField] TextMeshProUGUI pointsText;
    // Start is called before the first frame update
    private void Start()
    {
        scoreBoard = UnityEngine.Object.FindObjectOfType<ScoreBoard>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Time.timeScale = 0f;
        playerNameInput.SetActive(true);
        pointsText.text = "" + scoreBoard.GetScore();
        joystick.SetActive(false);
       
    }

    public void ReadStringInput(string name)
    {
        playerName = name;
        Debug.Log(playerName);
        ChangeScene();
    }

    
    private void ChangeScene()
    {
        highscoreHandler.AddHighscoreIfPossible (new HighscoreElement (playerName, scoreBoard.GetScore()));
        SceneManager.LoadScene("HUB");
    }
}
