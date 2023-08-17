using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //Stats
    public float speed = 3;
    public float maxHealth = 10;
    public float health;
    public float maxDefense = 5;
    public float defense;
    public float attack = 3;
    public float criticalRateUp = 0.6f;
    public float criticalDamage = 1;
    public bool isInvincible = false;
    public float invincibleTimer;
    public float timeInvincible = 2.0f;
        
    private string levelname;
    
    //Arrays from pickUps
    
    
    
    [SerializeField] Camera _Camera;
    PlayerInput_map _Input;
    Vector2 _Movement;
    Vector2 _DampedSpeed;

    Rigidbody2D _Rigidbody;

    private void Start()
    {
        levelname = SceneManager.GetActiveScene().name;
        health = maxHealth;
        defense = maxDefense;
    }
    

    private void Awake()
    {
        _Input = new PlayerInput_map();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _Input.Enable();

        _Input.Player.Move.performed += OnMovement;
        _Input.Player.Move.canceled += OnMovement;
    }

    private void OnDisable()
    {
        _Input.Player.Move.performed -= OnMovement;
        _Input.Player.Move.canceled -= OnMovement;

        _Input.Disable();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _Movement = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;

            }
        }
    }

    private void FixedUpdate()
    {
        _DampedSpeed = Vector2.SmoothDamp(_DampedSpeed, _Movement, ref _DampedSpeed, 0.05f);

        _Rigidbody.velocity = _DampedSpeed * speed;

    }

    /*public void pickingUp(int pickUps)
    {
        switch (pickUps)
        {
            case 1:
                
                break;
            
        }
        
    }*/
    public void TakeDamage(int amount)
    {
        if (isInvincible)
            return;

        isInvincible = true;
        invincibleTimer = timeInvincible;

        health -= amount;
        if (health <= 0)
        {
            Death();
        }
        /*else
        {
            clipDamage.Play(); // feedback of the damage
        }*/
    }
    
    public void Death()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(levelname);
    }
    

}

