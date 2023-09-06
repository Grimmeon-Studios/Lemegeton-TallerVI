using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class ProjectileTouchMovement : MonoBehaviour
{
    public bool joystickActive = false;
    public Vector2 scaledMovement;
    

    [SerializeField] private Vector2 JoystickSize = new Vector2(250, 250);
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
            Debug.Log("Finger Lost");
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (MovementFinger == null)
        {
            // Check if the touch position is within the Joystick's RectTransform's boundaries
            RectTransform joystickRectTransform = Joystick.RectTransform;
            Vector2 touchPosition = TouchedFinger.screenPosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(joystickRectTransform, touchPosition, null))
            {
                Debug.Log("Finger is touching the screen");
                MovementFinger = TouchedFinger;
                MovementAmount = Vector2.zero;
                Joystick.RectTransform.sizeDelta = JoystickSize;
            }
        }
    }

    private void FixedUpdate()
    {
        if (joystickActive == true)
        {
            
        }
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.yellow
            }
        };
        if (MovementFinger != null)
        {
            GUI.Label(new Rect(1200, 35, 500, 20), $"Finger Start Position: {MovementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(1200, 65, 500, 20), $"X Axis Movement Amount: {MovementAmount.x}", labelStyle);
            GUI.Label(new Rect(1200, 95, 500, 20), $"Y Axis Movement Amount: {MovementAmount.y}", labelStyle);
            GUI.Label(new Rect(1200, 125, 500, 20), $"Scaled Movement Amount: {scaledMovement}", labelStyle);
            GUI.Label(new Rect(1200, 155, 500, 20), $"JoystickActive: {joystickActive}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(1200, 35, 500, 20), "No Current Movement Touch", labelStyle);
            GUI.Label(new Rect(1200, 65, 500, 20), $"JoystickActive: {joystickActive}", labelStyle);
        }

        GUI.Label(new Rect(1200, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}
