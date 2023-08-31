using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int enemies;

    private EdgeCollider2D edgeCollider;

    // The first Collider is the one who detects the player and trapts it in the Room
    [SerializeField] private BoxCollider2D mainBoxCollider;


    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        mainBoxCollider = GetComponent<BoxCollider2D>();
        edgeCollider.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D @object)
    {
        if (@object.CompareTag("Player"))
        {
            edgeCollider.enabled = true;
            Debug.Log("Player Entered");
        }
        else if(@object.CompareTag("Enemy"))
        {
            enemies = +1;
        }
        mainBoxCollider.enabled = false;
    }
}
