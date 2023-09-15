using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum incubState
{
    Chasing,
    Dead
};

public class IncubusScript : MonoBehaviour
{
    Rigidbody2D rb;

    public Vector3 defaultStats; // hp, attack, speed

    #region MOVE RELATED
    [SerializeField] float moveSpeed;
    Vector2 position;
    #endregion

    #region ATTACK RELATED
    [SerializeField] float contactDamage;

    [SerializeField]
    int pushForce;
    #endregion

    #region AI RELATED
    GameObject player;
    public incubState currState = incubState.Chasing;
    #endregion

    #region DROPS RELATED
    public GameObject sulfurPrefab;
    //public GameObject drop2Prefab;
    #endregion

    #region ELSE
    [SerializeField] private float health;
    public float Health { get => health; set => health = value; }
    public float ContactDamage { get => contactDamage; set => contactDamage = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    // GameObject door;
    #endregion




    void Start()
    {
        player = GameObject.Find("Player");
        // door = GameObject.Find("Door");
        rb = GetComponent<Rigidbody2D>();
        // LM = door.GetComponent<LevelManagement>();
    }

    void Update()
    {
        position = rb.position;
        Vector2 v2 = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 Look = v2 - position;
        Look.Normalize();
        // Debug.Log($"X: {Look.x} Y: {Look.y}");
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
        }
    }

    void Chase()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, player.transform.position, MoveSpeed));
    }

    public void takeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
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
                GameObject dropObject = Instantiate(sulfurPrefab, objectPos, Quaternion.identity);
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
