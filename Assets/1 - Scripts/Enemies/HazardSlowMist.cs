using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardSlowMist : MonoBehaviour
{
    private PlayerManager player;
    private float originalSpeed;
    private float slowedSpeed;
    private bool isSlowed = false;

    public float slowFactor = 0.5f; 
    public float slowDuration = 3f;
    private float slowDurationSet;
    private Button dashBttn;
    
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        dashBttn = player.gameObject.GetComponent<Dash>().dashButton;
        slowDurationSet = slowDuration;
    }

    private void Update()
    {
        if (isSlowed)
        {
            slowDuration -= Time.deltaTime;
            if (slowDuration <= 0)
            {
                // Restaurar la velocidad original
                player.speed = originalSpeed;
                isSlowed = false;
                slowDuration = slowDurationSet;
                dashBttn.interactable = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dashBttn.interactable = false;
            // Ralentizar al jugador
            if (isSlowed)
            {
                slowedSpeed = originalSpeed * slowFactor;
                player.speed = slowedSpeed;
                isSlowed = true;
            }
            else
            {
                if (player.GetComponent<Dash>().GetUsingDash())
                {
                    originalSpeed = player.GetComponent<Dash>().GetOriginalSpeed();
                    slowedSpeed = originalSpeed * slowFactor;
                    player.speed = slowedSpeed;
                    isSlowed = true;
                }
                else
                {
                    originalSpeed = player.speed;
                    slowedSpeed = originalSpeed * slowFactor;
                    player.speed = slowedSpeed;
                    isSlowed = true;
                }
                
            }
            
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Restaurar la velocidad original cuando el jugador sale de la zona
            player.speed = originalSpeed;
            isSlowed = false;
        }
    }*/
}
