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
    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        UpdateStatsText();
        Time.timeScale = 0f;
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

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    private void UpdateStatsText()
    {
        speedText.text = "Speed: " + player.speed;
        maxHealthText.text = "Max Health: " + player.maxHealth;
        healthText.text = "Health: " + player.health;
        maxDefenseText.text = "Max Defense: " + player.maxDefense;
        defenseText.text = "Defense: " + player.defense;
        attackText.text = "Attack: " + player.attack;
        shotSpeedText.text = "Shot Speed: " + player.shotSpeed;
        shotRangeText.text = "Shot Range: " + player.shotRange;
        shotDamageText.text = "Shot Damage: " + player.shotDamage;
        criticalRateUpText.text = "Critical Rate Up: " + (player.criticalRateUp * 100);
        criticalDamageText.text = "Critical Damage: " + player.criticalDamage;
        timeInvincibleText.text = "Time Invincible: " + player.timeInvincible;
    }
    
}
