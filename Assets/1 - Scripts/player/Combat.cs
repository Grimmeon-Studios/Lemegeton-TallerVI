using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Melee Attack")]
    [SerializeField] private GameObject meleeController;
    [SerializeField] private SpriteRenderer meleeSprite;
    [SerializeField] private float meleeRadius;
    [SerializeField] private int meleeDamage;
    [SerializeField] private float combat_CD;

    bool isOnCd = false;

    //[SerializeField] private FloatingJoystick Joystick;
    [SerializeField] public float radio = 3.0f;

    private Transform playerTransform;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        //ChangeTargetPos(meleeController);
        transform.position = playerTransform.position;

    }

    public void Melee()
    {
        if (isOnCd == true)
            return;

        StartCoroutine(AnimationPlaceholder(0.2f));
    

        Collider2D[] radius = Physics2D.OverlapCircleAll(meleeController.transform.position, meleeRadius);

        foreach (Collider2D collision in radius)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.transform.GetComponent<IncubusScript>().takeDamage(meleeDamage);
            }

            if (collision.CompareTag("EnemySoul"))
            {
                collision.gameObject.GetComponent<LostSoulScript>().takeDamage(meleeDamage);
            }

            if (collision.CompareTag("EnemyAndras"))
            {
                collision.gameObject.GetComponent<AndrasScript>().takeDamage(meleeDamage);
            }
        }

        isOnCd = true;

        StartCoroutine(CombatCD(combat_CD));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(meleeController.transform.position, meleeRadius);
    }

    /*
    public Vector2 PositionOnCircle(Vector2 direction)
    {
        // Calculate a random angle in radians
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Calculate the position on the circle's circumference
        float x = gameObject.transform.position.x + radio * Mathf.Cos(randomAngle);
        float y = gameObject.transform.position.y + radio * Mathf.Sin(randomAngle);

        return new Vector2(x, y);
    }


    private void ChangeTargetPos(GameObject target)
    {
        Vector2 newPosInCircle = PositionOnCircle(rb.velocity);
        Vector3 newPos = new Vector3(newPosInCircle.x, newPosInCircle.y);
        target.transform.position = newPos;
    }

    */

    private IEnumerator AnimationPlaceholder(float waitTime)
    {
        meleeSprite.enabled = true;

        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        meleeSprite.enabled = false;
    }

    private IEnumerator CombatCD(float waitTime)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // After waiting, execute the method
        isOnCd = false;
    }
}
