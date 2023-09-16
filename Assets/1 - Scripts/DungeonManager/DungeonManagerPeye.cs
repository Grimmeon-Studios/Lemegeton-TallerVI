using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManagerPeye : MonoBehaviour
{

    private ChronometerManager chronometer;
    private BoxCollider2D gameAreaCollider;
    private GameObject playerObj;

    private int difficultylvl;
    private bool circleCleared;
    private bool PlayerAlive;
    private int currentCircle = 0;    

    // Rooms Verification HashSet
    //private HashSet<int> roomsInsideCollider = new HashSet<int>();


    private bool isDivisibleFor3;
    private int additionalEnemyCount = 0;
    [Header("Enemy Stats Scale")]
    [Header("X = Health | Y = Attack | Z = Speed")]
    public Vector3 incubus_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 LostSoul_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 andras_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed

    [Header("Spawn Points")]
    public Transform circle1;
    public Transform circle2;
    public Transform circle3;
    public Transform circle4;
    public Transform circle5;
    private int totalRooms;



    // Private Members Encapsulation
    public int _difficultylvl { get => difficultylvl; set => difficultylvl = value; }
    public bool _circleCleared { get => circleCleared; set => circleCleared = value; }
    public bool IsDivisibleFor3 { get => isDivisibleFor3; set => isDivisibleFor3 = value; }
    public int AdditionalEnemyCount { get => additionalEnemyCount; set => additionalEnemyCount = value; }

    private void Awake()
    {
        playerObj = FindObjectOfType<PlayerManager>().gameObject;

        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();

        gameAreaCollider = GetComponent<BoxCollider2D>();
        additionalEnemyCount = 0;

        
    }

    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Room"))
        {
            totalRooms++;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            PlayerAlive = true;
            //Debug.Log("Player has been detected");
        }
    }

    public void OnRoomCleared(GameObject currentRoom)
    {
        EnemyManagement();

        if (currentRoom.GetComponent<RoomPeye>() != null)
        {
            if (currentRoom.CompareTag("Room"))
            {
                if (totalRooms == 0)
                {
                    Debug.Log("DungeonCleared");
                    circleCleared = true;
                    OnCircleCleared();
                }

                Destroy(currentRoom);
                totalRooms--;
            }
        }
    }

    public void OnCircleCleared()
    {
        if (circleCleared)
        {
            currentCircle++;
        }

        switch (currentCircle)
        {
            case 0:
                playerObj.transform.position = circle1.position;
                gameObject.transform.position = circle1.position;
                break;

            case 1:
                playerObj.transform.position = circle2.position;
                gameObject.transform.position = circle2.position;
                break;

            case 2:
                playerObj.transform.position = circle3.position;
                gameObject.transform.position = circle3.position;
                break;

            case 3:
                playerObj.transform.position = circle4.position;
                gameObject.transform.position = circle4.position;
                break;

            case 4:
                playerObj.transform.position = circle5.position;
                gameObject.transform.position = circle5.position;
                break;
        }

        circleCleared = false;
    }

    public void EnemyManagement()
    {
        if (difficultylvl % 3 == 0)
        {
            AdditionalEnemyCount++;
        }
        else
        {
            incubus_StatsMultiplier += incubus_StatsMultiplier;
            LostSoul_StatsMultiplier += LostSoul_StatsMultiplier;
            andras_StatsMultiplier += andras_StatsMultiplier;
        }
            
    }

}
