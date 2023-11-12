using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum andrasState
{
    Wander,
    Run,
    Dead
};

public class AndrasScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathVFX;

    [SerializeField] private AudioSource SFXTakeDamage, SFXDie, SFXShot;

    Rigidbody2D rb;
    //Animator animator;
    //AudioSource audioSource;
    private ScoreBoard scoreBoard;
    private ChronometerManager chrono;

    private bool dead = false;

    public Vector3 defaultStats; // hp, attack, speed

    #region MOVE RELATED
    public float moveSpeed = 0.5f;
    Vector2 position;
    bool firstMove = false;
    #endregion

    #region ATTACK RELATED
    public float firerate;
    float timer;
    public GameObject acidPrefab;
    public Transform firePoint;
    public float shotDamage;
    public float shootingForce;
    #endregion

    #region AI RELATED
    GameObject player;
    public andrasState currState = andrasState.Wander;
    public float range = 20;
    bool chooseDir = false;
    Vector3 randomDir;
    #endregion

    #region idk RELATED
    public float health;
    //GameObject door;
    //LevelManagement LM;
    #endregion

    #region DROPS RELATED
    //public GameObject drop1Prefab;
    //public GameObject drop2Prefab;
    #endregion

    #region  AUDIO
    //public AudioClip acidClip;
    #endregion

    public SpriteRenderer andras;
    void Start()
    {
        deathVFX.gameObject.SetActive(false);
        scoreBoard = UnityEngine.Object.FindObjectOfType<ScoreBoard>();
        chrono = UnityEngine.Object.FindObjectOfType<ChronometerManager>();
        player = GameObject.Find("_Player");
        //door = GameObject.Find("Door");
        rb = GetComponent<Rigidbody2D>();
        timer = firerate;
        //LM = door.GetComponent<LevelManagement>();
        //animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector2 Look = player.GetComponent<Rigidbody2D>().position - (Vector2)firePoint.position;
        Look.Normalize();

        //animator.SetFloat("Look X", Look.x);
        //animator.SetFloat("Look Y", Look.y);

    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0 && isPlayerInRange(range) == true)
        {
            shoot();
            timer = firerate;
        }

        switch (currState)
        {
            case (andrasState.Wander):
                Wander();
                break;
            case (andrasState.Run):
                Run();
                break;
            case (andrasState.Dead):
                StartCoroutine(WaitAndDie(0.7f));
                dead = true;
                break;
        }

        if (isPlayerInRange(range) && currState != andrasState.Dead)
        {
            currState = andrasState.Run;
        }
        else if (!isPlayerInRange(range) && currState != andrasState.Dead)
        {
            currState = andrasState.Wander;
        }
    }

    private IEnumerator chooseDirection()
    {
        chooseDir = true;
        if (!firstMove)
        {
            yield return new WaitForSeconds(0.1f);
            //animator.SetBool("Moving", true);
            firstMove = true;
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
        randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomDir.Normalize();
        chooseDir = false;
    }

    void Wander()
    {

        if (!chooseDir)
        {
            StartCoroutine(chooseDirection());
        }
        Vector2 position = rb.position;

        position.x = position.x + moveSpeed * randomDir.x * Time.deltaTime;
        position.y = position.y + moveSpeed * randomDir.y * Time.deltaTime;

        rb.MovePosition(position);
    }

    void Run()
    {
        // animator.SetBool("Moving", true);
        StopCoroutine(chooseDirection());
    }

    void shoot()
    {
        Vector2 aimDirection = player.GetComponent<Rigidbody2D>().position - (Vector2)firePoint.position;

        aimDirection.Normalize();

        GameObject acidObject = Instantiate(acidPrefab, firePoint.position, Quaternion.identity);
        AndrasBullet aBullet = acidObject.GetComponent<AndrasBullet>();

        SFXShot.Play();
        aBullet.shoot(aimDirection, shootingForce, shotDamage);
        //AcidShotController asController = acidObject.GetComponent<AcidShotController>();
        //asController.shoot(aimDirection, 10);
        //audioSource.PlayOneShot(acidClip);
    }

    private bool isPlayerInRange(float range)
    {

        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            SFXTakeDamage.Play();
            andras.DOColor(new Color(0.4622642f,0.4622642f,0.4622642f), 0.2f).OnComplete(() =>
            { 
                andras.DOColor(Color.white, 0.1f).OnComplete(() =>
                {
                    DOTween.KillAll(gameObject);
                });
            });

        }
        else
        {
            SFXDie.Play();
            currState = andrasState.Dead;
        }
        
    }

    IEnumerator WaitAndDie(float seconds)
    {
        if(!dead)
        {
            if (scoreBoard != null && chrono != null)
            {
                scoreBoard.GetPoints(100 * chrono.difficultyLvl);
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
