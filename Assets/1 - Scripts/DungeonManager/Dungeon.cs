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
    private int currentCircle = 0;

    private bool isDivisibleFor3;
    private int additionalEnemyCount = 0;
    private int unclearedRooms = 0;

    [Header("Enemy Stats Scale")]
    [Header("X = Health | Y = Attack | Z = Speed")]
    public Vector3 incubus_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 LostSoul_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed
    public Vector3 andras_StatsMultiplier = new Vector3(0, 0, 0); // hp, attack, speed



    private void Awake()
    {
        playerObj = FindObjectOfType<PlayerManager>().gameObject;

        chronometer = gameObject.GetComponentInChildren<ChronometerManager>();

        gameAreaCollider = GetComponent<BoxCollider2D>();
        additionalEnemyCount = 0;

    }

    private void Update()
    {
        difficultylvl = chronometer.difficultyLvl;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            Debug.Log(" has been detected, now uncleared Rooms = " + unclearedRooms);
            unclearedRooms++;
        }
    }

    public void OnRoomCleared(Room currentRoom)
    {
        if (currentRoom == null)
        {

        }
    }
}
