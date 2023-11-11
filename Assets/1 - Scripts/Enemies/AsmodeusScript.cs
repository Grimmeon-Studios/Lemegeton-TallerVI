using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum asmoState
{
    Dead,
    ShootingFast,
    Still,
    BulletHell,
    Striking
    //Wander
};

public class AsmodeusScript : MonoBehaviour
{
    Rigidbody2D rb;
    //Animator animator;
    //AudioSource audioSource;
    private ScoreBoard scoreBoard;
    private ChronometerManager chrono;
    private bool dead = false;

    private bool isCoroutineRunning = false;
    private bool isStrikingCrRunning = false;

    [SerializeField] private bool isTouchingWalls = false;

    public Vector3 defaultStats; // hp, attack, speed

    #region MOVE RELATED
    public float moveSpeed;
    Vector2 position;
    bool firstMove = false;
    [SerializeField] private GameObject spawnPoint;
    #endregion

    #region ATTACK RELATED
    public float firerate;
    float timer;
    public GameObject acidPrefab;
    public Transform firePoint;
    public float shotDamage;
    [SerializeField] private float shootingForce;
    [SerializeField] private int shootsDone = 0;
    #endregion

    #region AI RELATED
    GameObject player;
    public asmoState currState = asmoState.ShootingFast;
    [SerializeField] private float strikingSpeed = 1f; // Speed of movement
    [SerializeField] private float perlinScale = 1f; // Scale of the noise
    private Vector2 startPos;
    private int nextState = 0;
    [SerializeField] private int maxShots;
    // public float range = 20;
    bool chooseDir = false;
    Vector3 randomDir;
    #endregion

    #region idk RELATED
    public float health;
    [SerializeField] private ParticleSystem deathVFX;
    //GameObject door;
    //LevelManagement LM;
    #endregion

    #region DROPS RELATED
    //public GameObject drop1Prefab;
    //public GameObject drop2Prefab;
    #endregion

    #region  AUDIO
    [SerializeField] private AudioSource SFXTakeDamage, SFXDie, SFXShot;
    //public AudioClip acidClip;
    #endregion

    public SpriteRenderer asmoSprite;

    void Start()
    {
        deathVFX.gameObject.SetActive(false);
        scoreBoard = UnityEngine.Object.FindObjectOfType<ScoreBoard>();
        chrono = UnityEngine.Object.FindObjectOfType<ChronometerManager>();

        player = GameObject.Find("_Player");
        //door = GameObject.Find("Door");
        rb = GetComponent<Rigidbody2D>();
        timer = firerate;

        startPos = transform.position;
    
        //LM = door.GetComponent<LevelManagement>();
        //animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        Vector2 Look = player.GetComponent<Rigidbody2D>().position - (Vector2)firePoint.position;
        Look.Normalize();

        switch (currState)
        {
            //case (asmoState.Wander):
            //    Wander();
            //    break;
            case (asmoState.ShootingFast):
                if (timer < 0)
                {
                    Shoot();
                    timer = firerate;
                }
                ShootingFast();
                break;
            case (asmoState.BulletHell):
                if (timer < 0)
                {
                    BulletHell();
                    timer = firerate;

                    //Striking();

                    if (!isStrikingCrRunning)
                    {
                        StartCoroutine(StrikingCr(7f));
                    }
                }
                break;
            case (asmoState.Striking):
                Striking();

                if (!isStrikingCrRunning)
                {
                    StartCoroutine(StrikingCr(7f));
                }
                
                break;
            case (asmoState.Still):
                if (!isCoroutineRunning)
                {
                    StartCoroutine(WaitStill(3f)); 
                } 
                break;
            case (asmoState.Dead):
                StartCoroutine(WaitAndDie(0.7f));
                dead = true;
                break;
        }

        if(shootsDone > maxShots)
        {
            currState = asmoState.Still;
            shootsDone = 0;
        }

        if(currState != asmoState.Still)
        {
            StopCoroutine(WaitStill(1));
        }
        //if (isPlayerInRange(range) && currState != asmoState.Dead)
        //{
        //    currState = asmoState.Run;
        //}
        //else if (!isPlayerInRange(range) && currState != asmoState.Dead)
        //{
        //    currState = asmoState.Wander;
        //}
    }

    //private IEnumerator chooseDirection()
    //{
    //    chooseDir = true;
    //    if (!firstMove)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        //animator.SetBool("Moving", true);
    //        firstMove = true;
    //    }
    //    else
    //    {
    //        yield return new WaitForSeconds(Random.Range(1f, 2f));
    //    }
    //    randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    //    randomDir.Normalize();
    //    chooseDir = false;
    //}

    //void Wander()
    //{

    //    if (!chooseDir)
    //    {
    //        StartCoroutine(chooseDirection());
    //    }
    //    Vector2 position = rb.position;

    //    position.x = position.x + moveSpeed * randomDir.x * Time.deltaTime;
    //    position.y = position.y + moveSpeed * randomDir.y * Time.deltaTime;

    //    rb.MovePosition(position);
    //}

    void ShootingFast()
    {
        // animator.SetBool("Moving", true);
        // StopCoroutine(chooseDirection());
        //if (rb.velocity == Vector2.zero)
        //{
        //    StopCoroutine(WaitStill(1));
        //}
        
        Vector2 position = rb.position;
        Vector2 moveDir = rb.position - player.GetComponent<Rigidbody2D>().position;
        moveDir.Normalize();

        position.x = position.x + moveSpeed * moveDir.x * Time.fixedDeltaTime;
        position.y = position.y + moveSpeed * moveDir.y * Time.fixedDeltaTime;

        rb.MovePosition(position);

    }

    void BulletHell()
    {
        rb.velocity = Vector2.zero;
        transform.position = spawnPoint.transform.position;
    }


    void Striking()
    {
        float perlinX = (Mathf.PerlinNoise(Time.time * strikingSpeed, 0) - 0.5f) * perlinScale * 2;
        float perlinY = (Mathf.PerlinNoise(0, Time.time * strikingSpeed) - 0.5f) * perlinScale * 2;

        transform.position = startPos + new Vector2(perlinX, perlinY);
    }

    void Shoot()
    {
        Vector2 aimDirection = player.GetComponent<Rigidbody2D>().position - (Vector2)firePoint.position;

        aimDirection.Normalize();

        GameObject acidObject = Instantiate(acidPrefab, firePoint.position, Quaternion.identity);
        SoulBullet sBullet = acidObject.GetComponent<SoulBullet>();

        SFXShot.Play();
        sBullet.shoot(aimDirection, shootingForce, shotDamage);

        shootsDone++;
        //AcidShotController asController = acidObject.GetComponent<AcidShotController>();
        //asController.shoot(aimDirection, 10);
        //audioSource.PlayOneShot(acidClip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "BossWall" && currState != asmoState.Striking)
        {
            isTouchingWalls = true;
            gameObject.transform.position = spawnPoint.transform.position;
        }
    }
        //private bool isPlayerInRange(float range)
        //{

        //    return Vector3.Distance(transform.position, player.transform.position) <= range;
        //}

    public void takeDamage(float damage)
    {
        health -= damage;

        if (health > 0)
        {
            SFXTakeDamage.Play();
            asmoSprite.DOColor(new Color(0.4622642f,0.4622642f,0.4622642f), 0.2f).OnComplete(() =>
            {
                 asmoSprite.DOColor(Color.white, 0.1f).OnComplete(() =>
                 {
                     DOTween.KillAll(gameObject);
                 });
            });

        }
        else
        {
            SFXDie.Play();
            currState = asmoState.Dead;
        }
    }

    IEnumerator WaitStill(float seconds)
    {
        if (!dead)
        {
            isCoroutineRunning = true;
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(seconds);

            nextState = Random.Range(1, 4);
            ChangeState(nextState);
            isCoroutineRunning = false;
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator StrikingCr(float seconds)
    {
        if (!dead)
        {
            isStrikingCrRunning = true;
            yield return new WaitForSeconds(seconds);

            currState = asmoState.Still;
            isStrikingCrRunning = false;
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
    }

    void ChangeState(int nextState)
    {
        switch (nextState)
        {
            case 1:
                currState = asmoState.ShootingFast;
                break;
            case 2:
                currState = asmoState.BulletHell;
                break;
            case 3:
                currState = asmoState.Striking;
                break;
            default:
                break;
        }
    }


    IEnumerator WaitAndDie(float seconds)
    {
        if (!dead)
        {
            if (scoreBoard != null && chrono != null)
            {
                scoreBoard.GetPoints(10 * chrono.difficultyLvl);
            }

            Debug.Log("Se murio definitivamente");
            //GetComponent<BoxCollider2D>().size = new Vector2(0, 0);
            GetComponent<SpriteRenderer>().enabled = false;
            acidPrefab = null;
            deathVFX.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(0);

        }
    }
}
