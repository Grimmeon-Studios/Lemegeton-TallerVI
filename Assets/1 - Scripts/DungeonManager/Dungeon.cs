using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private ChronometerManager chronometer;
    private BoxCollider2D gameAreaCollider;
    private GameObject playerObj;

    private int difficultylvl;
    private bool circleCleared;
    private bool PlayerAlive;
    private bool isDivisibleFor3;
    private int unclearedRooms = 0;

    [Header("Enemy Stats Scale")]
    [Header("X = Health | Y = Attack | Z = Speed")]
    public Vector3 incubus_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 LostSoul_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 andras_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed

    public int additionalEnemyCount = 0;
    public int currentCircle;


    public List<GameObject> circlesToInstantiate = new List<GameObject>();

    public List<GameObject> circle1_var = new List<GameObject>();
    public List<GameObject> circle2_var = new List<GameObject>();
    public List<GameObject> circle3_var = new List<GameObject>();
    public List<GameObject> circle4_var = new List<GameObject>();
    public List<GameObject> circle5_var = new List<GameObject>();



    private void Awake()
    {
        playerObj = FindObjectOfType<PlayerManager>().gameObject;

        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();

        GenerateDungeon();

        circleCleared = false;

        gameAreaCollider = GetComponent<BoxCollider2D>();
        additionalEnemyCount = 0;
        currentCircle = 1;

    }

    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl;

        Debug.Log("Current Circle: " + currentCircle.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            unclearedRooms++;
            Debug.Log(" has been detected, now uncleared Rooms = " + unclearedRooms.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            unclearedRooms--;
            Debug.Log("Update uncleared Rooms = " + unclearedRooms.ToString());

            if (unclearedRooms == 0)
            {
                circleCleared = true;
                chronometer.currentCircleLvl = currentCircle;
                OnCircleCleared();
            }
        }
    }

    private void OnCircleCleared()
    {
        if (circleCleared)
        {
            currentCircle++;


            if (currentCircle >= 1)
            {
                circlesToInstantiate[currentCircle - 2].gameObject.SetActive(false);
            }
            circlesToInstantiate[currentCircle-1].gameObject.SetActive(true);

            playerObj.transform.position = Vector3.zero;
            circleCleared = false;
        }
    }

    public void EnemyManagement()
    {
        if (difficultylvl % 3 == 0)
        {
            additionalEnemyCount++;
        }
        else
        {
            incubus_StatsMultiplier += incubus_StatsMultiplier;
            LostSoul_StatsMultiplier += LostSoul_StatsMultiplier;
            andras_StatsMultiplier += andras_StatsMultiplier;
        }

    }

    private int UpdateReaminigRooms(int sign)
    {
        int result;

        result = unclearedRooms + (1*sign);

        return result;
    }

    private void GenerateDungeon()
    {
        int ranCircle1 = UnityEngine.Random.Range(0, 4);
        int ranCircle2 = UnityEngine.Random.Range(0, 4);
        int ranCircle3 = UnityEngine.Random.Range(0, 4);
        int ranCircle4 = UnityEngine.Random.Range(0, 4);
        int ranCircle5 = UnityEngine.Random.Range(0, 4);

        circlesToInstantiate.Add(circle1_var[ranCircle1]);
        circlesToInstantiate.Add(circle2_var[ranCircle2]);
        circlesToInstantiate.Add(circle3_var[ranCircle3]);
        circlesToInstantiate.Add(circle4_var[ranCircle4]);
        circlesToInstantiate.Add(circle5_var[ranCircle5]);
        

        for (int i = 0; i < circlesToInstantiate.Count; i++)
        {
            if (circlesToInstantiate[i] != null)
            {
                Instantiate(circlesToInstantiate[i]);
                circlesToInstantiate[i].gameObject.SetActive(false);
            }
            else if (circlesToInstantiate.Count < 5)
            {
                Debug.Log("Var list is incomplete ");
            }
        }

        circlesToInstantiate[0].gameObject.SetActive(true);
    }

}
