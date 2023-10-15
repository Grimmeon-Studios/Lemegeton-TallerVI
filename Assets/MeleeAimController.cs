using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private float combat_CDTimer;

    [SerializeField] private PlayerManager _playerManager;
    private float meleeCriticalDamage;
    private float meleeCritialRateUp;
    private float meleeDamage;
    private Vector2 currentAimDirection;

    bool isOnCd = false;
    private bool hasEnemyNearby = false;

    //[SerializeField] private FloatingJoystick Joystick;
    [SerializeField] public float radio = 3.0f;
    [SerializeField] public float transition = 3.0f;

    [Header("Button Config.")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;



    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isOnCd == true)
        {
            combat_CDTimer = combat_CDTimer + Time.deltaTime;
            buttonText.text = combat_CDTimer.ToString("F1");
        }

        meleeDamage = _playerManager.attack;
        meleeCriticalDamage = _playerManager.criticalDamage;
        meleeCritialRateUp = _playerManager.criticalRateUp;
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

        
        
        float randomValue = Random.Range(0f, 1f);
    
        // Comprueba si se produce un ataque crítico.
        if (randomValue < meleeCritialRateUp)//If critical damage
        {
            meleeDamage = CalculateCriticalDamage(_playerManager.attack,_playerManager.criticalDamage);
            Debug.Log("¡Ataque crítico! Daño total: " + meleeDamage);
        }
        else //If there's no critical damage
        {
            meleeDamage = _playerManager.attack;
            Debug.Log("Ataque normal. Daño total: " + meleeDamage);
        }
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
    private float CalculateCriticalDamage(float baseAttack, float criticalDmg)
    {
        float criticalDamage = baseAttack*2 + (baseAttack * criticalDmg);
        return criticalDamage;
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
        button.interactable = false;

        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // After waiting, execute the method
        isOnCd = false;
        button.interactable = true;
        combat_CDTimer = 0;
        buttonText.text = "Hit!";
    }

    public void UpdateLastAimDirection(Vector2 newDirection)
    {
        lastAimDirection = newDirection;
        Debug.Log("Updating lasAimPos");
    }

}
