using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //healthbar slider
    public Slider slider;
    
    //sets max health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    //sets current health
    public void SetHealth(int health)
    {
        slider.value = health;
    }

}
