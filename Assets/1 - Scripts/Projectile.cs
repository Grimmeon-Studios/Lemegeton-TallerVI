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
        this.range = range;
    }

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        startPos = gameObject.transform.position;
        range = player.shotRange;
    }

    private void Update()   
    {
        distance = Vector2.Distance(startPos, gameObject.transform.position);
        //Debug.Log("Distance: "+distance);
        //Debug.Log("Range: "+range);
        if (distance > range)
        {
            Destroy(gameObject);
            Debug.Log("Proyectile Out of Reach, therefore Desrtoyed");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall") || other.CompareTag("EnemySoul") ||other.CompareTag("EnemyAndras"))
        {
            if (other.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<IncubusScript>().takeDamage(player.shotDamage);
            }

            if (other.CompareTag("EnemySoul"))
            {
                other.gameObject.GetComponent<LostSoulScript>().takeDamage(player.shotDamage);
            }

            if (other.CompareTag("EnemyAndras"))
            {
                other.gameObject.GetComponent<AndrasScript>().takeDamage(player.shotDamage);
            }

            Destroy(gameObject);
        }
    }
}
