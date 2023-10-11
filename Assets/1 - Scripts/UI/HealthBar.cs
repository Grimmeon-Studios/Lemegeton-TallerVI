using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI number;
    private float max;
    public void SetMaxHealth(float maxhealth)
    {
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
        fill.color = gradient.Evaluate(1f);
        number.text = maxhealth.ToString("0.0") + " / " + maxhealth.ToString("0.0");
        max = maxhealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        number.text = health.ToString("0.0") + " / " + max.ToString("0.0");
    }
}
