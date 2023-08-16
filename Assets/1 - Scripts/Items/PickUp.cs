using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private int pickUps;
    [SerializeField] private float porcentaje;
    private PlayerManager player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7) // Layer Player num 7
        {
            PickingUp();
        }
    }

    void PickingUp()
    {
        switch (pickUps)
        {
            case 1://Velocity
                player._Speed += porcentaje;
                break;
            case 2: //Attack
                player.attack += porcentaje;
                break;
            case 3://Defense
                player.defense += porcentaje;
                break;
            case 4://Critical
                player.critical += porcentaje;
                break;
            case 5://Health
                player.health += porcentaje;
                break;
            default:
                Console.WriteLine("PickUp no instanciado");
                break;
            
        }
        Destroy(gameObject);
    }
}
