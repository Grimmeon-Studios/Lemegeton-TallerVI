using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ToTheBoss : MonoBehaviour
{
    [SerializeField] private SavingSystem saving;
    private void OnTriggerEnter2D(Collider2D other)
    {
        saving.SaveGame();
        SceneManager.LoadScene("BOSS");
    }
}