using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{

    private ChronometerManager chronometer;
    private BoxCollider2D gameAreaCollider;

    private int difficultylvl;
    private bool circleCleared;
    private bool PlayerAlive;
    private bool DungeonGenerated;
    

    // Rooms Verification HashSet
    private HashSet<GameObject> roomsInsideCollider = new HashSet<GameObject>();


    private bool isDivisibleFor3;
    private int additionalEnemyCount = 0;
    private Vector3 enemyStatsMultiplier = new Vector3(1, 1, 1); // hp, attack, speed


    [Header("Prefabs for dungeons")]

    [Header("First Circle")]
    [SerializeField] private GameObject Circle1_Variant1; 
    [SerializeField] private GameObject Circle1_Variant2; 
    [SerializeField] private GameObject Circle1_Variant3; 
    [SerializeField] private GameObject Circle1_Variant4; 

    [Header("Second Circle")]
    [SerializeField] private GameObject Circle2_Variant1; 
    [SerializeField] private GameObject Circle2_Variant2; 
    [SerializeField] private GameObject Circle2_Variant3; 
    [SerializeField] private GameObject Circle2_Variant4; 

    [Header("Third Circle")]
    [SerializeField] private GameObject Circle3_Variant1; 
    [SerializeField] private GameObject Circle3_Variant2; 
    [SerializeField] private GameObject Circle3_Variant3; 
    [SerializeField] private GameObject Circle3_Variant4;

    private List<GameObject> circlesToInstantiate = new List<GameObject>();



    // Private Members Encapsulation
    public int _difficultylvl { get => difficultylvl; set => difficultylvl = value; }
    public bool _circleCleared { get => circleCleared; set => circleCleared = value; }
    public bool IsDivisibleFor3 { get => isDivisibleFor3; set => isDivisibleFor3 = value; }
    public int AdditionalEnemyCount { get => additionalEnemyCount; set => additionalEnemyCount = value; }
    public Vector3 EnemyStatsMultiplier { get => enemyStatsMultiplier; set => enemyStatsMultiplier = value; }

    private void Awake()
    {
        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();

        gameAreaCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Room") && !roomsInsideCollider.Contains(other.gameObject))
        {
            roomsInsideCollider.Add(other.gameObject);
            //Debug.Log("Added " + other.gameObject + " to the list.");
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            PlayerAlive = true;
            //Debug.Log("Player has been detected");
        }
    }

    public void OnRoomCleared(GameObject currentRoom)
    {
        if(currentRoom.GetComponent<Room>() != null)
        {
            if (currentRoom.CompareTag("Room") && roomsInsideCollider.Contains(currentRoom))
            {
                roomsInsideCollider.Remove(currentRoom);

                if (roomsInsideCollider.Count == 0)
                {
                    Debug.Log("DungeonCleared");
                    circleCleared = true;
                    OnCircleCleared();
                }

                Destroy(currentRoom);
            }
        }
    }

    public void OnCircleCleared()
    {
        if (circleCleared)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void EnemyManagement()
    {
        if (difficultylvl % 3 == 0)
        {
            AdditionalEnemyCount++;
        }
        else
        {
            EnemyStatsMultiplier += new Vector3(5f, 1f, 0.2f); 
        }
            
    }

    private void GenerateDungeon()
    {
        int ranCircle1 = UnityEngine.Random.Range(0, 4);
        int ranCircle2 = UnityEngine.Random.Range(0, 4);
        int ranCircle3 = UnityEngine.Random.Range(0, 4);

        switch(ranCircle1)
        {
            case 0:
                circlesToInstantiate.Add(Circle1_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle1_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle1_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle1_Variant4);
                break;
        }

        switch(ranCircle2)
        {
            case 0:
                circlesToInstantiate.Add(Circle2_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle2_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle2_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle2_Variant4);
                break;
        }

        switch (ranCircle3)
        {
            case 0:
                circlesToInstantiate.Add(Circle3_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle3_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle3_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle3_Variant4);
                break;
        }
    }

}
