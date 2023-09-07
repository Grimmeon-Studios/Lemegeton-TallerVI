using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool colision = false;
    PlayerInput_map _Input;

    private bool pressingE = false;
    private void Start()
    {
        _Input = new PlayerInput_map();
        player = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collide enter");
        if (other.gameObject.CompareTag("Player")) // Layer Player num 8
        {
            Debug.Log("picking and making bool true");
            colision = true;
            PickingUp();
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        Debug.Log("collide exit");
        if (other.gameObject.CompareTag("Player")) // Layer Player num 8
        {
            Debug.Log("picking and making bool false");
            colision = false;
        }
    }

    public void OnPickingUp (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("LA E");
            PickingUp();
        }
    }
    public void OnPickingUpVirtual()
    {
        Debug.Log("se presiono");
        if (colision == true)
        {
            Debug.Log("el bool esta bueno");
            PickingUp();
        }

    }
    public void OnEnable()
    {
        _Input.Enable();
        _Input.Player.Interaction.performed += OnPickingUp;
    }

    private void OnDisable()
    {
        _Input.Disable();
        _Input.Player.Interaction.performed -= OnPickingUp;
    }*/
    
    void PickingUp()
    {
        Debug.Log("si dio");
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
