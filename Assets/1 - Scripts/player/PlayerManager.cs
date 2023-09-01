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
    
    //private string levelname;

    [SerializeField] Camera _Camera;
    [SerializeField] private PlayerTouchMovement _playerTouchMovementScript;
    PlayerInput_map _Input;

    public Vector2 _Movement;
    public Vector2 _DampedSpeed;

    public Rigidbody2D _Rigidbody;

    private void Awake()
    {
        _Input = new PlayerInput_map();
        _Rigidbody = GetComponent<Rigidbody2D>();
        //levelname = SceneManager.GetActiveScene().name;
        health = maxHealth;
        defense = maxDefense;
    }
    private void OnEnable()
    {
        _Input.Enable();

        //_Input.Player.Fire Significa atacar pero por bugs del New Input System no puedo cambiarle el nombre
        //_Input.Player.Fire.performed += PerformAttack();

        _Input.Player.Move.performed += OnMovement;
        _Input.Player.Move.canceled += OnMovement;
    }

    private void OnDisable()
    {
        //_Input.Player.Fire.performed -= PerformAttack();

        //_Input.Player.Fire Significa atacar pero por bugs del New Input System no puedo cambiarle el nombre
        _Input.Player.Move.performed -= OnMovement;
        _Input.Player.Move.canceled -= OnMovement;

        _Input.Disable();
    }

    //private Action<InputAction.CallbackContext> PerformAttack()
    //{
    //    throw new NotImplementedException();
    //}
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
    /*public void pickingUp(int pickUps)
   {
       switch (pickUps)
       {
           case 1:
               
               break;
           
       }
       
   }*/
    private void OnMovement(InputAction.CallbackContext context)
    {
        _Movement = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if(_playerTouchMovementScript.joystickActive == false)
        {
            _DampedSpeed = Vector2.SmoothDamp(_DampedSpeed, _Movement, ref _DampedSpeed, 0.05f);
            _Rigidbody.velocity = _DampedSpeed * speed;
        }
    }
    public void TakeDamage(float amount)
    {
        if (isInvincible)
            return;

        isInvincible = true;
        invincibleTimer = timeInvincible;

        health -= amount;
        Debug.Log("Health" + health);
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
        //Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 60,
            normal = new GUIStyleState()
            {
                textColor = Color.green
            }
        };

        GUIStyle invincibleLabel = new GUIStyle()
        {
            fontSize = 60,
            normal = new GUIStyleState()
            {
                textColor = Color.yellow
            }
        };

        GUI.Label(new Rect(10, 350, 500, 20), $"Health: ({health})", labelStyle);

        if (isInvincible)
        {
            GUI.Label(new Rect(10, 280, 500, 20), $"Invincible: ({invincibleTimer})", invincibleLabel);
        }
    }
}
