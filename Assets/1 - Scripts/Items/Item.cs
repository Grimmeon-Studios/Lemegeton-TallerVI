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

    public List<Sprite> spriteTexturesList;
    //public ItemsTextures textures;
    public SpriteRenderer sprite;

    public string descriptionText;

    private void Start()
    {
        spriteTexturesList = new List<Sprite>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        /*for (int i = 0; i < textures.spriteList.Length; i++)
        {
            spriteTexturesList[i] = textures.spriteList[i];
        }*/
        int switchIndex = UnityEngine.Random.Range(0, activeItemsNumber+1);
        
        if (spriteTexturesList.Count <= 12)
        {
            switch (switchIndex)
            {
                case 0: // Movement Speed
                    item_speed = 0.2f;
                    descriptionText = "Increases the player speed ("+item_speed+")";
                    sprite.sprite = spriteTexturesList[0];
                    break;

                case 1: // Maximun Health
                    item_maxHealth = 0.5f;
                    descriptionText = "Increases the player maximum health (" + item_maxHealth + ")";
                    sprite.sprite = spriteTexturesList[1];
                    break;

                case 2: // Health Regeneration
                    item_health = 1;
                    descriptionText = "Heals the player by ("+item_health+")";
                    sprite.sprite = spriteTexturesList[2];
                    break;

                case 3: // Maximun Defence
                    item_maxDefense = 0.5f;
                    descriptionText = "Raises the player maximum defence by ("+item_maxDefense +")";
                    sprite.sprite = spriteTexturesList[3];
                    break;

                case 4: // Defence Regeneration
                    item_defense = 5;
                    descriptionText = "Increases the player defence regeneration speed by (" + item_defense + ")";
                    sprite.sprite = spriteTexturesList[4];
                    break;

                case 5: // Player Melee Damage
                    item_attack = 2;
                    descriptionText = "Increases the player melee damage (" + item_attack + ")";
                    sprite.sprite = spriteTexturesList[5];
                    break;

                case 6: // Player Shot Damage
                    item_shotDamage = 3;
                    descriptionText = "Increases the player Shot damage (" + item_shotDamage + ")";
                    sprite.sprite = spriteTexturesList[6];
                    break;

                case 7: // Player Shot Speed
                    item_shotSpeed = 0.6f;
                    descriptionText = "Increases the Shot speed (" + item_shotSpeed + ")";
                    sprite.sprite = spriteTexturesList[7];
                    break;

                case 8: // Player Shot Range
                    item_shotRange = 1.5f;
                    descriptionText = "Increases the Shot range (" + item_shotRange + ")";

                    sprite.sprite = spriteTexturesList[8];
                    break;

                case 9: // Critical Damage Probability
                    item_criticalRateUp = 2;
                    descriptionText = "Increases the chances to deal a critical hit (" + item_criticalRateUp + ")";
                    sprite.sprite = spriteTexturesList[9];
                    break;

                case 10: // Critical Damage
                    item_criticalDamage = 3;
                    descriptionText = "Increases the damage of a critical hit (" + item_criticalDamage + ")";
                    sprite.sprite = spriteTexturesList[10];
                    break;

                case 11: // Invencibility Time after being hit
                    item_timeInvincible = 0.2f;
                    descriptionText = "Increases the invulnerablility time after getting hit (" + item_timeInvincible + ")";
                    sprite.sprite = spriteTexturesList[11];
                    break;
            }
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
