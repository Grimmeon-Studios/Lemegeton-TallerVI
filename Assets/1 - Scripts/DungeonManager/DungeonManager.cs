using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private float currentTime;
    private int difficultylvl;
    private bool floorClear;
    
    private BoxCollider2D gameAreaCollider;

    public int roomsNumber;
    public int remainingRooms;

    // Rooms Verification HashSet
    private HashSet<GameObject> roomsInsideCollider = new HashSet<GameObject>();

    // Private Members Encapsulation
    public float _currentTime { get => currentTime; set => currentTime = value; }
    public int _difficultylvl { get => difficultylvl; set => difficultylvl = value; }
    public bool _floorClear { get => floorClear; set => floorClear = value; }

    private void Awake()
    {
        gameAreaCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Room") && gameAreaCollider.bounds.Contains(other.transform.position))
        {
            roomsInsideCollider.Add(other.gameObject);
            Debug.Log("Added " + other.gameObject.name + " to the list.");
        }
    }

}
