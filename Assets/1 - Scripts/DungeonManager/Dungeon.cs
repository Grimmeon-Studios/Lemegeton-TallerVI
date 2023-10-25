using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class Dungeon : MonoBehaviour
{
    private ChronometerManager chronometer;
    private BoxCollider2D gameAreaCollider;
    private GameObject playerObj;
    [SerializeField] private LayerMask collisionLayerMask;

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

        //Debug.Log(circlesToInstantiate[0].name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            unclearedRooms++;
            //Debug.Log(" has been detected, now uncleared Rooms = " + unclearedRooms.ToString());
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
                Debug.Log("Circle Cleared");
                circleCleared = true;
                OnCircleCleared();
            }
        }
    }

    private void OnCircleCleared()
    {
        if (circleCleared)
        {
            Debug.Log("circle " + circlesToInstantiate[currentCircle-1].gameObject.name.ToString() + " cleared");
            circlesToInstantiate[currentCircle-1].transform.Find("Dungeon").gameObject.SetActive(false);

            currentCircle++;

            if (currentCircle > 1)
            {
                Debug.Log("circle " + circlesToInstantiate[currentCircle - 1].gameObject.name.ToString() + " Will be activated");
                Debug.Log(circlesToInstantiate[currentCircle - 1].transform.Find("Dungeon").gameObject);
                circlesToInstantiate[currentCircle - 1].transform.Find("Dungeon").gameObject.SetActive(true);

                if (currentCircle > circlesToInstantiate.Count)
                {
                    SceneManager.LoadScene(0);
                }
            }

            playerObj.transform.position = Vector3.zero;
            circleCleared = false;
            chronometer.currentCircleLvl = currentCircle;
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
        //System.Random rng = new System.Random();

        //int n = circlesToInstantiate.Count;
        //while (n > 1)
        //{
        //    n--;
        //    int k = rng.Next(n + 1);
        //    GameObject value = circlesToInstantiate[k];
        //    circlesToInstantiate[k] = circlesToInstantiate[n];
        //    circlesToInstantiate[n] = value;
        //}


        circlesToInstantiate = circlesToInstantiate.OrderBy(x => Guid.NewGuid()).ToList();

        for (int i = 0; i < circlesToInstantiate.Count; i++)
        {
            if (circlesToInstantiate[i] != null)
            {
                Instantiate(circlesToInstantiate[i]);
                circlesToInstantiate[i].SetActive(true);
            }
            else
            {
                Debug.Log("Var list is Null ");
            }
        }

        //circlesToInstantiate.Clear();

        if (circlesToInstantiate.Count != 0)
        {
            Debug.Log("First circle is called " + circlesToInstantiate[0].name.ToString());
            circlesToInstantiate[0].transform.Find("Dungeon").gameObject.SetActive(true);
        }
    }
}
