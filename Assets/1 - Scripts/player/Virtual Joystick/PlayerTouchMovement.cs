using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    public bool joystickActive = false;
    public Vector2 scaledMovement;


    [SerializeField] private Vector2 JoystickSize = new Vector2(300, 300);
    public FloatingJoystick Joystick;
    public PlayerManager _PlayerManager;
    [SerializeField] private Canvas _Canvas;
    [SerializeField] private Crosshair atkController;

    private Vector2 dampedSpeed;
    private Finger MovementFinger;
    private Vector2 MovementAmount;
    private Vector2 lastNonZeroMovementAmount;
    PlayerInput_map _Input;

    private void Awake()
    {
        atkController = FindAnyObjectByType<Crosshair>();
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        joystickActive = false;
        
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, Joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;

            if (MovementAmount != Vector2.zero)
            {
                lastNonZeroMovementAmount = MovementAmount;
            }
            atkController.UpdateLastAimDirection(MovementAmount);
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            joystickActive = false;
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (joystickActive == false && MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            joystickActive = true;
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }

        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return StartPosition;
    }

    private void FixedUpdate()
    {
        if (joystickActive == true)
        {
            dampedSpeed = Vector2.SmoothDamp(dampedSpeed, MovementAmount, ref dampedSpeed, 0.05f);
            _PlayerManager._Rigidbody.velocity = dampedSpeed * _PlayerManager.speed;
        }
    }


    //private void OnGUI()
    //{
    //    GUIStyle labelStyle = new GUIStyle()
    //    {
    //        fontSize = 24,
    //        normal = new GUIStyleState()
    //        {
    //            textColor = Color.white
    //        }
    //    };
    //    if (MovementFinger != null)
    //    {
    //        GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {MovementFinger.currentTouch.startScreenPosition}", labelStyle);
    //        GUI.Label(new Rect(10, 65, 500, 20), $"X Axis Movement Amount: {MovementAmount.x}", labelStyle);
    //        GUI.Label(new Rect(10, 95, 500, 20), $"Y Axis Movement Amount: {MovementAmount.y}", labelStyle);
    //        GUI.Label(new Rect(10, 125, 500, 20), $"Scaled Movement Amount: {scaledMovement}", labelStyle);
    //        GUI.Label(new Rect(10, 155, 500, 20), $"JoystickActive: {joystickActive}", labelStyle);
    //    }
    //    else
    //    {
    //        GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
    //        GUI.Label(new Rect(10, 65, 500, 20), $"JoystickActive: {joystickActive}", labelStyle);
    //    }

    //    GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    //}
}
