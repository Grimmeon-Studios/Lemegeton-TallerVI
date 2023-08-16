using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //Stats
    public float _Speed = 3;
    public float health = 5;
    public float defense = 2;
    public float attack = 3;
    public float critical = 1;
    
    //Arrays from pickUps
    
    
    
    [SerializeField] Camera _Camera;
    PlayerInput_map _Input;
    Vector2 _Movement;
    Vector2 _DampedSpeed;

    Rigidbody2D _Rigidbody;

    private void Awake()
    {
        _Input = new PlayerInput_map();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _Input.Enable();

        _Input.Player.Move.performed += OnMovement;
        _Input.Player.Move.canceled += OnMovement;
    }

    private void OnDisable()
    {
        _Input.Player.Move.performed -= OnMovement;
        _Input.Player.Move.canceled -= OnMovement;

        _Input.Disable();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _Movement = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _DampedSpeed = Vector2.SmoothDamp(_DampedSpeed, _Movement, ref _DampedSpeed, 0.05f);

        _Rigidbody.velocity = _DampedSpeed * _Speed;

    }

    /*public void pickingUp(int pickUps)
    {
        switch (pickUps)
        {
            case 1:
                
                break;
            
        }
        
    }*/

}

