using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HazardFreezingZone : MonoBehaviour
{
    private PlayerManager player;
    private float originalSpeed;
    [SerializeField] private GameObject FreezingZone; 
    private float durationFreeze = 1.3f;
    private Button dashBttn;
    private SpriteRenderer sprite;

    public ParticleSystem VFXFrozen;
    GameObject VFXObj;
    private Vector3 yOffset = new Vector3(0, 1.8f);
    private bool isFrozen;

    [SerializeField] private AudioSource SFXFreeze;
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        sprite = GetComponent<SpriteRenderer>();
        dashBttn = player.gameObject.GetComponent<Dash>().dashButton;
        isFrozen = false;
        VFXObj = VFXFrozen.gameObject;
    }

    private void Update()
    {
        if (isFrozen)
        {
            if(player != null)
            {
                VFXObj.transform.position = player.transform.position + yOffset;

            }
        }

    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            isFrozen = true;
            VFXFrozen.Play();
            SFXFreeze.Play();
            dashBttn.interactable = false;
            if (player.GetComponent<Dash>().GetUsingDash())
            {
                originalSpeed = player.GetComponent<Dash>().GetOriginalSpeed();
            }
            else
            {
                originalSpeed = player.speed;
            }
            yield return new WaitForSeconds(0.2f);
            player.speed = 0;
            yield return new WaitForSeconds(durationFreeze);
            isFrozen = false;
            dashBttn.interactable = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(DeactivateVFX());
            
            player.speed = originalSpeed;
        }
    }

    IEnumerator DeactivateVFX()
    {
        VFXFrozen.Stop();
        sprite.DOColor(Color.clear, 1.7f);
        yield return new WaitForSeconds(2);
        VFXObj.transform.position = Vector3.zero;
        DOTween.Kill(gameObject);
        Destroy(gameObject);
    }

}
