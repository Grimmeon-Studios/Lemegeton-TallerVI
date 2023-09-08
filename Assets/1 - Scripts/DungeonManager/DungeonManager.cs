using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private ChronometerManager chronometer;

    private int difficultylvl;
    private bool circleCleared;
    private bool PlayerAlive;
    
    private BoxCollider2D gameAreaCollider;

    private int roomsNumber;
    private int remainingRooms;

    // Rooms Verification HashSet
    private HashSet<GameObject> roomsInsideCollider = new HashSet<GameObject>();

    // Private Members Encapsulation
    public int _difficultylvl { get => difficultylvl; set => difficultylvl = value; }
    public bool _circleCleared { get => circleCleared; set => circleCleared = value; }

    private void Awake()
    {
        gameAreaCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl + 1;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Room") && !roomsInsideCollider.Contains(other.gameObject))
        {
            roomsInsideCollider.Add(other.gameObject);
            Debug.Log("Added " + other.gameObject + " to the list.");
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            PlayerAlive = true;
            Debug.Log("Player has been detected");
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
                    OnCicleCleared();
                }

                Destroy(currentRoom);
            }
        }
    }

    public void OnCicleCleared()
    {
        if (circleCleared)
        {
            SceneManager.LoadScene(0);
        }
    }
}
