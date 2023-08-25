using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (other.CompareTag("Player")) 
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
