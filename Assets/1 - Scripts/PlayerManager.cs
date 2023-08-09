using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Vector2 MovementInput { get; set; }

    [SerializeField]
    private InputActionReference input_reference;
    private float maxSpeed = 5f;

    private Rigidbody2D rigidB;
    private float currentSpeed;

         
    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(MovementInput.magnitude > 0 && currentSpeed >= 0)
        {
        
        }
    }

    void Update()
    {
        MovementInput = input_reference.action.ReadValue<Vector2>();
    }
}
