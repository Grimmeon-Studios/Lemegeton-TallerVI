using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float speed = 3;
    public float maxHealth = 10;
    public float health;
    public float maxDefense = 5;
    public float defense;
    public float attack = 3;
    public float shotSpeed = 1;
    public float shotRange = 10;
    public int shotDamage = 5;
    public float criticalRateUp = 0.6f;
    public float criticalDamage = 1;
    public bool isInvincible = false;
    public float invincibleTimer;
    public float timeInvincible = 2.0f;
    public int sulfur;

    //private string levelname;

    [SerializeField] Camera _Camera;
    [SerializeField] private PlayerTouchMovement _playerTouchMovementScript;
    PlayerInput_map _Input;

    [Header("Movement config.")]
    public Vector2 _Movement;
    public Vector2 _DampedSpeed;
    public Rigidbody2D _Rigidbody;

    [Header("Items and Statue attributes")]
    private bool itemNearBy;
    private Item _Item;
    private bool statueNearBy;
    private GameObject _Statue;
    [SerializeField] private GameObject selecctionMark;
    [SerializeField] private Vector2 selectionOffset;
    //private SulfurPickup _Sulfur;

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

        if(_Item != null)
        {
            selecctionMark.SetActive(true);
            selecctionMark.transform.position = _Item.transform.position + (Vector3)selectionOffset;
        }
        else if(_Item == null)
        {
            selecctionMark.SetActive(false);
            selecctionMark.transform.position = gameObject.transform.position;
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
        if (_playerTouchMovementScript.joystickActive == false)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Item"))
        {
            itemNearBy = true;
            _Item = collision.GetComponent<Item>();
        }
        else if (collision != null && collision.CompareTag("Statue"))
        {
            statueNearBy = true;
            _Statue = collision.gameObject;
            Debug.Log(_Statue + ": In range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Item"))
        {
            itemNearBy = false;
            _Item = null;
        }
        else if (collision != null && collision.CompareTag("Statue"))
        {
            statueNearBy = false;
            _Statue = null;
            Debug.Log(_Statue + ": Too Far");
        }
    }

    public void PickUp()
    {
        if (itemNearBy == true)
        {
            Debug.Log("Trying to pick Up");
            speed += _Item.item_speed;
            maxHealth += _Item.item_maxHealth;
            health += _Item.item_health;
            maxDefense += _Item.item_maxDefense;
            defense += _Item.item_defense;
            attack += _Item.item_attack;
            shotDamage += _Item.item_shotDamage;
            shotSpeed += _Item.item_shotSpeed;
            shotRange += _Item.item_shotRange;
            criticalRateUp += _Item.item_criticalRateUp;
            criticalDamage += _Item.item_criticalDamage;
            timeInvincible += _Item.item_timeInvincible;

        }

        if(itemNearBy == true)
        {
            Collider2D[] deleteItemsRadius = Physics2D.OverlapCircleAll(gameObject.transform.position, 10);
            if (deleteItemsRadius != null && deleteItemsRadius.Length > 0)
            {
                foreach (Collider2D collision in deleteItemsRadius)
                {
                    GameObject @object = collision.gameObject;
                    if (@object.CompareTag("Item"))
                    {
                        Debug.Log(@object.name + " has been detected and will be destoyed");
                        Destroy(@object);
                    }
                }
            }
            else
            {
                Debug.Log("No Items to pick Up");
            }
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null && collision.CompareTag("Sulfur"))
        {
            PlayerManager player = GetComponent<PlayerManager>();
            if (player != null)
            {
                player.AddSulfur(1);
                Destroy(collision.gameObject);
            }

        }
    }
    public void AddSulfur(int amount)
    {
        sulfur += amount;
    }

    public void ActivateStatue()
    {
        Debug.Log("Activate Statue attempt");
        Debug.Log("Is Statue near by?: " + statueNearBy + "Is statue Null?: " + _Statue);
        if (statueNearBy == true && _Statue != null)
        {
            Debug.Log("ActivateStatue called.");
            ItemStatueSpawner statueSpawner = _Statue.GetComponent<ItemStatueSpawner>();
            if (statueSpawner != null)
            {
                Debug.Log("statueSpawner found.");
                statueSpawner.DropItems();
            }
        }
    }


}
