using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    private float currentTime;
    private int difficultylvl = 5;
    private bool circleCleared;
    private bool PlayerAlive;
    
    private BoxCollider2D gameAreaCollider;

    public int roomsNumber;
    public int remainingRooms;

    // Rooms Verification HashSet
    private HashSet<GameObject> roomsInsideCollider = new HashSet<GameObject>();

    // Private Members Encapsulation
    public float _currentTime { get => currentTime; set => currentTime = value; }
    public int _difficultylvl { get => difficultylvl; set => difficultylvl = value; }
    public bool _circleCleared { get => circleCleared; set => circleCleared = value; }

    private void Awake()
    {
        gameAreaCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        
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
