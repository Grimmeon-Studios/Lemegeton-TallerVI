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
    private int currentCircle = 1;    

    // Rooms Verification HashSet
    private HashSet<GameObject> roomsInsideCollider = new HashSet<GameObject>();


    private bool isDivisibleFor3;
    private int additionalEnemyCount = 0;
    [Header("Enemy Stats Scale")]
    [Header("X = Health | Y = Attack | Z = Speed")]
    public Vector3 incubus_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 LostSoul_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 andras_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed


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

    [Header("Fourth Circle")]
    [SerializeField] private GameObject Circle4_Variant1;
    [SerializeField] private GameObject Circle4_Variant2;
    [SerializeField] private GameObject Circle4_Variant3;
    [SerializeField] private GameObject Circle4_Variant4;

    [Header("Fifth Circle")]
    [SerializeField] private GameObject Circle5_Variant1;
    [SerializeField] private GameObject Circle5_Variant2;
    [SerializeField] private GameObject Circle5_Variant3;
    [SerializeField] private GameObject Circle5_Variant4;

    [Header("Sixth Circle")]
    [SerializeField] private GameObject Circle6_Variant1;
    [SerializeField] private GameObject Circle6_Variant2;
    [SerializeField] private GameObject Circle6_Variant3;
    [SerializeField] private GameObject Circle6_Variant4;

    [Header("Seventh Circle")]
    [SerializeField] private GameObject Circle7_Variant1;
    [SerializeField] private GameObject Circle7_Variant2;
    [SerializeField] private GameObject Circle7_Variant3;
    [SerializeField] private GameObject Circle7_Variant4;

    [Header("Eigth Circle")]
    [SerializeField] private GameObject Circle8_Variant1;
    [SerializeField] private GameObject Circle8_Variant2;
    [SerializeField] private GameObject Circle8_Variant3;
    [SerializeField] private GameObject Circle8_Variant4;

    [Header("Nineth Circle")]
    [SerializeField] private GameObject Circle9_Variant1;
    [SerializeField] private GameObject Circle9_Variant2;
    [SerializeField] private GameObject Circle9_Variant3;
    [SerializeField] private GameObject Circle9_Variant4;

    private List<GameObject> circlesToInstantiate = new List<GameObject>();



    // Private Members Encapsulation
    public int _difficultylvl { get => difficultylvl; set => difficultylvl = value; }
    public bool _circleCleared { get => circleCleared; set => circleCleared = value; }
    public bool IsDivisibleFor3 { get => isDivisibleFor3; set => isDivisibleFor3 = value; }
    public int AdditionalEnemyCount { get => additionalEnemyCount; set => additionalEnemyCount = value; }

    private void Awake()
    {
        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();

        gameAreaCollider = GetComponent<BoxCollider2D>();
        additionalEnemyCount = 0;

        GenerateDungeon();
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
        EnemyManagement();

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
            currentCircle++;
        }

        circlesToInstantiate[currentCircle-1].gameObject.SetActive(false);
        circlesToInstantiate[currentCircle].gameObject.SetActive(true);
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

    private void GenerateDungeon()
    {
        int ranCircle1 = UnityEngine.Random.Range(0, 4);
        int ranCircle2 = UnityEngine.Random.Range(0, 4);
        int ranCircle3 = UnityEngine.Random.Range(0, 4);
        int ranCircle4 = UnityEngine.Random.Range(0, 4);
        int ranCircle5 = UnityEngine.Random.Range(0, 4);
        int ranCircle6 = UnityEngine.Random.Range(0, 4);
        int ranCircle7 = UnityEngine.Random.Range(0, 4);
        int ranCircle8 = UnityEngine.Random.Range(0, 4);
        int ranCircle9 = UnityEngine.Random.Range(0, 4);

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

        switch (ranCircle4)
        {
            case 0:
                circlesToInstantiate.Add(Circle4_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle4_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle4_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle4_Variant4);
                break;
        }

        switch (ranCircle5)
        {
            case 0:
                circlesToInstantiate.Add(Circle5_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle5_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle5_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle5_Variant4);
                break;
        }

        switch (ranCircle6)
        {
            case 0:
                circlesToInstantiate.Add(Circle6_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle6_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle6_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle6_Variant4);
                break;
        }

        switch (ranCircle7)
        {
            case 0:
                circlesToInstantiate.Add(Circle7_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle7_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle7_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle7_Variant4);
                break;
        }

        switch (ranCircle8)
        {
            case 0:
                circlesToInstantiate.Add(Circle8_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle8_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle8_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle8_Variant4);
                break;
        }

        switch (ranCircle9)
        {
            case 0:
                circlesToInstantiate.Add(Circle9_Variant1);
                break;

            case 1:
                circlesToInstantiate.Add(Circle9_Variant2);
                break;

            case 2:
                circlesToInstantiate.Add(Circle9_Variant3);
                break;

            case 3:
                circlesToInstantiate.Add(Circle9_Variant4);
                break;
        }

        for (int i = 0; i < circlesToInstantiate.Count; i++)
        {
            if (circlesToInstantiate[i] != null)
            {
                Instantiate(circlesToInstantiate[i]);
                circlesToInstantiate[i].SetActive(false);
            }
            else
            {
                Debug.Log("HashSet empty");
            }
        }
    }

}
