using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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


    [Header("Other Config.")]
    public int additionalEnemyCount = 0;
    public int currentCircle;


    [Header("Dungeon  Lists")]
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

        gameAreaCollider = GetComponent<BoxCollider2D>();

        GenerateDungeon();

        circleCleared = false;

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
                
            }

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
                circlesToInstantiate[i].SetActive(false);
            }
            else if (circlesToInstantiate.Count < 5)
            {
                Debug.Log("Var list is incomplete ");
            }
        }

        circlesToInstantiate.Clear();

        HandleDungeons();
    }

    void HandleDungeons()
    {
        // Define a layer mask to specify which layers you want to check for collisions.
        // You can set this mask in the Unity Inspector to specify which layers to consider.
        LayerMask collisionLayerMask = LayerMask.GetMask("YourCollisionLayer");

        // Set up a BoxCollider2D to represent the area you want to check for collisions.
        // You can adjust the size and position based on your needs.
        Bounds bounds = gameAreaCollider.bounds;

        // Check for collisions within the bounds of the BoxCollider2D.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, collisionLayerMask);

        // Now, 'colliders' contains all colliders that overlap with your BoxCollider2D.
        // You can loop through them and perform any necessary actions.
        Debug.Log("handling Dungeons");

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Circle1"))
            {
                // Handle the collision with the object that has the specified tag.
                circlesToInstantiate.Add(collider.gameObject);
                Debug.Log("circle 1 added");
            }

            if (collider.CompareTag("Circle2"))
            {
                // Handle the collision with the object that has the specified tag.
                circlesToInstantiate.Add(collider.gameObject);
                Debug.Log("circle 2 added");

            }

            if (collider.CompareTag("Circle3"))
            {
                // Handle the collision with the object that has the specified tag.
                circlesToInstantiate.Add(collider.gameObject);
                Debug.Log("circle 3 added");

            }

            if (collider.CompareTag("Circle4"))
            {
                // Handle the collision with the object that has the specified tag.
                circlesToInstantiate.Add(collider.gameObject);
                Debug.Log("circle 4 added");

            }

            if (collider.CompareTag("Circle5"))
            {
                // Handle the collision with the object that has the specified tag.
                circlesToInstantiate.Add(collider.gameObject);
                Debug.Log("circle 5 added");

            }
        }

        // Check if the list has elements before accessing them.
        if (circlesToInstantiate.Count > 0)
        {
            Debug.Log("First circle is called " + circlesToInstantiate[0].name.ToString());
            circlesToInstantiate[0].SetActive(true);
        }
    }



}
