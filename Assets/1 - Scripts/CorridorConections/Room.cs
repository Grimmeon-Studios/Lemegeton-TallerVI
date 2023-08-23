using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int enemies;

    private EdgeCollider2D edgeCollider;
    private BoxCollider2D boxCollider;
    //private SpriteRenderer spriteRenderer;

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.enabled = false;
        edgeCollider.enabled = false;
        Debug.Log(edgeCollider.points.Length);
    }

    public void OnTriggerEnter2D(Collider2D @object)
    {
        if (@object.CompareTag("Player"))
        {
            edgeCollider.enabled = true;
            Vector2 boxSize = boxCollider.size;
            Debug.Log("Player Entered");
            //spriteRenderer.enabled = true;
        }
        else if(@object.CompareTag("Enemy"))
        {
            enemies = +1;
        }

        boxCollider.enabled = false;
    }
}
