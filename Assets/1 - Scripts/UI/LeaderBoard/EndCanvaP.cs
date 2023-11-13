using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class EndCanvaP : MonoBehaviour
{
    [SerializeField] HighscoreHandler highscoreHandler;
    private string playerName;
    [SerializeField] private GameObject Canva;
    [SerializeField] private GameObject joystick;
    private ScoreBoard scoreBoard;
    [SerializeField] TextMeshProUGUI pointsText;

    private PlayerManager player;
    
    private bool runningP;
    // Start is called before the first frame update
    private void Start()
    {
        runningP = true;
        scoreBoard = FindObjectOfType<ScoreBoard>();
        player = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (player.endCanva && runningP)
        {
            Time.timeScale = 0f;
            Canva.SetActive(true);
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
        runningP = false;
        highscoreHandler.AddHighscoreIfPossible (new HighscoreElement (playerName, scoreBoard.GetScore()));
        Time.timeScale = 1f;
        SceneManager.LoadScene("HUB");
    }
}
