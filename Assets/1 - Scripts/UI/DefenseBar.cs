using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenseBar : MonoBehaviour
{
       public Slider slider;
       public Image fill;
       public TextMeshProUGUI number;
       private float max;
       public void SetMaxDefense(float maxdefense)
       {
           slider.maxValue = maxdefense;
           slider.value = maxdefense;
           number.text = maxdefense.ToString("0.0") + " / " + maxdefense.ToString("0.0");
           max = maxdefense;
       }
   
       public void SetDefense(float defense)
       {
           slider.value = defense;
           number.text = defense.ToString("0.0") + " / " + max.ToString("0.0");
       }
}
