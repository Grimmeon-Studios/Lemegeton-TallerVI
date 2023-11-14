using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class Dungeon : MonoBehaviour
{
    private ChronometerManager chronometer;
    private BoxCollider2D gameAreaCollider;
    private CameraTransition camTransition;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private LayerMask collisionLayerMask;

    [SerializeField] private SavingSystem saving;
    
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

    private Vector3 incubus_StatsMultiplierPriv;
    private Vector3 LostSoul_StatsMultiplierPriv;
    private Vector3 andras_StatsMultiplierPriv;

    [Header("Other Config.")]
    public int additionalEnemyCount = 0;
    public int currentCircle;


    [Header("Dungeon  Lists")]
    public List<GameObject> circlesToInstantiate = new List<GameObject>();
    public List<GameObject> InstatiatedCircles = new List<GameObject>();
    private BoxCollider2D boxColl;
    public TextMeshProUGUI totalRoomsTxt;
    public TextMeshProUGUI remainingRoomsTxt;
    private int totalRooms;
    private int remainingRooms;



    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();
        camTransition = FindObjectOfType<CameraTransition>();

        gameAreaCollider = GetComponent<BoxCollider2D>();

        additionalEnemyCount = 0;
        currentCircle = 1;
        chronometer.currentCircleLvl = currentCircle;
        
        onSetUp = true;

        incubus_StatsMultiplierPriv = incubus_StatsMultiplier;
        LostSoul_StatsMultiplierPriv = LostSoul_StatsMultiplier;
        andras_StatsMultiplierPriv = andras_StatsMultiplier;

        GenerateDungeon();

        StartCoroutine(SetUpTimer(1f));
        StartCoroutine(WaitAndCount());

        SetVolume(0f);

        StartCoroutine(ChangeVolumeOverTime(0.2f, 6f));
    }


    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl;
        remainingRoomsTxt.text = remainingRooms.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            unclearedRooms++;
            //Debug.Log(" has been detected, now uncleared Rooms = " + unclearedRooms.ToString());
        }

        if(collision.gameObject.CompareTag("Circle") && onSetUp == true)
        {
            //Debug.Log("Circle Detected");
            if(!InstatiatedCircles.Contains(collision.gameObject))
                InstatiatedCircles.Add(collision.gameObject);

            if (InstatiatedCircles.Count == circlesToInstantiate.Count)
            {
                listComplete = true;
                if (listComplete)
                {
                    //Debug.Log("List Complete");
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
            remainingRooms--;
            //Debug.Log("Update uncleared Rooms = " + unclearedRooms.ToString());
            if (unclearedRooms == 0 && onSetUp == false)
            {
                Debug.Log("Circle " + currentCircle + " Cleared");
                //circleCleared = true;
                StartCoroutine(CircleTransition(true));
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
                if (currentCircle > InstatiatedCircles.Count)
                {
                    saving.SaveGame();
                    SceneManager.LoadScene("Boss Dungeon");
                }
                else
                {
                    Debug.Log("circle " + InstatiatedCircles[currentCircle - 1].gameObject.name.ToString() + " Will be activated");
                    InstatiatedCircles[currentCircle - 1].SetActive(true);

                    StartCoroutine(WaitAndCount());
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
            Debug.Log("Adding more enemies");
            additionalEnemyCount++;
        }
        else
        {
            Debug.Log("Rising Stats");
            incubus_StatsMultiplier += incubus_StatsMultiplierPriv;
            LostSoul_StatsMultiplier += LostSoul_StatsMultiplierPriv;
            andras_StatsMultiplier += andras_StatsMultiplierPriv;
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
        //AudioListener.volume = 0.99f;
    }

    private int CountRooms()
    {
        int count = 0;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxColl.size, 0f);

        foreach(var rooms in colliders)
        {
            if (rooms.CompareTag("Room"))
            {
                count++;
            }
        }

        return count;
    }

    private IEnumerator SetUpTimer(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        onSetUp = false;
    }

    IEnumerator WaitAndCount()
    {

        yield return new WaitForSeconds(2f);

        totalRooms = CountRooms();
        remainingRooms = totalRooms;
        totalRoomsTxt.text = totalRooms.ToString();
    }

    public IEnumerator CircleTransition(bool cleared)
    {
        camTransition.CircleClearedTransition(currentCircle);
        //AudioListener.volume = 1f;
        yield return new WaitForSeconds(7f);
        //AudioListener.volume = 0.99f;
        OnCircleCleared(cleared);
    }

    // Coroutine to smoothly change the volume over time
    IEnumerator ChangeVolumeOverTime(float targetVolume, float duration)
    {
        float startVolume = AudioListener.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate the volume over time
            AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the volume is set to the target volume after the loop
        AudioListener.volume = targetVolume;
    }

    // Function to set the volume
    void SetVolume(float volume)
    {
        // Make sure the volume is within the valid range (0 to 1)
        volume = Mathf.Clamp01(volume);

        // Set the AudioListener volume
        AudioListener.volume = volume;
    }
}
