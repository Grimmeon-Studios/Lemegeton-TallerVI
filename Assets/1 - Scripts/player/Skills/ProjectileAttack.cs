using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] public float radio = 5.0f;

    PlayerInput_map _Input;
    Vector2 _Position;

    private void Awake()
    {
        _Input = new PlayerInput_map();
    }

    private void Update()
    {
        _Position = fixedJoystick.Direction;
        ChangeTargetPos(targetObj);
        //Debug.Log("Input Value " +  _Position);
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
        Vector2 newPosInCircle = PositionInCircle(Mathf.Atan2(_Position.y, _Position.x));
        Debug.Log("New Position: " + newPosInCircle);
        Vector3 newPos = new Vector3(newPosInCircle.x, newPosInCircle.y);
        target.transform.position = newPos;
    }
}
