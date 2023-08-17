using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public float _Speed = 3;
    [SerializeField] Camera _Camera;
    [SerializeField] private PlayerTouchMovement virtualJoystick;
    PlayerInput_map _Input;

    public Vector2 _Movement;
    public Vector2 _DampedSpeed;

    public Rigidbody2D _Rigidbody;

    private void Awake()
    {
        _Input = new PlayerInput_map();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        _Input.Enable();

        //_Input.Player.Fire Significa atacar pero por bugs del New Input System no puedo cambiarle el nombre
        _Input.Player.Fire.performed += PerformAttack();

        _Input.Player.Move.performed += OnMovement;
        _Input.Player.Move.canceled += OnMovement;
    }

    private void OnDisable()
    {
        _Input.Player.Fire.performed -= PerformAttack();

        //_Input.Player.Fire Significa atacar pero por bugs del New Input System no puedo cambiarle el nombre
        _Input.Player.Move.performed -= OnMovement;
        _Input.Player.Move.canceled -= OnMovement;

        _Input.Disable();
    }

    private Action<InputAction.CallbackContext> PerformAttack()
    {
        throw new NotImplementedException();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _Movement = context.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        if(virtualJoystick.joystickActive == false)
        {
            _DampedSpeed = Vector2.SmoothDamp(_DampedSpeed, _Movement, ref _DampedSpeed, 0.05f);
            _Rigidbody.velocity = _DampedSpeed * _Speed;
        }
    }

}
