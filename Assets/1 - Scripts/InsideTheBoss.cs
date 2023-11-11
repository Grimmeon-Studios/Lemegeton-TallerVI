using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideTheBoss : MonoBehaviour
{
    [SerializeField] private SavingSystem saving;
    void Start()
    {
        saving.LoadGame();
    }
    
}
