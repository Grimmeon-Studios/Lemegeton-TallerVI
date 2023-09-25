using System.Collections;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float crosshairDistance = 2.0f; // Distance from the player where the crosshair appears
    private Rigidbody2D rb;

    public Vector2 lastAimDirection = Vector2.zero;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private SpriteRenderer meleeSprite;
    [SerializeField] private float meleeRadius;
    [SerializeField] private float targetLockRadius;
    [SerializeField] private float combat_CD;

    [SerializeField] private PlayerManager _playerManager;

    private float meleeDamage;
    private Vector2 currentAimDirection;

    bool isOnCd = false;
    private bool hasEnemyNearby = false;

    //[SerializeField] private FloatingJoystick Joystick;
    [SerializeField] public float radio = 3.0f;
    [SerializeField] public float transition = 3.0f;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        meleeDamage = _playerManager.attack;
        Vector2 closestEnemyDir = GetDirectionToClosestEnemy();

        // Ensure the playerTransform is set (you can also assign it manually in the Inspector)
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not set for the Crosshair.");
            return;
        }

        // Use the player's velocity directly for movement
        Vector2 playerVelocity = rb.velocity.normalized*1.2f;

        if (closestEnemyDir != Vector2.zero)
        {
            // Apply smoothing only when enemies are nearby
            float smoothFactor = 3.0f; // You can adjust this value as needed

            // Smoothing for position
            Vector2 playerPosition = playerTransform.position;
            Vector2 targetPosition = playerPosition + closestEnemyDir.normalized * crosshairDistance;
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothFactor);

            // Smoothing for rotation
            float angle = Mathf.Atan2(closestEnemyDir.y, closestEnemyDir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothFactor);

            currentAimDirection = closestEnemyDir;
        }
        else
        {
            // No enemies nearby, use player's velocity for movement
            currentAimDirection = playerVelocity;

            if(currentAimDirection != lastAimDirection)
            {
                lastAimDirection = currentAimDirection;

                // Position the crosshair object based on the player's aim (no smoothing)
                Vector2 playerPosition = playerTransform.position;
                Vector2 crosshairPosition = playerPosition + playerVelocity * crosshairDistance;
                transform.position = crosshairPosition;

                // Rotate the crosshair object based on the player's aim (no smoothing)
                float angle = Mathf.Atan2(playerVelocity.y, playerVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
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

    public Vector2 GetDirectionToClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_playerManager.gameObject.transform.position, targetLockRadius, enemyLayers);
        Vector2 closestEnemyDirection = Vector2.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            GameObject enemy = collider.gameObject;
            Vector2 directionToEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            // Add a minimum distance threshold to avoid rapid switching
            if (distanceToEnemy < closestDistance && distanceToEnemy > 0.1f)
            {
                closestDistance = distanceToEnemy;
                closestEnemyDirection = directionToEnemy.normalized;
            }

        }

        return closestEnemyDirection;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, meleeRadius);
        Gizmos.DrawWireSphere(_playerManager.gameObject.transform.position, targetLockRadius);
    }

    private IEnumerator AnimationPlaceholder(float waitTime)
    {
        //meleeSprite.enabled = true;

        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        //meleeSprite.enabled = false;
    }

    private IEnumerator CombatCD(float waitTime)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // After waiting, execute the method
        isOnCd = false;
    }

    public void UpdateLastAimDirection(Vector2 newDirection)
    {
        lastAimDirection = newDirection;
        Debug.Log("Updating lasAimPos");
    }

}
