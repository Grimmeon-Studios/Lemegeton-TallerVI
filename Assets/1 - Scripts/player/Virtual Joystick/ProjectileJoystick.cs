using UnityEngine;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour
{
    public Image knob;
    public GameObject joystickBG;
    public float joystickRadius = 50f;

    private Vector2 initialKnobPosition;

    private void Start()
    {
        joystickRadius = joystickBG.transform.localScale.x / 2;
        initialKnobPosition = knob.rectTransform.anchoredPosition;
    }



    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (Vector2.Distance(touch.position, initialKnobPosition) <= joystickRadius)
                {
                    // Handle touch began event, e.g., set joystick active.
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (Vector2.Distance(touch.position, initialKnobPosition) <= joystickRadius)
                {
                    // Calculate knob's position based on touch input.
                    Vector2 newPosition = initialKnobPosition + (touch.position - initialKnobPosition).normalized * joystickRadius;
                    knob.rectTransform.anchoredPosition = newPosition;

                    // Use newPosition for controlling your character's movement.
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Handle touch ended event, e.g., set joystick inactive.
                knob.rectTransform.anchoredPosition = initialKnobPosition;
            }
        }
    }
}
