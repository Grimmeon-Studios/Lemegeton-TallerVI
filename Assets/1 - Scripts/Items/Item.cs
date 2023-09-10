using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    private int item_shotDamage;
    private float item_criticalRateUp;
    private float item_criticalDamage;
    private float item_timeInvincible;

    private bool playerIsNearby = false;
    private PlayerManager player;
    private Item thisItem;
    private HashSet<Item> itemsHash;

    void Awake()
    {
        int switchIndex = UnityEngine.Random.Range(0, activeItemsNumber+1);
        itemsHash = new HashSet<Item>();

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
            
        }
    }

    public void PickUpItem()
    {
        Debug.Log("Trying to pick Up");

        // Check if the player object is not null
        if (player != null)
        {
            player.speed += item_speed;
            player.maxHealth += item_maxHealth;
            player.health += item_health;
            player.maxDefense += item_maxDefense;
            player.defense += item_defense;
            player.attack += item_attack;
            player.shotDamage += item_shotDamage;
            player.shotSpeed += item_shotSpeed;
            player.shotRange += item_shotRange;
            player.criticalRateUp += item_criticalRateUp;
            player.criticalDamage += item_criticalDamage;
            player.timeInvincible += item_timeInvincible;

            DestroyAllInHashSet();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            Debug.Log("GameObject: " + collision.gameObject.name + " Entered");
            playerIsNearby = true;
            player = collision.gameObject.GetComponent<PlayerManager>();
        }
        else if (collision != null && collision.CompareTag("Item"))
        {
            Debug.Log("GameObject: " + collision.gameObject.name + " Entered");
            itemsHash.Add(collision.gameObject.GetComponent<Item>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            Debug.Log("GameObject: " + collision.gameObject.name + " Exited");
            playerIsNearby = false;
            player = null;
        }
        else if (collision != null && collision.CompareTag("Item"))
        {
            Debug.Log("GameObject: " + collision.gameObject.name + " Exited");
            itemsHash.Remove(collision.gameObject.GetComponent<Item>());
        }
    }

    public void DestroyAllInHashSet()
    {
        Debug.Log("Trying to destroy nearby items");
        if (itemsHash.Count > 0)
        {
            foreach (Item itm in itemsHash)
            {
                Debug.Log("Entered Foreach");

                GameObject obj = itm.gameObject;
                Debug.Log("Item: " + obj.name + " Will be Destroyed");
                Destroy(obj);
            }
        }
        else
            Debug.Log("No items in HashSet");
    }
}
