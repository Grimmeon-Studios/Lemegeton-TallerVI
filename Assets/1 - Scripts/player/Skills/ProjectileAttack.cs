using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private ProjectileJoystick projectileJoysick;
    [SerializeField] public float radio = 5.0f;

    PlayerInput_map _Input;
    Vector2 _Position;

    private void Awake()
    {
        _Input = new PlayerInput_map();
    }

    private void Update()
    {
        _Position = projectileJoysick.JoystickInput;
        ChangeTargetPos(targetObj);

        if(projectileJoysick._isDragging == true)
        {

        }
    }

    public Vector2 PositionInCircle(Vector2 direction)
    {
        float x = player.transform.position.x + radio * direction.x;
        float y = player.transform.position.y + radio * direction.y;
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
        Vector2 newPosInCircle = PositionInCircle(projectileJoysick.JoystickInput);
        Debug.Log("New Position: " + newPosInCircle);
        Vector3 newPos = new Vector3(newPosInCircle.x, newPosInCircle.y);
        target.transform.position = newPos;
    }
}
