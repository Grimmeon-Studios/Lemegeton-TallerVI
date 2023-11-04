using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardFreezingZone : MonoBehaviour
{
    private PlayerManager player;
    private float originalSpeed;
    [SerializeField] private GameObject FreezingZone; 
    private float durationFreeze = 1.3f;
    
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            originalSpeed = player.speed;
        }
    }

    private IEnumerator OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            yield return new WaitForSeconds(0.2f);
            player.speed = 0;
            yield return new WaitForSeconds(durationFreeze);
            Destroy(gameObject);
            player.speed = originalSpeed;
        }
    }

}
