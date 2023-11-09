using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class pointstest : MonoBehaviour
{
    [SerializeField] HighscoreHandler highscoreHandler;
    [SerializeField] string playerName;
    private ScoreBoard scoreBoard;
    // Start is called before the first frame update
    private void Start()
    {
        scoreBoard = UnityEngine.Object.FindObjectOfType<ScoreBoard>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        highscoreHandler.AddHighscoreIfPossible (new HighscoreElement (playerName, scoreBoard.GetScore()));
        
    }
}
