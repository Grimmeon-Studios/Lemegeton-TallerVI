using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Melee Atack")]
    [SerializeField] private Transform meleeController;
    [SerializeField] private float meleeRadius;
    [SerializeField] private int meleeDamage;


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Melee();
        }
    }
    private void Melee()
    {
        Collider2D[] radius = Physics2D.OverlapCircleAll(meleeController.position, meleeRadius);

        foreach (Collider2D collision in radius)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.transform.GetComponent<IncubusScript>().takeDamage(meleeDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(meleeController.position, meleeRadius);
    }
}
