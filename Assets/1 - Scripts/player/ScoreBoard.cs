using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI scoredPoints;

    private float score;
    private float timeLimit = 2.5f; // Set the initial time limit in seconds
    private float currentTime = 0.0f;
    private bool isRunning = false;
    

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        scoredPoints.gameObject.SetActive(false);

        InvokeRepeating("UpdateScore", 0, 30);
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeLimit)
            {
                // Timer has reached the time limit
                scoredPoints.gameObject.SetActive(false);
                isRunning = false;
            }
        }
    }

    public float GetScore()
    {
        return score;
    }

    public void SetScore(float saveScore)
    {
        score = saveScore;
    }

    public void GetPoints(int ponits)
    {
        currentTime = 0;
        isRunning = true;
        scoredPoints.gameObject.SetActive(true);
        scoredPoints.text = "+" + ponits;
        score = Mathf.FloorToInt(Mathf.Lerp(score, score + ponits, 1f));
        totalScore.text = score.ToString();
    }

    private void UpdateScore()
    {
        totalScore.text = score.ToString();
    }
}
