using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardLavaFloor : MonoBehaviour
{
    private PlayerManager player;

    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("collideLAVA");
        if (collision.gameObject.tag == "Player")
        {
           //Debug.Log("playercollideLAVA");
            player.HazardLava();

        }
    }
   
}
