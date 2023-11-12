using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class HazardSlowMist : MonoBehaviour
{
    private PlayerManager player;
    private float originalSpeed;
    private float slowedSpeed;
    private bool isSlowed = false;

    public float slowFactor = 0.5f; 
    public float slowDuration = 3f;
    public ParticleSystem VFXSlowed;
    GameObject VFXObj;
    private Vector3 yOffset = new Vector3(0, 1.8f);

    private float slowDurationSet;
    private Button dashBttn;


    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        dashBttn = player.gameObject.GetComponent<Dash>().dashButton;
        slowDurationSet = slowDuration;
        VFXObj = VFXSlowed.gameObject;
    }

    private void Update()
    {
        if (isSlowed)
        {
            VFXObj.transform.position = player.gameObject.transform.position + yOffset;

            slowDuration -= Time.deltaTime;
            if (slowDuration <= 0)
            {
                // Restaurar la velocidad original
                player.speed = originalSpeed;
                isSlowed = false;
                slowDuration = slowDurationSet;
                dashBttn.interactable = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            dashBttn.interactable = false;
            // Ralentizar al jugador
            if (isSlowed)
            {
                slowedSpeed = originalSpeed * slowFactor;
                player.speed = slowedSpeed;
                isSlowed = true;
                
            }
            else
            {
                if (player.GetComponent<Dash>().GetUsingDash())
                {
                    originalSpeed = player.GetComponent<Dash>().GetOriginalSpeed();
                    slowedSpeed = originalSpeed * slowFactor;
                    player.speed = slowedSpeed;
                    isSlowed = true;
                }
                else
                {
                    originalSpeed = player.speed;
                    slowedSpeed = originalSpeed * slowFactor;
                    player.speed = slowedSpeed;
                    isSlowed = true;
                }
                
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VFXSlowed.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DeactivateVFX());
        }
    }

    IEnumerator DeactivateVFX()
    {
        VFXSlowed.Stop();
        yield return new WaitForSeconds(0.7f);

        VFXObj.transform.position = Vector3.zero;
    }
}
