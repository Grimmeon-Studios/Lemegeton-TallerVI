using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    public float criticalDamage = 0.25f;
    public bool isInvincible = false;
    public float invincibleTimer;
    public float timeInvincible = 2.0f;

    //private string levelname;

    [SerializeField] Camera _Camera;
    [SerializeField] private PlayerTouchMovement _playerTouchMovementScript;
    [SerializeField] private AudioSource SFXHit;
    //PlayerInput_map _Input;

    [Header("Movement config.")]
    public Vector2 _Movement;
    public Vector2 _DampedSpeed;
    public Rigidbody2D _Rigidbody;
    public Animator animator;
    
    [Header("Items and Statue attributes")]
    public Stack<Item> itemsHeld = new Stack<Item>();
    private bool itemNearBy;
    private Item _Item;
    private bool statueNearBy;
    private GameObject _Statue;
    private Item lastNearByItem;
    [SerializeField] private GameObject selecctionMark;
    [SerializeField] private Vector2 selectionOffset;

    [Header("UI")]
    [SerializeField] private itemsNotification _Notification;
    
    [Header("Bars")]
    private HealthBar _healthBar;
    private DefenseBar _defenseBar;
    private void Start()
    {
        //_Input = new PlayerInput_map();
        _Rigidbody = GetComponent<Rigidbody2D>();
        //_Notification = FindObjectOfType<itemsNotification>();
        //levelname = SceneManager.GetActiveScene().name;
        health = maxHealth;
        defense = maxDefense;
        _healthBar = FindObjectOfType<HealthBar>();
        _healthBar.SetMaxHealth(maxHealth);
        _defenseBar = FindObjectOfType<DefenseBar>();
        _defenseBar.SetMaxDefense(maxDefense);

        Debug.Log("Notification Object: " + _Notification.gameObject.name.ToString());
    }
    //private void OnEnable()
    //{
    //    _Input.Enable();

    //    //_Input.Player.Fire Significa atacar pero por bugs del New Input System no puedo cambiarle el nombre
    //    //_Input.Player.Fire.performed += PerformAttack();

    //    _Input.Player.Move.performed += OnMovement;
    //    _Input.Player.Move.canceled += OnMovement;
    //}

    //private void OnDisable()
    //{
    //    //_Input.Player.Fire.performed -= PerformAttack();

    //    //_Input.Player.Fire Significa atacar pero por bugs del New Input System no puedo cambiarle el nombre
    //    _Input.Player.Move.performed -= OnMovement;
    //    _Input.Player.Move.canceled -= OnMovement;

    //    _Input.Disable();
    //}

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
            lastNearByItem = _Item;
            selecctionMark.SetActive(true);
            selecctionMark.transform.position = _Item.transform.position + (Vector3)selectionOffset;
        }
        else if(_Item == null)
        {
            selecctionMark.SetActive(false);
            selecctionMark.transform.position = gameObject.transform.position;
        }

        _healthBar.SetHealth(health);
        _defenseBar.SetDefense(defense);
        AnimMovement();
       
    }

    private void AnimMovement()
    {
        
        if (_Rigidbody.velocity.normalized[1] >= 0.2f)
        {
            animator.SetFloat("Vertical",1);
            animator.SetFloat("Horizontal",0);
        }
        else if (_Rigidbody.velocity.normalized[1] <= -0.2f)
        {
            animator.SetFloat("Vertical",-1);
            animator.SetFloat("Horizontal",0);
        }
        else if (_Rigidbody.velocity.normalized[0] >= 0.2f)
        {
            animator.SetFloat("Horizontal",1);
            animator.SetFloat("Vertical",0);
        }
        else if (_Rigidbody.velocity.normalized[0] <= -0.2f)
        {
            animator.SetFloat("Horizontal",-1);
            animator.SetFloat("Vertical",0);
        }
        
        //Debug.Log(_Rigidbody.velocity.normalized);
        animator.SetFloat("Speed",_Rigidbody.velocity.sqrMagnitude);
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
        
        if (defense > 0)
        {
            defense -- ;
        }
        else
        {
            SFXHit.Play();

            health -= amount;
            Debug.Log("Health" + health);
        }
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

    //private void OnGUI()
    //{
    //    GUIStyle labelStyle = new GUIStyle()
    //    {
    //        fontSize = 60,
    //        normal = new GUIStyleState()
    //        {
    //            textColor = Color.green
    //        }
    //    };

    //    GUIStyle invincibleLabel = new GUIStyle()
    //    {
    //        fontSize = 60,
    //        normal = new GUIStyleState()
    //        {
    //            textColor = Color.yellow
    //        }
    //    };

    //    GUI.Label(new Rect(10, 350, 500, 20), $"Health: ({health})", labelStyle);

    //    if (isInvincible)
    //    {
    //        GUI.Label(new Rect(10, 280, 500, 20), $"Invincible: ({invincibleTimer})", invincibleLabel);
    //    }
    //}

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
        else if (collision == null)
        {
            _Item = null;
            statueNearBy = false;
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
        else if(collision == null)
            _Item = null;
    }

    public void PickUp()
    {
        if (itemNearBy == true)
        {
            Debug.Log("Trying to pick Up");
            speed += _Item.item_speed;
            if (health < maxHealth)
            {
                if ((_Item.item_health + health) <= maxHealth)
                {
                    health += _Item.item_health;
                }
                else
                {
                    health = maxHealth;
                }
            }
           
            maxHealth += _Item.item_maxHealth;
            _healthBar.SetMaxHealth(maxHealth);

            if (defense < maxDefense)
            {
                if ((_Item.item_defense + defense) <= maxDefense)
                {
                    defense += _Item.item_defense;
                }
                else
                {
                    defense = maxDefense;
                }
            }
            maxDefense += _Item.item_maxDefense;
            _defenseBar.SetMaxDefense(maxDefense);
            
            attack += _Item.item_attack;
            shotDamage += _Item.item_shotDamage;
            shotSpeed += _Item.item_shotSpeed;
            shotRange += _Item.item_shotRange;
            
            criticalRateUp = Mathf.Clamp(criticalRateUp + _Item.item_criticalRateUp, 0f,1f);
            
            criticalDamage += _Item.item_criticalDamage;
            timeInvincible += _Item.item_timeInvincible;

            itemsHeld.Push(_Item);
            _Notification.gameObject.SetActive(true);
            _Notification.ItemPickedUp();
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

    public float GetSpeed()
    {
        return speed;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetMaxDefense()
    {
        return maxDefense;
    }
    public float GetDefense()
    {
        return defense;
    }
    public float GetAttack()
    {
        return attack;
    }
    public float GetShotSpeed()
    {
        return shotSpeed;
    }
    public float GetShotRange()
    {
        return shotRange;
    }
    public int GetShotDamage()
    {
        return shotDamage;
    }
    public float GetCriticalDamage()
    {
        return criticalDamage;
    }
    public float GetCriticalRateUp()
    {
        return criticalRateUp;
    }
    public float GetTimeInvincible()
    {
        return timeInvincible;
    }

    public void SetStats(float sp,float maxHth, float hth, float maxDf, float df, float atk, float shotSp, float shotRng, int shotDmg, float crtDmg, float crtRtUp, float tmInvin)
    {
        speed = sp;
        maxHealth = maxHth;
        health = hth;
        maxDefense = maxDf;
        defense = df;
        attack = atk;
        shotSpeed = shotSp;
        shotRange = shotRng;
        shotDamage = shotDmg;
        criticalDamage = crtDmg;
        criticalRateUp = crtRtUp;
        timeInvincible = tmInvin;
    }
}
