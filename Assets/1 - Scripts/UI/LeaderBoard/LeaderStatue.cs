using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderStatue : MonoBehaviour
{   
    [SerializeField] GameObject panel;
    [SerializeField] GameObject joystick;
    [SerializeField] GameObject mainMenu;
    [SerializeField] private GameObject panel2;
    [SerializeField] private AudioSource SFXClick;
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

    public void OpenPanelMainMenu()
    {
        SFXClick.Play();
        panel2.SetActive (true);
        mainMenu.SetActive(false);
    }

    public void ClosePlanelMainMenu()
    {
        SFXClick.Play();
        panel2.SetActive (false);
        mainMenu.SetActive(true);
    }
    public void ClosePanel () 
    {
        SFXClick.Play();
        panel.SetActive (false);
        player.speed = originalSpeed;
        joystick.SetActive(true);
    }
}
