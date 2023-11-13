using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{  
    [Header("Buttons")]
    [SerializeField] GameObject pauseMenu;
    
    [Header("Stats")]
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI maxHealthText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI maxDefenseText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI shotSpeedText;
    [SerializeField] TextMeshProUGUI shotRangeText;
    [SerializeField] TextMeshProUGUI shotDamageText;
    [SerializeField] TextMeshProUGUI criticalRateUpText;
    [SerializeField] TextMeshProUGUI criticalDamageText;
    [SerializeField] TextMeshProUGUI timeInvincibleText;
    private PlayerManager player;
    [SerializeField] GameObject joystick;

    public void Pause()
    {
        player = FindObjectOfType<PlayerManager>();
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        LoadStats();
        joystick.SetActive(false);
    }

    public void LoadStats()
    {
        UpdateStatsText();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        joystick.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HUB");
    }

    private void UpdateStatsText()
    {
        speedText.text = "Speed: " + player.GetSpeed();
        maxHealthText.text = "Max Health: " + player.GetMaxHealth();
        healthText.text = "Health: " + player.GetHealth();
        maxDefenseText.text = "Max Defense: " + player.GetMaxDefense();
        defenseText.text = "Defense: " + player.GetDefense();
        attackText.text = "Attack: " + player.GetAttack();
        shotSpeedText.text = "Shot Speed: " + player.GetShotSpeed();
        shotRangeText.text = "Shot Range: " + player.GetShotRange();
        shotDamageText.text = "Shot Damage: " + player.GetShotDamage();
        criticalRateUpText.text = "Critical Rate Up: " + (player.GetCriticalRateUp()*100) + "%";
        criticalDamageText.text = "Critical Damage: " + player.GetCriticalDamage();
        timeInvincibleText.text = "Time Invincible: " + player.GetTimeInvincible();
    }
    
}
