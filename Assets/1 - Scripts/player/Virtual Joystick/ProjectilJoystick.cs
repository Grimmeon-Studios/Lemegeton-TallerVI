using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public GameObject background; // Reference to the background GameObject
    public GameObject knob; // Reference to the knob GameObject
    public Vector2 JoystickInput { get; private set; } // The Vector2 representing the knob's position
    public bool _isDragging { get => isDragging; set => isDragging = value; }

    private RectTransform backgroundRect;
    private RectTransform knobRect;
    private Vector2 knobStartPosition;
    private bool isDragging = false;

    private void Start()
    {
        backgroundRect = background.GetComponent<RectTransform>();
        knobRect = knob.GetComponent<RectTransform>();
        knobStartPosition = knobRect.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        JoystickInput = eventData.position;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        knobRect.anchoredPosition = knobStartPosition;
        JoystickInput = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundRect, eventData.position, eventData.pressEventCamera, out touchPos))
        {
            Vector2 dir = touchPos - backgroundRect.anchoredPosition;
            float radius = backgroundRect.sizeDelta.x / 2;

            // Clamp the knob's position within the boundaries of the background
            knobRect.anchoredPosition = dir.magnitude <= radius ? dir : dir.normalized * radius;
            JoystickInput = new Vector2(knobRect.anchoredPosition.x / radius, knobRect.anchoredPosition.y / radius);

            // Clamp the input within a range of -1 to 1
            JoystickInput = Vector2.ClampMagnitude(JoystickInput, 1f);
        }
    }
}
