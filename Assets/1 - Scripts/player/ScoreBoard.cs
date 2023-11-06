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
        score = 0;
        scoredPoints.gameObject.SetActive(false);
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

    public void GetPoints(int ponits)
    {
        currentTime = 0;
        isRunning = true;
        scoredPoints.gameObject.SetActive(true);
        scoredPoints.text = "+" + ponits;
        //score = Mathf.FloorToInt(Mathf.Lerp(score, score + ponits, 1f));
        score = score + ponits;
        totalScore.text = score.ToString();
    }
}
