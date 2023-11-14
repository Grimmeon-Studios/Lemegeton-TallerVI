using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpike : MonoBehaviour
{
    private PlayerManager player;
    private Dungeon dungeon;

    [SerializeField] private AudioSource SFXSpikes;
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        dungeon = FindObjectOfType<Dungeon>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SFXSpikes.Play();

            switch (dungeon.currentCircle)
            {
                case 1:
                    player.TakeDamage(1.5f);
                    break;
                case 2:
                    player.TakeDamage(2.5f);
                    break;
                case 3:
                    player.TakeDamage(4f);
                    break;
                default:
                    player.TakeDamage(1.5f);
                    break;
            }
        }
    }
}
