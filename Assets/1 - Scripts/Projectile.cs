using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int speed;
    private float size;
    private float range;

    private float distance;
    private Vector2 startPos;

    private Rigidbody2D rg;

    [SerializeField] PlayerManager player;
    public Projectile (int speed, float size, float range)
    {
        this.speed = speed;
        this.size = size;
        this.range = player.shotRange;
    }

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        startPos = gameObject.transform.position;
    }

    private void Update()   
    {
        distance = Vector2.Distance(startPos, gameObject.transform.position);
        //Debug.Log("Distance: "+distance);
        //Debug.Log("Distance: "+range);
        //if (distance > range)
        //    Destroy(gameObject);
    }
}
