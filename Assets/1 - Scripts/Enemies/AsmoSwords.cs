using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Cinemachine.CinemachineFreeLook;

public class AsmoSwords : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float damage;
    [SerializeField] private float speed = 10f; // Speed of rotation

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Rotate the GameObject every frame
        transform.Rotate(0, 0, speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {      
        GameObject player = other.gameObject;
        PlayerManager pm = player.GetComponent<PlayerManager>();
        //Robin playerController = player.GetComponent<Robin>();

        if (pm != null)
        {
            //pm.changeHealth(-0.5f);
            pm.TakeDamage(damage);
        }

    }
}
