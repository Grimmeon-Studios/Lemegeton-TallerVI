using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Room Blockers")]
    [SerializeField] private GameObject roomBlocker1;
    [SerializeField] private GameObject roomBlocker2;
    [SerializeField] private GameObject roomBlocker3;
    [SerializeField] private GameObject roomBlocker4;

    [Header("Player Config")]
    [SerializeField] private GameObject Joystick;
    [SerializeField] private GameObject ShotBtn;
    [SerializeField] private GameObject DashBtn;
    [SerializeField] private GameObject player;

    [Header("Enemies Prefabs")]
    [SerializeField] private GameObject belhor; 
    [SerializeField] private GameObject andras; 
    [SerializeField] private GameObject lostSoul;

    [Header("Intro Spawn Points")]
    [SerializeField] private GameObject belhorSp1;
    [SerializeField] private GameObject belhorSp2;

    [Header("Critical Spawn Points")]
    [SerializeField] private GameObject belhorSp3;
    [SerializeField] private GameObject belhorSp4;

    [Header("Shoot Spawn Points")]
    [SerializeField] private GameObject andrasSp1;
    [SerializeField] private GameObject andrasSp2;
    [SerializeField] private GameObject andrasSp3;
    [SerializeField] private GameObject andrasSp4;
    [SerializeField] private GameObject andrasSp5;
    [SerializeField] private GameObject andrasSp6;

    [Header("Dash Spawn Points")]
    [SerializeField] private GameObject sheddimSp1;
    [SerializeField] private GameObject sheddimSp2;
    [SerializeField] private GameObject sheddimSp3;
    [SerializeField] private GameObject belhorSp6;
    [SerializeField] private GameObject belhorSp7;

    private PlayerManager playerManager;
    private float previousSpeed;

    public int currentEnemies;

    private void Start()
    {
        playerManager = player.GetComponent<PlayerManager>();
        previousSpeed = playerManager.speed;
        playerManager.speed = 0;
    }

    private void Update()
    {
        if(currentEnemies >= 2)
        {
            roomBlocker1.SetActive(false);
        }
        
        if(currentEnemies >= 4)
        {
            roomBlocker2.SetActive(false);
        }
        
        if(currentEnemies >= 10)
        {
            roomBlocker3.SetActive(false);
        }
        
        if (currentEnemies >= 15)
        {
            roomBlocker4.SetActive(false);
        }
    }

    //Intro Tutorial
    public void Tutorial_Intro()
    {
        roomBlocker1.SetActive(true);
        currentEnemies = 0;
    }

    public void Tutorial_Outro()
    {
        playerManager.speed = previousSpeed;
        Instantiate(belhor,belhorSp1.transform.position,Quaternion.identity);
        Instantiate(belhor,belhorSp2.transform.position,Quaternion.identity);
        belhorSp1.SetActive(false);
        belhorSp2.SetActive(false);

    }

    // Defense
    public void Defense_Intro()
    {
        playerManager.speed = 0;
    }

    public void Defense_Outro()
    {
        playerManager.speed = previousSpeed;
        
    }

    // Pause
    public void Pause_Intro()
    {
        playerManager.speed = 0;
    }

    public void Pause_Outro()
    {
        playerManager.speed = previousSpeed;
        Instantiate(belhor, belhorSp3.transform.position, Quaternion.identity);
        Instantiate(belhor, belhorSp4.transform.position, Quaternion.identity);
        belhorSp3.SetActive(false);
        belhorSp4.SetActive(false);

    }

    // Statue
    public void Satue_Intro()
    {
        playerManager.speed = 0;
    }

    public void Satue_Outro()
    {
        playerManager.speed = previousSpeed;
    }

    // Shot
    public void Shot_Intro()
    {
        playerManager.speed = 0;
    }

    public void Shot_Outro()
    {
        playerManager.speed = previousSpeed;
        ShotBtn.SetActive(true);

        Instantiate(andras, andrasSp1.transform.position, Quaternion.identity);
        Instantiate(andras, andrasSp2.transform.position, Quaternion.identity);
        Instantiate(andras, andrasSp3.transform.position, Quaternion.identity);
        Instantiate(andras, andrasSp4.transform.position, Quaternion.identity);
        Instantiate(andras, andrasSp5.transform.position, Quaternion.identity);
        Instantiate(andras, andrasSp6.transform.position, Quaternion.identity);

        andrasSp1.SetActive(false);
        andrasSp2.SetActive(false);
        andrasSp3.SetActive(false);
        andrasSp4.SetActive(false);
        andrasSp5.SetActive(false);
        andrasSp6.SetActive(false);

    }


    // Dash
    public void Dash_Intro()
    {
        playerManager.speed = 0;
        ShotBtn.SetActive(false);

    }

    public void Dash_Outro()
    {
        playerManager.speed = previousSpeed;
        ShotBtn.SetActive(true);
        DashBtn.SetActive(true);

        Instantiate(lostSoul, sheddimSp1.transform.position, Quaternion.identity);
        Instantiate(lostSoul, sheddimSp2.transform.position, Quaternion.identity);
        Instantiate(lostSoul, sheddimSp3.transform.position, Quaternion.identity);
        Instantiate(belhor, belhorSp6.transform.position, Quaternion.identity);
        Instantiate(belhor, belhorSp7.transform.position, Quaternion.identity);

        sheddimSp1.SetActive(false);
        sheddimSp2.SetActive(false);
        sheddimSp3.SetActive(false);
        belhorSp6.SetActive(false);
        belhorSp7.SetActive(false);
    }

    // Asmodeus
    public void Asmodeus_Intro()
    {
        playerManager.speed = 0;
        ShotBtn.SetActive(false);
        DashBtn.SetActive(false);
    }

    public void Asmodeus_Outro()
    {
        SceneManager.LoadScene("Dungeon Main");
    }
}
