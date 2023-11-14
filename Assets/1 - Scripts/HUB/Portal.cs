using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{

    [SerializeField] private Image blackScreen;
    [SerializeField] private PlayerManager player;
    [SerializeField] private GameObject joystickCanva;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        //joystickCanva = GameObject.Find("Joystick Canva");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            player._Rigidbody.velocity = Vector3.zero;
            joystickCanva.SetActive(false);
            player._playerTouchMovementScript.enabled = false;
            blackScreen.gameObject.SetActive(true);
            blackScreen.DOColor(Color.black, 2f).OnComplete(() => { StartCoroutine(WaintAndLoadScene()); });
        }
    }

    IEnumerator WaintAndLoadScene()
    {
        DOTween.Kill(gameObject);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextScene);

    }
}
