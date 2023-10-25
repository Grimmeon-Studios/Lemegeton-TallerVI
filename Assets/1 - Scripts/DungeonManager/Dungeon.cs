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
    [SerializeField] private GameObject playerObj;
    [SerializeField] private LayerMask collisionLayerMask;

    private int difficultylvl;
    private bool listComplete;
    private bool onSetUp;
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
    public List<GameObject> InstatiatedCircles = new List<GameObject>();
    


    private void Awake()
    {
        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();

        gameAreaCollider = GetComponent<BoxCollider2D>();

        additionalEnemyCount = 0;
        currentCircle = 1;
        chronometer.currentCircleLvl = currentCircle;
        
        onSetUp = true;

        GenerateDungeon();

        StartCoroutine(SetUpTimer(1f));
    }

    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            unclearedRooms++;
            Debug.Log(" has been detected, now uncleared Rooms = " + unclearedRooms.ToString());
        }

        if(collision.gameObject.CompareTag("Circle") && onSetUp == true)
        {
            Debug.Log("Circle Detected");
            InstatiatedCircles.Add(collision.gameObject);
            //collision.gameObject.SetActive(false);

            if (InstatiatedCircles.Count == circlesToInstantiate.Count)
            {
                listComplete = true;
                if (listComplete)
                {
                    Debug.Log("List Complete");
                    ShuffleCircles();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            unclearedRooms--;
            Debug.Log("Update uncleared Rooms = " + unclearedRooms.ToString());

            if (unclearedRooms == 0 && chronometer.difficultyLvl > 1)
            {
                Debug.Log("Circle " + currentCircle + " Cleared");
                //circleCleared = true;
                OnCircleCleared(true);
            }
        }
    }

    private void OnCircleCleared(bool cleared)
    {
        if (cleared)
        {
            GameObject currentMap = InstatiatedCircles[currentCircle - 1];
            Debug.Log("circle " + currentMap.name.ToString() + " cleared");
            currentMap.SetActive(false);

            currentCircle = currentCircle + 1;

            if (currentCircle > 1)
            {
                Debug.Log("circle " + InstatiatedCircles[currentCircle - 1].gameObject.name.ToString() + " Will be activated");
                InstatiatedCircles[currentCircle - 1].SetActive(true);

                if (currentCircle > InstatiatedCircles.Count)
                {
                    SceneManager.LoadScene(0);
                }
            }

            playerObj.transform.position = Vector3.zero;
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

        for (int i = 0; i < circlesToInstantiate.Count; i++)
        {
            if (circlesToInstantiate[i] != null)
            {
                Instantiate(circlesToInstantiate[i]);
                circlesToInstantiate[i].gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Var list is Null ");
            }
        }
    }

    private void ShuffleCircles()
    {
        InstatiatedCircles = InstatiatedCircles.OrderBy(x => Guid.NewGuid()).ToList();
        for(int i = 0; i < circlesToInstantiate.Count; i++)
        {
            InstatiatedCircles[i].gameObject.SetActive(false);
        }
        InstatiatedCircles[0].gameObject.SetActive(true);
    }

    private IEnumerator SetUpTimer(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        onSetUp = false;
    }
}
