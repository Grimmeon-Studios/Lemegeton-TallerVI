using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Player Stats")]
    public int activeItemsNumber;

    public float item_speed;
    public float item_maxHealth;
    public float item_health;
    public float item_maxDefense;
    public float item_defense;
    public float item_attack;
    public float item_shotSpeed;
    public float item_shotRange;
    public int item_shotDamage;
    public float item_criticalRateUp;
    public float item_criticalDamage;
    public float item_timeInvincible;


    void Awake()
    {
        int switchIndex = UnityEngine.Random.Range(0, activeItemsNumber+1);

        switch (switchIndex)
        {
            case 0: // Movement Speed
                item_speed = 0.2f;
                break;

            case 1: // Maximun Health
                item_maxHealth = 0.5f;
                break;

            case 2: // Health Regeneration
                item_health = 1;
                break;

            case 3: // Maximun Defence
                item_maxDefense = 0.5f;
                break;

            case 4: // Defence Regeneration
                item_defense = 5;
                break;

            case 5: // Player Melee Damage
                item_attack = 2;
                break;

            case 6: // Player Shot Damage
                item_shotDamage = 3;
                break;

            case 7: // Player Shot Speed
                item_shotSpeed = 0.6f;
                break;

            case 8: // Player Shot Range
                item_shotRange = 1.5f;
                break;

            case 9: // Critical Damage Probability
                item_criticalRateUp = 2;
                break;

            case 10: // Critical Damage
                item_criticalDamage = 3;
                break;

            case 11: // Invencibility Time after being hit
                item_timeInvincible = 0.2f;
                break;
        }
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision != null && collision.gameObject.name == "_Player")
    //    {
    //        Debug.Log("GameObject: " + collision.gameObject.name + " Entered");
    //        playerIsNearby = true;
    //        player = collision.gameObject.GetComponent<PlayerManager>();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision != null && collision.CompareTag("Player"))
    //    {
    //        Debug.Log("GameObject: " + collision.gameObject.name + " Exited");
    //        playerIsNearby = false;
    //        player = null;
    //    }
    //}


}
