using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardLavaFloor : MonoBehaviour
{
    private PlayerManager player;
    [SerializeField] private AudioSource SFXFire;

    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SFXFire.Play();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("collideLAVA");
        if (collision.gameObject.tag == "Player")
        {
            //SFXFire.Play();
           //Debug.Log("playercollideLAVA");
            player.HazardLava();

        }
    }
   
}
