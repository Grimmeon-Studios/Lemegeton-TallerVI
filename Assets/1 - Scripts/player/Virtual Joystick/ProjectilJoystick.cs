using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ProjectileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    [SerializeField] private ProjectileAttack playerProjectileMethod;
    //[SerializeField] private TextMeshProUGUI cd_text;
    public GameObject background; // Reference to the background GameObject
    public GameObject knob; // Reference to the knob GameObject
    public Vector2 JoystickInput { get; private set; } // The Vector2 representing the knob's position
    public bool _isDragging { get => isDragging; set => isDragging = value; }

    private RectTransform backgroundRect;
    private RectTransform knobRect;
    private Vector2 knobStartPosition;
    private bool isDragging = false;

    public bool onCD;
    private float CDtime;
    private float timerCD;
    public GameObject joystickBlocker;
    private void Start()
    {
        backgroundRect = background.GetComponent<RectTransform>();
        knobRect = knob.GetComponent<RectTransform>();
        knobStartPosition = knobRect.anchoredPosition;
        onCD = false;
        CDtime = playerProjectileMethod.proyectile_CD;
        timerCD = 0f;
        joystickBlocker.SetActive(false);
        
    }

    private void Update()
    {
        if(onCD)
        {
            timerCD += Time.deltaTime;
            background.GetComponent<Image>().fillAmount = (timerCD / CDtime);
            joystickBlocker.SetActive(true);
            if (timerCD >= CDtime)
            {
                onCD = false;
                timerCD = 0f;
                joystickBlocker.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(onCD==false)
        {
            isDragging = true;
            JoystickInput = eventData.position;
            OnDrag(eventData);
            background.GetComponent<Image>().fillAmount = 0f;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        knobRect.anchoredPosition = knobStartPosition;
        JoystickInput = Vector2.zero;
        Aguapanela();
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

    public void Aguapanela()
    {
        playerProjectileMethod.LaunchProjectile();
        onCD = true;
        //00D6FF
        //gameObject.GetComponent<Image>().tintColor.a = 0.1f;

    }
}
