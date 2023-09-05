using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lookAheadGameObj;
    [SerializeField] private FixedJoystick joystickRef;
    [SerializeField] private float radio = 5.0f;

    private Vector2 joystick_direction;

    PlayerInput_map _Input;

    private void Awake()
    {
        _Input = new PlayerInput_map();
        joystick_direction = joystickRef.input;
    }

    private void Update()
    {
        if(joystickRef.isShooting)
        {
            ChangeTargetPos(lookAheadGameObj);
        }
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

    private void ChangeTargetPos(GameObject target)
    {
        Vector2 newPosInCircle = PositionInCircle(Mathf.Atan2(joystick_direction.y, joystick_direction.x));
        Debug.Log("New Position: " + newPosInCircle);
        Debug.Log("New Angle: " + PositionInCircle(Mathf.Atan2(joystick_direction.y, joystick_direction.x)));
        Debug.Log("New : " + joystick_direction.y +" "+ joystick_direction.x);
        Vector3 newPos = new Vector3(newPosInCircle.x, newPosInCircle.y);
        target.transform.position = newPos;
    }
}
