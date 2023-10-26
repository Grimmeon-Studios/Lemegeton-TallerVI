using TMPro;
using UnityEngine;

public class ChronometerManager : MonoBehaviour
{
    [Header("Chronometer Parameters")]
    [SerializeField] private TextMeshProUGUI timerText; // Reference to the Text component where the timer will be displayed
    [SerializeField] private TextMeshProUGUI difficultyLvl_Txt;
    [SerializeField] private TextMeshProUGUI currentCircle_Txt;
    [SerializeField] private GameObject difficultyColor;
    [SerializeField] private Dungeon dungeonManager;
    private RectTransform difficultyColor_Transform;
    private float startTime;
    private float pausedTime;
    private bool isPaused = false;

    [SerializeField] private float timeScale;
    private int minutes;
    private int seconds;

    [SerializeField] private float timeStepsForLevel;

    [HideInInspector] public int difficultyLvl;
    [HideInInspector] public int currentCircleLvl;

    void Start()
    {
        //dungeonManager = GetComponent<Dungeon>();
        StartChronometer();
        difficultyColor_Transform = difficultyColor.GetComponent<RectTransform>();
        difficultyLvl = 0;
        InvokeRepeating("AumentarContador", 0f, timeStepsForLevel);
        currentCircle_Txt.text = currentCircleLvl.ToString();
    }

    void Update()
    {
        if (!isPaused)
        {
            float currentTime = Time.time - startTime - pausedTime;
            if (timeScale > 0) 
            {
                currentTime = (Time.time - startTime - pausedTime) * timeScale;
                DisplayTime(currentTime);
                difficultyColor_Transform.position = new Vector2((currentTime * -1), 0);
            }
            else
            {
                DisplayTime(currentTime);
                difficultyColor_Transform.position = new Vector2((currentTime * -1), 0);
            }

            currentCircle_Txt.text = currentCircleLvl.ToString();
            difficultyLvl_Txt.text = difficultyLvl.ToString();
        
        }
    }

    private void DisplayTime(float timePassed)
    {
        minutes = Mathf.FloorToInt(timePassed / 60);
        seconds = Mathf.FloorToInt(timePassed % 60);
        //difficultyLvl = difficultyLvl + minutes;
        // Format the time as "00:00"
        string timeText = minutes.ToString("00") + ":" + seconds.ToString("00");
        // Update the TextMeshProUGUI component
        timerText.text = timeText;
        //Debug.Log(difficultyLvl);
    }


    public void StartChronometer()
    {
        startTime = Time.time;
        isPaused = false;
    }

    public void PauseChronometer()
    {
        if (!isPaused)
        {
            pausedTime += Time.time - startTime;
            isPaused = true;
        }
    }

    public void ResumeChronometer()
    {
        if (isPaused)
        {
            startTime = Time.time;
            isPaused = false;
        }
    }

    public void ResetChronometer()
    {
        startTime = Time.time;
        pausedTime = 0f;
        isPaused = false;
        DisplayTime(0);
    }

    void AumentarContador()
    {
        difficultyLvl++;
        dungeonManager.EnemyManagement();
        //Debug.Log("gei");
    }
}