using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HazardFreezingZone : MonoBehaviour
{
    private PlayerManager player;
    private float originalSpeed;
    [SerializeField] private GameObject FreezingZone; 
    private float durationFreeze = 1.3f;
    private Button dashBttn;
    
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        dashBttn = player.gameObject.GetComponent<Dash>().dashButton;
    }
    
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            dashBttn.interactable = false;
            if (player.GetComponent<Dash>().GetUsingDash())
            {
                originalSpeed = player.GetComponent<Dash>().GetOriginalSpeed();
            }
            else
            {
                originalSpeed = player.speed;
            }
            yield return new WaitForSeconds(0.2f);
            player.speed = 0;
            yield return new WaitForSeconds(durationFreeze);
            Destroy(gameObject);
            player.speed = originalSpeed;
        }
    }

}
