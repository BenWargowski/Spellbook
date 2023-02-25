using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    

    public void UpdateBar(float Health, float maxHealth)
    {
        slider.value = Health/maxHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
