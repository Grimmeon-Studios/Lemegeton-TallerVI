using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    private PlayerManager player;
    private ScoreBoard scoreboard;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        scoreboard = FindObjectOfType<ScoreBoard>();
    }

    public void SaveGame()
    {
        Debug.Log("Saving Stats");
        //Get all of the variables that want to be save  (We have to create a function to gets the saved variables)
        float playerSpeed = player.GetSpeed();
        float playerMaxHealth = player.GetMaxHealth();
        float playerHealth = player.GetHealth();
        float playerMaxDefense = player.GetMaxDefense();
        float playerDefense = player.GetDefense();
        float playerAttack = player.GetAttack();
        float playerShotSpeed = player.GetShotSpeed();
        float playerShotRange = player.GetShotRange();
        int playerShotDamage = player.GetShotDamage();
        float playerCriticalRateUp = player.GetCriticalRateUp();
        float playerCriticalDamage = player.GetCriticalDamage();
        float playerTimeInvincible = player.GetTimeInvincible();
        
        
        //Set all of them into Prefs
        PlayerPrefs.SetFloat("speed", playerSpeed);
        PlayerPrefs.SetFloat("maxHealth", playerMaxHealth);
        PlayerPrefs.SetFloat("health", playerHealth);
        PlayerPrefs.SetFloat("maxDefense", playerMaxDefense);
        PlayerPrefs.SetFloat("defense", playerDefense);
        PlayerPrefs.SetFloat("attack", playerAttack);
        PlayerPrefs.SetFloat("shotSpeed", playerShotSpeed);
        PlayerPrefs.SetFloat("shotRange", playerShotRange);
        PlayerPrefs.SetInt("shotDamage", playerShotDamage);
        PlayerPrefs.SetFloat("criticalRateUp", playerCriticalRateUp);
        PlayerPrefs.SetFloat("criticalDamage", playerCriticalDamage);
        PlayerPrefs.SetFloat("timeInvincible", playerTimeInvincible);
        
    }

    public void LoadGame()
    {
        Debug.Log("Loading Stats");
        //Select the stats :D
        float playerSpeed = PlayerPrefs.GetFloat("speed");
        float playerMaxHealth = PlayerPrefs.GetFloat("maxHealth");
        float playerHealth = PlayerPrefs.GetFloat("health");
        float playerMaxDefense = PlayerPrefs.GetFloat("maxDefense");
        float playerDefense = PlayerPrefs.GetFloat("defense");
        float playerAttack = PlayerPrefs.GetFloat("attack");
        float playerShotSpeed = PlayerPrefs.GetFloat("shotSpeed");
        float playerShotRange = PlayerPrefs.GetFloat("shotRange");
        int playerShotDamage = PlayerPrefs.GetInt("shotDamage");
        float playerCriticalRateUp = PlayerPrefs.GetFloat("criticalRateUp");
        float playerCriticalDamage = PlayerPrefs.GetFloat("criticalDamage");
        float playerTimeInvincible = PlayerPrefs.GetFloat("timeInvincible");
        
        //Set Stats     (We have to create a function to set the saved variables)
        player.SetStats(playerSpeed,playerMaxHealth,playerHealth,playerMaxDefense,playerDefense,playerAttack,playerShotSpeed,playerShotRange, playerShotDamage,playerCriticalDamage,playerCriticalRateUp,playerTimeInvincible);
        
    }

    
}
