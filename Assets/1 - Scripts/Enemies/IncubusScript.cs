using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum incubState
{
    Chasing,
    Attacking,
    Dead
};

public class IncubusScript : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform incubusTransform;
    public Vector3 defaultStats; // hp, attack, speed
    private PlayerManager playerManager;
        
    #region MOVE RELATED

    [SerializeField] float moveSpeed;
    Vector2 position;

    #endregion

    #region ATTACK RELATED

    [SerializeField] float contactDamage;

    [SerializeField] int pushForce;
    private bool isAttacking = false;
    private float attackCooldown = 1.5f; // Tiempo de espera en segundos
    private float attackDuration = 1f;
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private SpriteRenderer attackArea;
    [SerializeField] float Distance = 8.0f;
    private Vector2 currentAimDirection;
        #endregion

    #region AI RELATED
    GameObject player;
    public incubState currState = incubState.Chasing;
    #endregion

    #region ELSE
    [SerializeField] private float health;
    public float Health { get => health; set => health = value; }
    public float ContactDamage { get => contactDamage; set => contactDamage = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    // GameObject door;
    #endregion

    #region Animation
    private Animator animator;
    
    [SerializeField] Sprite[] spritesFire;
    #endregion
    //#region DROPS RELATED
    //public GameObject drop1Prefab;
    //public GameObject drop2Prefab;
    //#endregion
    public SpriteRenderer incubus;
    
    
    void Start()
    {
        player = GameObject.Find("Player"); 
        playerManager = FindObjectOfType<PlayerManager>();
        // door = GameObject.Find("Door");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // LM = door.GetComponent<LevelManagement>();
        attackArea.gameObject.SetActive(false);
    }

    void Update()
    {
        position = rb.position;
        Vector2 v2 = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 Look = v2 - position;
        Look.Normalize();
        // Debug.Log($"X: {Look.x} Y: {Look.y}");
        if (Vector2.Distance(position, player.transform.position) <= attackRange && !isAttacking)
        {
            currState = incubState.Attacking;
        }
    }

    void FixedUpdate()
    {

        switch (currState)
        {
            case (incubState.Chasing):
                Chase();
                break;
            case (incubState.Dead):
                Die();
                break;
            case (incubState.Attacking):
                StartCoroutine(Attack());
                break;
        }
        Vector2 playerDir = player.transform.position - transform.position;
        if (playerDir != Vector2.zero)
        {
            // Apply smoothing only when enemies are nearby
            float smoothFactor = 3.0f; // You can adjust this value as needed

            // Smoothing for position
            Vector2 incubusPosition = incubusTransform.position;
            Vector2 targetPosition = incubusPosition + playerDir.normalized * Distance;
            attackArea.transform.position = Vector2.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothFactor);

            // Smoothing for rotation
            float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);
            attackArea.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothFactor);

            currentAimDirection = playerDir;
        }
    }

    void Chase()
    {
        attackArea.sprite = spritesFire[0];
        attackArea.gameObject.SetActive(false);
        rb.MovePosition(Vector2.MoveTowards(rb.position, player.transform.position, MoveSpeed));
        if (Vector2.Distance(position, player.transform.position) <= attackRange && !isAttacking)
        {
            currState = incubState.Attacking;
        }
    }
    private IEnumerator Attack()
    {
        animator.SetBool("IsAboutAttack", true);
        isAttacking = true;
        attackArea.gameObject.SetActive(true);
        attackArea.sprite = spritesFire[0];
        // Espera el tiempo de espera
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("IsAboutAttack", false);
        while (Vector2.Distance(position, player.transform.position) <= attackRange)
        {
            animator.SetBool("IsAttackingAn", true);
            attackArea.sprite = spritesFire[1];
            playerManager.TakeDamage(ContactDamage);
            playerManager._Rigidbody.AddForce((playerManager.transform.position - this.transform.position) * pushForce);
            //animation
            yield return new WaitForSeconds(attackCooldown);
            animator.SetBool("IsAttackingAn", false);
            
        }
        attackArea.sprite = spritesFire[0];
        attackArea.gameObject.SetActive(false);
        // Comprueba nuevamente si el jugador todavía está en el rango
        isAttacking = false;
        if (Vector2.Distance(position, player.transform.position) <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else
        {
            currState = incubState.Chasing;
        }
    }
    public void takeDamage(float damage)
    {
        Health -= damage;
        
        if (Health > 0)
        {
            incubus.DOColor(new Color(0.4622642f,0.4622642f,0.4622642f), 0.2f).OnComplete(() =>
            {
               incubus.DOColor(Color.white, 0.1f);
            });
            DOTween.Kill(transform);
        }
        else
        {
            currState = incubState.Dead;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        GameObject player = other.gameObject;

        PlayerManager playerManager = player.GetComponent<PlayerManager>();

        if (playerManager != null)
        {
            playerManager.TakeDamage(ContactDamage);
            playerManager._Rigidbody.AddForce((playerManager.transform.position - this.transform.position) * pushForce);
        }
    }

    void Die()
    {
        int opcDrop;
        for (int i = 1; i <= Random.Range(1, 3); i++)
        {
            opcDrop = Random.Range(1, 3);

            if (opcDrop == 1)
            {
                Vector2 objectPos = transform.position;
                objectPos.x += Random.Range(-1f, 1f);
                objectPos.y += Random.Range(-1f, 1f);
                // GameObject dropObject = Instantiate(drop1Prefab, objectPos, Quaternion.identity);
            }
            else if (opcDrop == 2)
            {
                Vector2 objectPos = transform.position;
                objectPos.x += Random.Range(-1f, 1f);
                objectPos.y += Random.Range(-1f, 1f);
                // GameObject dropObject = Instantiate(drop2Prefab, objectPos, Quaternion.identity);
            }
        }
        // LM.enemyCount -= 1;
        Destroy(gameObject);
    }
}
