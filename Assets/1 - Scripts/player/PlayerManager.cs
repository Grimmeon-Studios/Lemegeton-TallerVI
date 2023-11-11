using DG.Tweening;
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

    //PlayerInput_map _Input;

    [Header("Movement config.")]
    [SerializeField] private PlayerTouchMovement _playerTouchMovementScript;
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
    [SerializeField] private GameObject attackBtn;
    [SerializeField] private GameObject interactionBtn;
    
    [Header("Bars")]
    private HealthBar _healthBar;
    private DefenseBar _defenseBar;
    
    [Header("Audio")]
    [SerializeField] private AudioSource SFXHit;
    [SerializeField] private AudioSource SFXArmorHit;
    [SerializeField] private AudioSource SFXPickUp;

    [Header("PartSystem")]
    [SerializeField] private ParticleSystem takeDamageVFX;

    private float lastDamageTime;
    private bool isRechargingDefense;
    private float rechargeStartTime;
    private float rechargeDuration = 5.0f;
    private float previousDefenseValue;
    private bool stopRechargeDefense;

    private SpriteRenderer _spriteRenderer;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("Notification Object: " + _Notification.gameObject.name.ToString());

        isRechargingDefense = false;
        isInvincible = false;
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

        // Verifica si han pasado 8 segundos desde el último daño y la recarga no está en curso
        if (Time.time - lastDamageTime >= 8.0f && !isRechargingDefense)
        {
            previousDefenseValue = defense;
            isRechargingDefense = true;
            rechargeStartTime = Time.time;
        }

        // Restablece gradualmente la defensa durante la recarga
        if (isRechargingDefense)
        {
            float elapsedTime = Time.time - rechargeStartTime;

            if (elapsedTime < rechargeDuration)
            {
                // Incrementa gradualmente la defensa a su valor máximo durante la recarga
                defense = Mathf.Lerp(previousDefenseValue, maxDefense, elapsedTime / rechargeDuration);
                _defenseBar.SetDefense(defense);
                if (stopRechargeDefense)
                {
                    defense = Convert.ToUInt32(defense);
                    previousDefenseValue = defense;
                    isRechargingDefense = false;
                    stopRechargeDefense = false;
                }
            }
            else
            {
                // Finaliza la recarga cuando se alcanza la duración deseada
                isRechargingDefense = false;
                defense = maxDefense;
                _defenseBar.SetDefense(defense);
            }
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
        lastDamageTime = Time.time;

        if (isRechargingDefense)
        {
            stopRechargeDefense = true;
            defense = previousDefenseValue;
            _defenseBar.SetDefense(defense);
        }
        if (defense > 0)
        {
            SFXArmorHit.Play();
            defense-- ;

            float loopsTime = timeInvincible / 6;
            _spriteRenderer.DOColor(new Color(1, 1, 1, 0.3f), loopsTime).SetEase(Ease.InOutCubic).SetLoops(6).OnComplete(() =>
            {
                _spriteRenderer.DOColor(Color.white, 0.3f).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    DOTween.Kill(gameObject);
                });
            });
        }
        else
        {
            SFXHit.Play();
            takeDamageVFX.Play();

            health -= amount;
            Debug.Log("Health" + health);

            float loopsTime = timeInvincible / 6;
            _spriteRenderer.DOColor(new Color(1, 1, 1, 0.3f), loopsTime).SetEase(Ease.InOutCubic).SetLoops(6).OnComplete(() =>
            {
                _spriteRenderer.DOColor(Color.white, 0.3f).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    DOTween.Kill(gameObject);
                });
            });
        }

        if (health <= 0)
        {
            Death();
        }
        /*else
        {
            DamageVFX.Play(); // feedback of the damage
        }*/
        
    }
    public void HazardLava()
    {
        defense = 0f;
        isInvincible = true;
        invincibleTimer = timeInvincible;
        lastDamageTime = Time.time;
        if (isRechargingDefense)
        {
            stopRechargeDefense = true;
            previousDefenseValue = 0f;
            defense = previousDefenseValue;
            _defenseBar.SetDefense(defense);
        }
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
            _Item = collision.GetComponent<Item>();

            itemNearBy = true;
            selecctionMark.SetActive(true);
            selecctionMark.transform.position = _Item.transform.position + (Vector3)selectionOffset;


            interactionBtn.SetActive(true);
            attackBtn.SetActive(false);

            _Notification.gameObject.SetActive(true);
            _Notification.DisplayNearByItem(_Item,false);
        }
        else if (collision != null && collision.CompareTag("Statue"))
        {
            statueNearBy = true;
            _Statue = collision.gameObject;
            if (!_Statue.GetComponent<ItemStatueSpawner>().isStatueUsed)
            {
                interactionBtn.SetActive(true);
                attackBtn.SetActive(false);
            }
            
        }
        else if (collision == null)
        {
            _Item = null;
            statueNearBy = false;
            attackBtn.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Item"))
        {
            _Item = collision.GetComponent<Item>();

            _Notification.DisplayNearByItem(_Item, true);

            itemNearBy = false;
            interactionBtn.SetActive(false);
            attackBtn.SetActive(true);

            selecctionMark.SetActive(false);
            selecctionMark.transform.position = gameObject.transform.position;
        }
        else if (collision != null && collision.CompareTag("Statue"))
        {
            statueNearBy = false;
            _Statue = null;

            interactionBtn.SetActive(false);
            attackBtn.SetActive(true);
            Debug.Log(_Statue + ": Too Far");
        }
        else if(collision == null)
            _Item = null;
    }

    public void PickUp()
    {
        if (itemNearBy == true)
        {
            SFXPickUp.Play();

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

    public void SwitchMovement(bool b)
    {
        if (b)
        {
            speed = 10;
        }
        else if (!b)
        {
            speed = 0;
        }
    }
}
