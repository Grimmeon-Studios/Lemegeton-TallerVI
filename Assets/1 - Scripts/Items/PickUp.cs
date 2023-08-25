using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private int pickUp;
    [SerializeField] private float sumPorcentaje;
    public PlayerManager player;
    
    //An instance of the ScriptableObjects
    public Velocity velocity;
    public Attack attack;
    public DefenseUp defenseUp;
    public DefenseHeal defenseHeal;
    public CriticalDamage critDamage;
    public CriticalRateUp critRate;
    public Health health;
    public MaxHealth maxHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) // Layer Player num 7
        {
            PickingUp();
        }
    }

    void PickingUp()
    {
        switch (pickUp)
        {
            case 1://Velocity
                player.speed += velocity.porcentaje + sumPorcentaje;
                break;
            case 2: //Attack
                player.attack += attack.porcentaje + sumPorcentaje;
                break;
            case 3://Defense Up
                player.maxDefense += defenseUp.porcentaje + sumPorcentaje;
                break;
            case 4://Defense Heal
                player.defense += defenseHeal.porcentaje + sumPorcentaje;
                break;
            case 5://Critical Damage
                player.criticalDamage += critDamage.porcentaje + sumPorcentaje;
                break;
            case 6://Critical Rate Up
                player.criticalRateUp += critRate.porcentaje + sumPorcentaje;
                break;
            case 7://Health
                if (player.health < player.maxHealth)
                {
                    if ((health.porcentaje + sumPorcentaje + player.health) <= player.maxHealth)
                    {
                        player.health += health.porcentaje + sumPorcentaje;
                    }
                }
                break;
            case 8://MaxHealth
                player.maxHealth += maxHealth.porcentaje + sumPorcentaje;
                break;
            default:
                Console.WriteLine("PickUp no instanciado");
                break;
            
        }
        Destroy(gameObject);
    }
}
