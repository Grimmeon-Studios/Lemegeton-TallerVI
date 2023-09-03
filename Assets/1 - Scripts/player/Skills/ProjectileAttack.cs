using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] public float radio = 5.0f;

    PlayerInput_map _Input;

    private void Awake()
    {
        _Input = new PlayerInput_map();
    }

    public Vector2 PositionInCircle(float angleInDegrees)
    {
        float angleinRads = angleInDegrees * Mathf.Deg2Rad; // Convertir a radianes
        float x = player.transform.position.x + radio * Mathf.Cos(angleinRads);
        float y = player.transform.position.y + radio * Mathf.Sin(angleinRads);
        return new Vector2(x, y);
    }

    private void OnEnable()
    {
        _Input.Enable();
        _Input.Player.ProjectileAttack.performed += OnProjectileAttack;
    }

    private void OnDisable()
    {
        _Input.Disable();
        _Input.Player.ProjectileAttack.performed -= OnProjectileAttack;
    }

    private void OnProjectileAttack(InputAction.CallbackContext obj)
    {

    }
}
