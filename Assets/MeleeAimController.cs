using System.Collections;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float crosshairDistance = 2.0f; // Distance from the player where the crosshair appears
    private Rigidbody2D rb;

    private Vector2 lastAimDirection = Vector2.zero;

    [SerializeField] private SpriteRenderer meleeSprite;
    [SerializeField] private float meleeRadius;
    [SerializeField] private float combat_CD;

    [SerializeField] private PlayerManager _playerManager;

    private float meleeDamage;
    bool isOnCd = false;

    //[SerializeField] private FloatingJoystick Joystick;
    [SerializeField] public float radio = 3.0f;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        meleeDamage = _playerManager.attack;
        // Ensure the playerTransform is set (you can also assign it manually in the Inspector)
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not set for the Crosshair.");
            return;
        }

        // Get the player's current movement direction (from your PlayerManager or Rigidbody)
        Vector2 currentAimDirection = rb.velocity;

        // Only update the crosshair if the aim direction has changed
        if (currentAimDirection != lastAimDirection)
        {
            lastAimDirection = currentAimDirection;

            // Position the crosshair object based on the player's aim
            Vector2 playerPosition = playerTransform.position;

            if (currentAimDirection.magnitude > 0.01f)
            {
                Vector2 crosshairPosition = playerPosition + currentAimDirection.normalized * crosshairDistance;
                transform.position = crosshairPosition;
            }
        }
    }

    public void Melee()
    {
        if (isOnCd == true)
            return;

        StartCoroutine(AnimationPlaceholder(0.2f));


        Collider2D[] radius = Physics2D.OverlapCircleAll(gameObject.transform.position, meleeRadius);

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
        Gizmos.DrawWireSphere(gameObject.transform.position, meleeRadius);
    }

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
