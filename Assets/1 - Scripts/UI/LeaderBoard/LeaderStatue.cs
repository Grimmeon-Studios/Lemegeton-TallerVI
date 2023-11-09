using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderStatue : MonoBehaviour
{   
    [SerializeField] GameObject panel;
    [SerializeField] GameObject joystick;
    private PlayerManager player;
    private float originalSpeed;
    private void OnTriggerEnter2D(Collider2D other)
    {
        player = FindObjectOfType<PlayerManager>();
        originalSpeed = player.speed;
        if (other.gameObject.tag == "Player")
        {
            panel.SetActive (true);
            joystick.SetActive(false);
            player.speed = 0f;
        }

    }

    public void ClosePanel () 
    {
        panel.SetActive (false);
        player.speed = originalSpeed;
        joystick.SetActive(true);
    }
}
