using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Cinemachine.CinemachineFreeLook;

public class AndrasBullet : MonoBehaviour
{
    Rigidbody2D rb;
    float damage;
    GameObject player;
    Vector2 initPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("_Player");

        initPos = this.gameObject.transform.position;

        //LostSoulScript lsScript = this.GetComponentInParent<LostSoulScript>();
        //damage = lsScript.shotDamage;
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }

        Vector2 aimDirection = player.GetComponent<Rigidbody2D>().position - (Vector2)initPos;

        aimDirection.Normalize();

        rb.AddForce(aimDirection * 40f, ForceMode2D.Force);
    }

    public void shoot(Vector2 direction, float force, float shotDamage)
    {
        damage = shotDamage;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
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

        Destroy(gameObject);
    }
}
