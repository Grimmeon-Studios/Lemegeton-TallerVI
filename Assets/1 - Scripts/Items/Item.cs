using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Player Stats")]
    public int activeItemsNumber;

    private float item_speed;
    private float item_maxHealth;
    private float item_health;
    private float item_maxDefense;
    private float item_defense;
    private float item_attack;
    private float item_shotSpeed;
    private float item_shotRange;
    private float item_shotDamage;
    private float item_criticalRateUp;
    private float item_criticalDamage;
    private float item_timeInvincible;

    private bool playerIsNearby = false;
    private PlayerManager player;
    private HashSet<Item> items;

    void Awake()
    {
        int switchIndex = UnityEngine.Random.Range(0, activeItemsNumber);

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

    void Update()
    {
        if (playerIsNearby)
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        player.speed = player.speed + item_speed;
        player.maxHealth = player.maxHealth + item_maxHealth;
        player.health = player.health+ item_health;
        player.maxDefense = player.maxDefense + item_maxDefense;
        player.defense = player.defense + item_defense;
        player.attack = player.attack + item_attack;
        player.shotDamage = player.shotDamage + item_shotDamage;
        player.shotSpeed = player.shotSpeed + item_shotSpeed;
        player.shotRange = player.shotRange + item_shotRange;
        player.criticalRateUp = player.criticalRateUp + item_criticalRateUp;
        player.criticalDamage = player.criticalDamage + item_criticalDamage;
        player.timeInvincible = player.timeInvincible + item_timeInvincible;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            playerIsNearby = true;
            player = collision.gameObject.GetComponent<PlayerManager>();
        }
        else if(collision != null && collision.CompareTag("Item"))
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            playerIsNearby = false;
            player = null;
        }
    }



}
