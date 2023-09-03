using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class ProjectileTouchMovement : MonoBehaviour
{
    public bool joystickActive = false;
    public Vector2 scaledMovement;
    

    [SerializeField] private Vector2 JoystickSize = new Vector2(100, 100);
    [SerializeField] private FloatingJoystick Joystick;
    [SerializeField] private PlayerManager _PlayerManager;
    [SerializeField] private Canvas _Canvas;


    private Finger MovementFinger;
    private Vector2 MovementAmount;
    PlayerInput_map _Input;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        
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
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            joystickActive = false;
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (MovementFinger == null && TouchedFinger.screenPosition.x <= Joystick.gameObject.transform.localPosition.x)
        {
            joystickActive = true;
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
        }
    }

    //private Vector2 ClampStartPosition(Vector2 StartPosition)
    //{
    //    if (StartPosition.x < JoystickSize.x / 2)
    //    {
    //        StartPosition.x = JoystickSize.x / 2;
    //    }

    //    if (StartPosition.y < JoystickSize.y / 2)
    //    {
    //        StartPosition.y = JoystickSize.y / 2;
    //    }
    //    else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
    //    {
    //        StartPosition.y = Screen.height - JoystickSize.y / 2;
    //    }

    //    return StartPosition;
    //}

    private void FixedUpdate()
    {
        if (joystickActive == true)
        {
            
        }
    }

}
