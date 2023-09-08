using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Melee Attack")]
    [SerializeField] private Transform meleeController;
    [SerializeField] private SpriteRenderer meleeSprite;
    [SerializeField] private float meleeRadius;
    [SerializeField] private int meleeDamage;


    /*private void DoAttack()
    {
        if (Input.GetButtonSpace("Fire1"))
        {
            Melee();
        }
    }*/
    public void Melee()
    {
        StartCoroutine(AnimationPlaceholder(0.2f));
    

        Collider2D[] radius = Physics2D.OverlapCircleAll(meleeController.position, meleeRadius);

        foreach (Collider2D collision in radius)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.transform.GetComponent<IncubusScript>().takeDamage(meleeDamage);
            }

            if (collision.CompareTag("EnemySoul"))
            {
                //collision.gameObject.GetComponent<LostSoulScript>().takeDamage(player.shotDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(meleeController.position, meleeRadius);
    }

    private IEnumerator AnimationPlaceholder(float waitTime)
    {
        Debug.Log("Coroutine started");
        meleeSprite.enabled = true;

        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);


        Debug.Log("Coroutine ended");
        meleeSprite.enabled = false;
    }
}
